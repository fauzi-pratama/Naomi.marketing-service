using Naomi.marketing_service.Models.Response;

namespace Naomi.marketing_service.Services.S3Service
{
    public interface IS3Service
    {
        Task<S3ServiceResponse<Stream>> DownloadS3FileAsync(string promoId, string appCode);
        Task<S3ServiceResponse<bool>> UploadFileAsync(MemoryStream file, string promoId, string fileName, string typecontent, string appCode, DateTime endDate);
        Task<bool> DeleteFileAsync(string promoId, string appCode);
    }
}
