using Microsoft.EntityFrameworkCore;
using Naomi.marketing_service.Models.Contexts;
using Naomi.marketing_service.Models.Entities;
using static Naomi.marketing_service.Models.Request.ChannelMaterialRequest;

namespace Naomi.marketing_service.Services.PromoMaterialService
{
    public class PromoMaterialService : IPromoMaterialService
    {
        private readonly DataDbContext _dbContext;
        public PromoMaterialService(DataDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        #region GetData
        public async Task<Tuple<List<PromotionMaterial>, int>> GetPromotionMaterial(string? searchName, int pageNo, int pageSize)
        {
            List<PromotionMaterial> promoMaterials = new();
            int Skip = (pageNo - 1) * pageSize;

            if (!string.IsNullOrEmpty(searchName))
            {
                promoMaterials = await _dbContext.PromotionMaterial.Where(x => x.MaterialName!.Trim().ToUpper().Contains(searchName.Trim().ToUpper())).ToListAsync() ?? new List<PromotionMaterial>();
            }
            else
            {
                promoMaterials = await _dbContext.PromotionMaterial.ToListAsync() ?? new List<PromotionMaterial>();
            }

            int totalPages = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(promoMaterials.Count) / Convert.ToDouble(pageSize)));
            promoMaterials = promoMaterials.OrderBy(x => x.MaterialName).Skip(Skip).Take(pageSize).ToList();
            return new Tuple<List<PromotionMaterial>, int>(promoMaterials ?? new List<PromotionMaterial>(), totalPages);
        }
        #endregion

        #region InsertData
        public async Task<Tuple<PromotionMaterial, string>> InsertPromotionMaterial(PromotionMaterial newPromoMaterial)
        {
            bool isExist = _dbContext.PromotionMaterial.Any(x => x.MaterialName!.Trim().ToUpper() == newPromoMaterial.MaterialName!.Trim().ToUpper());
            if (isExist)
                return new Tuple<PromotionMaterial, string>(new PromotionMaterial(), string.Format("Promotion Material name {0} is already exist", newPromoMaterial.MaterialName));

            try
            {
                newPromoMaterial.CreatedDate = DateTime.UtcNow;
                newPromoMaterial.CreatedBy = newPromoMaterial.Username;
                newPromoMaterial.UpdatedDate = DateTime.UtcNow;
                newPromoMaterial.UpdatedBy = newPromoMaterial.Username;
                newPromoMaterial.ActiveFlag = true;

                // Add entry to the DB context
                _dbContext.PromotionMaterial.Add(newPromoMaterial);
                await _dbContext.SaveChangesAsync();

                return new Tuple<PromotionMaterial, string>(newPromoMaterial, "Data has been saved");
            }
            catch (Exception ex)
            {
                return new Tuple<PromotionMaterial, string>(new PromotionMaterial(), ex.Message);
            }
        }
        #endregion
    }
}
