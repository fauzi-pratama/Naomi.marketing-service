using Naomi.marketing_service.Models.Contexts;
using Naomi.marketing_service.Models.Message.Pub;

namespace Naomi.marketing_service.Services.MembershipService
{
    public class MembershipService : IMembershipService
    {
        
        public MembershipService()
        {
            
        }

        #region GetData
        public async Task<Tuple<List<Member>, int>> GetMember(string? searchName, int pageNo, int pageSize)
        {
            List<Member> viewMembers = new()
            {
                new Member()
                {
                    Status = "SILVER"
                },
                new Member()
                {
                    Status = "GOLD"
                },
                new Member()
                {
                    Status = "PLATINUM"
                }
            };
            int Skip = (pageNo - 1) * pageSize;

            if (!string.IsNullOrEmpty(searchName))
                viewMembers = viewMembers.Where(x => x.Status!.Trim().ToUpper().Contains(searchName.Trim().ToUpper())).ToList() ?? new List<Member>();

            int totalPages = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(viewMembers.Count) / Convert.ToDouble(pageSize)));
            viewMembers = viewMembers.OrderBy(x => x.Status).Skip(Skip).Take(pageSize).ToList();
            return new Tuple<List<Member>, int>(viewMembers ?? new List<Member>(), totalPages);
        }
        #endregion
    }
}
