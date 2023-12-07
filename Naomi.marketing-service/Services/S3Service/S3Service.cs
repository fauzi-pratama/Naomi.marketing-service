using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Amazon.S3;
using System.Net;
using Amazon;

using Naomi.marketing_service.Models.Contexts;
using Naomi.marketing_service.Models.Entities;
using Naomi.marketing_service.Models.Response;
using Helper = Naomi.marketing_service.Helpers.Helper;

namespace Naomi.marketing_service.Services.S3Service
{
    public class S3Service : IS3Service
    {
        private readonly DataDbContext _dbContext;
        public S3Service(DataDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        #region DownloadFile
        public async Task<S3ServiceResponse<Stream>> DownloadS3FileAsync(string promoId, string appCode)
        {
            S3ServiceResponse<Stream> response = new();
            PromotionAppDisplay promoApp = _dbContext.PromotionAppDisplay.Where(x => x.AppCode == appCode).FirstOrDefault() ?? new PromotionAppDisplay();
            if (promoApp != null && promoApp.Id != Guid.Empty)
            {
                string accessKey = Helper.fDecrypt(promoApp.AccessKey! ?? "", "", DateTime.Parse("1900/01/01")).ToString();
                string secretKey = Helper.fDecrypt(promoApp.SecretKey! ?? "", "", DateTime.Parse("1900/01/01")).ToString();
                string bucketName = promoApp.BucketName! ?? "";
                string region = promoApp.Region! ?? "";
                string linkFolder = promoApp.BaseDirectory! ?? "";

                string fileName = "";
                PromotionAppImage promoImg = _dbContext.PromotionAppImage.Where(x => x.PromotionHeaderId.ToString() == promoId && x.AppCode == appCode).FirstOrDefault() ?? new PromotionAppImage();
                if (promoImg != null && promoImg.Id != Guid.Empty)
                    fileName = promoImg.FileName! ?? "";

                //setelah baseDirectory adalah promotionId kemudian image name (yg kebetulan menggunakan promotionId)
                if (string.IsNullOrEmpty(linkFolder))
                    linkFolder = string.Format("{0}/{1}", promoId, fileName);
                else
                    linkFolder = string.Format("{0}/{1}/{2}", linkFolder, promoId, fileName);


                try
                {
                    using var amazonS3client = new AmazonS3Client(accessKey, secretKey, RegionEndpoint.GetBySystemName(region));
                    var transferUtility = new TransferUtility(amazonS3client);

                    var responseS3 = await transferUtility.S3Client.GetObjectAsync(new GetObjectRequest()
                    {
                        // Bucket Name
                        BucketName = bucketName,
                        // File Name
                        Key = linkFolder
                    });
                    // Return File not found if the file doesn't exist
                    if (responseS3.ResponseStream == null)
                    {
                        response.http_response = 99;
                        response.response_code = "404";
                        response.response_message = "Not found";
                    }
                    else
                    {
                        response.data = responseS3.ResponseStream;
                        response.http_response = 00;
                        response.response_code = "200";
                        response.response_message = responseS3.Headers.ContentType;
                        response.file_path = fileName;
                    }

                }
                catch (AmazonS3Exception amazonS3Exception)
                {
                    if (amazonS3Exception.ErrorCode != null && (amazonS3Exception.ErrorCode.Equals("InvalidAccessKeyId") ||
                        amazonS3Exception.ErrorCode.Equals("InvalidSecurity")))
                    {
                        response.http_response = 99;
                        response.response_code = "404";
                        response.response_message = "Please check the AWS Credentials.";
                    }
                    else
                    {
                        response.http_response = 99;
                        response.response_code = "404";
                        response.response_message = amazonS3Exception.Message;
                    }
                }
            }
            else
            {
                response.http_response = 99;
                response.response_code = "404";
                response.response_message = "No App found";
            }

            return response;
        }
        #endregion

        #region UploadFile
        public async Task<S3ServiceResponse<bool>> UploadFileAsync(MemoryStream file, string promoId, string fileName, string typecontent, string appCode, DateTime endDate)
        {
            S3ServiceResponse<bool> response = new();

            PromotionAppDisplay promoApp = _dbContext.PromotionAppDisplay.Where(x => x.AppCode == appCode).FirstOrDefault() ?? new PromotionAppDisplay();
            if (promoApp != null && promoApp.Id != Guid.Empty)
            {
                string accessKey = Helper.fDecrypt(promoApp.AccessKey! ?? "", "", DateTime.Parse("1900/01/01")).ToString();
                string secretKey = Helper.fDecrypt(promoApp.SecretKey! ?? "", "", DateTime.Parse("1900/01/01")).ToString();
                string bucketName = promoApp.BucketName! ?? "";
                string region = promoApp.Region! ?? "";
                string linkFolder = promoApp.BaseDirectory! ?? "";

                //setelah baseDirectory adalah promotionId kemudian image name (yg kebetulan menggunakan promotionId)
                if (string.IsNullOrEmpty(linkFolder))
                    linkFolder = string.Format("{0}/{1}", promoId, fileName);
                else
                    linkFolder = string.Format("{0}/{1}/{2}", linkFolder, promoId, fileName);

                string uploadedFileUrl = "";

                try
                {
                    using var newMemoryStream = new MemoryStream();
                    file.CopyTo(newMemoryStream);

                    var uploadRequest = new TransferUtilityUploadRequest
                    {
                        InputStream = newMemoryStream,
                        Key = linkFolder, 
                        BucketName = bucketName,
                        ContentType = typecontent
                    };

                    using (var amazonClient = new AmazonS3Client(accessKey, secretKey, RegionEndpoint.GetBySystemName(region)))
                    {
                        var fileTransferUtility = new TransferUtility(amazonClient);
                        await fileTransferUtility.UploadAsync(uploadRequest);

                        GetPreSignedUrlRequest urlRequest = new()
                        {
                            BucketName = bucketName,
                            Key = linkFolder,
                            Expires = endDate
                        };
                        uploadedFileUrl = amazonClient.GetPreSignedURL(urlRequest);
                    }

                    response.data = true;
                    response.http_response = 00;
                    response.response_code = "200";
                    response.response_message = "SUCCESS";
                    response.file_path = uploadedFileUrl;
                }
                catch (Exception ex)
                {
                    response.http_response = 99;
                    response.response_code = "404";
                    response.response_message = ex.Message;
                }
            }
            else
            {
                response.http_response = 99;
                response.response_code = "404";
                response.response_message = "No storage settings found";
            }

            return response;
        }
        #endregion

        #region DeleteFile
        public async Task<bool> DeleteFileAsync(string promoId, string appCode)
        {
            PromotionAppDisplay promoApp = _dbContext.PromotionAppDisplay.Where(x => x.AppCode == appCode).FirstOrDefault() ?? new PromotionAppDisplay();
            if (promoApp != null && promoApp.Id != Guid.Empty)
            {
                try
                {
                    string accessKey = Helper.fDecrypt(promoApp.AccessKey! ?? "", "", DateTime.Parse("1900/01/01")).ToString();
                    string secretKey = Helper.fDecrypt(promoApp.SecretKey! ?? "", "", DateTime.Parse("1900/01/01")).ToString();
                    string bucketName = promoApp.BucketName! ?? "";
                    string region = promoApp.Region! ?? "";
                    string linkFolder = promoApp.BaseDirectory! ?? "";

                    string fileName = "";
                    PromotionAppImage promoImg = _dbContext.PromotionAppImage.Where(x => x.PromotionHeaderId.ToString() == promoId && x.AppCode == appCode).FirstOrDefault() ?? new PromotionAppImage();
                    if (promoImg != null && promoImg.Id != Guid.Empty)
                        fileName = promoImg.FileName! ?? "";

                    //setelah baseDirectory adalah promotionId kemudian image name (yg kebetulan menggunakan promotionId)
                    if (string.IsNullOrEmpty(linkFolder))
                        linkFolder = string.Format("{0}/{1}", promoId, fileName);
                    else
                        linkFolder = string.Format("{0}/{1}/{2}", linkFolder, promoId, fileName);


                    var deleteRequest = new DeleteObjectRequest()
                    {
                        // Bucket Name
                        BucketName = bucketName,
                        // File Name
                        Key = linkFolder
                    };

                    using var amazonClient = new AmazonS3Client(accessKey, secretKey, RegionEndpoint.GetBySystemName(region));
                    var fileTransferUtility = new TransferUtility(amazonClient);
                    await fileTransferUtility.S3Client.DeleteObjectAsync(deleteRequest);
                }
                catch
                {
                    return false;
                }
                return true;
            }
            return false;
        }
        #endregion
    }
}
