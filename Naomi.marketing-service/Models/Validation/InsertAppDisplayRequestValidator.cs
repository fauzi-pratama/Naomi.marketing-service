using FluentValidation;
using Naomi.marketing_service.Models.Request;
using static Naomi.marketing_service.Models.Request.PromotionClassRequest;

namespace Naomi.marketing_service.Models.Validation
{
    public class InsertAppDisplayRequestValidator : AbstractValidator<AppDisplayRequest>
    {
        public InsertAppDisplayRequestValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Stop;

            #region AppName
            RuleFor(x => x.AppName).NotNull().WithMessage("App name is required")
                                   .NotEmpty().WithMessage("App name is required")
                                   .NotEqual("string", StringComparer.OrdinalIgnoreCase).WithMessage("App name is required")
                                   .MaximumLength(200).WithMessage("App name must be 200 chars or less");
            #endregion

            #region BucketName
            RuleFor(x => x.BucketName).NotEqual("string", StringComparer.OrdinalIgnoreCase).When(x => !string.IsNullOrEmpty(x.BucketName)).WithMessage("Please check Bucket Name")
                                      .MaximumLength(200).When(x => !string.IsNullOrEmpty(x.BucketName)).WithMessage("Bucket name must be 200 chars or less");
            #endregion

            #region Region
            RuleFor(x => x.Region).NotEqual("string", StringComparer.OrdinalIgnoreCase).When(x => !string.IsNullOrEmpty(x.Region)).WithMessage("Please check Region")
                                  .MaximumLength(200).When(x => !string.IsNullOrEmpty(x.Region)).WithMessage("Region must be 200 chars or less");
            #endregion

            #region SecretKey
            RuleFor(x => x.SecretKey).NotEqual("string", StringComparer.OrdinalIgnoreCase).When(x => !string.IsNullOrEmpty(x.SecretKey)).WithMessage("Please check Secret Key")
                                  .MaximumLength(200).When(x => !string.IsNullOrEmpty(x.SecretKey)).WithMessage("Secret Key must be 200 chars or less");
            #endregion

            #region AccessKey
            RuleFor(x => x.AccessKey).NotEqual("string", StringComparer.OrdinalIgnoreCase).When(x => !string.IsNullOrEmpty(x.AccessKey)).WithMessage("Please check Access Key")
                                  .MaximumLength(200).When(x => !string.IsNullOrEmpty(x.AccessKey)).WithMessage("Access Key must be 200 chars or less");
            #endregion

            #region BaseDirectory
            RuleFor(x => x.BaseDirectory).NotEqual("string", StringComparer.OrdinalIgnoreCase).When(x => !string.IsNullOrEmpty(x.BaseDirectory)).WithMessage("Please check Base Directory")
                                  .MaximumLength(200).When(x => !string.IsNullOrEmpty(x.BaseDirectory)).WithMessage("Base Directory must be 200 chars or less");
            #endregion

            #region Username
            RuleFor(x => x.Username).NotNull().WithMessage("Username is required")
                                    .NotEmpty().WithMessage("Username is required")
                                    .NotEqual("string", StringComparer.OrdinalIgnoreCase).WithMessage("Username is required")
                                    .MaximumLength(50).WithMessage("Username must be 50 chars or less");
            #endregion
        }
    }
}
