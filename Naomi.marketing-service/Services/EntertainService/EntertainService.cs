using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Naomi.marketing_service.Models.Contexts;
using Naomi.marketing_service.Models.Entities;
using Naomi.marketing_service.Models.Message.Pub;
using Naomi.marketing_service.Models.Request;
using Naomi.marketing_service.Models.Response;
using Naomi.marketing_service.Services.PubService;
using static Naomi.marketing_service.Models.Request.EntertainBudgetRequest;
using static Naomi.marketing_service.Models.Response.EntertainBudgetResponse;
using System.Linq.Dynamic.Core;
using System.Reflection.Metadata.Ecma335;
using Amazon.S3.Model.Internal.MarshallTransformations;
using Microsoft.AspNetCore.Identity;

namespace Naomi.marketing_service.Services.EntertainService
{
    public class EntertainService : IEntertainService
    {
        private readonly DataDbContext _dbContext;
        public readonly IMapper _mapper;
        private readonly IPubService _pubService;

        public EntertainService(DataDbContext dbContext, IMapper mapper, IPubService pubService)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _pubService = pubService;
        }

        #region GetData
        public async Task<Tuple<List<NipEntertainResponse>, int, string>> GetNipEntertain(DateTime? monthYear, string? searchName, int pageNo = 1, int pageSize = 10)
        {
            if (monthYear == null)
                return new Tuple<List<NipEntertainResponse>, int, string>(new List<NipEntertainResponse>(), 1, "Period is required");

            List<NipEntertainResponse> nipEntertainList = new();
            List<PromotionEntertain> promoEntertainList = await _dbContext.PromotionEntertain.Where(x => x.ActiveFlag && x.StartDate.HasValue && x.StartDate!.Value.Month == monthYear!.Value.Month && x.StartDate!.Value.Year == monthYear!.Value.Year).ToListAsync() ?? new List<PromotionEntertain>(); 
            int Skip = (pageNo - 1) * pageSize;

            if (!string.IsNullOrEmpty(searchName))
                promoEntertainList = promoEntertainList.Where(x => x.EmployeeNIP!.Trim().ToUpper().Contains(searchName.Trim().ToUpper()) || x.EmployeeName!.Trim().ToUpper().Contains(searchName.Trim().ToUpper())).ToList();

            int totalPages = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(promoEntertainList.Count) / Convert.ToDouble(pageSize)));
            promoEntertainList = promoEntertainList.OrderBy(x => x.StartDate!).ThenBy(x => x.EmployeeNIP).Skip(Skip).Take(pageSize).ToList();

            foreach (var item in promoEntertainList)
            {
                nipEntertainList.Add
                (
                    new NipEntertainResponse
                    {
                        EmployeeNIP = item.EmployeeNIP,
                        EmployeeName = item.EmployeeName,
                        JobPosition = item.JobPosition,
                        BudgetEntertain = item.EntertainBudget
                    }
                );
            }
            return new Tuple<List<NipEntertainResponse>, int, string>(nipEntertainList ?? new List<NipEntertainResponse>(), totalPages, "");
        }

        public async Task<Tuple<List<PromoEntertainListView>, int, string>> GetPromoEntertainListAsync(string orderColumn, string orderMethod, int pageNo = 1, int pageSize = 10, PromotionsViewSearch? viewSearch = null)
        {
            DateTime startDate = DateTime.Parse("1/1/1900");
            DateTime endDate = DateTime.Parse("1/1/1900");

            if (viewSearch == null || (viewSearch != null && !viewSearch.StartDate.HasValue))
                startDate = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);
            else if (viewSearch != null && viewSearch.StartDate.HasValue)
                startDate = (DateTime)viewSearch.StartDate;

            if (viewSearch == null || (viewSearch != null && !viewSearch.EndDate.HasValue))
            {
                endDate = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);
                endDate = endDate.AddMonths(1);
                endDate = endDate.AddDays(-1);
            }
            else if (viewSearch != null && viewSearch.EndDate.HasValue)
                endDate = (DateTime)viewSearch.EndDate;

            if (startDate != DateTime.Parse("1/1/1900") && endDate != DateTime.Parse("1/1/1900") && endDate < startDate)
                return new Tuple<List<PromoEntertainListView>, int, string>(new List<PromoEntertainListView>(), 1, "End date must be equal or greater than Start date");

            if (string.IsNullOrEmpty(orderColumn)) orderColumn = "EmployeeNIP";
            if (string.IsNullOrEmpty(orderMethod)) orderMethod = "ASC";

            int Skip = (pageNo - 1) * pageSize;
            List<PromoEntertainListView> viewTable = await (from pEntertain in _dbContext.Set<PromotionEntertain>()
                                                            select new PromoEntertainListView
                                                            {
                                                                Id = pEntertain.Id,
                                                                EmployeeNIP = pEntertain.EmployeeNIP ?? "",
                                                                EmployeeName = pEntertain.EmployeeName ?? "",
                                                                JobPosition = pEntertain.JobPosition ?? "",
                                                                StartDate = pEntertain.StartDate,
                                                                EndDate = pEntertain.EndDate,
                                                                EntertainBudget = pEntertain.EntertainBudget
                                                            }).ToListAsync();

            if (startDate != DateTime.Parse("1/1/1900") || endDate != DateTime.Parse("1/1/1900"))
                viewTable = FilterEntPromoByDate(viewTable, startDate, endDate);

            if (viewSearch != null && !string.IsNullOrEmpty(viewSearch!.PromotionSearch))
                viewTable = FilterEntPromoBySearchVar(viewTable, viewSearch.PromotionSearch.Trim().ToUpper());

            int totalPages = (int)Math.Ceiling((double)viewTable.Count / (double)pageSize);
            viewTable = viewTable.AsQueryable().OrderBy(orderColumn + " " + orderMethod).Skip(Skip).Take(pageSize).ToList() ?? new List<PromoEntertainListView>();
            return new Tuple<List<PromoEntertainListView>, int, string>(viewTable, totalPages, "");
        }

        public async Task<Tuple<PromotionEntertain, string>> GetPromoEntertainByNIP(string? empNIP, DateTime? monthYear)
        {
            if (string.IsNullOrEmpty(empNIP))
                return new Tuple<PromotionEntertain, string>(new PromotionEntertain(), "Employee NIP is required");

            DateTime startDate = new(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);
            if (monthYear.HasValue)
                startDate = new(monthYear.Value.Year, monthYear.Value.Month, 1);

            PromotionEntertain promoEntertain = await _dbContext.PromotionEntertain.Include(p => p.EmpEmails!.Where(x => (bool)x.DefaultEmail!))
                                                      .Where(x => x.EmployeeNIP!.Trim().ToUpper() == empNIP!.Trim().ToUpper() && x.StartDate!.Value == startDate)
                                                      .OrderByDescending(x => x.StartDate!)
                                                      .FirstOrDefaultAsync() ?? new PromotionEntertain();
            return new Tuple<PromotionEntertain, string>(promoEntertain, "");
        }

        public async Task<PromotionEntertain> GetEntertainByIdAsync(Guid id)
        {
            return await _dbContext.PromotionEntertain.Where(x => x.Id == id)
                                                      .Include(p => p.EmpEmails)
                                                      .FirstOrDefaultAsync() ?? new PromotionEntertain();
        }
        #endregion

        #region InsertData
        public async Task<Tuple<PromotionEntertain, string>> CreateEntertain(CreateEntertain promotionEntertain)
        {
            //set startdate, enddate
            DateTime monthYear = promotionEntertain.MonthYear;

            DateTime sDate = new(monthYear.Year, monthYear.Month, 1);
            DateTime eDate = sDate.AddMonths(1);
            eDate = eDate.AddDays(-1);

            string msg = ValidateCreateEntPromo(promotionEntertain);
            if (msg != "")
                return new Tuple<PromotionEntertain, string>(new PromotionEntertain(), msg);

            PromotionEntertain entertainData = new()
            {
                Id = Guid.NewGuid(),
                EmployeeNIP = promotionEntertain.EmployeeNIP,
                EmployeeName = promotionEntertain.EmployeeName,
                JobPosition = promotionEntertain.JobPosition,
                StartDate = sDate,
                EndDate = eDate,
                EntertainBudget = promotionEntertain.EntertainBudget,
                ActiveFlag = promotionEntertain.ActiveFlag ?? true,
                CreatedDate = DateTime.UtcNow,
                CreatedBy = promotionEntertain.Username,
                UpdatedDate = DateTime.UtcNow,
                UpdatedBy = promotionEntertain.Username
            };


            entertainData.EmpEmails = SetPromoEntertainEmail(promotionEntertain!.EmpEmails!, promotionEntertain.Username!);
            PromoEmailUser promoEmailUser = new()
            {
                Nip = promotionEntertain.EmployeeNIP,
                Email = promotionEntertain!.EmpEmails!.Where(x => x.DefaultEmail).Select(x => x.Email).FirstOrDefault(),
                ActiveFlag = entertainData.ActiveFlag
            };


            // Add entry to the DB context.
            _dbContext.PromotionEntertain.Add(entertainData);

            await _dbContext.SaveChangesAsync();
            _pubService.SendPromoEmailUserMessage(promoEmailUser);

            return new Tuple<PromotionEntertain, string>(entertainData, "Data has been saved");
        }
        #endregion

        #region UpdateData
        public async Task<Tuple<PromotionEntertain, string>> UpdateEntertain(UpdateEntertain updateEntertain)
        {
            //set startdate, enddate
            DateTime monthYear = updateEntertain.MonthYear;

            DateTime sDate = new(monthYear.Year, monthYear.Month, 1);
            DateTime eDate = sDate.AddMonths(1);
            eDate = eDate.AddDays(-1);

            PromotionEntertain existingEntertain = await GetEntertainByIdAsync(updateEntertain.Id);
            if (existingEntertain == null || existingEntertain.Id == Guid.Empty)
                return new Tuple<PromotionEntertain, string>(new PromotionEntertain(), "Data not found");

            string msg = ValidateUpdateEntPromo(updateEntertain, existingEntertain);
            if (msg != "")
                return new Tuple<PromotionEntertain, string>(new PromotionEntertain(), msg);

            _dbContext.PromotionEntertainEmail.RemoveRange(existingEntertain.EmpEmails!);

            existingEntertain.EntertainBudget = updateEntertain.EntertainBudget;
            existingEntertain.StartDate = sDate;
            existingEntertain.EndDate = eDate;
            existingEntertain.ActiveFlag = updateEntertain.ActiveFlag ?? false;
            existingEntertain.UpdatedDate = DateTime.UtcNow;
            existingEntertain.UpdatedBy = updateEntertain.Username;
            existingEntertain.EmpEmails = SetPromoEntertainEmail(updateEntertain!.EmpEmails!, updateEntertain.Username!);

            PromoEmailUser promoEmailUser = new()
            {
                Nip = existingEntertain.EmployeeNIP,
                Email = updateEntertain!.EmpEmails!.Where(x => x.DefaultEmail).Select(x => x.Email).FirstOrDefault(),
                ActiveFlag = updateEntertain.ActiveFlag
            };


            await _dbContext.SaveChangesAsync();
            _pubService.SendPromoEmailUserMessage(promoEmailUser);

            return new Tuple<PromotionEntertain, string>(existingEntertain, "Data has been save");
        }
        #endregion

        #region Validation
        public string ValidateCreateEntPromo(CreateEntertain promoEntertain)
        {
            //validate monthyear
            DateTime monthYear = new(promoEntertain.MonthYear.Year, promoEntertain.MonthYear.Month, 1);
            var msg = ValidateBudgetPeriod(monthYear);
            if (msg != "") return msg;

            PromotionEntertain? dataExist = _dbContext.PromotionEntertain.Where(x => x.EmployeeNIP!.Trim().ToUpper() == promoEntertain.EmployeeNIP!.Trim().ToUpper() && x.StartDate.HasValue && x.StartDate!.Value.Year == monthYear.Year && x.StartDate!.Value.Month == monthYear.Month).FirstOrDefault();
            if (dataExist != null && dataExist.Id != Guid.Empty)
                return string.Format("Data for {0} is already exist", promoEntertain.MonthYear.ToString("MMM yyyy"));

            //check if email is valid
            msg = ValidateEntEmails(promoEntertain.EmpEmails!);
            if (msg != "") return msg;

            return "";
        }
        public string ValidateUpdateEntPromo(UpdateEntertain updateEntertain, PromotionEntertain existingEntertain)
        {
            if (updateEntertain.EntertainBudget == 0)
                return "Budget is required";

            //validate monthyear
            DateTime monthYear = new(updateEntertain.MonthYear.Year, updateEntertain.MonthYear.Month, 1);
            var msg = ValidateBudgetPeriod(monthYear);
            if (msg != "") return string.Format("Cannot edit past period ({0})", monthYear.ToString("MMM yyyy"));

            if (existingEntertain.StartDate!.Value.Year == DateTime.UtcNow.Year && existingEntertain.StartDate!.Value.Month == DateTime.UtcNow.Month && existingEntertain.EntertainBudget != updateEntertain.EntertainBudget)
                return string.Format("Cannot edit budget for running period ({0})", existingEntertain.StartDate!.Value.ToString("MMM yyyy"));

            if (existingEntertain.StartDate!.Value.Year == DateTime.UtcNow.Year && existingEntertain.StartDate!.Value.Month == DateTime.UtcNow.Month && existingEntertain.StartDate! != monthYear)
                return string.Format("Cannot change month year for running period ({0})", existingEntertain.StartDate!.Value.ToString("MMM yyyy"));

            PromotionEntertain? dataExist = _dbContext.PromotionEntertain.Where(x => x.EmployeeNIP!.Trim().ToUpper() == existingEntertain.EmployeeNIP!.Trim().ToUpper() && x.StartDate.HasValue && x.StartDate!.Value.Year == monthYear.Year && x.StartDate!.Value.Month == monthYear.Month && x.Id != existingEntertain.Id).FirstOrDefault();
            if (dataExist != null && dataExist.Id != Guid.Empty)
                return string.Format("Data for {0} is already exist", updateEntertain.MonthYear.ToString("MMM yyyy"));

            //check if email is valid
            msg = ValidateEntEmails(updateEntertain.EmpEmails!);
            if (msg != "") return msg;

            return "";
        }
        public string ValidateBudgetPeriod(DateTime entPeriod)
        {
            if (entPeriod < new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1))
                return string.Format("Budget period must be equal or greater than {0}", DateTime.UtcNow.ToString("MMM yyyy"));

            return "";
        }
        public string ValidateEntEmails(List<EmpEmail> entEmails)
        {
            if (entEmails == null || entEmails.Count == 0)
                return "Email is required";

            foreach (var item in entEmails.Select(x => x.Email))
            {
                if (string.IsNullOrEmpty(item))
                    return "Email is required";
                else
                {
                    var atIndex = item.IndexOf("@");
                    if (atIndex <= 0)
                        return "Please insert valid email address";

                    var dotIndex = item.LastIndexOf(".");
                    if ((dotIndex < atIndex) || (dotIndex == item.Length - 1))
                        return "Please insert valid email address";
                }
            }
            var defEmail = entEmails.Count(x => x.DefaultEmail);
            if (defEmail == 0)
                return "Please select default email";
            if (defEmail > 1)
                return "Please select only one default email";

            return "";
        }
        #endregion

        #region Others
        public List<PromotionEntertainEmail> SetPromoEntertainEmail(List<EmpEmail> empEmails, string? username)
        {
            List<PromotionEntertainEmail> newEmpEmails = new();
            foreach (var item in empEmails)
            {
                newEmpEmails.Add(new PromotionEntertainEmail()
                {
                    Email = item.Email,
                    DefaultEmail = item.DefaultEmail,
                    ActiveFlag = true,
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = username,
                    UpdatedDate = DateTime.UtcNow,
                    UpdatedBy = username
                });
            }
            return newEmpEmails;
        }
        public List<PromoEntertainListView> FilterEntPromoByDate(List<PromoEntertainListView> vTable, DateTime startDate, DateTime endDate)
        {
            vTable = vTable.Where(x => x.StartDate != null && x.EndDate != null).ToList();

            if (endDate == DateTime.Parse("1/1/1900")) //filter by startDate
                return vTable.Where(x => ((DateTime)x.StartDate!).Date == startDate).ToList();

            if (startDate == DateTime.Parse("1/1/1900")) // filter by endDate
                return vTable.Where(x => ((DateTime)x.EndDate!).Date == endDate).ToList();

            return vTable.Where(x => ((DateTime)x.StartDate!).Date >= startDate.Date && ((DateTime)x.EndDate!).Date <= endDate.Date).ToList();
        }
        public List<PromoEntertainListView> FilterEntPromoBySearchVar(List<PromoEntertainListView> vTable, string searchVar)
        {
            return vTable.Where(x => x.EmployeeNIP!.Trim().ToUpper().Contains(searchVar)
                                  || x.EmployeeName!.Trim().ToUpper().Contains(searchVar)
                                  || x.JobPosition!.Trim().ToUpper().Contains(searchVar)).ToList();
        }
        #endregion

        #region EntertainJob
        public async Task CreateEntertainBudgetAuto()
        {
            DateTime currentPeriod = new(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);
            DateTime currentPeriodEnd = currentPeriod.AddMonths(1);
            currentPeriodEnd = currentPeriodEnd.AddDays(-1);
            DateTime lastPeriod = currentPeriod.AddMonths(-1);

            List<PromotionEntertain> prevBudgets = await _dbContext.PromotionEntertain.Include(p => p.EmpEmails)
                                                                                      .Where(x => x.ActiveFlag && x.StartDate.HasValue && x.StartDate!.Value.Year == lastPeriod.Year && x.StartDate!.Value.Month == lastPeriod.Month).ToListAsync();

            List<PromotionEntertain> newEntBudgets = new();
            foreach (var budget in prevBudgets)
            {
                //check if exist
                PromotionEntertain? currEntBudget = await _dbContext.PromotionEntertain.Where(x => x.EmployeeNIP == budget.EmployeeNIP && x.StartDate!.HasValue && x.StartDate!.Value.Year == currentPeriod.Year && x.StartDate!.Value.Month == currentPeriod.Month).FirstOrDefaultAsync();
                if (currEntBudget == null)
                {
                    PromotionEntertain newBudget = new()
                    {
                        Id = Guid.NewGuid(),
                        EmployeeNIP = budget.EmployeeNIP,
                        EmployeeName = budget.EmployeeName,
                        JobPosition = budget.JobPosition,
                        StartDate = currentPeriod,
                        EndDate = currentPeriodEnd,
                        EntertainBudget = budget.EntertainBudget,
                        ActiveFlag = true,
                        CreatedDate = DateTime.UtcNow,
                        CreatedBy = "ENTERTAINJOB",
                        UpdatedDate = DateTime.UtcNow,
                        UpdatedBy = "ENTERTAINJOB"
                    };
                    newBudget.EmpEmails = SetPromoEntertainEmail(_mapper.Map<List<EmpEmail>>(budget.EmpEmails!), "ENTERTAINJOB");

                    newEntBudgets.Add(newBudget);
                }
            }

            if (newEntBudgets.Count > 0)
            {
                _dbContext.PromotionEntertain.AddRange(newEntBudgets);
                await _dbContext.SaveChangesAsync();
            }
        }
        #endregion
    }
}
