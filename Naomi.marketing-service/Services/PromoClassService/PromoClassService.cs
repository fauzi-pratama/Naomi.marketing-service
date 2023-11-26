using Microsoft.EntityFrameworkCore;
using Naomi.marketing_service.Models.Contexts;
using Naomi.marketing_service.Models.Entities;
using Naomi.marketing_service.Services.PubService;
using static Naomi.marketing_service.Models.Request.PromotionClassRequest;

namespace Naomi.marketing_service.Services.PromoClassService
{
    public class PromoClassService : IPromoClassService
    {
        private readonly DataDbContext _dbContext;
        private readonly IPubService _pubService;
        public PromoClassService(DataDbContext dbContext, IPubService pubService)
        {
            _dbContext = dbContext;
            _pubService = pubService;
        }

        #region GetData
        public async Task<List<PromotionClass>> GetPromotionClass(Guid id)
        {
            List<PromotionClass> promotions = new();

            if (id == Guid.Empty)
                promotions = await _dbContext.PromotionClass.ToListAsync() ?? new List<PromotionClass>();
            else
                promotions = await _dbContext.PromotionClass.Where(x => x.Id == id).ToListAsync() ?? new List<PromotionClass>();

            return promotions;
        }
        public async Task<Tuple<PromotionClass, string>> GetPromotionClassByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
                return new Tuple<PromotionClass, string>(new PromotionClass(), "Promotion Class id is required");

            PromotionClass promoClass = await _dbContext.PromotionClass.FirstOrDefaultAsync(x => x.Id == id) ?? new PromotionClass();
            return new Tuple<PromotionClass, string>(promoClass, "");
        }
        #endregion

        #region InsertData
        public async Task<Tuple<PromotionClass, string>> InsertPromotionClass(PromotionClass newPromoClass)
        {
            bool isExist = _dbContext.PromotionClass.Any(x => x.PromotionClassKey!.Trim().ToUpper() == newPromoClass.PromotionClassKey!.Trim().ToUpper());
            if (isExist)
                return new Tuple<PromotionClass, string>(new PromotionClass(), string.Format("Promotion Class key {0} is already exist", newPromoClass.PromotionClassKey));

            try
            {
                if (newPromoClass.PromotionClassKey!.Trim().ToUpper() == "ITEM")
                    newPromoClass.LineNum = 1;
                else if (newPromoClass.PromotionClassKey!.Trim().ToUpper() == "CART")
                    newPromoClass.LineNum = 2;
                else if (newPromoClass.PromotionClassKey!.Trim().ToUpper() == "MOP")
                    newPromoClass.LineNum = 3;
                else
                    newPromoClass.LineNum = 4;

                newPromoClass.CreatedDate = DateTime.UtcNow;
                newPromoClass.CreatedBy = newPromoClass.Username;
                newPromoClass.UpdatedDate = DateTime.UtcNow;
                newPromoClass.UpdatedBy = newPromoClass.Username;
                newPromoClass.ActiveFlag = true;
                
                // Add entry to the DB context.
                _dbContext.PromotionClass.Add(newPromoClass);
                await _dbContext.SaveChangesAsync();

                // Send message topic to the message stream.
                _pubService.SendPromoClassMessage(newPromoClass, "Create");

                return new Tuple<PromotionClass, string>(newPromoClass, "Data has been saved");
            }
            catch (Exception ex)
            {
                return new Tuple<PromotionClass, string>(new PromotionClass(), ex.Message);
            }
        }
        #endregion

        #region UpdateData
        public async Task<Tuple<PromotionClass, string>> UpdatePromotionClass(PromotionClass updatedPromoClass)
        {
            var responsePromoClass = await GetPromotionClassByIdAsync(updatedPromoClass.Id);
            if (responsePromoClass.Item1 == null || responsePromoClass.Item1.Id == Guid.Empty)
                return new Tuple<PromotionClass, string>(new PromotionClass(), string.Format("Promotion Class id {0} is not found", updatedPromoClass.Id));

            bool isExist = _dbContext.PromotionClass.Any(x => x.PromotionClassKey!.Trim().ToUpper() == updatedPromoClass.PromotionClassKey!.Trim().ToUpper() && x.Id != updatedPromoClass.Id);
            if (isExist)
                return new Tuple<PromotionClass, string>(new PromotionClass(), string.Format("Promotion Class key {0} is already exist", updatedPromoClass.PromotionClassKey));

            var updatedPromotionClass = responsePromoClass.Item1;
            try
            {
                updatedPromotionClass.PromotionClassKey = updatedPromoClass.PromotionClassKey;
                updatedPromotionClass.PromotionClassName = updatedPromoClass.PromotionClassName;

                updatedPromotionClass.UpdatedDate = DateTime.UtcNow;
                updatedPromotionClass.UpdatedBy = updatedPromoClass.Username;

                
                if (updatedPromotionClass.PromotionClassKey!.Trim().ToUpper() == "ITEM")
                    updatedPromotionClass.LineNum = 1;
                else if (updatedPromotionClass.PromotionClassKey!.Trim().ToUpper() == "CART")
                    updatedPromotionClass.LineNum = 2;
                else if (updatedPromotionClass.PromotionClassKey!.Trim().ToUpper() == "MOP")
                    updatedPromotionClass.LineNum = 3;
                else
                    updatedPromotionClass.LineNum = 4;

                await _dbContext.SaveChangesAsync();

                // Send message topic to the message stream.
                _pubService.SendPromoClassMessage(updatedPromotionClass, "Update");

                return new Tuple<PromotionClass, string>(updatedPromotionClass, "Data has been saved");
            }
            catch (Exception ex)
            {
                return new Tuple<PromotionClass, string>(new PromotionClass(), ex.Message);
            }
        }
        #endregion
    }
}
