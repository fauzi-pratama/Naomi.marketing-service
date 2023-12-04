using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Naomi.marketing_service.Models.Contexts;
using Naomi.marketing_service.Models.Entities;
using Naomi.marketing_service.Models.Message.Pub;
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
        private readonly IPubService _pubService;
        private readonly IPromoStatusService _promoStatusService;

        public ApprovalService(DataDbContext dbContext, IMapper mapper, IPubService pubService, IPromoStatusService promoStatusService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _pubService = pubService;
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
                    }

                    ////getPromoMessage
                    //promoCreated = (PromoCreated)await GetPromoMessage("Create", promoHeader, promoHeader.PromoRuleRequirements ?? new List<PromotionRuleRequirement>(), promoHeader.PromoRuleResults ?? new List<PromotionRuleResult>(), promoHeader.PromoRuleMops ?? new List<PromotionRuleMopGroup>());
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
                    _pubService.SendPromoCreatedMessage(promoHeader, "Create");
                }

                return new Tuple<PromotionApprovalDetail, string>(updatedApprovalStatus, "");

            }
            catch (Exception ex)
            {
                return new Tuple<PromotionApprovalDetail, string>(new PromotionApprovalDetail(), ex.Message);
            }
        }
        #endregion
    }
}
