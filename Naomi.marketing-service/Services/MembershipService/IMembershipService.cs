using Naomi.marketing_service.Models.Message.Pub;

namespace Naomi.marketing_service.Services.MembershipService
{
    public interface IMembershipService
    {
        Task<Tuple<List<Member>, int>> GetMember(string? searchName, int pageNo, int pageSize);
    }
}
