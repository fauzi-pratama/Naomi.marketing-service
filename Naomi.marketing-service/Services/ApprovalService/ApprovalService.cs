using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Naomi.marketing_service.Models.Contexts;
using Naomi.marketing_service.Models.Entities;
using Naomi.marketing_service.Models.Request;
using Naomi.marketing_service.Services.PromoStatusService;
using Naomi.marketing_service.Services.PubService;
using static Naomi.marketing_service.Models.Response.ApprovalMappingResponse;

namespace Naomi.marketing_service.Services.ApprovalService
{
    public class ApprovalService : IApprovalService
    {
        private readonly DataDbContext _dbContext;
        public readonly IMapper _mapper;
        private readonly IPromoStatusService _promoStatusService;

        public ApprovalService(DataDbContext dbContext, IMapper mapper, IPromoStatusService promoStatusService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _promoStatusService = promoStatusService;
        }

        #region GetData
        public async Task<Tuple<List<ApprovalMappingView>, string>> GetApprovalMapping(Guid? companyId, string? companyCode)
        {
            if ((companyId == null || companyId == Guid.Empty) && (string.IsNullOrEmpty(companyCode) || companyCode!.Trim().ToLower() == "string"))
                return new Tuple<List<ApprovalMappingView>, string>(new List<ApprovalMappingView>(), "Company is required");

            List<ApprovalMappingView> approvalMappings = new();

            ApprovalMapping apprvMapping = new();
            if (companyId != null && companyId != Guid.Empty)
                apprvMapping = await _dbContext.ApprovalMapping.Where(x => x.CompanyId == companyId).FirstOrDefaultAsync() ?? new ApprovalMapping();
            else if (!string.IsNullOrEmpty(companyCode!))
                apprvMapping = await _dbContext.ApprovalMapping.Where(x => x.CompanyCode!.Trim().ToUpper() == companyCode!.Trim().ToUpper()).FirstOrDefaultAsync() ?? new ApprovalMapping();

            if (apprvMapping.Id != Guid.Empty)
            {
                ApprovalMappingView viewModelHeader = new()
                {
                    Id = apprvMapping.Id,
                    CompanyId = apprvMapping.CompanyId,
                    CompanyCode = apprvMapping.CompanyCode,
                    ActiveFlag = apprvMapping.ActiveFlag
                };

                //get details
                var viewDetail = _dbContext.ApprovalMappingDetail!.Where(x => x.ApprovalMappingId == apprvMapping.Id).OrderBy(x => x.ApprovalLevel).ToList();
                List<ApprovalMappingViewDetail> approvalMappingDetails = _mapper.Map<List<ApprovalMappingViewDetail>>(viewDetail);

                viewModelHeader.ApprovalMappingList = approvalMappingDetails;
                approvalMappings.Add(viewModelHeader);
            }

            return new Tuple<List<ApprovalMappingView>, string>(approvalMappings.OrderBy(x => x.Id).ToList() ?? new List<ApprovalMappingView>(), "");
        }
        public async Task<Tuple<ApprovalMappingView?, string>> GetApprovalMappingById(Guid approvalMappingId)
        {
            if (approvalMappingId == Guid.Empty)
                return new Tuple<ApprovalMappingView?, string>(new ApprovalMappingView(), "Approval mapping Id is required");

            ApprovalMappingView? approvalMappingView = null;
            List<ApprovalMappingViewDetail> approvalMappingViewDetails = new();

            ApprovalMapping? mappingHeader = await _dbContext.ApprovalMapping!.FirstOrDefaultAsync(x => x.Id == approvalMappingId);
            List<ApprovalMappingDetail>? mappingDetails = _dbContext.ApprovalMappingDetail!.Where(x => x.ApprovalMappingId == approvalMappingId).OrderBy(x => x.ApprovalLevel).ToList();

            if (mappingHeader != null && mappingDetails != null)
            {
                foreach (var itemDetail in mappingDetails)
                {
                    ApprovalMappingViewDetail mappingDetail = new()
                    {
                        Id = itemDetail.Id,
                        ApprovalLevel = itemDetail.ApprovalLevel,
                        ApproverId = itemDetail.ApproverId,
                        JobPosition = itemDetail.JobPosition
                    };
                    approvalMappingViewDetails.Add(mappingDetail);
                }

                approvalMappingView = new()
                {
                    Id = mappingHeader.Id,
                    CompanyId = mappingHeader.CompanyId,
                    CompanyCode = mappingHeader.CompanyCode,
                    ActiveFlag = mappingHeader.ActiveFlag,
                    ApprovalMappingList = approvalMappingViewDetails
                };
            }

            return new Tuple<ApprovalMappingView?, string>(approvalMappingView ?? new ApprovalMappingView(), "");
        }
        #endregion

        #region InsertData
        public async Task<Tuple<ApprovalMappingView, string>> InsertApprovalMapping(ApprovalMappingView approvalMapping)
        {
            //check if exist
            ApprovalMapping apprvMap = await _dbContext.ApprovalMapping.Where(x => x.CompanyId == approvalMapping.CompanyId).FirstOrDefaultAsync() ?? new ApprovalMapping();
            if (apprvMap != null && apprvMap.Id != Guid.Empty)
                return new Tuple<ApprovalMappingView, string>(approvalMapping, string.Format("Approval Mapping for Company {0} is already exist. Please use Edit Approval to modify the data.", approvalMapping.CompanyCode));

            try
            {
                Guid headerId = Guid.NewGuid();
                approvalMapping.Id = headerId;
                ApprovalMapping apprvMapping = new()
                {
                    Id = headerId,
                    CompanyId = approvalMapping.CompanyId,
                    CompanyCode = approvalMapping.CompanyCode,
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = approvalMapping.Username,
                    UpdatedDate = DateTime.UtcNow,
                    UpdatedBy = approvalMapping.Username,
                    ActiveFlag = true
                };

                List<ApprovalMappingDetail> approvalMappingDetails = new();
                foreach (var mappingDetail in approvalMapping.ApprovalMappingList!)
                {
                    ApprovalMappingDetail newDetail = new()
                    {
                        Id = Guid.NewGuid(),
                        ApprovalMappingId = headerId,
                        ApprovalLevel = mappingDetail.ApprovalLevel,
                        ApproverId = mappingDetail.ApproverId,
                        JobPosition = mappingDetail.JobPosition,
                        CreatedDate = DateTime.UtcNow,
                        CreatedBy = approvalMapping.Username,
                        UpdatedDate = DateTime.UtcNow,
                        UpdatedBy = approvalMapping.Username,
                        ActiveFlag = true
                    };
                    approvalMappingDetails.Add(newDetail);
                }

                _dbContext.ApprovalMapping.Add(apprvMapping);
                _dbContext.ApprovalMappingDetail.AddRange(approvalMappingDetails);

                await _dbContext.SaveChangesAsync();

                ////call GeneratePromoApproval
                //GeneratePromoApproval request = new()
                //{
                //    CompanyId = approvalMapping.CompanyId,
                //    BrandId = approvalMapping.BrandId
                //};
                //await GeneratePromoApproval(request);

                return new Tuple<ApprovalMappingView, string>(approvalMapping, "Data berhasil disimpan");
            }
            catch (Exception ex)
            {
                return new Tuple<ApprovalMappingView, string>(new ApprovalMappingView(), ex.Message);
            }

            
        }
        #endregion

        #region UpdateData
        public async Task<Tuple<ApprovalMappingView, string>> UpdateApprovalMapping(ApprovalMappingView approvalMapping)
        {
            ApprovalMapping? existingApprvMapping = await _dbContext.ApprovalMapping!.Where(x => x.Id == approvalMapping.Id!)
                                                                                     .Include(c => c.ApprovalMappingDetail).FirstOrDefaultAsync();

            if (existingApprvMapping == null || existingApprvMapping.Id == Guid.Empty)
                return new Tuple<ApprovalMappingView, string>(new ApprovalMappingView(), string.Format("Approval Mapping Id {0} is not found", approvalMapping.Id));

            try
            {
                existingApprvMapping.ActiveFlag = approvalMapping.ActiveFlag;
                existingApprvMapping.UpdatedDate = DateTime.UtcNow;
                existingApprvMapping.UpdatedBy = approvalMapping.Username;

                //delete existing ApprovalMappingDetail
                if (existingApprvMapping.ApprovalMappingDetail!.Count > 0)
                    _dbContext.ApprovalMappingDetail.RemoveRange(existingApprvMapping.ApprovalMappingDetail!);

                List<ApprovalMappingDetail> approvalMappingDetails = new();
                foreach (var mappingDetail in approvalMapping.ApprovalMappingList!)
                {
                    ApprovalMappingDetail newDetail = new()
                    {
                        ApprovalLevel = mappingDetail.ApprovalLevel,
                        ApproverId = mappingDetail.ApproverId,
                        JobPosition = mappingDetail.JobPosition,
                        CreatedDate = DateTime.UtcNow,
                        CreatedBy = approvalMapping.Username,
                        UpdatedDate = DateTime.UtcNow,
                        UpdatedBy = approvalMapping.Username,
                        ActiveFlag = approvalMapping.ActiveFlag
                    };
                    approvalMappingDetails.Add(newDetail);
                }
                existingApprvMapping.ApprovalMappingDetail = approvalMappingDetails;
                
                await _dbContext.SaveChangesAsync();

                return new Tuple<ApprovalMappingView, string>(approvalMapping, "Data has been saved");
            }
            catch (Exception ex)
            {
                return new Tuple<ApprovalMappingView, string>(new ApprovalMappingView(), ex.Message);
            }
            
        }
        #endregion

        #region GeneratePromoApproval
        public async Task<List<PromotionApproval>> GeneratePromoApproval(GeneratePromoApproval request)
        {
            ApprovalMapping approvalMapping = new();
            List<ApprovalMappingDetail> approvalMappingDetails = new();
            List<PromotionApproval> promoApprovals = new();
            List<PromotionApprovalDetail> promoApprvDetails = new();
            List<PromotionHeader> promotionHeaders = new();
            List<PromotionHistory> promotionHistories = new();
            Guid headerId = Guid.NewGuid(); 
            PromotionStatus statusDraft = await _promoStatusService.GetPromotionStatusByNameAsync("DRAFT");

            if (request.CompanyId != Guid.Empty)
            {
                approvalMapping = await _dbContext.ApprovalMapping.Where(x => x.CompanyId == request.CompanyId && x.ActiveFlag).FirstOrDefaultAsync() ?? new ApprovalMapping();
                if (approvalMapping == null || approvalMapping.Id == Guid.Empty)
                    return promoApprovals;

                approvalMappingDetails = await _dbContext.ApprovalMappingDetail!.Where(x => x.ApprovalMappingId == approvalMapping.Id).ToListAsync() ?? new List<ApprovalMappingDetail>();

                if (request.PromotionHeaderId != Guid.Empty)
                {
                    promoApprovals.Add(SetPromoApprovalHeader(headerId, approvalMapping.Id, request.PromotionHeaderId));
                    promoApprvDetails.AddRange(SetPromoApprovalDetail(approvalMappingDetails, headerId));
                }
                else
                {
                    var promoHeaderIds = await _dbContext.PromotionApproval!.Select(x => x.PromotionHeaderId).Distinct().ToListAsync();
                    promotionHeaders = await _dbContext.PromotionHeader!.Where(x => !promoHeaderIds.Contains(x.Id) && x.CompanyId == request.CompanyId && x.StartDate.Date >= DateTime.UtcNow.Date).ToListAsync() ?? new List<PromotionHeader>();
                    foreach (PromotionHeader promoHeader in promotionHeaders)
                    {
                        headerId = Guid.NewGuid();
                        promoApprovals.Add(SetPromoApprovalHeader(headerId, approvalMapping.Id, promoHeader.Id));
                        promoApprvDetails.AddRange(SetPromoApprovalDetail(approvalMappingDetails, headerId));

                        //set promo status back to draft
                        if (statusDraft != null)
                        {
                            PromotionHistory newPromoHistory = new()
                            {
                                Id = Guid.NewGuid(),
                                PromotionHeaderId = promoHeader.Id,
                                PromotionStatusId = statusDraft.Id,
                                CreatedDate = DateTime.UtcNow,
                                CreatedBy = promoHeader.Username,
                                UpdatedDate = DateTime.UtcNow,
                                UpdatedBy = promoHeader.Username,
                                ActiveFlag = true
                            };
                            promotionHistories.Add(newPromoHistory);
                        }

                        promoHeader.PromotionStatusId = statusDraft!.Id;
                        _dbContext.PromotionHeader.Update(promoHeader);
                    }
                }

                _dbContext.PromotionApproval.AddRange(promoApprovals);
                _dbContext.PromotionApprovalDetail.AddRange(promoApprvDetails);
                _dbContext.PromotionHistory.AddRange(promotionHistories);
                await _dbContext.SaveChangesAsync();
            }

            return promoApprovals;
        }
        public PromotionApproval SetPromoApprovalHeader(Guid promoApprId, Guid apprvMapId, Guid promoHeaderId)
        {
            PromotionApproval newPromoApproval = new()
            {
                Id = promoApprId,
                ApprovalMappingId = apprvMapId,
                PromotionHeaderId = promoHeaderId,
                ActiveFlag = true,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow
            };

            return newPromoApproval;
        }
        public List<PromotionApprovalDetail> SetPromoApprovalDetail(List<ApprovalMappingDetail> approvalDetails, Guid apprvHeaderId)
        {
            List<PromotionApprovalDetail> newPromoApprvDetail = new();
            foreach (var detil in approvalDetails)
            {
                newPromoApprvDetail.Add(
                    new PromotionApprovalDetail
                    {
                        Id = Guid.NewGuid(),
                        PromotionApprovalId = apprvHeaderId,
                        ApprovalLevel = detil.ApprovalLevel,
                        ApproverId = detil.ApproverId,
                        JobPosition = detil.JobPosition,
                        Approve = false,
                        ApprovalDate = null,
                        ApprovalNotes = null,
                        ActiveFlag = true,
                        CreatedDate = DateTime.UtcNow,
                        UpdatedDate = DateTime.UtcNow
                    });
            }
            return newPromoApprvDetail;
        }
        #endregion
    }
}
