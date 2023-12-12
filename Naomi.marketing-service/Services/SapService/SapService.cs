using Microsoft.EntityFrameworkCore;
using Naomi.marketing_service.Models.Contexts;
using Naomi.marketing_service.Models.Entities;
using Naomi.marketing_service.Models.Message.Consume;
using Newtonsoft.Json;
using static System.Net.Mime.MediaTypeNames;
using System.Text;
using Naomi.marketing_service.Configurations;
using Microsoft.Extensions.Options;

namespace Naomi.marketing_service.Services.SapService
{
    public class SapService : ISapService
    {
        private readonly DataDbContext _dbContext;
        private readonly IOptions<AppConfig> _appConfig;

        public SapService(DataDbContext dbContext, IOptions<AppConfig> appConfig)
        {
            _dbContext = dbContext;
            _appConfig = appConfig;
        }

        #region GetData
        public async Task<MopViewModel> GetMopByIdAsync(Guid id)
        {
            return await _dbContext.MopViewModel.FirstOrDefaultAsync(x => x.Id == id) ?? new MopViewModel();
        }

        public async Task<Tuple<List<CompanyViewModel>, int>> GetCompanyViewModel(string? searchName, int pageNo, int pageSize)
        {
            List<CompanyViewModel> companies = new();
            int Skip = (pageNo - 1) * pageSize;

            if (!string.IsNullOrEmpty(searchName))
            {
                companies = await _dbContext.CompanyViewModel.Where(x => x.ActiveFlag && (x.CompanyCode!.Trim().ToUpper().Contains(searchName.Trim().ToUpper()) || x.CompanyDescription!.Trim().ToUpper().Contains(searchName.Trim().ToUpper()))).ToListAsync() ?? new List<CompanyViewModel>();
            }
            else
            {
                companies = await _dbContext.CompanyViewModel.Where(x => x.ActiveFlag).ToListAsync() ?? new List<CompanyViewModel>();
            }

            int totalPages = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(companies.Count) / Convert.ToDouble(pageSize)));
            companies = companies.OrderBy(x => x.CompanyCode).Skip(Skip).Take(pageSize).ToList();
            return new Tuple<List<CompanyViewModel>, int>(companies ?? new List<CompanyViewModel>(), totalPages);
        }

        public async Task<Tuple<List<SiteViewModel>, int, string>> GetSiteViewModel(string? companyCode, List<string>? zoneList, string? searchName, int pageNo, int pageSize)
        {
            if (string.IsNullOrEmpty(companyCode) || companyCode.Trim().ToLower() == "string")
                return new Tuple<List<SiteViewModel>, int, string>(new List<SiteViewModel>(), 1, "Company code is required");
            if (zoneList == null || zoneList.Count == 0)
                return new Tuple<List<SiteViewModel>, int, string>(new List<SiteViewModel>(), 1, "Zone is required");

            List<SiteViewModel> sites = new();
            int Skip = (pageNo - 1) * pageSize;

            if (!string.IsNullOrEmpty(searchName) && searchName.Trim().ToLower() != "string")
            {
                sites = await _dbContext.SiteViewModel.Where(x => x.ActiveFlag && x.CompanyCode!.Trim().ToUpper() == companyCode.Trim().ToUpper() && (x.SiteCode!.Trim().ToUpper().Contains(searchName.Trim().ToUpper()) || x.SiteDescription!.Trim().ToUpper().Contains(searchName.Trim().ToUpper()))).ToListAsync() ?? new List<SiteViewModel>();
            }
            else
            {
                sites = await _dbContext.SiteViewModel.Where(x => x.ActiveFlag && x.CompanyCode!.Trim().ToUpper() == companyCode.Trim().ToUpper()).ToListAsync() ?? new List<SiteViewModel>();
            }

            if (zoneList.Count == 1)
            {
                string[] arrZone = zoneList[0].Split(",");
                zoneList.Clear();
                zoneList.AddRange(arrZone);
            }
            zoneList = zoneList.ConvertAll(x => x.Trim().ToUpper());

            sites = sites.Where(x => zoneList.Exists(y => y == x.ZoneCode!.Trim().ToUpper())).ToList();

            int totalPages = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(sites.Count) / Convert.ToDouble(pageSize)));
            sites = sites.OrderBy(x => x.SiteCode).Skip(Skip).Take(pageSize).ToList();
            return new Tuple<List<SiteViewModel>, int, string>(sites ?? new List<SiteViewModel>(), totalPages, "");
        }

        public async Task<Tuple<List<ZoneViewModel>, int, string>> GetZoneViewModel(string? companyCode, string? searchName, int pageNo, int pageSize)
        {
            if (string.IsNullOrEmpty(companyCode) || companyCode.Trim().ToLower() == "string")
                return new Tuple<List<ZoneViewModel>, int, string>(new List<ZoneViewModel>(), 1, "Company code is required");

            int Skip = (pageNo - 1) * pageSize;

            List<ZoneViewModel> zones = await _dbContext.ZoneViewModel.Where(x => x.ActiveFlag && x.CompanyCode!.Trim().ToUpper() == companyCode.Trim().ToUpper()).ToListAsync();
            if (!string.IsNullOrEmpty(searchName) && searchName.Trim().ToLower() != "string")
            {
                zones = zones.Where(x => x.ZoneCode!.Trim().ToUpper().Contains(searchName.Trim().ToUpper()) || x.ZoneName!.Trim().ToUpper().Contains(searchName.Trim().ToUpper())).ToList();
            }

            int totalPages = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(zones.Count) / Convert.ToDouble(pageSize)));
            zones = zones.OrderBy(x => x.ZoneCode).Skip(Skip).Take(pageSize).ToList();
            return new Tuple<List<ZoneViewModel>, int, string>(zones ?? new List<ZoneViewModel>(), totalPages, "");
        }

        public async Task<Tuple<List<MopViewModel>, int, string>> GetMopViewModel(string? companyCode, string? siteCode, string? searchName, bool isPromotion, int pageNo, int pageSize)
        {
            if (string.IsNullOrEmpty(companyCode) || companyCode.Trim().ToLower() == "string")
                return new Tuple<List<MopViewModel>, int, string>(new List<MopViewModel>(), 1, "Company code is required");

            int Skip = (pageNo - 1) * pageSize;
            List<MopViewModel> mops = await _dbContext.MopViewModel.Where(x => x.ActiveFlag && x.IsPromotion == isPromotion && x.CompanyCode!.Trim().ToUpper() == companyCode.Trim().ToUpper()).ToListAsync() ?? new List<MopViewModel>();
            if (!string.IsNullOrEmpty(siteCode) && siteCode.Trim().ToLower() != "string")
            {
                mops = mops.Where(x => x.SiteCode!.Trim().ToUpper() == siteCode.Trim().ToUpper()).ToList();
            }
            if (!string.IsNullOrEmpty(searchName) && searchName.Trim().ToLower() != "string")
            {
                mops = mops.Where(x => x.MopCode!.Trim().ToUpper().Contains(searchName.Trim().ToUpper()) || x.MopName!.Trim().ToUpper().Contains(searchName.Trim().ToUpper())).ToList();
            }

            int totalPages = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(mops.Count) / Convert.ToDouble(pageSize)));
            mops = mops.OrderBy(x => x.MopCode).Skip(Skip).Take(pageSize).ToList();
            return new Tuple<List<MopViewModel>, int, string>(mops ?? new List<MopViewModel>(), totalPages, "");
        }
        #endregion


        #region SetIsPromotion
        public async Task<Tuple<MopViewModel, string>> SetMopIsPromotion(Guid mopId, bool isPromotion)
        {
            if (mopId == Guid.Empty)
                return new Tuple<MopViewModel, string>(new MopViewModel(), "Mop Id is required");
            
            MopViewModel updatedMop = await GetMopByIdAsync(mopId);
            if (updatedMop == null || updatedMop.Id == Guid.Empty)
                return new Tuple<MopViewModel, string>(new MopViewModel(), string.Format("Mop Id {0} is not found", mopId));

            updatedMop.IsPromotion = isPromotion;
            updatedMop.UpdatedDate = DateTime.UtcNow;

            _dbContext.MopViewModel.Update(updatedMop);
            await _dbContext.SaveChangesAsync();

            return new Tuple<MopViewModel, string>(updatedMop, "Data has been updated");
        }
        #endregion


        #region GetInitialSapData
        public async Task<bool> GetCompany()
        {
            List<CompanySapApiResponse> responseData = await GetCompanyList();
            if (responseData.Count < 1)
                return false;

            List<CompanyViewModel> listCompanyViewModel = new();
            foreach (var item in responseData)
            {
                if (item.companyCode != null && item.companyCode != "" &&
                    item.companyDescription != null && item.companyDescription != "")
                {
                    CompanyViewModel? company = _dbContext.CompanyViewModel.FirstOrDefault(x => x.CompanyCode!.Trim().ToUpper() == item.companyCode!.Trim().ToUpper());

                    if (company == null)
                    {
                        company = new()
                        {
                            Id = Guid.Parse(item.id!),
                            CompanyCode = item.companyCode.Trim().ToUpper(),
                            CompanyDescription = item.companyDescription,
                            CreatedBy = "System Initial",
                            CreatedDate = DateTime.UtcNow,
                            UpdatedBy = "System Initial",
                            UpdatedDate = DateTime.UtcNow,
                            ActiveFlag = true
                        };

                        listCompanyViewModel.Add(company);
                    }
                }
            }
            if (listCompanyViewModel.Count > 0)
                _dbContext.CompanyViewModel.AddRange(listCompanyViewModel);

            await _dbContext.SaveChangesAsync();

            return true;
        }
        public async Task<List<CompanySapApiResponse>> GetCompanyList()
        {
            List<CompanySapApiResponse> response = new();

            using HttpClient httpClient = new();
            using HttpResponseMessage responseHttp = await httpClient.PostAsync(_appConfig.Value.UrlInitialCompanyWorkPlaceService, null);


            if (responseHttp.StatusCode != System.Net.HttpStatusCode.OK)
                return response;

            string apiResponse = await responseHttp.Content.ReadAsStringAsync();

            if (apiResponse == null)
                return response;

            response = JsonConvert.DeserializeObject<List<CompanySapApiResponse>>(apiResponse)!;
            if (response == null)
                return new List<CompanySapApiResponse>();

            return response;
        }
        public async Task<bool> GetSiteZoneSAP()
        {
            List<CompanySapApiResponse> listCompanyApiResponse = await GetCompanyList();

            if (listCompanyApiResponse.Count < 1)
                return false;

            foreach (var loopCompany in listCompanyApiResponse.Select(x => x.companyCode))
            {
                if (string.IsNullOrEmpty(loopCompany))
                    continue;

                Dictionary<string, object> paramBodyJson = new()
                {
                    {"code", loopCompany},
                };

                using HttpClient httpClient = new();
                StringContent bodyJson = new(JsonConvert.SerializeObject(paramBodyJson), Encoding.UTF8, Application.Json);
                HttpResponseMessage httpResponseMessage = await httpClient.PostAsync(_appConfig.Value.UrlInitialSiteZoneSapAdapter, bodyJson);

                string responseContent = await httpResponseMessage.Content.ReadAsStringAsync();
                List<SiteZoneSapApiResponse>? listDataSiteSap = JsonConvert.DeserializeObject<List<SiteZoneSapApiResponse>>(responseContent);

                if (listDataSiteSap == null || listDataSiteSap.Count < 1)
                    continue;

                List<ZoneViewModel> listZoneViewModel = new();
                List<SiteViewModel> listSiteViewModel = new();


                List<SiteZoneSapApiResponse>? listDistinctSiteSap = listDataSiteSap.DistinctBy(x => new { x.pricingZone, x.siteCode }).ToList();
                foreach (var loopSite in listDistinctSiteSap)
                {
                    if (!loopSite.activeFlag)
                        continue;

                    if (!string.IsNullOrEmpty(loopSite.pricingZone) && !string.IsNullOrEmpty(loopSite.siteCode))
                    {
                        SiteViewModel? newSite = _dbContext.SiteViewModel.FirstOrDefault(x => x.CompanyCode!.Trim().ToUpper() == loopCompany!.Trim().ToUpper() && x.ZoneCode!.Trim().ToUpper() == loopSite.pricingZone!.Trim().ToUpper() && x.SiteCode!.Trim().ToUpper() == loopSite.siteCode!.Trim().ToUpper());

                        if (newSite == null)
                        {
                            newSite = new()
                            {
                                Id = Guid.NewGuid(),
                                CompanyCode = loopCompany.Trim().ToUpper(),
                                ZoneCode = loopSite.pricingZone.Trim().ToUpper(),
                                SiteCode = loopSite.siteCode.Trim().ToUpper(),
                                SiteDescription = loopSite.siteDescription,
                                CreatedBy = "System Initial",
                                CreatedDate = DateTime.UtcNow,
                                UpdatedBy = "System Initial",
                                UpdatedDate = DateTime.UtcNow,
                                ActiveFlag = true
                            };

                            listSiteViewModel.Add(newSite);
                        }
                    }
                }


                List<SiteZoneSapApiResponse>? listDistinctZoneSap = listDataSiteSap.DistinctBy(x => x.pricingZone).ToList();
                foreach (var loopZone in listDistinctZoneSap.Select(x => x.pricingZone))
                {
                    if (!string.IsNullOrEmpty(loopZone))
                    {
                        ZoneViewModel? newZone = _dbContext.ZoneViewModel.FirstOrDefault(x => x.CompanyCode!.Trim().ToUpper() == loopCompany!.Trim().ToUpper() && x.ZoneCode!.Trim().ToUpper() == loopZone!.Trim().ToUpper());

                        if (newZone == null)
                        {
                            newZone = new()
                            {
                                Id = Guid.NewGuid(),
                                CompanyCode = loopCompany.Trim().ToUpper(),
                                ZoneCode = loopZone.Trim().ToUpper(),
                                ZoneName = loopZone,
                                CreatedBy = "System Initial",
                                CreatedDate = DateTime.UtcNow,
                                UpdatedBy = "System Initial",
                                UpdatedDate = DateTime.UtcNow,
                                ActiveFlag = true
                            };

                            listZoneViewModel.Add(newZone);
                        }
                    }
                }

                if (listZoneViewModel.Count > 0)
                    _dbContext.ZoneViewModel.AddRange(listZoneViewModel);

                if (listSiteViewModel.Count > 0)
                    _dbContext.SiteViewModel.AddRange(listSiteViewModel);
                
                await _dbContext.SaveChangesAsync();
            }

            return true;
        }
        public async Task<bool> GetMop()
        {
            using HttpClient httpClient = new();
            using HttpResponseMessage response = await httpClient.GetAsync(_appConfig.Value.UrlInitialMopPaymentService);

            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                return false;

            string apiResponse = await response.Content.ReadAsStringAsync();

            if (apiResponse == null)
                return false;

            MopPaymentApiResponse mopResponse = JsonConvert.DeserializeObject<MopPaymentApiResponse>(apiResponse)!;
            List<DataMopResponse> listDataMopResponse = mopResponse.Data!.Where(q => q.status).ToList();

            if (listDataMopResponse.Count < 1)
                return false;

            List<MopViewModel> listMopViewModel = new();
            List<CompanySapApiResponse> companyList = await GetCompanyList();

            if (companyList.Count < 1)
                return false;

            foreach (var loopDataMopResponse in listDataMopResponse)
            {
                string? companyCode = companyList.Find(q => q.companyCode == loopDataMopResponse.salesOrganizationCode)!.companyCode;
                MopViewModel? mopViewModel = _dbContext.MopViewModel.FirstOrDefault(x => x.MopCode!.Trim().ToUpper() == loopDataMopResponse.mopCode!.Trim().ToUpper() && x.SiteCode == loopDataMopResponse.siteCode
                                             && x.CompanyCode == companyCode);

                if (mopViewModel == null)
                {
                    mopViewModel = new()
                    {
                        Id = Guid.Parse(loopDataMopResponse.Id!),
                        CompanyCode = companyCode!.Trim().ToUpper(),
                        SiteCode = loopDataMopResponse.siteCode!.Trim().ToUpper(),
                        MopCode = loopDataMopResponse.mopCode!.Trim().ToUpper(),
                        MopName = loopDataMopResponse.mopName,
                        CreatedDate = DateTime.UtcNow,
                        CreatedBy = "System Initial",
                        UpdatedDate = DateTime.UtcNow,
                        UpdatedBy = "System Initial",
                        ActiveFlag = true,
                    };

                    listMopViewModel.Add(mopViewModel);
                }
            }

            if (listMopViewModel.Count > 0)
                _dbContext.MopViewModel.AddRange(listMopViewModel);
            await _dbContext.SaveChangesAsync();

            return true;
        }
        #endregion

        #region ConsumeSapData
        public async Task InsertCompany(SiteMessage msg)
        {
            CompanyViewModel newCompany = new();

            if (!string.IsNullOrEmpty(msg.company_code))
            {
                //check if exist
                newCompany = _dbContext.CompanyViewModel.Where(x => x.CompanyCode!.Trim().ToUpper() == msg.company_code.Trim().ToUpper()).FirstOrDefault() ?? new CompanyViewModel();

                newCompany.CompanyCode = msg.company_code.Trim().ToUpper();
                newCompany.CompanyDescription = msg.company_description;
                newCompany.UpdatedDate = DateTime.UtcNow;
                newCompany.UpdatedBy = "Consumer Sap Data";

                if (newCompany == null || newCompany.Id == Guid.Empty)
                {
                    newCompany!.Id = Guid.NewGuid();
                    newCompany.ActiveFlag = true;
                    newCompany.CreatedDate = DateTime.UtcNow;
                    newCompany.CreatedBy = "Consumer Sap Data";
                    _dbContext.CompanyViewModel.Add(newCompany);
                }

                await _dbContext.SaveChangesAsync();
            }
        }
        public async Task InsertSite(SiteMessage msg)
        {
            SiteViewModel newSite = new();

            if (!string.IsNullOrEmpty(msg.site_code) && !string.IsNullOrEmpty(msg.pricing_zone) && !string.IsNullOrEmpty(msg.company_code))
            {
                //check if exist
                newSite = _dbContext.SiteViewModel.Where(x => x.CompanyCode!.Trim().ToUpper() == msg.company_code!.Trim().ToUpper() &&
                                                              x.ZoneCode!.Trim().ToUpper() == msg.pricing_zone!.Trim().ToUpper() &&
                                                              x.SiteCode!.Trim().ToUpper() == msg.site_code!.Trim().ToUpper()).FirstOrDefault() ?? new SiteViewModel();

                newSite.CompanyCode = msg.company_code!.Trim().ToUpper();
                newSite.ZoneCode = msg.pricing_zone!.Trim().ToUpper();
                newSite.SiteCode = msg.site_code!.Trim().ToUpper();
                newSite.SiteDescription = msg.site_description;
                newSite.UpdatedDate = DateTime.UtcNow;
                newSite.UpdatedBy = "Consumer Sap Data";

                if (newSite == null || newSite.Id == Guid.Empty)
                {
                    newSite!.Id = Guid.NewGuid();
                    newSite.ActiveFlag = true;
                    newSite.CreatedDate = DateTime.UtcNow;
                    newSite.CreatedBy = "Consumer Sap Data";
                    _dbContext.SiteViewModel.Add(newSite);
                }

                await _dbContext.SaveChangesAsync();
            }
        }
        public async Task InsertZone(SiteMessage msg)
        {
            ZoneViewModel newZone = new();

            if (!string.IsNullOrEmpty(msg.pricing_zone) && !string.IsNullOrEmpty(msg.company_code))
            {
                //check if exist
                newZone = _dbContext.ZoneViewModel.Where(x => x.CompanyCode!.Trim().ToUpper() == msg.company_code!.Trim().ToUpper() &&
                                                              x.ZoneCode!.Trim().ToUpper() == msg.pricing_zone!.Trim().ToUpper()).FirstOrDefault() ?? new ZoneViewModel();

                newZone.CompanyCode = msg.company_code!.Trim().ToUpper();
                newZone.ZoneCode = msg.pricing_zone!.Trim().ToUpper();
                newZone.ZoneName = msg.pricing_zone!.Trim().ToUpper();
                newZone.UpdatedDate = DateTime.UtcNow;
                newZone.UpdatedBy = "Consumer Sap Data";

                if (newZone == null || newZone.Id == Guid.Empty)
                {
                    newZone!.Id = Guid.NewGuid();
                    newZone.ActiveFlag = true;
                    newZone.CreatedDate = DateTime.UtcNow;
                    newZone.CreatedBy = "Consumer Sap Data";
                    _dbContext.ZoneViewModel.Add(newZone);
                }

                await _dbContext.SaveChangesAsync();
            }
        }
        public async Task InsertMop(MopMessage msg)
        {
            MopViewModel newMop = new();

            if (!string.IsNullOrEmpty(msg.mop_code) && !string.IsNullOrEmpty(msg.site_code) && !string.IsNullOrEmpty(msg.sales_organization_code))
            {
                //check if exist
                newMop = _dbContext.MopViewModel.Where(x => x.CompanyCode!.Trim().ToUpper() == msg.sales_organization_code!.Trim().ToUpper() &&
                                                            x.SiteCode!.Trim().ToUpper() == msg.site_code!.Trim().ToUpper() &&
                                                            x.MopCode!.Trim().ToUpper() == msg.mop_code!.Trim().ToUpper()).FirstOrDefault() ?? new MopViewModel();

                newMop.CompanyCode = msg.sales_organization_code!.Trim().ToUpper();
                newMop.SiteCode = msg.site_code!.Trim().ToUpper();
                newMop.MopCode = msg.mop_code!.Trim().ToUpper();
                newMop.MopName = msg.mop_name;
                newMop.UpdatedDate = DateTime.UtcNow;
                newMop.UpdatedBy = "Consumer Sap Data";

                if (newMop == null || newMop.Id == Guid.Empty)
                {
                    newMop!.Id = Guid.NewGuid();
                    newMop.ActiveFlag = true;
                    newMop.CreatedDate = DateTime.UtcNow;
                    newMop.CreatedBy = "Consumer Sap Data";
                    _dbContext.MopViewModel.Add(newMop);
                }

                await _dbContext.SaveChangesAsync();
            }
        }
        #endregion
    }
}
