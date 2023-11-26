using Microsoft.EntityFrameworkCore;
using Naomi.marketing_service.Models.Contexts;
using Naomi.marketing_service.Models.Entities;
using Naomi.marketing_service.Services.PromoClassService;
using Naomi.marketing_service.Services.PubService;
using static Naomi.marketing_service.Models.Request.PromotionTypeRequest;

namespace Naomi.marketing_service.Services.PromoTypeService
{
    public class PromoTypeService : IPromoTypeService
    {
        private readonly DataDbContext _dbContext;
        private readonly IPubService _pubService;
        private readonly IPromoClassService _promoClassService;
        
        public PromoTypeService(DataDbContext dbContext, IPubService pubService, IPromoClassService promoClassService)
        {
            _dbContext = dbContext;
            _pubService = pubService;
            _promoClassService = promoClassService;
        }

        #region GetData
        public async Task<List<PromotionType>> GetPromotionType(Guid id)
        {
            List<PromotionType> promotions = new();

            if (id == Guid.Empty)
            {
                promotions = await _dbContext.PromotionType.ToListAsync() ?? new List<PromotionType>();
            }
            else
            {
                promotions = await _dbContext.PromotionType.Where(x => x.Id == id).ToListAsync() ?? new List<PromotionType>();
            }

            return promotions;
        }
        public async Task<Tuple<PromotionType, string>> GetPromotionTypeByIdAsync(Guid? id)
        {
            if (id == null || id == Guid.Empty)
                return new Tuple<PromotionType, string>(new PromotionType(), "Promotion Type id is required");

            PromotionType promoType = await _dbContext.PromotionType.FirstOrDefaultAsync(x => x.Id == id) ?? new PromotionType();
            return new Tuple<PromotionType, string>(promoType, "");
        }
        public async Task<Tuple<List<PromotionType>, string>> GetPromotionTypeByClassIdAsync(Guid classId)
        {
            if (classId == Guid.Empty)
                return new Tuple<List<PromotionType>, string>(new List<PromotionType>(), "Promotion Class id is required");

            List<PromotionType> promoTypeList = await _dbContext.PromotionType.Where(x => x.PromotionClassId == classId).ToListAsync() ?? new List<PromotionType>();
            return new Tuple<List<PromotionType>, string>(promoTypeList, "");
        }
        #endregion

        #region InsertData
        public async Task<Tuple<PromotionType, string>> InsertPromotionType(PromotionType newPromoType)
        {
            //validate PromotionClass
            var responsePromoClass = await _promoClassService.GetPromotionClassByIdAsync(newPromoType.PromotionClassId);
            if (responsePromoClass.Item1 == null || responsePromoClass.Item1.Id == Guid.Empty)
                return new Tuple<PromotionType, string>(new PromotionType(), string.Format("Promotion Class id {0} is not found", newPromoType.PromotionClassId));

            bool isExist = _dbContext.PromotionType.Any(x => x.PromotionTypeName!.Trim().ToUpper() == newPromoType.PromotionTypeName!.Trim().ToUpper());
            if (isExist)
                return new Tuple<PromotionType, string>(new PromotionType(), string.Format("Promotion Type name {0} is already exist", newPromoType.PromotionTypeName));

            try
            {
                if (newPromoType.PromotionTypeKey!.Trim().ToUpper() == "ITEM" || newPromoType.PromotionTypeKey!.Trim().ToUpper() == "SPECIAL PRICE" || newPromoType.PromotionTypeKey!.Trim().ToUpper() == "SP")
                    newPromoType.LineNum = 1;
                else
                    newPromoType.LineNum = 2;

                newPromoType.CreatedDate = DateTime.UtcNow;
                newPromoType.CreatedBy = newPromoType.Username;
                newPromoType.UpdatedDate = DateTime.UtcNow;
                newPromoType.UpdatedBy = newPromoType.Username;
                newPromoType.ActiveFlag = true;

                // Add entry to the DB context.
                _dbContext.PromotionType.Add(newPromoType);
                await _dbContext.SaveChangesAsync();

                // Send message topic to the message stream.
                _pubService.SendPromoTypeMessage(newPromoType, "Create");

                return new Tuple<PromotionType, string>(newPromoType, "Data has been saved");
            }
            catch (Exception ex)
            {
                return new Tuple<PromotionType, string>(new PromotionType(), ex.Message);
            }
        }
        #endregion

        #region UpdateData
        public async Task<Tuple<PromotionType, string>> UpdatePromotionType(PromotionType updatedPromoType)
        {
            //validate PromotionClass
            var responsePromoClass = await _promoClassService.GetPromotionClassByIdAsync(updatedPromoType.PromotionClassId!);
            if (responsePromoClass.Item1 == null || responsePromoClass.Item1.Id == Guid.Empty)
                return new Tuple<PromotionType, string>(new PromotionType(), string.Format("Promotion Class id {0} is not found", updatedPromoType.Id));

            //validate PromotionType
            var responsePromoType = await GetPromotionTypeByIdAsync(updatedPromoType.Id);
            if (responsePromoType.Item1 == null || responsePromoType.Item1.Id == Guid.Empty)
                return new Tuple<PromotionType, string>(new PromotionType(), string.Format("Promotion Type id {0} is not found", updatedPromoType.Id));

            bool isExist = _dbContext.PromotionType.Any(x => x.PromotionTypeName!.Trim().ToUpper() == updatedPromoType.PromotionTypeName!.Trim().ToUpper() && x.Id != updatedPromoType.Id);
            if (isExist)
                return new Tuple<PromotionType, string>(new PromotionType(), string.Format("Promotion Type name {0} is already exist", updatedPromoType.PromotionTypeName));

            PromotionType updatedPromotionType = responsePromoType.Item1;
            try
            {
                updatedPromotionType.PromotionClassId = updatedPromoType.PromotionClassId;
                updatedPromotionType.PromotionTypeKey = updatedPromoType.PromotionTypeKey;
                updatedPromotionType.PromotionTypeName = updatedPromoType.PromotionTypeName;
                updatedPromotionType.UpdatedDate = DateTime.UtcNow;
                updatedPromotionType.UpdatedBy = updatedPromoType.Username;

                if (updatedPromoType.PromotionTypeKey!.Trim().ToUpper() == "ITEM" || updatedPromoType.PromotionTypeKey!.Trim().ToUpper() == "SPECIAL PRICE" || updatedPromoType.PromotionTypeKey!.Trim().ToUpper() == "SP")
                    updatedPromoType.LineNum = 1;
                else
                    updatedPromoType.LineNum = 2;

                await _dbContext.SaveChangesAsync();

                // Send message topic to the message stream.
                _pubService.SendPromoTypeMessage(updatedPromoType, "Update");
            }
            catch (Exception ex)
            {
                return new Tuple<PromotionType, string>(new PromotionType(), ex.Message);
            }

            return new Tuple<PromotionType, string>(updatedPromoType, "data has been saved");
        }
        #endregion
    }
}
