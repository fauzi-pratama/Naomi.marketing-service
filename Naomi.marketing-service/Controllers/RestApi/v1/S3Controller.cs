using Microsoft.AspNetCore.Mvc;
using Naomi.marketing_service.Models.Response;
using Naomi.marketing_service.Services.S3Service;

namespace Naomi.marketing_service.Controllers.RestApi.v1
{
    [Route("/v1/")]
    [ApiController]
    public class S3Controller : ControllerBase
    {
        private readonly IS3Service _s3Service;
        private readonly ILogger<S3Controller> _logger;
        public S3Controller(IS3Service s3Service, ILogger<S3Controller> logger)
        {
            _s3Service = s3Service;
            _logger = logger;
        }

        #region DownloadImage
        [HttpGet("download_s3_file_async")]
        public async Task<IActionResult> DownloadS3FileAsync(string promoId, string appCode)
        {
            _logger.LogInformation("Calling download_s3_file_async with promo Id: {promoId} and app Code: {appCode}", promoId, appCode);

            S3ServiceResponse<Stream> result = await _s3Service.DownloadS3FileAsync(promoId, appCode);

            if (result.response_code == "200")
            {
                _logger.LogInformation("Success download_s3_file_async with promo Id: {promoId} and app Code: {appCode}", promoId, appCode);

                /* response_message digunakan untuk menampung contentType, filePath digunakan untuk menampung filename + extension */
                return File(result.data!, result.response_message!, result.file_path);
            }

            _logger.LogError("Failed download_s3_file_async with promo Id: {promoId} and app Code: {appCode}", promoId, appCode);
            return NotFound(result);
        }
        #endregion
    }
}
