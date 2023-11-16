
using Microsoft.EntityFrameworkCore;
using Naomi.marketing_service.Models.Entities;

namespace Naomi.marketing_service.Models.Contexts
{
    public class DataDbContext : DbContext
    {
        public DataDbContext(DbContextOptions<DataDbContext> options) : base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true); //for set timestamp without timezone     
        }

        public DbSet<ApprovalMapping> ApprovalMapping { get; set; }
        public DbSet<ApprovalMappingDetail> ApprovalMappingDetail { get; set; }
        public DbSet<BrandViewModel> BrandViewModel { get; set; }
        public DbSet<CompanyViewModel> CompanyViewModel { get; set; }
        public DbSet<MopViewModel> MopViewModel { get; set; }
        public DbSet<PromotionAppDisplay> PromotionAppDisplay { get; set; }
        public DbSet<PromotionAppImage> PromotionAppImage { get; set; }
        public DbSet<PromotionApproval> PromotionApproval { get; set; }
        public DbSet<PromotionApprovalDetail> PromotionApprovalDetail { get; set; }
        public DbSet<PromotionChannel> PromotionChannel { get; set; }
        public DbSet<PromotionClass> PromotionClass { get; set; }
        public DbSet<PromotionEntertain> PromotionEntertain { get; set; }
        public DbSet<PromotionEntertainEmail> PromotionEntertainEmail { get; set; }
        public DbSet<PromotionHeader> PromotionHeader { get; set; }
        public DbSet<PromotionHistory> PromotionHistory { get; set; }
        public DbSet<PromotionMaterial> PromotionMaterial { get; set; }
        public DbSet<PromotionRuleMopGroup> PromotionRuleMopGroup { get; set; }
        public DbSet<PromotionRuleRequirement> PromotionRuleRequirement { get; set; }
        public DbSet<PromotionRuleResult> PromotionRuleResult { get; set; }
        public DbSet<PromotionStatus> PromotionStatus { get; set; }
        public DbSet<PromotionType> PromotionType { get; set; }
        public DbSet<SiteViewModel> SiteViewModel { get; set; }
        public DbSet<ZoneViewModel> ZoneViewModel { get; set; }
    }
}
