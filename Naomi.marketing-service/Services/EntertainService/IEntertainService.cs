using Naomi.marketing_service.Models.Entities;
using Naomi.marketing_service.Models.Request;
using Naomi.marketing_service.Models.Response;
using static Naomi.marketing_service.Models.Request.EntertainBudgetRequest;
using static Naomi.marketing_service.Models.Response.EntertainBudgetResponse;

namespace Naomi.marketing_service.Services.EntertainService
{
    public interface IEntertainService
    {
        Task<Tuple<List<NipEntertainResponse>, int, string>> GetNipEntertain(DateTime? monthYear, string? searchName, int pageNo = 1, int pageSize = 10);
        Task<Tuple<List<PromoEntertainListView>, int, string>> GetPromoEntertainListAsync(string orderColumn, string orderMethod, int pageNo = 1, int pageSize = 10, PromotionsViewSearch? viewSearch = null);
        Task<Tuple<PromotionEntertain, string>> GetPromoEntertainByNIP(string? empNIP, DateTime? monthYear);
        Task<Tuple<PromotionEntertain, string>> CreateEntertain(CreateEntertain promotionEntertain);
        Task<Tuple<PromotionEntertain, string>> UpdateEntertain(UpdateEntertain updateEntertain);
        List<PromotionEntertainEmail> SetPromoEntertainEmail(List<EmpEmail> empEmails, string? username);

        #region EntertainJob
        Task CreateEntertainBudgetAuto();
        #endregion
    }
}
