using Microsoft.EntityFrameworkCore;
using Naomi.marketing_service.Models.Contexts;
using Naomi.marketing_service.Models.Entities;
using Helper = Naomi.marketing_service.Helpers.Helper;

namespace Naomi.marketing_service.Services.PromoAppService
{
    public class PromoAppService : IPromoAppService
    {
        private readonly DataDbContext _dbContext;
        public PromoAppService(DataDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        #region GetData
        public async Task<Tuple<List<PromotionAppDisplay>, int>> GetPromotionAppDisplay(string? searchName, int pageNo, int pageSize)
        {
            List<PromotionAppDisplay> promoAppDisplay = new();
            int Skip = (pageNo - 1) * pageSize;

            if (!string.IsNullOrEmpty(searchName))
            {
                promoAppDisplay = await _dbContext.PromotionAppDisplay.Where(x => x.AppName!.Trim().ToUpper().Contains(searchName!.Trim().ToUpper())).ToListAsync() ?? new List<PromotionAppDisplay>();
            }
            else
            {
                promoAppDisplay = await _dbContext.PromotionAppDisplay.ToListAsync() ?? new List<PromotionAppDisplay>();
            }

            promoAppDisplay.ForEach(x =>
            {
                x.AccessKey = Helper.fDecrypt(x.AccessKey! ?? "", "", DateTime.Parse("1900/01/01")).ToString();
                x.SecretKey = Helper.fDecrypt(x.SecretKey! ?? "", "", DateTime.Parse("1900/01/01")).ToString();
            });

            int totalPages = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(promoAppDisplay.Count) / Convert.ToDouble(pageSize)));
            promoAppDisplay = promoAppDisplay.OrderBy(x => x.AppCode).Skip(Skip).Take(pageSize).ToList();
            return new Tuple<List<PromotionAppDisplay>, int>(promoAppDisplay ?? new List<PromotionAppDisplay>(), totalPages);
        }
        public async Task<PromotionAppDisplay> GetPromotionAppDisplayByCodeAsync(string? appCode)
        {
            PromotionAppDisplay promoAppDisplay = await _dbContext.PromotionAppDisplay.Where(x => x.AppCode!.Trim().ToLower() == appCode!.Trim().ToLower()).FirstOrDefaultAsync() ?? new PromotionAppDisplay();
            if (promoAppDisplay != null && promoAppDisplay.Id != Guid.Empty)
            {
                promoAppDisplay.AccessKey = Helper.fDecrypt(promoAppDisplay.AccessKey! ?? "", "", DateTime.Parse("1900/01/01")).ToString();
                promoAppDisplay.SecretKey = Helper.fDecrypt(promoAppDisplay.SecretKey! ?? "", "", DateTime.Parse("1900/01/01")).ToString();
            }

            return promoAppDisplay!;
        }
        #endregion

        #region InsertData
        public async Task<Tuple<PromotionAppDisplay, string>> InsertPromotionAppDisplay(PromotionAppDisplay newPromoAppDisplay)
        {
            //validate app name
            bool isExist = _dbContext.PromotionAppDisplay.Any(x => x.AppName!.Trim().ToUpper() == newPromoAppDisplay.AppName!.Trim().ToUpper());
            if (isExist)
                return new Tuple<PromotionAppDisplay, string>(new PromotionAppDisplay(), string.Format("App Name {0} is already exist", newPromoAppDisplay.AppName));

            //simpan original value accessKey dan secretKey
            string oriAKey = newPromoAppDisplay.AccessKey!;
            string oriSKey = newPromoAppDisplay.SecretKey!;

            //accessKey dan secretKey di encrypt
            string aKey = Helper.fEncrypt(newPromoAppDisplay.AccessKey! ?? "", "", DateTime.Parse("1900/01/01")).ToString();
            string sKey = Helper.fEncrypt(newPromoAppDisplay.SecretKey! ?? "", "", DateTime.Parse("1900/01/01")).ToString();

            newPromoAppDisplay.AccessKey = aKey;
            newPromoAppDisplay.SecretKey = sKey;
            newPromoAppDisplay.Id = Guid.NewGuid();
            newPromoAppDisplay.CreatedDate = DateTime.UtcNow;
            newPromoAppDisplay.CreatedBy = newPromoAppDisplay.Username;
            newPromoAppDisplay.UpdatedDate = DateTime.UtcNow;
            newPromoAppDisplay.UpdatedBy = newPromoAppDisplay.Username;
            newPromoAppDisplay.ActiveFlag = true;


            // Add entry to the DB context.
            newPromoAppDisplay.AppCode = GeneratePromoAppCode();
            _dbContext.PromotionAppDisplay.Add(newPromoAppDisplay);
            await _dbContext.SaveChangesAsync();

            //set ulang original value untuk dikembalikan ke response
            newPromoAppDisplay.AccessKey = oriAKey;
            newPromoAppDisplay.SecretKey = oriSKey;

            return new Tuple<PromotionAppDisplay, string>(newPromoAppDisplay, "Data has been saved");
        }
        #endregion

        #region UpdateData
        public async Task<Tuple<PromotionAppDisplay, string>> UpdatePromotionAppDisplay(PromotionAppDisplay promoAppDisplayEdit)
        {
            var updatedPromoAppDisplay = await GetPromotionAppDisplayByCodeAsync(promoAppDisplayEdit.AppCode!);

            if (updatedPromoAppDisplay == null || updatedPromoAppDisplay.Id == Guid.Empty)
                return new Tuple<PromotionAppDisplay, string>(new PromotionAppDisplay(), string.Format("App code {0} is not found", promoAppDisplayEdit.AppCode!));

            updatedPromoAppDisplay.BucketName = promoAppDisplayEdit.BucketName;
            updatedPromoAppDisplay.Region = promoAppDisplayEdit.Region;
            updatedPromoAppDisplay.BaseDirectory = promoAppDisplayEdit.BaseDirectory;

            //simpan original value accessKey dan secretKey
            string oriAKey = promoAppDisplayEdit.AccessKey!;
            string oriSKey = promoAppDisplayEdit.SecretKey!;

            //accessKey dan secretKey di encrypt
            string aKey = Helper.fEncrypt(promoAppDisplayEdit.AccessKey! ?? "", "", DateTime.Parse("1900/01/01")).ToString();
            string sKey = Helper.fEncrypt(promoAppDisplayEdit.SecretKey! ?? "", "", DateTime.Parse("1900/01/01")).ToString();

            updatedPromoAppDisplay.AccessKey = aKey;
            updatedPromoAppDisplay.SecretKey = sKey;

            updatedPromoAppDisplay.UpdatedDate = DateTime.UtcNow;
            updatedPromoAppDisplay.UpdatedBy = promoAppDisplayEdit.Username;

            await _dbContext.SaveChangesAsync();

            //set ulang original value untuk dikembalikan ke response
            updatedPromoAppDisplay.AccessKey = oriAKey;
            updatedPromoAppDisplay.SecretKey = oriSKey;

            return new Tuple<PromotionAppDisplay, string>(updatedPromoAppDisplay, "");
        }
        #endregion

        #region GenerateAppCode
        public string GeneratePromoAppCode()
        {
            int runningNumber = 1;
            PromotionAppDisplay lastAppCode = _dbContext.PromotionAppDisplay.OrderByDescending(x => x.AppCode).FirstOrDefault() ?? new PromotionAppDisplay();
            if (lastAppCode.Id != Guid.Empty && !string.IsNullOrEmpty(lastAppCode.AppCode) && int.TryParse(lastAppCode.AppCode!.AsSpan(3, 5), out runningNumber))
            {
                runningNumber++;
            }

            return String.Format("{0}{1:00000}", "PRD", runningNumber);
        }
        #endregion
    }
}
