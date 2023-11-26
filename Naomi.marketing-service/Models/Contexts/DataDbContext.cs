
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PromotionHeader>()
                        .HasIndex(x => x.PromotionCode)
                        .IsUnique();

            modelBuilder.Entity<PromotionClass>().HasData(
                new PromotionClass()
                {
                    Id = new Guid("302BE9CD-5E08-454D-B8E5-582D336750D7"),
                    PromotionClassKey = "ITEM",
                    PromotionClassName = "ITEM",
                    LineNum = 1,
                    CreatedBy = "System",
                    CreatedDate = DateTime.Now,
                    ActiveFlag = true,
                    UpdatedBy = "System",
                    UpdatedDate = DateTime.Now
                },
                new PromotionClass()
                {
                    Id = new Guid("8713BD36-48D6-43DD-94B9-407C3AFF1528"),
                    PromotionClassKey = "CART",
                    PromotionClassName = "CART",
                    LineNum = 2,
                    CreatedBy = "System",
                    CreatedDate = DateTime.Now,
                    ActiveFlag = true,
                    UpdatedBy = "System",
                    UpdatedDate = DateTime.Now
                },
                new PromotionClass()
                {
                    Id = new Guid("DBF358CB-F43B-4D69-9176-8EE63AC8953F"),
                    PromotionClassKey = "MOP",
                    PromotionClassName = "MOP",
                    LineNum = 3,
                    CreatedBy = "System",
                    CreatedDate = DateTime.Now,
                    ActiveFlag = true,
                    UpdatedBy = "System",
                    UpdatedDate = DateTime.Now
                },
                new PromotionClass()
                {
                    Id = new Guid("C386C5F1-D3D2-4E7F-AD6A-34B4F185325C"),
                    PromotionClassKey = "Entertain",
                    PromotionClassName = "Entertain",
                    LineNum = 4,
                    CreatedBy = "System",
                    CreatedDate = DateTime.Now,
                    ActiveFlag = true,
                    UpdatedBy = "System",
                    UpdatedDate = DateTime.Now
                });

            modelBuilder.Entity<PromotionType>().HasData(
                new PromotionType()
                {
                    Id = new Guid("FAC8E236-2FB7-4B4A-B644-0680F60FD0A0"),
                    PromotionClassId = new Guid("302BE9CD-5E08-454D-B8E5-582D336750D7"),
                    PromotionTypeKey = "ITEM",
                    PromotionTypeName = "BUY X GET Y ITEM",
                    LineNum = 1,
                    CreatedBy = "System",
                    CreatedDate = DateTime.Now,
                    ActiveFlag = true,
                    UpdatedBy = "System",
                    UpdatedDate = DateTime.Now
                },
                new PromotionType()
                {
                    Id = new Guid("E0D70F81-6A25-434D-9055-E50554EF585C"),
                    PromotionClassId = new Guid("302BE9CD-5E08-454D-B8E5-582D336750D7"),
                    PromotionTypeKey = "SP",
                    PromotionTypeName = "SPECIAL PRICE ITEM",
                    LineNum = 1,
                    CreatedBy = "System",
                    CreatedDate = DateTime.Now,
                    ActiveFlag = true,
                    UpdatedBy = "System",
                    UpdatedDate = DateTime.Now
                },
                new PromotionType()
                {
                    Id = new Guid("1F57489B-CCA0-4392-AE00-3D145012D375"),
                    PromotionClassId = new Guid("302BE9CD-5E08-454D-B8E5-582D336750D7"),
                    PromotionTypeKey = "AMOUNT",
                    PromotionTypeName = "DISCOUNT AMOUNT ITEM",
                    LineNum = 2,
                    CreatedBy = "System",
                    CreatedDate = DateTime.Now,
                    ActiveFlag = true,
                    UpdatedBy = "System",
                    UpdatedDate = DateTime.Now
                },
                new PromotionType()
                {
                    Id = new Guid("886470D3-5E0B-41ED-BAA7-10CD94511E10"),
                    PromotionClassId = new Guid("302BE9CD-5E08-454D-B8E5-582D336750D7"),
                    PromotionTypeKey = "PERCENT",
                    PromotionTypeName = "DISCOUNT PERCENT ITEM",
                    LineNum = 2,
                    CreatedBy = "System",
                    CreatedDate = DateTime.Now,
                    ActiveFlag = true,
                    UpdatedBy = "System",
                    UpdatedDate = DateTime.Now
                },
                new PromotionType()
                {
                    Id = new Guid("BD4F0C46-7D03-45FA-B33C-77028218593A"),
                    PromotionClassId = new Guid("302BE9CD-5E08-454D-B8E5-582D336750D7"),
                    PromotionTypeKey = "BUNDLE",
                    PromotionTypeName = "DISCOUNT BUNDLING ITEM",
                    LineNum = 2,
                    CreatedBy = "System",
                    CreatedDate = DateTime.Now,
                    ActiveFlag = true,
                    UpdatedBy = "System",
                    UpdatedDate = DateTime.Now
                },
                new PromotionType()
                {
                    Id = new Guid("86ED449A-E4BC-4C28-A6E5-3BA18E491E63"),
                    PromotionClassId = new Guid("8713BD36-48D6-43DD-94B9-407C3AFF1528"),
                    PromotionTypeKey = "AMOUNT",
                    PromotionTypeName = "DISCOUNT AMOUNT CART",
                    LineNum = 2,
                    CreatedBy = "System",
                    CreatedDate = DateTime.Now,
                    ActiveFlag = true,
                    UpdatedBy = "System",
                    UpdatedDate = DateTime.Now
                },
                new PromotionType()
                {
                    Id = new Guid("2524251A-565A-46C0-93D5-DEEA80C63FF5"),
                    PromotionClassId = new Guid("8713BD36-48D6-43DD-94B9-407C3AFF1528"),
                    PromotionTypeKey = "PERCENT",
                    PromotionTypeName = "DISCOUNT PERCENT CART",
                    LineNum = 2,
                    CreatedBy = "System",
                    CreatedDate = DateTime.Now,
                    ActiveFlag = true,
                    UpdatedBy = "System",
                    UpdatedDate = DateTime.Now
                },
                new PromotionType()
                {
                    Id = new Guid("3C7ED57D-8235-453F-8F97-BA93B3747B4F"),
                    PromotionClassId = new Guid("DBF358CB-F43B-4D69-9176-8EE63AC8953F"),
                    PromotionTypeKey = "AMOUNT",
                    PromotionTypeName = "DISCOUNT AMOUNT MOP",
                    LineNum = 2,
                    CreatedBy = "System",
                    CreatedDate = DateTime.Now,
                    ActiveFlag = true,
                    UpdatedBy = "System",
                    UpdatedDate = DateTime.Now
                },
                new PromotionType()
                {
                    Id = new Guid("DDA43968-95BD-4D94-8737-FD621D0A5895"),
                    PromotionClassId = new Guid("DBF358CB-F43B-4D69-9176-8EE63AC8953F"),
                    PromotionTypeKey = "PERCENT",
                    PromotionTypeName = "DISCOUNT PERCENT MOP",
                    LineNum = 2,
                    CreatedBy = "System",
                    CreatedDate = DateTime.Now,
                    ActiveFlag = true,
                    UpdatedBy = "System",
                    UpdatedDate = DateTime.Now
                },
                new PromotionType()
                {
                    Id = new Guid("57AE0D50-1D3B-4A33-8D7C-A4CAB863AA30"),
                    PromotionClassId = new Guid("C386C5F1-D3D2-4E7F-AD6A-34B4F185325C"),
                    PromotionTypeKey = "PERCENT",
                    PromotionTypeName = "DISCOUNT PERCENT Entertain",
                    LineNum = 2,
                    CreatedBy = "System",
                    CreatedDate = DateTime.Now,
                    ActiveFlag = true,
                    UpdatedBy = "System",
                    UpdatedDate = DateTime.Now
                });

        }

    }
}
