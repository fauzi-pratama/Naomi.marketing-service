using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Naomi.marketing_service.Models.Contexts;
using Naomi.marketing_service.Models.Entities;
using Naomi.marketing_service.Models.Message.Pub;
using Naomi.marketing_service.Models.Request;
using Naomi.marketing_service.Models.Response;
using Naomi.marketing_service.Services.PromoClassService;
using Naomi.marketing_service.Services.PromoStatusService;
using Naomi.marketing_service.Services.PromoTypeService;
using Naomi.marketing_service.Services.PubService;
using Naomi.marketing_service.Services.S3Service;
using Newtonsoft.Json;
using static Naomi.marketing_service.Models.Request.PromotionDetailRequest;
using static Naomi.marketing_service.Models.Response.PromotionHeaderResponse;
using System.Linq.Dynamic.Core;
using Naomi.marketing_service.Services.PromoAppService;
using Naomi.marketing_service.Services.ApprovalService;

namespace Naomi.marketing_service.Services.PromotionService
{
    public class PromotionService : IPromotionService
    {
        private readonly DataDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IPromoStatusService _promoStatusService;
        private readonly IPromoClassService _promoClassService;
        private readonly IPromoTypeService _promoTypeService;
        private readonly IS3Service _s3Service;
        private readonly IPubService _pubService;
        private readonly IApprovalService _approvalService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public PromotionService(DataDbContext dbContext, IMapper mapper, IPromoStatusService promoStatusService, IPromoClassService promoClassService, 
            IPromoTypeService promoTypeService, IS3Service s3Service, IPubService pubService, IHttpContextAccessor httpContextAccessor,
            IApprovalService approvalService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _promoStatusService = promoStatusService;
            _promoClassService = promoClassService;
            _promoTypeService = promoTypeService;
            _s3Service = s3Service;
            _pubService = pubService;
            _httpContextAccessor = httpContextAccessor;
            _approvalService = approvalService;
        }

        #region GetData
        public async Task<Tuple<List<PromotionListView>, int, string>> GetPromotionListAsync(string orderColumn, string orderMethod, int pageNo = 1, int pageSize = 10, PromotionsViewSearch? viewSearch = null)
        {
            DateTime startDate = SetSearchDate(viewSearch, "Start");
            DateTime endDate = SetSearchDate(viewSearch, "End");

            if (startDate != DateTime.Parse("1/1/1900") && endDate != DateTime.Parse("1/1/1900") && endDate < startDate)
                return new Tuple<List<PromotionListView>, int, string>(new List<PromotionListView>(), 1, "End date must be equal or greater than Start date");

            if (string.IsNullOrEmpty(orderColumn)) orderColumn = "CreatedDate";
            if (string.IsNullOrEmpty(orderMethod)) orderMethod = "DESC";

            if (pageNo == 0) pageNo = 1;
            if (pageSize == 0) pageSize = 10;

            int Skip = (pageNo - 1) * pageSize;
            List<PromotionListView> viewTable = new();

            var viewResult2 = await (from pHeader in _dbContext.Set<PromotionHeader>()
                                     from pClass in _dbContext.Set<PromotionClass>().Where(x => x.Id == pHeader.PromotionClassId).DefaultIfEmpty()
                                     from pType in _dbContext.Set<PromotionType>().Where(x => x.Id == pHeader.PromotionTypeId).DefaultIfEmpty()
                                     from pStatus in _dbContext.Set<PromotionStatus>().Where(x => x.Id == pHeader.PromotionStatusId).DefaultIfEmpty()
                                     select new PromotionListResultView
                                     {
                                         PromoId = pHeader.Id,
                                         PromoTypeId = string.IsNullOrEmpty(pHeader.NipEntertain!) ? pHeader.PromotionTypeId! : pHeader.PromotionClassId!,
                                         promoTypeName = string.IsNullOrEmpty(pHeader.NipEntertain!) ? pType.PromotionTypeName! : pClass.PromotionClassName!,
                                         PromoName = pHeader.PromotionName,
                                         StartDate = pHeader.StartDate,
                                         EndDate = pHeader.EndDate,
                                         Depts = pHeader.Depts,
                                         PromoStatusId = pHeader.PromotionStatusId,
                                         PromoStatusName = pStatus.PromotionStatusName,
                                         CreatedDate = pHeader.CreatedDate
                                     }).ToListAsync();

            viewTable.AddRange(SetPromoListViewModel(viewResult2));

            if (startDate != DateTime.Parse("1/1/1900") || endDate != DateTime.Parse("1/1/1900"))
                viewTable = FilterPromoByDate(viewTable, startDate, endDate);

            if (viewSearch != null && viewSearch!.Status != null && viewSearch!.Status!.Count > 0)
            {
                if (viewSearch.Status.Count == 1)
                {
                    string[] arrStat = viewSearch.Status[0].Split(",");
                    viewSearch.Status.Clear();
                    viewSearch.Status.AddRange(arrStat);
                }

                viewTable = viewTable.Where(x => viewSearch.Status.Exists(y => y == x.Status)).ToList();
            }

            if (viewSearch != null && !string.IsNullOrEmpty(viewSearch!.PromotionSearch))
                viewTable = FilterPromoBySearchVar(viewTable, viewSearch.PromotionSearch.Trim().ToUpper());

            int totalPages = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(viewTable.Count) / Convert.ToDouble(pageSize)));
            viewTable = viewTable.AsQueryable().OrderBy(orderColumn + " " + orderMethod).Skip(Skip).Take(pageSize).ToList() ?? new List<PromotionListView>();

            return new Tuple<List<PromotionListView>, int, string>(viewTable, totalPages, "");
        }
        public async Task<PromotionViewModel> GetPromotionViewAsync(Guid promoHeaderId, string userId)
        {
            PromotionViewModel viewModel = new();
            List<PromotionApprovalDetail>? apprStatuses = new();
            List<ApprovalStatus> newApprStatuses = new();
            int? nextApprLvl = 0;

            PromotionHeader pHeader = await _dbContext.PromotionHeader
                                            .Include(p => p.PromoRuleRequirements)
                                            .Include(p => p.PromoRuleResults)
                                            .Include(p => p.PromoRuleMops)
                                            .Where(x => x.Id == promoHeaderId).FirstOrDefaultAsync() ?? new PromotionHeader();
            if (pHeader!.Id != Guid.Empty)
            {
                viewModel = new()
                {
                    Id = pHeader.Id,
                    RedemptionCode = pHeader.RedemptionCode,
                    PromotionCode = pHeader.PromotionCode,
                    PromotionName = pHeader.PromotionName,
                    PromotionStatus = (from pStatus in _dbContext.Set<PromotionStatus>().Where(x => x.Id == pHeader.PromotionStatusId)
                                       select pStatus.PromotionStatusName).FirstOrDefault(),
                    CompanyId = pHeader.CompanyId,
                    CompanyName = pHeader.CompanyName,
                    Depts = pHeader.Depts,
                    StartDate = pHeader.StartDate,
                    EndDate = pHeader.EndDate,
                    PromotionChannels = pHeader.PromotionChannel,
                    Objective = pHeader.Objective,
                    PromoMaterials = pHeader.PromotionMaterial,
                    Zones = pHeader.Zones,
                    Sites = pHeader.Sites,
                    TargetSales = pHeader.TargetSales,
                    MaxPromoUsedType = pHeader.MaxPromoUsedType,
                    MaxPromoUsedQty = pHeader.MaxPromoUsedQty,
                    MultiplePromo = pHeader.MultiplePromo,
                    MultiplePromoMaxQty = pHeader.MultiplePromoMaxQty,
                    MaxQtyPromo = pHeader.MaxQtyPromo,
                    PromotionClassId = pHeader.PromotionClassId,
                    PromotionClassName = (from pClass in _dbContext.Set<PromotionClass>().Where(x => x.Id == pHeader.PromotionClassId)
                                          select pClass.PromotionClassName).FirstOrDefault(),
                    PromotionTypeId = pHeader.PromotionTypeId!,
                    PromotionTypeName = (from pType in _dbContext.Set<PromotionType>().Where(x => x.Id == pHeader.PromotionTypeId)
                                         select pType.PromotionTypeName).FirstOrDefault(),
                    RequirementExp = pHeader.RequirementExp,
                    ResultExp = pHeader.ResultExp,
                    MopPromoSelectionId = pHeader.MopPromoSelectionId,
                    MopPromoSelectionCode = pHeader.MopPromoSelectionCode,
                    MopPromoSelectionName = pHeader.MopPromoSelectionName,
                    MinTransaction = pHeader.MinTransaction,
                    Value = pHeader.Value,
                    MaxDisc = pHeader.MaxDisc,
                    MemberOnly = pHeader.MemberOnly,
                    NewMember = pHeader.NewMember,
                    Members = pHeader.Members,
                    ActiveFlag = pHeader.ActiveFlag,
                    NipEntertain = pHeader.NipEntertain,
                    EntertainBudget = (from pEntBudget in _dbContext.Set<PromotionEntertain>().Where(x => x.EmployeeNIP == pHeader.NipEntertain! && x.StartDate.HasValue &&
                                                                                                          x.StartDate.Value.Month == pHeader.StartDate.Month &&
                                                                                                          x.StartDate.Value.Year == pHeader.StartDate.Year)
                                       select pEntBudget.EntertainBudget).FirstOrDefault(),
                    DisplayOnApp = pHeader.DisplayOnApp,
                    PromoDisplayed = pHeader.PromoDisplayed, 
                    ShortDesc = pHeader.ShortDesc,
                    PromoTermsCondition = pHeader.PromoTermsCondition,
                    PromoRuleRequirements = (from pRuleReqs in _dbContext.Set<PromotionRuleRequirement>().Where(x => x.PromotionHeaderId == pHeader.Id).OrderBy(x => x.LineNum)
                                             select new CreatePromoRuleReq
                                             {
                                                 LineNum = pRuleReqs.LineNum,
                                                 StockCode = pRuleReqs.StockCode,
                                                 StockName = pRuleReqs.StockName,
                                                 Qty = pRuleReqs.Qty
                                             }).ToList(),
                    PromoRuleResults = (from pRuleRess in _dbContext.Set<PromotionRuleResult>().Where(x => x.PromotionHeaderId == pHeader.Id).OrderBy(x => x.LineNum)
                                        select new CreatePromoRuleResult
                                        {
                                            LineNum = pRuleRess.LineNum,
                                            StockCode = pRuleRess.StockCode,
                                            StockName = pRuleRess.StockName,
                                            Qty = pRuleRess.Qty,
                                            MaxDisc = pRuleRess.MaxDisc
                                        }).ToList(),
                    PromoRuleMops = (from pRuleMops in _dbContext.Set<PromotionRuleMopGroup>().Where(x => x.PromotionHeaderId == pHeader.Id).OrderBy(x => x.LineNum)
                                     select new MopGroup
                                     {
                                         LineNum = pRuleMops.LineNum,
                                         MopGroupCode = pRuleMops.MopGroupCode,
                                         MopGroupName = pRuleMops.MopGroupName
                                     }).ToList()
                };

                apprStatuses = await (from apprH in _dbContext.PromotionApproval!.Where(x => x.PromotionHeaderId == pHeader.Id)
                                      from apprD in _dbContext.PromotionApprovalDetail!.Where(x => x.PromotionApprovalId == apprH.Id).DefaultIfEmpty()
                                      select apprD).OrderBy(x => x.ApprovalLevel).ToListAsync() ?? new List<PromotionApprovalDetail>();

                nextApprLvl = await (from apprH in _dbContext.PromotionApproval!.Where(x => x.PromotionHeaderId == pHeader.Id)
                                     from apprD in _dbContext.PromotionApprovalDetail!.Where(x => x.PromotionApprovalId == apprH.Id && !x.ApprovalDate.HasValue).DefaultIfEmpty()
                                     select new PromotionApprovalDetail
                                     {
                                         ApprovalLevel = apprD.ApprovalLevel
                                     }).MinAsync(x => (int?)x.ApprovalLevel) ?? 0;

                newApprStatuses.AddRange(SetViewStatuses(apprStatuses, nextApprLvl));

                viewModel.IsApprove = newApprStatuses!.Count(x => x.ApprvStatus!.Trim().ToUpper() == "ON REVIEW" && x.ApproverId == userId) == 1;

                viewModel.ApprovalStatuses = newApprStatuses;

                #region get first imagelink from PromotionAppImage table
                PromotionAppImage promoImg = _dbContext.PromotionAppImage.Where(x => x.PromotionHeaderId! == promoHeaderId).FirstOrDefault() ?? new PromotionAppImage();
                if (promoImg != null && promoImg.Id != Guid.Empty)
                    viewModel.ImageLink = promoImg.ImageLink!;
                #endregion
            }

            return viewModel;
        }
        public async Task<Tuple<List<PromotionListView>, int, string>> GetPromotionApprovalListAsync(string userId, string orderColumn, string orderMethod, int pageNo = 1, int pageSize = 10, PromotionsViewSearch? viewSearch = null)
        {
            DateTime startDate = SetSearchDate(viewSearch, "Start");
            DateTime endDate = SetSearchDate(viewSearch, "End");

            if (startDate != DateTime.Parse("1/1/1900") && endDate != DateTime.Parse("1/1/1900") && endDate < startDate)
                return new Tuple<List<PromotionListView>, int, string>(new List<PromotionListView>(), 1, "End date must be equal or greater than Start date");

            if (string.IsNullOrEmpty(orderColumn)) orderColumn = "CreatedDate";
            if (string.IsNullOrEmpty(orderMethod)) orderMethod = "DESC";

            int Skip = (pageNo - 1) * pageSize;
            List<PromotionListView> viewTable = new();

            var viewResult = await (from pHeader in _dbContext.Set<PromotionHeader>()
                                    from pType in _dbContext.Set<PromotionType>().Where(x => x.Id == pHeader.PromotionTypeId)
                                    from pStatus in _dbContext.Set<PromotionStatus>().Where(x => x.Id == pHeader.PromotionStatusId && x.PromotionStatusName!.Trim().ToUpper() == "DRAFT")
                                    from pApprvHeader in _dbContext.Set<PromotionApproval>().Where(x => x.PromotionHeaderId == pHeader.Id)
                                    select new PromotionListResultView
                                    {
                                        PromoId = pHeader.Id,
                                        PromoTypeId = pHeader.PromotionTypeId,
                                        promoTypeName = pType.PromotionTypeName,
                                        PromoName = pHeader.PromotionName,
                                        StartDate = pHeader.StartDate,
                                        EndDate = pHeader.EndDate,
                                        Depts = pHeader.Depts,
                                        CreatedDate = pHeader.CreatedDate,
                                        PromoApprovalId = pApprvHeader.Id
                                    }).ToListAsync();

            List<PromotionListResultView> vResult = new();
            PromotionApprovalDetail apprvDetail = new();
            foreach (var item in viewResult)
            {
                apprvDetail = await (from apprH in _dbContext.PromotionApproval!.Where(x => x.PromotionHeaderId == item.PromoId)
                                     from apprD in _dbContext.PromotionApprovalDetail!.Where(x => x.PromotionApprovalId == apprH.Id && x.ApprovalDate == null)
                                     select apprD).OrderBy(x => x.ApprovalLevel).FirstOrDefaultAsync() ?? new PromotionApprovalDetail();

                if (apprvDetail.ApproverId == userId)
                {
                    vResult.Add(item);
                }
            }

            viewTable.AddRange(SetPromoListViewModel(vResult));

            if (startDate != DateTime.Parse("1/1/1900") || endDate != DateTime.Parse("1/1/1900"))
                viewTable = FilterPromoByDate(viewTable, startDate, endDate);

            if (viewSearch != null && !string.IsNullOrEmpty(viewSearch!.PromotionSearch))
                viewTable = FilterPromoBySearchVar(viewTable, viewSearch.PromotionSearch.Trim().ToUpper());

            int totalPages = (int)Math.Ceiling((double)viewTable.Count / (double)pageSize);
            viewTable = viewTable.AsQueryable().OrderBy(orderColumn + " " + orderMethod).Skip(Skip).Take(pageSize).ToList() ?? new List<PromotionListView>();
            
            return new Tuple<List<PromotionListView>, int, string>(viewTable, totalPages, "");
        }
        public async Task<PromotionHeader> GetPromotionByIdAsync(Guid id)
        {
            return await _dbContext.PromotionHeader.Where(x => x.Id == id)
                                                                .Include(p => p.PromoRuleRequirements)
                                                                .Include(p => p.PromoRuleResults)
                                                                .Include(p => p.PromoRuleMops)
                                                                .Include(p => p.PromoAppImages)
                                                                .FirstOrDefaultAsync() ?? new PromotionHeader();
        }

        #endregion

        #region InsertData
        public async Task<Tuple<PromotionHeader, string>> CreatePromotion(CreatePromotion promotionHeader)
        {
            PromotionHeader header = new();
            List<PromotionHistory> histories = new();
            PromotionType promoType = new();

            PromotionStatus statusDraft = await _promoStatusService.GetPromotionStatusByNameAsync("DRAFT");
            PromotionClass promoClass = new();
            var promoClassResponse = await _promoClassService.GetPromotionClassByIdAsync((Guid)promotionHeader.PromotionClassId!);
            if (promoClassResponse.Item1 != null && promoClassResponse.Item1.Id != Guid.Empty)
                promoClass = promoClassResponse.Item1;

            if (promoClass.LineNum! == 4)
            {
                promoType = await _dbContext.PromotionType.Where(x => x.PromotionClassId == promoClass.Id).FirstOrDefaultAsync() ?? new PromotionType();
            }
            else
            {
                var promoTypeResponse = await _promoTypeService.GetPromotionTypeByIdAsync(promotionHeader.PromotionTypeId!);
                if (promoTypeResponse.Item1 != null && promoTypeResponse.Item1.Id != Guid.NewGuid())
                    promoType = promoTypeResponse.Item1;
            }

            string msg = ValidateAddPromoRequest(promotionHeader, promoClass!, promoType!);
            if (msg != "")
                return new Tuple<PromotionHeader, string>(new PromotionHeader(), msg);

            Guid newHeaderId = Guid.NewGuid();
            header.Id = newHeaderId;

            header.PromotionClassId = (Guid)promotionHeader.PromotionClassId!;
            if (promoClass.LineNum! == 4 && promotionHeader.PromotionTypeId == Guid.Empty)
                promotionHeader.PromotionTypeId = promoType.Id!;

            header.PromotionTypeId = promotionHeader.PromotionTypeId;

            header.PromotionStatusId = statusDraft.Id;

            header.PromotionName = promotionHeader.PromotionName;
            header.CompanyId = (Guid)promotionHeader.CompanyId!;
            header.CompanyCode = promotionHeader.CompanyCode;
            header.CompanyName = promotionHeader.CompanyName;
            header.Depts = promotionHeader.Depts;
            header.StartDate = promotionHeader.StartDate;
            header.EndDate = (DateTime)promotionHeader.EndDate!;

            if (!string.IsNullOrEmpty(promotionHeader.PromotionChannel) && promotionHeader.PromotionChannel!.Trim().ToLower() != "null" && promotionHeader.PromotionChannel!.Trim().ToLower() != "string")
                header.PromotionChannel = promotionHeader.PromotionChannel;

            if (!string.IsNullOrEmpty(promotionHeader.Objective) && promotionHeader.Objective!.Trim().ToLower() != "null" && promotionHeader.Objective!.Trim().ToLower() != "string")
                header.Objective = promotionHeader.Objective;

            if (!string.IsNullOrEmpty(promotionHeader.PromotionMaterial) && promotionHeader.PromotionMaterial!.Trim().ToLower() != "null" && promotionHeader.PromotionMaterial!.Trim().ToLower() != "string")
                header.PromotionMaterial = promotionHeader.PromotionMaterial;

            header.Zones = promotionHeader.Zones;
            header.Sites = promotionHeader.Sites;
            header.TargetSales = promotionHeader.TargetSales == null ? 0 : (int)promotionHeader.TargetSales!;

            if (!string.IsNullOrEmpty(promotionHeader.MaxPromoUsedType) && promotionHeader.MaxPromoUsedType!.Trim().ToLower() != "null")
                header.MaxPromoUsedType = promotionHeader.MaxPromoUsedType;

            header.MaxPromoUsedQty = promotionHeader.MaxPromoUsedQty == null ? 0 : (int)promotionHeader.MaxPromoUsedQty!;

            header.RequirementExp = promotionHeader.RequirementExp;
            header.ResultExp = promotionHeader.ResultExp;
            header.MopPromoSelectionId = promotionHeader.MopPromoSelectionId;

            if (!string.IsNullOrEmpty(promotionHeader.MopPromoSelectionCode) && promotionHeader.MopPromoSelectionCode!.Trim().ToLower() != "null" && promotionHeader.MopPromoSelectionCode!.Trim().ToLower() != "string")
                header.MopPromoSelectionCode = promotionHeader.MopPromoSelectionCode;

            header.MopPromoSelectionName = promotionHeader.MopPromoSelectionName;

            if (!string.IsNullOrEmpty(promotionHeader.NipEntertain) && promotionHeader.NipEntertain!.Trim().ToLower() != "null" && promotionHeader.NipEntertain!.Trim().ToLower() != "string")
                header.NipEntertain = promotionHeader.NipEntertain;

            if (promoClass!.LineNum == 4)
                header.Value = "100%";
            else if (!string.IsNullOrEmpty(promotionHeader.Value) && promotionHeader.Value!.Trim().ToLower() != "null" && promotionHeader.Value!.Trim().ToLower() != "string")
                header.Value = promotionHeader.Value;

            if (!string.IsNullOrEmpty(promotionHeader.MaxDisc) && promotionHeader.MaxDisc!.Trim().ToLower() != "null" && promotionHeader.MaxDisc!.Trim().ToLower() != "string")
                header.MaxDisc = promotionHeader.MaxDisc;

            header.MinTransaction = promotionHeader.MinTransaction == null ? 0 : (double)promotionHeader.MinTransaction!;
            header.MemberOnly = promotionHeader.MemberOnly == null ? false : (bool)promotionHeader.MemberOnly!;
            header.NewMember = promotionHeader.NewMember == null ? false : (bool)promotionHeader.NewMember!;
            header.Members = promotionHeader.Members;
            if (!string.IsNullOrEmpty(promotionHeader.Members) && promotionHeader.Members != "[]" && promotionHeader.Members!.Trim().ToLower() != "null" && promotionHeader.Members!.Trim().ToLower() != "string")
                header.MemberOnly = true;

            #region additional for SuperApps
            if (!string.IsNullOrEmpty(promotionHeader.RedemptionCode) && promotionHeader.RedemptionCode!.Trim().ToLower() != "null" && promotionHeader.RedemptionCode!.Trim().ToLower() != "string")
                header.RedemptionCode = promotionHeader.RedemptionCode;

            header.DisplayOnApp = promotionHeader.DisplayOnApp;
            header.PromoDisplayed = promotionHeader.PromoDisplayed;

            if (!string.IsNullOrEmpty(promotionHeader.ShortDesc) && promotionHeader.ShortDesc!.Trim().ToLower() != "null" && promotionHeader.ShortDesc!.Trim().ToLower() != "string")
                header.ShortDesc = promotionHeader.ShortDesc;

            if (!string.IsNullOrEmpty(promotionHeader.PromoTermsCondition) && promotionHeader.PromoTermsCondition!.Trim().ToLower() != "null" && promotionHeader.PromoTermsCondition!.Trim().ToLower() != "<p>null</p>" && promotionHeader.PromoTermsCondition!.Trim().ToLower() != "string")
                header.PromoTermsCondition = promotionHeader.PromoTermsCondition!;
            #endregion

            header.ActiveFlag = true;

            var promoEntertain = await _dbContext.PromotionEntertain.Where(x => x.EmployeeNIP == promotionHeader.NipEntertain && x.StartDate.HasValue && x.StartDate.Value.Month == promotionHeader.StartDate.Month && x.StartDate.Value.Year == promotionHeader.StartDate.Year).FirstOrDefaultAsync();

            if (promoClass != null)
            {
                header.MultiplePromo = promoClass!.LineNum == 1 && (bool)promotionHeader.MultiplePromo!;

                if (promoEntertain != null)
                    header.EntertainBudget = promoClass!.LineNum == 4 ? promoEntertain!.EntertainBudget! : null;

                header.MultiplePromoMaxQty = SetMultipleQty(promoClass.LineNum, (bool)promotionHeader.MultiplePromo!, (int)promotionHeader.MultiplePromoMaxQty!);

                if (promoClass!.LineNum != 1)
                    header.MaxQtyPromo = 0;
                else if (promoClass!.LineNum == 1 && promotionHeader.MaxQtyPromo < 1)
                    header.MaxQtyPromo = 1;
                else
                    header.MaxQtyPromo = promotionHeader.MaxQtyPromo == null ? 0 : (int)promotionHeader.MaxQtyPromo!;
            }

            if (promoType != null && (promoType.PromotionTypeKey == "BUNDLE" || promoType.PromotionTypeKey == "BUNDLING"))
            {
                promotionHeader.RequirementExp = "AND";
                promotionHeader.ResultExp = "AND";
                header.RequirementExp = promotionHeader.RequirementExp;
                header.ResultExp = promotionHeader.ResultExp;
            }

            if (statusDraft != null)
            {
                PromotionHistory newHistory = new()
                {
                    Id = Guid.NewGuid(),
                    PromotionHeaderId = newHeaderId,
                    PromotionStatusId = statusDraft.Id,
                    ActiveFlag = true,
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = promotionHeader.Username,
                    UpdatedDate = DateTime.UtcNow,
                    UpdatedBy = promotionHeader.Username
                };
                histories.Add(newHistory);
            }
            //set promotionHistory based on ApprovalMapping
            bool isGenerateApproval = false;
            var approvalMapping = await _dbContext.ApprovalMapping!.Where(x => x.ActiveFlag && x.CompanyId == promotionHeader.CompanyId).FirstOrDefaultAsync();
            if (approvalMapping != null)
            {
                isGenerateApproval = true;
                //call GeneratePromoApproval
                GeneratePromoApproval request = new()
                {
                    CompanyId = (Guid)promotionHeader.CompanyId!,
                    CompanyCode = promotionHeader.CompanyCode,
                    PromotionHeaderId = newHeaderId
                };
                await _approvalService.GeneratePromoApproval(request);
            }
            else
            {
                PromotionStatus? status = new();
                if (promotionHeader.StartDate <= DateTime.UtcNow && promotionHeader.EndDate >= DateTime.UtcNow)
                {
                    status = await _promoStatusService.GetPromotionStatusByNameAsync("RUN");
                }
                else if (promotionHeader.StartDate > DateTime.UtcNow)
                {
                    status = await _promoStatusService.GetPromotionStatusByNameAsync("SCHEDULE");
                }
                if (status!.Id != Guid.Empty)
                {
                    histories.Add(
                        new PromotionHistory
                        {
                            Id = Guid.NewGuid(),
                            PromotionHeaderId = newHeaderId,
                            PromotionStatusId = status.Id,
                            ActiveFlag = true,
                            CreatedDate = DateTime.UtcNow,
                            CreatedBy = promotionHeader.Username,
                            UpdatedDate = DateTime.UtcNow,
                            UpdatedBy = promotionHeader.Username
                        });

                    header.PromotionStatusId = status.Id;
                }
            }

            header.Username = promotionHeader.Username!;

            //promotionRuleRequirement
            List<CreatePromoRuleReq> createRuleReqs = new();
            if (!string.IsNullOrEmpty(promotionHeader.RuleReqs) && promotionHeader.RuleReqs != "[]" && promotionHeader.RuleReqs!.Trim().ToLower() != "null")
                createRuleReqs = string.IsNullOrEmpty(promotionHeader.RuleReqs) ? new List<CreatePromoRuleReq>() : JsonConvert.DeserializeObject<List<CreatePromoRuleReq>>(promotionHeader.RuleReqs) ?? new List<CreatePromoRuleReq>();

            List<PromotionRuleRequirement> ruleRequirements = _mapper.Map<List<PromotionRuleRequirement>>(createRuleReqs);
            ruleRequirements.ForEach(x => { x.PromotionHeaderId = newHeaderId; x.ActiveFlag = true; });

            //promotionRuleResult
            List<CreatePromoRuleResult> createRuleRess = new();
            if (!string.IsNullOrEmpty(promotionHeader.RuleRess) && promotionHeader.RuleRess != "[]" && promotionHeader.RuleRess!.Trim().ToLower() != "null")
                createRuleRess = string.IsNullOrEmpty(promotionHeader.RuleRess) ? new List<CreatePromoRuleResult>() : JsonConvert.DeserializeObject<List<CreatePromoRuleResult>>(promotionHeader.RuleRess) ?? new List<CreatePromoRuleResult>();

            List<PromotionRuleResult> ruleResults = _mapper.Map<List<PromotionRuleResult>>(createRuleRess);
            ruleResults.ForEach(x => { x.PromotionHeaderId = newHeaderId; x.ActiveFlag = true; });

            //promotionRuleMopGroup
            List<MopGroup> ruleMopsCreate = new();
            if (!string.IsNullOrEmpty(promotionHeader.RuleMops) && promotionHeader.RuleMops != "[]" && promotionHeader.RuleMops!.Trim().ToLower() != "null" && promotionHeader.RuleMops!.Trim().ToLower() != "string")
                ruleMopsCreate = string.IsNullOrEmpty(promotionHeader.RuleMops) ? new List<MopGroup>() : JsonConvert.DeserializeObject<List<MopGroup>>(promotionHeader.RuleMops) ?? new List<MopGroup>();

            List<PromotionRuleMopGroup> ruleMops = _mapper.Map<List<PromotionRuleMopGroup>>(ruleMopsCreate);
            ruleMops.ForEach(x => { x.PromotionHeaderId = newHeaderId; x.ActiveFlag = true; });

            #region UploadPromotionImage
            List<PromoDisplayed> promoDisplays = new();
            List<PromotionAppDisplay> appList = new();
            List<PromotionAppImage> promoImages = new();
            if (!string.IsNullOrEmpty(promotionHeader.PromoDisplayed) && promotionHeader.PromoDisplayed != "[]" && promotionHeader.PromoDisplayed!.Trim().ToLower() != "null")
                promoDisplays = string.IsNullOrEmpty(promotionHeader.PromoDisplayed) ? new List<PromoDisplayed>() : JsonConvert.DeserializeObject<List<PromoDisplayed>>(promotionHeader.PromoDisplayed) ?? new List<PromoDisplayed>();
            if (promoDisplays.Count > 0)
                appList = SetPromoAppDisplay(promoDisplays);

            if (appList.Count > 0 && promotionHeader.PromoImage != null && promotionHeader.Username!.Trim().ToUpper() != "WAWA")
            {
                IFormFile file = promotionHeader.PromoImage;
                using var ms = new MemoryStream();
                await file.CopyToAsync(ms);
                ms.Seek(0, SeekOrigin.Begin);
                foreach (var item in appList.Select(x => x.AppCode!.Trim()))
                {
                    string fileExt = Path.GetExtension(promotionHeader.PromoImage.FileName);
                    string fileName = string.Format("{0}{1}", newHeaderId.ToString(), fileExt);
                    var uploadedFile = await _s3Service.UploadFileAsync(ms, newHeaderId.ToString(), fileName, promotionHeader.PromoImage.ContentType, item!, (DateTime)promotionHeader.EndDate);
                    if (uploadedFile != null && uploadedFile.data)
                    {
                        promoImages.Add(new PromotionAppImage()
                        {
                            PromotionHeaderId = newHeaderId,
                            AppCode = item,
                            ImageLink = uploadedFile.file_path,
                            FileName = fileName,
                            ActiveFlag = true
                        });
                    }
                    else
                    {
                        return new Tuple<PromotionHeader, string>(new PromotionHeader(), uploadedFile == null ? "" : uploadedFile!.response_message!);
                    }
                }
            }
            header.PromoImage = promotionHeader.PromoImage;
            #endregion

            header.PromoRuleRequirements = ruleRequirements;
            header.PromoRuleResults = ruleResults;
            header.PromoRuleMops = ruleMops;

            header.PromotionCode = GeneratePromotionCode(promotionHeader.CompanyCode!);
            header.CreatedDate = DateTime.UtcNow;
            header.CreatedBy = promotionHeader.Username;
            header.UpdatedDate = DateTime.UtcNow;
            header.UpdatedBy = promotionHeader.Username;
            header.ActiveFlag = true;

            _dbContext.PromotionHeader.Add(header);
            _dbContext.PromotionHistory.AddRange(histories);
            _dbContext.PromotionAppImage.AddRange(promoImages);

            await _dbContext.SaveChangesAsync();

            if (!isGenerateApproval && promotionHeader.Username!.Trim().ToUpper() != "ENTERTAINJOB" && promotionHeader.Username!.Trim().ToUpper() != "WAWA")
            {
                await PublishPromoMessage("Create", header, ruleRequirements, ruleResults, ruleMops);
            }

            return new Tuple<PromotionHeader, string>(header, "data has been saved");
        }
        #endregion

        #region UpdateData
        public async Task<Tuple<PromotionHeader, string>> UpdatePromotion(UpdatePromotion promotionHeader)
        {
            PromotionHeader updatedPromoHeader = await GetPromotionByIdAsync(promotionHeader.Id);

            string msg = await ValidateUpdatePromo(updatedPromoHeader, promotionHeader);
            if (msg != "")
                return new Tuple<PromotionHeader, string>(new PromotionHeader(), msg);

            updatedPromoHeader.EndDate = promotionHeader.EndDate;
            updatedPromoHeader.Zones = promotionHeader.Zones;
            updatedPromoHeader.Sites = promotionHeader.Sites;
            updatedPromoHeader.Username = promotionHeader.Username;
            updatedPromoHeader.MemberOnly = promotionHeader.MemberOnly;
            updatedPromoHeader.NewMember = promotionHeader.NewMember;
            updatedPromoHeader.Members = promotionHeader.Members;
            if (!string.IsNullOrEmpty(promotionHeader.Members) && promotionHeader.Members != "[]")
                updatedPromoHeader.MemberOnly = true;

            //MaxQtyPromo
            PromotionClass? promoClass = null;
            var promoClassResponse = await _promoClassService.GetPromotionClassByIdAsync(updatedPromoHeader.PromotionClassId);
            if (promoClassResponse.Item1 != null && promoClassResponse.Item1.Id != Guid.Empty)
                promoClass = promoClassResponse.Item1;

            if (promoClass != null && promoClass.Id != Guid.Empty)
            {
                if (promoClass!.LineNum != 1)
                    updatedPromoHeader.MaxQtyPromo = 0;
                else if (promoClass!.LineNum == 1 && promotionHeader.MaxQtyPromo < 1)
                    updatedPromoHeader.MaxQtyPromo = 1;
                else
                    updatedPromoHeader.MaxQtyPromo = promotionHeader.MaxQtyPromo;
            }

            //delete existing promotionRuleRequirement
            _dbContext.PromotionRuleRequirement.RemoveRange(updatedPromoHeader.PromoRuleRequirements!);
            //promotionRuleRequirement
            List<CreatePromoRuleReq> updateRuleReqs = new();
            if (!string.IsNullOrEmpty(promotionHeader.RuleReqs) && promotionHeader.RuleReqs != "[]")
                updateRuleReqs = string.IsNullOrEmpty(promotionHeader.RuleReqs) ? new List<CreatePromoRuleReq>() : JsonConvert.DeserializeObject<List<CreatePromoRuleReq>>(promotionHeader.RuleReqs) ?? new List<CreatePromoRuleReq>();

            List<PromotionRuleRequirement> ruleRequirements = _mapper.Map<List<PromotionRuleRequirement>>(updateRuleReqs);
            ruleRequirements.ForEach(x => { x.PromotionHeaderId = updatedPromoHeader.Id; x.ActiveFlag = true; });

            //delete existing promotionRuleResult
            _dbContext.PromotionRuleResult.RemoveRange(updatedPromoHeader.PromoRuleResults!);
            //promotionRuleResult
            List<CreatePromoRuleResult> updateRuleRess = new();
            if (!string.IsNullOrEmpty(promotionHeader.RuleRess) && promotionHeader.RuleRess != "[]")
                updateRuleRess = string.IsNullOrEmpty(promotionHeader.RuleRess) ? new List<CreatePromoRuleResult>() : JsonConvert.DeserializeObject<List<CreatePromoRuleResult>>(promotionHeader.RuleRess) ?? new List<CreatePromoRuleResult>();

            List<PromotionRuleResult> ruleResults = _mapper.Map<List<PromotionRuleResult>>(updateRuleRess);
            ruleResults.ForEach(x => { x.PromotionHeaderId = updatedPromoHeader.Id; x.ActiveFlag = true; });

            updatedPromoHeader.PromoRuleRequirements = ruleRequirements;
            updatedPromoHeader.PromoRuleResults = ruleResults;

            #region Additional for SuperApps
            updatedPromoHeader.PromoDisplayed = promotionHeader.PromoDisplayed;

            if (!string.IsNullOrEmpty(promotionHeader.ShortDesc) && promotionHeader.ShortDesc!.Trim().ToLower() != "null" && promotionHeader.ShortDesc!.Trim().ToLower() != "string")
                updatedPromoHeader.ShortDesc = promotionHeader.ShortDesc;

            if (!string.IsNullOrEmpty(promotionHeader.PromoTermsCondition) && promotionHeader.PromoTermsCondition!.Trim().ToLower() != "null" && promotionHeader.PromoTermsCondition!.Trim().ToLower() != "<p>null</p>" && promotionHeader.PromoTermsCondition!.Trim().ToLower() != "string")
                updatedPromoHeader.PromoTermsCondition = promotionHeader.PromoTermsCondition;


            #region UploadPromotionImage
            //remove existing file in S3
            if (updatedPromoHeader.PromoAppImages!.Count > 0)
            {
                foreach (var item in updatedPromoHeader.PromoAppImages.Select(x => x.AppCode))
                {
                    await _s3Service.DeleteFileAsync(updatedPromoHeader.Id.ToString(), item!);
                }
            }
            //delete existing promotionAppImage
            _dbContext.PromotionAppImage.RemoveRange(updatedPromoHeader.PromoAppImages!);

            List<PromoDisplayed> promoDisplays = new();
            List<PromotionAppDisplay> appList = new();
            List<PromotionAppImage> promoImages = new();
            if (!string.IsNullOrEmpty(promotionHeader.PromoDisplayed) && promotionHeader.PromoDisplayed != "[]")
                promoDisplays = string.IsNullOrEmpty(promotionHeader.PromoDisplayed) ? new List<PromoDisplayed>() : JsonConvert.DeserializeObject<List<PromoDisplayed>>(promotionHeader.PromoDisplayed) ?? new List<PromoDisplayed>();
            if (promoDisplays.Count > 0)
                appList = SetPromoAppDisplay(promoDisplays);

            if (appList.Count > 0 && promotionHeader.PromoImage != null && promotionHeader.Username!.Trim().ToUpper() != "WAWA")
            {
                IFormFile file = promotionHeader.PromoImage;
                using var ms = new MemoryStream();
                await file.CopyToAsync(ms);
                ms.Seek(0, SeekOrigin.Begin);
                foreach (var item in appList.Select(x => x.AppCode))
                {
                    string fileExt = Path.GetExtension(promotionHeader.PromoImage.FileName);
                    string fileName = string.Format("{0}{1}", promotionHeader.Id.ToString(), fileExt);
                    var uploadedFile = await _s3Service.UploadFileAsync(ms, promotionHeader.Id.ToString(), fileName, promotionHeader.PromoImage.ContentType, item!, promotionHeader.EndDate);
                    if (uploadedFile.data)
                    {
                        promoImages.Add(new PromotionAppImage()
                        {
                            PromotionHeaderId = promotionHeader.Id,
                            AppCode = item,
                            ImageLink = uploadedFile.file_path,
                            FileName = fileName,
                            ActiveFlag = true
                        });
                    }
                    else
                    {
                        return new Tuple<PromotionHeader, string>(new PromotionHeader(), uploadedFile.response_message);
                    }
                }
            }
            updatedPromoHeader.PromoImage = promotionHeader.PromoImage;
            updatedPromoHeader.PromoAppImages = promoImages;
            #endregion

            #endregion

            //check promotion status. If rejected then set the status to draft, clear approvaldate for all levels
            PromotionHistory promoHistory = new();
            List<PromotionApprovalDetail> apprDetails = new();
            var rejectStatus = await _promoStatusService.GetPromotionStatusByNameAsync("REJECT");
            if (rejectStatus != null && rejectStatus.Id == updatedPromoHeader.PromotionStatusId)
            {
                var status = await _promoStatusService.GetPromotionStatusByNameAsync("DRAFT");
                if (status!.Id != Guid.Empty)
                {
                    promoHistory = new()
                    {
                        Id = Guid.NewGuid(),
                        PromotionHeaderId = updatedPromoHeader.Id,
                        PromotionStatusId = status.Id,
                        ActiveFlag = true,
                        CreatedDate = DateTime.UtcNow,
                        CreatedBy = promotionHeader.Username,
                        UpdatedDate = DateTime.UtcNow,
                        UpdatedBy = promotionHeader.Username
                    };

                    updatedPromoHeader.PromotionStatusId = status.Id;
                }

                /*jika ada promotion approval maka datanya direset */
                apprDetails = await (from apprH in _dbContext.Set<PromotionApproval>().Where(x => x.PromotionHeaderId == updatedPromoHeader.Id)
                                     from apprD in _dbContext.Set<PromotionApprovalDetail>().Where(x => x.PromotionApprovalId == apprH.Id).DefaultIfEmpty()
                                     select apprD).ToListAsync();
                apprDetails.ForEach(x => { x.ApprovalDate = null; x.Approve = false; x.ApprovalNotes = null; });
            }

            updatedPromoHeader.UpdatedDate = DateTime.UtcNow;
            updatedPromoHeader.UpdatedBy = promotionHeader.Username;

            _dbContext.PromotionHeader.Update(updatedPromoHeader);
            _dbContext.PromotionRuleRequirement.AddRange(ruleRequirements);
            _dbContext.PromotionRuleResult.AddRange(ruleResults);
            _dbContext.PromotionAppImage.AddRange(promoImages);
            if (promoHistory != null && promoHistory.Id != Guid.Empty)
            {
                _dbContext.PromotionHistory.Add(promoHistory);
            }
            if (apprDetails != null && apprDetails.Count > 0)
            {
                _dbContext.PromotionApprovalDetail.UpdateRange(apprDetails);
            }

            await _dbContext.SaveChangesAsync();

            // Send message to the message stream.
            PromotionStatus promoStatus = await _promoStatusService.GetPromotionStatusByIdAsync(updatedPromoHeader.PromotionStatusId);
            if (promoStatus != null && promoStatus.Id != Guid.Empty &&
                (promoStatus.PromotionStatusKey!.Trim().ToUpper() == "SCHEDULED" || promoStatus.PromotionStatusName!.Trim().ToUpper() == "SCHEDULED"
                || promoStatus.PromotionStatusKey.Trim().ToUpper() == "RUNNING" || promoStatus.PromotionStatusName.Trim().ToUpper() == "RUNNING")
                && updatedPromoHeader.Username!.Trim().ToUpper() != "WAWA")
            {
                //message class promoUpdated
                await PublishPromoMessage("Update", updatedPromoHeader, ruleRequirements, ruleResults, new List<PromotionRuleMopGroup>());
            }

            return new Tuple<PromotionHeader, string>(updatedPromoHeader, "data has been updated");
        }
        #endregion

        #region PromoMessage
        public async Task<Object> GetPromoMessage(string CreateUpdate, PromotionHeader promoHeader, List<PromotionRuleRequirement> promoRuleReqs, List<PromotionRuleResult> promoRuleResults, List<PromotionRuleMopGroup> promoRuleMops)
        {
            var promoMessage = new PromoCreated();

            Requirement promoMessageReq = new();
            Result promoMessageRes = new();
            List<ItemRequirement> promoMessageReqItems = new();
            List<ItemResult> promoMessageResItems = new();
            List<MopGroup> promoMessageMops = new();

            promoMessage.PromoRuleId = promoHeader.Id.ToString();
            promoMessage.PromoRuleCode = promoHeader.PromotionCode;
            promoMessage.PromoRuleName = promoHeader.PromotionName;
            promoMessage.PromoRuleStartDate = promoHeader.StartDate;
            promoMessage.PromoRuleEndDate = promoHeader.EndDate;
            promoMessage.PromoWorkflowId = promoHeader.CompanyId.ToString();
            promoMessage.PromoWorkflowCode = promoHeader.CompanyCode;
            promoMessage.PromoWorkflowName = promoHeader.CompanyName;
            promoMessage.BrandId = null;
            promoMessage.BrandName = null;

            PromotionClass promoClass = promoClass = await _dbContext.PromotionClass!.Where(x => x.Id == promoHeader!.PromotionClassId)!.FirstOrDefaultAsync() ?? new PromotionClass();
            if (promoClass!.Id != Guid.Empty)
                promoMessage = SetPropertyBasedOnPromoClass(promoHeader, promoMessage, promoClass);

            PromotionType promoType = await _dbContext.PromotionType!.Where(x => x.Id == promoHeader!.PromotionTypeId)!.FirstOrDefaultAsync() ?? new PromotionType();
            if (promoType!.Id != Guid.Empty)
                promoMessage = SetPropertyBasedOnPromoType(promoHeader, promoMessage, promoClass, promoType);

            if (!string.IsNullOrEmpty(promoHeader.Depts) && promoHeader.Depts != "[]")
                promoMessage.Depts = string.IsNullOrEmpty(promoHeader.Depts) ? new List<Dept>() : JsonConvert.DeserializeObject<List<Dept>>(promoHeader.Depts) ?? new List<Dept>();

            if (!string.IsNullOrEmpty(promoHeader.Zones) && promoHeader.Zones != "[]")
                promoMessage.Zones = string.IsNullOrEmpty(promoHeader.Zones) ? new List<Zone>() : JsonConvert.DeserializeObject<List<Zone>>(promoHeader.Zones) ?? new List<Zone>();

            if (!string.IsNullOrEmpty(promoHeader.Sites) && promoHeader.Sites != "[]")
                promoMessage.Sites = string.IsNullOrEmpty(promoHeader.Sites) ? new List<Site>() : JsonConvert.DeserializeObject<List<Site>>(promoHeader.Sites) ?? new List<Site>();

            promoMessage.Username = string.IsNullOrEmpty(promoHeader.Username) ? "Default" : promoHeader.Username;

            promoMessage.MemberOnly = promoHeader.MemberOnly;
            promoMessage.NewMember = promoHeader.NewMember;
            if (!string.IsNullOrEmpty(promoHeader.Members) && promoHeader.Members != "[]")
                promoMessage.Members = string.IsNullOrEmpty(promoHeader.Members) ? new List<Member>() : JsonConvert.DeserializeObject<List<Member>>(promoHeader.Members) ?? new List<Member>();

            promoMessage.IsActive = CreateUpdate == "Create" || promoHeader.ActiveFlag;

            promoMessage.PromoExpMinTransAmount = promoHeader.MinTransaction.ToString();
            promoMessage.MopPromoSelectionId = promoHeader.MopPromoSelectionId;
            promoMessage.MopPromoSelectionCode = promoHeader.MopPromoSelectionCode;
            promoMessage.MopPromoSelectionName = promoHeader.MopPromoSelectionName;

            promoMessageReq.PromoRuleExpressionLinkExp = promoHeader.RequirementExp;
            promoMessageRes.PromoRuleResultLinkExp = promoHeader.ResultExp;

            promoMessageReqItems.AddRange(SetMessageRuleRequirement(promoRuleReqs));

            promoMessageResItems.AddRange(SetMessageRuleResult(promoRuleResults));

            if (promoClass!.PromotionClassKey!.Trim().ToUpper().Contains("ITEM") || promoClass!.PromotionClassName!.Trim().ToUpper().Contains("ITEM"))
            {
                promoMessageReq.Items = promoMessageReqItems.Count > 0 ? promoMessageReqItems : null;
                promoMessageRes.Items = promoMessageResItems.Count > 0 ? promoMessageResItems : null;

                promoMessage.Requirement = promoMessageReq;
                promoMessage.Result = promoMessageRes;
            }

            if (promoRuleMops != null)
                promoMessageMops.AddRange(SetMessageMopGroup(promoRuleMops));

            promoMessage.MopGroups = promoMessageMops.Count > 0 ? promoMessageMops : null;

            #region Additional for SuperApps
            promoMessage.PromoRuleRedemptionCode = promoHeader.RedemptionCode;
            promoMessage.PromoRuleDisplayOnApp = promoHeader.DisplayOnApp;
            if (!string.IsNullOrEmpty(promoHeader.PromoDisplayed) && promoHeader.PromoDisplayed != "[]")
                promoMessage.PromoDisplayed = string.IsNullOrEmpty(promoHeader.PromoDisplayed) ? new List<PromoDisplayed>() : JsonConvert.DeserializeObject<List<PromoDisplayed>>(promoHeader.PromoDisplayed) ?? new List<PromoDisplayed>();
            promoMessage.PromoRuleShortDesc = promoHeader.ShortDesc;
            promoMessage.PromoRuleTermsCondition = promoHeader.PromoTermsCondition;

            if (promoHeader.PromoImage != null)
            {
                string host = string.Format("{0}://{1}", _httpContextAccessor.HttpContext!.Request.Scheme, _httpContextAccessor.HttpContext!.Request.Host.Value);
                if (host.Trim().ToUpper().Contains("AURORA"))
                    host = string.Format("{0}/{1}", host, "marketing");
                string imgDownloadLink = string.Format("{0}/downloads3fileasync?promoId={1}&appCode=", host, promoHeader.Id.ToString());
                promoMessage.PromoImageLink = imgDownloadLink;
            }
            #endregion

            return (CreateUpdate == "Update") ? _mapper.Map<PromoUpdated>(promoMessage) : promoMessage;
        }
        public PromoCreated SetPropertyBasedOnPromoClass(PromotionHeader promoHeader, PromoCreated promoMessage, PromotionClass promoClass)
        {
            promoMessage.PromoRuleClass = promoClass.LineNum;
            promoMessage.PromoRuleItemType = promoClass.LineNum == 1 ? "CUSTOM" : "ALL";

            if (!string.IsNullOrEmpty(promoHeader.Value))
                promoHeader.Value = promoHeader.Value!.Contains('%') ? promoHeader.Value!.Replace(",", ".") : promoHeader.Value!.Replace(".", "");

            promoMessage.PromoRuleActionValue = promoClass.LineNum == 1 ? "" : promoHeader.Value;

            promoMessage.PromoRuleActionValueMax = promoClass.LineNum == 1 ? "" : promoHeader.MaxDisc;
            promoMessage.MultipleApp = promoClass.LineNum == 1 && promoHeader.MultiplePromo;

            //set Multiple Promo Qty
            promoMessage.MultipleAppQty = SetMultipleQty(promoClass.LineNum, promoHeader.MultiplePromo, promoHeader.MultiplePromoMaxQty);

            //set MaxQtyPromo
            promoMessage.MaxQtyPromo = promoHeader.MaxQtyPromo;

            promoMessage.NipEntertain = promoClass.LineNum == 4 ? promoHeader.NipEntertain : "";
            promoMessage.EntertainStatus = promoClass.LineNum == 4;
            promoMessage.PromoQuota = promoClass.LineNum == 4 ? 0 : promoHeader.MaxPromoUsedQty;
            promoMessage.PromoBalance = promoClass.LineNum != 4 ? 0 : ((int?)promoHeader.EntertainBudget! ?? 0);

            return promoMessage;
        }
        public PromoCreated SetPropertyBasedOnPromoType(PromotionHeader promoHeader, PromoCreated promoMessage, PromotionClass promoClass, PromotionType promoType)
        {
            promoMessage.PromoRuleLevel = promoType.LineNum;

            if (promoClass!.Id != Guid.Empty)
                promoMessage.PromoRuleActionType = promoClass.LineNum == 4 ? "PERCENT" : promoType.PromotionTypeKey;

            if (promoType.PromotionTypeKey == "BUNDLE" || promoType.PromotionTypeKey == "BUNDLING")
            {
                promoHeader.Value = promoHeader.Value!.Contains('%') ? promoHeader.Value!.Replace(",", ".") : promoHeader.Value!.Replace(".", "");
                promoMessage.PromoRuleActionValue = string.IsNullOrEmpty(promoHeader.Value) ? "" : promoHeader.Value;
            }
            return promoMessage;
        }
        public List<ItemRequirement> SetMessageRuleRequirement(List<PromotionRuleRequirement> promoRuleReqs)
        {
            List<ItemRequirement> itemReqList = new();
            foreach (var item in promoRuleReqs)
            {
                itemReqList.Add(
                    new ItemRequirement
                    {
                        LineNum = item.LineNum,
                        StockCodeId = item.StockCodeId.ToString(),
                        StockCode = item.StockCode,
                        Qty = item.Qty.ToString()
                    });
            }
            return itemReqList;
        }
        public List<ItemResult> SetMessageRuleResult(List<PromotionRuleResult> promoRuleResults)
        {
            List<ItemResult> itemResultList = new();
            foreach (var item in promoRuleResults)
            {
                itemResultList.Add(
                    new ItemResult
                    {
                        PromoRuleResultLineNum = item.LineNum,
                        PromoRuleResultItemId = item.StockCodeId.ToString(),
                        PromoRuleResultItem = item.StockCode,
                        Value = string.IsNullOrEmpty(item.Value) ? item.Qty.ToString() : item.Value,
                        PromoRuleResultMaxDisc = item.MaxDisc
                    });
            }
            return itemResultList;
        }
        public List<MopGroup> SetMessageMopGroup(List<PromotionRuleMopGroup> promoRuleMops)
        {
            List<MopGroup> mopList = new();
            foreach (var item in promoRuleMops)
            {
                mopList.Add(
                    new MopGroup
                    {
                        LineNum = item.LineNum,
                        MopGroupId = Guid.TryParse(item.MopGroupId, out Guid guidOutput) ? item.MopGroupId : guidOutput.ToString(),
                        MopGroupCode = item.MopGroupCode,
                        MopGroupName = item.MopGroupName
                    });
            }
            return mopList;
        }
        public async Task PublishPromoMessage(string CreateUpdate, PromotionHeader promotionHeader, List<PromotionRuleRequirement> ruleRequirements, List<PromotionRuleResult> ruleResults, List<PromotionRuleMopGroup> ruleMops)
        {
            PromoCreated promoCreated = (PromoCreated)await GetPromoMessage(CreateUpdate, promotionHeader, ruleRequirements, ruleResults, ruleMops);
            _pubService.SendPromoCreatedMessage(promoCreated, "Create");
        }
        #endregion

        #region SetActivePromo
        public async Task<PromotionHeader> UpdateActivePromo(Guid promoId, bool activeFlag)
        {
            PromotionHeader updatedPromoHeader = await GetPromotionByIdAsync(promoId);

            if (updatedPromoHeader == null)
                throw new InvalidOperationException($"Promotion id:{promoId} is not found");

            updatedPromoHeader.ActiveFlag = activeFlag;

            PromotionStatus? status = new();
            PromotionHistory promoHistory = new();
            if (activeFlag)
            {
                if (updatedPromoHeader!.StartDate <= DateTime.UtcNow && updatedPromoHeader.EndDate >= DateTime.UtcNow)
                {
                    status = await _promoStatusService.GetPromotionStatusByNameAsync("RUN");
                }
                else if (updatedPromoHeader.StartDate > DateTime.UtcNow)
                {
                    status = await _promoStatusService.GetPromotionStatusByNameAsync("SCHEDULE");
                }
            }
            else
            {
                status = await _promoStatusService.GetPromotionStatusByNameAsync("HOLD");
            }

            if (status!.Id != Guid.Empty)
            {
                promoHistory = new()
                {
                    Id = Guid.NewGuid(),
                    PromotionHeaderId = updatedPromoHeader.Id,
                    PromotionStatusId = status.Id,
                    ActiveFlag = true,
                    CreatedDate = DateTime.UtcNow,
                    UpdatedDate = DateTime.UtcNow
                };

                updatedPromoHeader.PromotionStatusId = status.Id;
            }

            //get promoUpdated
            PromoUpdated promoUpdated = new();
            bool needToPublish = false;
            PromotionStatus promoStatus = await _promoStatusService.GetPromotionStatusByIdAsync(updatedPromoHeader.PromotionStatusId);
            if (promoStatus != null && promoStatus.Id != Guid.Empty &&
                (promoStatus.PromotionStatusKey!.Trim().ToUpper() == "SCHEDULED" || promoStatus.PromotionStatusName!.Trim().ToUpper() == "SCHEDULED"
                || promoStatus.PromotionStatusKey!.Trim().ToUpper() == "RUNNING" || promoStatus.PromotionStatusName!.Trim().ToUpper() == "RUNNING"
                || promoStatus.PromotionStatusKey!.Trim().ToUpper().Contains("HOLD") || promoStatus.PromotionStatusName!.Trim().ToUpper().Contains("HOLD")))
            {
                List<PromotionRuleRequirement> promoRuleReqs = await _dbContext.PromotionRuleRequirement.Where(x => x.PromotionHeaderId == promoId).ToListAsync() ?? new List<PromotionRuleRequirement>();
                List<PromotionRuleResult> promoRuleResults = await _dbContext.PromotionRuleResult.Where(x => x.PromotionHeaderId == promoId).ToListAsync() ?? new List<PromotionRuleResult>();
                List<PromotionRuleMopGroup> promoRuleMops = await _dbContext.PromotionRuleMopGroup.Where(x => x.PromotionHeaderId == promoId).ToListAsync() ?? new List<PromotionRuleMopGroup>();
                promoUpdated = (PromoUpdated)await GetPromoMessage("Update", updatedPromoHeader, promoRuleReqs, promoRuleResults, promoRuleMops);
                needToPublish = true;
            }

            updatedPromoHeader.UpdatedDate = DateTime.UtcNow;
            
            _dbContext.PromotionHeader.Update(updatedPromoHeader);
            if (promoHistory != null && promoHistory.Id != Guid.Empty)
                _dbContext.PromotionHistory.Add(promoHistory);

            await _dbContext.SaveChangesAsync();

            // Send message to the message stream.
            if (needToPublish)
            {
                _pubService.SendPromoCreatedMessage(_mapper.Map<PromoCreated>(promoUpdated), "Update");
            }


            return updatedPromoHeader;
        }
        #endregion

        #region ApproveRejectPromotion
        public async Task<Tuple<PromotionApprovalDetail, string>> ApproveRejectPromotion(ApproveRejectPromotion promoApproval)
        {
            try
            {
                PromotionApproval updatedPromoApproval = await _dbContext.PromotionApproval.FirstOrDefaultAsync(x => x.PromotionHeaderId == promoApproval.PromotionHeaderId) ?? new PromotionApproval();
                var currentApprove = await _dbContext.PromotionApprovalDetail!.OrderBy(x => x.ApprovalLevel).FirstOrDefaultAsync(x => x.PromotionApprovalId == updatedPromoApproval!.Id && !x.ApprovalDate.HasValue);
                var updatedApprovalStatus = await _dbContext.PromotionApprovalDetail!.FirstOrDefaultAsync(x => x.PromotionApprovalId == updatedPromoApproval!.Id && x.ApprovalLevel == currentApprove!.ApprovalLevel && x.ApproverId == promoApproval.ApproverId);
                if (updatedApprovalStatus == null)
                    return new Tuple<PromotionApprovalDetail, string>(new PromotionApprovalDetail(), string.Format("Approval Status for User {0} is not found", promoApproval.ApproverId));

                PromotionHeader promoHeader = await _dbContext.PromotionHeader.Where(x => x.Id == promoApproval.PromotionHeaderId)
                                                                              .Include(p => p.PromoRuleRequirements)
                                                                              .Include(p => p.PromoRuleResults)
                                                                              .Include(p => p.PromoRuleMops)
                                                                              .FirstOrDefaultAsync() ?? new PromotionHeader();
                PromotionHistory promoHistory = new();
                List<PromotionApprovalDetail> checkNextPrevLevels = new();
                PromoCreated promoCreated = new();

                updatedApprovalStatus.Approve = promoApproval.Approve;
                updatedApprovalStatus.ApprovalDate = DateTime.UtcNow;
                updatedApprovalStatus.ApprovalNotes = promoApproval.ApprovalNotes;
                updatedApprovalStatus.UpdatedDate = DateTime.UtcNow;
                updatedApprovalStatus.UpdatedBy = promoApproval.Username;

                PromotionStatus status = new();
                if (!promoApproval.Approve)
                {
                    status = await _promoStatusService.GetPromotionStatusByNameAsync("REJECT");
                    if (status!.Id != Guid.Empty)
                    {
                        promoHistory = new()
                        {
                            Id = Guid.NewGuid(),
                            PromotionHeaderId = promoApproval.PromotionHeaderId,
                            PromotionStatusId = status.Id,
                            CreatedDate = DateTime.UtcNow,
                            CreatedBy = promoApproval.Username,
                            UpdatedDate = DateTime.UtcNow,
                            UpdatedBy = promoApproval.Username,
                            ActiveFlag = true
                        };

                        promoHeader.PromotionStatusId = status.Id;
                        promoHeader.UpdatedDate = DateTime.UtcNow;
                        promoHeader.UpdatedBy = promoApproval.Username;
                    }
                }

                //cek final approval ato bukan
                bool isFinalApprove = false;
                var finalApprove = await _dbContext.PromotionApprovalDetail!.Where(x => x.PromotionApprovalId == updatedPromoApproval!.Id).OrderByDescending(x => x.ApprovalLevel).FirstOrDefaultAsync();
                if (finalApprove != null && finalApprove.ApprovalLevel == updatedApprovalStatus.ApprovalLevel && promoApproval.Approve)
                {
                    isFinalApprove = true;
                    if (promoHeader!.StartDate <= DateTime.UtcNow && promoHeader.EndDate >= DateTime.UtcNow)
                    {
                        status = await _promoStatusService.GetPromotionStatusByNameAsync("RUN");
                    }
                    else if (promoHeader.StartDate > DateTime.UtcNow)
                    {
                        status = await _promoStatusService.GetPromotionStatusByNameAsync("SCHEDULE");
                    }
                    if (status!.Id != Guid.Empty)
                    {
                        promoHistory = new()
                        {
                            Id = Guid.NewGuid(),
                            PromotionHeaderId = promoApproval.PromotionHeaderId,
                            PromotionStatusId = status.Id,
                            CreatedDate = DateTime.UtcNow,
                            CreatedBy = promoApproval.Username,
                            UpdatedDate = DateTime.UtcNow,
                            UpdatedBy = promoApproval.Username,
                            ActiveFlag = true
                        };

                        promoHeader.PromotionStatusId = status.Id;
                        promoHeader.UpdatedDate = DateTime.UtcNow;
                        promoHeader.UpdatedBy = promoApproval.Username;
                    }
                }

                _dbContext.PromotionApprovalDetail.UpdateRange(checkNextPrevLevels);
                if (promoHistory.Id != Guid.Empty)
                {
                    _dbContext.PromotionHistory.Add(promoHistory);
                    _dbContext.PromotionHeader.Update(promoHeader);
                }

                await _dbContext.SaveChangesAsync();

                //publish event to promotionService
                if (isFinalApprove)
                {
                    promoCreated = (PromoCreated)await GetPromoMessage("Create", promoHeader, promoHeader.PromoRuleRequirements ?? new List<PromotionRuleRequirement>(), promoHeader.PromoRuleResults ?? new List<PromotionRuleResult>(), promoHeader.PromoRuleMops ?? new List<PromotionRuleMopGroup>());
                    _pubService.SendPromoCreatedMessage(promoCreated, "Create");
                }

                return new Tuple<PromotionApprovalDetail, string>(updatedApprovalStatus, "");

            }
            catch (Exception ex)
            {
                return new Tuple<PromotionApprovalDetail, string>(new PromotionApprovalDetail(), ex.Message);
            }
        }
        #endregion

        #region Others
        public DateTime SetSearchDate(PromotionsViewSearch? viewSearch, string startEnd)
        {
            DateTime newSearchDate = DateTime.Parse("1/1/1900");
            if (viewSearch != null && startEnd.Trim().ToUpper() == "START" && viewSearch.StartDate.HasValue)
                newSearchDate = (DateTime)viewSearch.StartDate;
            if (viewSearch != null && startEnd.Trim().ToUpper() == "END" && viewSearch.EndDate.HasValue)
                newSearchDate = (DateTime)viewSearch.EndDate;

            return newSearchDate;
        }
        public List<PromotionListView> SetPromoListViewModel(List<PromotionListResultView> viewResult)
        {
            List<PromotionListView> viewModel = new();
            foreach (var item in viewResult!)
            {
                List<Dept> deptList = string.IsNullOrEmpty(item.Depts) ? new List<Dept>() : JsonConvert.DeserializeObject<List<Dept>>(item.Depts) ?? new List<Dept>();
                string strDepts = "";
                if (deptList.Count > 0)
                {
                    List<string> deptNames = deptList!.Select(x => x.DeptName!).ToList();
                    strDepts = string.Join(", ", deptNames);
                }

                viewModel.Add(
                    new PromotionListView
                    {
                        Id = (Guid)item.PromoId!,
                        PromotionTypeId = item.PromoTypeId!,
                        PromotionType = item.promoTypeName ?? "",
                        PromotionName = item.PromoName ?? "",
                        StartDate = (DateTime)item.StartDate!,
                        EndDate = (DateTime)item.EndDate!,
                        Depts = strDepts,
                        PromotionStatusId = item.PromoStatusId! ?? Guid.Empty,
                        Status = item.PromoStatusName! ?? null,
                        CreatedDate = (DateTime)item.CreatedDate!
                    });
            }
            return viewModel;
        }
        public List<PromotionListView> FilterPromoByDate(List<PromotionListView> vTable, DateTime startDate, DateTime endDate)
        {
            if (endDate == DateTime.Parse("1/1/1900")) //filter by startDate
                return vTable.Where(x => x.StartDate.Date == startDate.Date).ToList();

            if (startDate == DateTime.Parse("1/1/1900")) //filter by endDate
                return vTable.Where(x => x.EndDate.Date == endDate.Date).ToList();

            return vTable.Where(x => (x.StartDate.Date <= startDate.Date && x.EndDate.Date >= startDate.Date)
                                                || (x.StartDate.Date <= endDate.Date && x.EndDate.Date >= endDate.Date)
                                                || (startDate.Date <= x.StartDate.Date && endDate.Date >= x.StartDate.Date)
                                                || (startDate.Date <= x.EndDate.Date && endDate.Date >= x.EndDate.Date)).ToList();
        }
        public List<PromotionListView> FilterPromoBySearchVar(List<PromotionListView> vTable, string searchVar)
        {
            return vTable.Where(a => a.PromotionType!.Trim().ToUpper().Contains(searchVar)
                                  || a.PromotionName!.Trim().ToUpper().Contains(searchVar)
                                  || a.Depts!.Trim().ToUpper().Contains(searchVar)).ToList();
        }
        public List<ApprovalStatus> SetViewStatuses(List<PromotionApprovalDetail> apprStatuses, int? nextApprLvl)
        {
            List<ApprovalStatus> newApprStatuses = new();
            foreach (var item in apprStatuses.OrderBy(x => x.ApprovalLevel))
            {
                ApprovalStatus newStatus = new()
                {
                    ApprovalLevel = item.ApprovalLevel,
                    ApproverId = item.ApproverId,
                    JobPosition = item.JobPosition,
                    ApprvNotes = item.ApprovalDate.HasValue ? item.ApprovalNotes : ""
                };

                if (item.ApprovalDate.HasValue && item.Approve)
                    newStatus.ApprvNotes = string.Concat("Approve - ", newStatus.ApprvNotes);
                if (item.ApprovalDate.HasValue && !item.Approve)
                    newStatus.ApprvNotes = string.Concat("Reject - ", newStatus.ApprvNotes);

                if (newStatus.ApprvNotes!.Length > 0 && newStatus.ApprvNotes!.Trim()[(newStatus.ApprvNotes!.Length - 1)..] == "-")
                    newStatus.ApprvNotes = newStatus.ApprvNotes!.Replace(" - ", "");

                if (nextApprLvl <= 0)
                    newStatus.ApprvStatus = "Done";
                else if (nextApprLvl == item.ApprovalLevel)
                    newStatus.ApprvStatus = "On Review";
                else if (nextApprLvl > item.ApprovalLevel)
                    newStatus.ApprvStatus = "Done";
                else
                    newStatus.ApprvStatus = "Not Started";

                newApprStatuses.Add(newStatus);
            }
            return newApprStatuses;
        }
        public string   ValidateAddPromoRequest(CreatePromotion promoHeader, PromotionClass promoClass, PromotionType promoType)
        {
            string msg = ValidatePromoHeaderRequest(promoHeader, promoClass);
            if (msg != "")
                return msg;

            if (promoHeader.StartDate.Date < DateTime.UtcNow.Date)
                return string.Format("Start date must be equal or greater than {0}", DateTime.UtcNow.ToString("dd-MMM-yyyy"));

            if (promoHeader.EndDate <= promoHeader.StartDate)
                return "End date must be greater than Start date";

            if (promoClass.LineNum == 1)
            {
                msg = ValidatePromoClassLineNum1(promoHeader, promoType);
                if (msg != "")
                    return msg;
            }

            if (promoClass.LineNum == 3)
            {
                if (string.IsNullOrEmpty(promoHeader.MopPromoSelectionCode) || promoHeader.MopPromoSelectionCode!.Trim().ToLower() == "string" || promoHeader.MopPromoSelectionCode!.Trim().ToLower() == "null")
                    return "Please select MOP Selection";
                if (string.IsNullOrEmpty(promoHeader.RuleMops) || promoHeader.RuleMops!.Trim().ToLower() == "[]" || promoHeader.RuleMops!.Trim().ToLower() == "string" || promoHeader.RuleMops!.Trim().ToLower() == "null")
                    return "Please select MOP Grouping";
            }

            if (promoClass.LineNum != 1 && promoClass.LineNum != 4 && (string.IsNullOrEmpty(promoHeader.Value) || promoHeader.Value!.Trim().ToLower() == "string" || promoHeader.Value!.Trim().ToLower() == "null"))
                return "Please insert value";

            if (promoClass.LineNum == 4 && (string.IsNullOrEmpty(promoHeader.NipEntertain!) || promoHeader.NipEntertain!.Trim().ToLower() == "string" || promoHeader.NipEntertain!.Trim().ToLower() == "null"))
                return "NIP is required";

            return "";
        }
        public string GeneratePromotionCode(string companyCode)
        {
            string promoCode = companyCode!.Trim().ToUpper();
            int runningNumber = 1;
            PromotionHeader lastPromoCode = _dbContext.PromotionHeader.Where(x => x.PromotionCode!.Trim().ToUpper().StartsWith(promoCode)).OrderByDescending(x => x.PromotionCode).FirstOrDefault() ?? new PromotionHeader();
            if (lastPromoCode.Id != Guid.Empty && !string.IsNullOrEmpty(lastPromoCode.PromotionCode) && int.TryParse(lastPromoCode.PromotionCode!.AsSpan(4, 7), out runningNumber))
            {
                runningNumber++;
            }

            return String.Format("{0}{1:0000000}", promoCode, runningNumber);
        }
        public string ValidatePromoHeaderRequest(CreatePromotion promoHeader, PromotionClass promoClass)
        {
            #region validate PromotionName
            
            var promoNameExist = _dbContext.PromotionHeader!.Where(x => x.PromotionName!.Trim().ToUpper() == promoHeader.PromotionName!.Trim().ToUpper()).Select(x => x.PromotionName!).FirstOrDefault() ?? "";
            if (!string.IsNullOrEmpty(promoNameExist))
                return string.Format("Promotion Name {0} is already exist", promoHeader.PromotionName);
            #endregion

            #region validate RedemptionCode
            if (!string.IsNullOrEmpty(promoHeader.RedemptionCode))
            {
                var redeemCode = (from pStatus in _dbContext.Set<PromotionStatus>().Where(x => (x.PromotionStatusKey!.Trim().ToUpper().Contains("EXPIRE") || x.PromotionStatusName!.Trim().ToUpper().Contains("EXPIRE")))
                                  from pHead in _dbContext.Set<PromotionHeader>().Where(x => x.CompanyCode == promoHeader.CompanyCode && x.RedemptionCode!.Trim().ToUpper() == promoHeader.RedemptionCode!.Trim().ToUpper() && x.PromotionStatusId != pStatus.Id)
                                  select new
                                  {
                                      pHead.RedemptionCode
                                  }).Select(x => x.RedemptionCode!).FirstOrDefault() ?? "";
                if (!string.IsNullOrEmpty(redeemCode))
                    return string.Format("Redemption Code {0} is already exist", redeemCode);
            }
            #endregion

            #region validate promotionClass promotionType
            if (promoClass.LineNum != 4 && promoHeader.PromotionTypeId == Guid.Empty)
                return "Promotion Type is required";
            #endregion

            return "";
        }
        public async Task<string> ValidateUpdatePromo(PromotionHeader updatedPromoHeader, UpdatePromotion request)
        {
            if (updatedPromoHeader == null || updatedPromoHeader.Id == Guid.Empty)
                return string.Concat("Promotion id: ", request.Id, " is not found");

            if (request.EndDate <= updatedPromoHeader.StartDate)
                return "End date must be greater than Start date";
            if (request.EndDate < DateTime.UtcNow)
                return string.Format("End date must be greater than {0}", DateTime.UtcNow.ToString("dd-MMM-yyyy hh:mm"));

            if (!string.IsNullOrEmpty(request.Zones) && request.Zones!.Trim().ToLower() == "string")
                return "Please check Zones";
            if (!string.IsNullOrEmpty(request.Sites) && request.Sites!.Trim().ToLower() == "string")
                return "Please check Sites";

            PromotionClass promoClass = new();
            var promoClassResponse = await _promoClassService.GetPromotionClassByIdAsync(updatedPromoHeader.PromotionClassId);
            if (promoClassResponse.Item1 != null && promoClassResponse.Item1.Id != Guid.Empty)
                promoClass = promoClassResponse.Item1;
            
            if (promoClass != null && promoClass.Id != Guid.Empty &&
                (promoClass.PromotionClassKey!.Trim().ToUpper() == "ITEM" || promoClass.PromotionClassName!.Trim().ToUpper() == "ITEM"))
            {
                if (string.IsNullOrEmpty(request.RuleReqs) || request.RuleReqs!.Trim().ToLower() == "string" || request.RuleReqs!.Trim().ToLower() == "null" || request.RuleReqs!.Trim().ToLower() == "[]")
                    return "Item requirement is required";
                if (string.IsNullOrEmpty(request.RuleRess) || request.RuleRess!.Trim().ToLower() == "string" || request.RuleRess!.Trim().ToLower() == "null" || request.RuleRess!.Trim().ToLower() == "[]")
                    return "Item result is required";
            }

            //additional for SuperApps
            if (string.IsNullOrEmpty(request.PromoDisplayed!) || request.PromoDisplayed! == null || request.PromoDisplayed!.Trim().ToLower() == "string" || request.PromoDisplayed!.Trim().ToLower() == "null" || request.PromoDisplayed!.Trim() == "[]")
                return "Please insert Promo Displayed";

            if (!string.IsNullOrEmpty(request.ShortDesc) && request.ShortDesc!.Trim().ToLower() == "string")
                return "Please check Short Description";
            if (!string.IsNullOrEmpty(request.PromoTermsCondition) && request.PromoTermsCondition!.Trim().ToLower() == "string")
                return "Please check Terms & Condition";

            return "";
        }
        public List<PromotionAppDisplay> SetPromoAppDisplay(List<PromoDisplayed> promoDisplays)
        {
            List<PromotionAppDisplay> promoAppDisplays = new();
            foreach (var item in promoDisplays)
            {
                promoAppDisplays.Add(new PromotionAppDisplay()
                {
                    AppCode = item.Code,
                    AppName = item.Name
                });
            }
            return promoAppDisplays;
        }
        public int SetMultipleQty(int classLineNum, bool multiple, int maxQty)
        {
            if (classLineNum != 1)
                return 1;

            if (classLineNum == 1 && !multiple && maxQty != 1)
                return 1;

            if (classLineNum == 1 && multiple && maxQty < 2)
                return 2;

            return maxQty;
        }
        public string ValidatePromoClassLineNum1(CreatePromotion promoHeader, PromotionType promoType)
        {
            if ((promoType.PromotionTypeKey!.Trim().ToUpper() == "BUNDLE" || promoType.PromotionTypeKey!.Trim().ToUpper() == "BUNDLING")
                    && (string.IsNullOrEmpty(promoHeader.Value) || promoHeader.Value!.Trim().ToLower() == "string" || promoHeader.Value!.Trim().ToLower() == "null"))
                return "Please input bundling price";

            if (!(promoType.PromotionTypeKey!.Trim().ToUpper() == "BUNDLE" || promoType.PromotionTypeKey!.Trim().ToUpper() == "BUNDLING"))
            {
                if (string.IsNullOrEmpty(promoHeader.RequirementExp) || promoHeader.RequirementExp.Trim().ToLower() == "string")
                    return "Please select requirement criteria";
                if (string.IsNullOrEmpty(promoHeader.ResultExp) || promoHeader.ResultExp.Trim().ToLower() == "string")
                    return "Please select result criteria";
            }

            if (string.IsNullOrEmpty(promoHeader.RuleReqs) || promoHeader.RuleReqs!.Trim().ToLower() == "string" || promoHeader.RuleReqs!.Trim().ToLower() == "null" || promoHeader.RuleReqs!.Trim().ToLower() == "[]")
                return "Item requirement is required";
            if (string.IsNullOrEmpty(promoHeader.RuleRess) || promoHeader.RuleRess!.Trim().ToLower() == "string" || promoHeader.RuleRess!.Trim().ToLower() == "null" || promoHeader.RuleRess!.Trim().ToLower() == "[]")
                return "Item result is required";

            return "";
        }
        #endregion
    }
}
