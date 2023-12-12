using Naomi.marketing_service.Models.Entities;
using Naomi.marketing_service.Models.Message.Consume;
using Naomi.marketing_service.Models.Request;

namespace Naomi.marketing_service.Services.SapService
{
    public interface ISapService
    {
        Task<Tuple<List<CompanyViewModel>, int>> GetCompanyViewModel(string? searchName, int pageNo, int pageSize);
        Task<Tuple<List<SiteViewModel>, int, string>> GetSiteViewModel(string? companyCode, List<string>? zoneList, string? searchName, int pageNo, int pageSize);
        Task<Tuple<List<ZoneViewModel>, int, string>> GetZoneViewModel(string? companyCode, string? searchName, int pageNo, int pageSize);
        Task<Tuple<List<MopViewModel>, int, string>> GetMopViewModel(string? companyCode, string? siteCode, string? searchName, bool isPromotion, int pageNo, int pageSize);
        Task<Tuple<MopViewModel, string>> SetMopIsPromotion(Guid mopId, bool isPromotion);

        #region InitialSapData
        Task<bool> GetCompany();
        Task<bool> GetSiteZoneSAP();
        Task<bool> GetMop();
        #endregion

        #region ConsumeSapData
        Task InsertCompany(SiteMessage msg);
        Task InsertSite(SiteMessage msg);
        Task InsertZone(SiteMessage msg);
        Task InsertMop(MopMessage msg);
        #endregion
    }
}
