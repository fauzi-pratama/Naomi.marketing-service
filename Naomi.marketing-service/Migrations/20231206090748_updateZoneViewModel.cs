using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Naomi.marketing_service.Migrations
{
    /// <inheritdoc />
    public partial class updateZoneViewModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "site_code",
                table: "zone_view_model");

            migrationBuilder.UpdateData(
                table: "promotion_class",
                keyColumn: "id",
                keyValue: new Guid("302be9cd-5e08-454d-b8e5-582d336750d7"),
                columns: new[] { "created_date", "updated_date" },
                values: new object[] { new DateTime(2023, 12, 6, 16, 7, 47, 806, DateTimeKind.Local).AddTicks(5476), new DateTime(2023, 12, 6, 16, 7, 47, 806, DateTimeKind.Local).AddTicks(5488) });

            migrationBuilder.UpdateData(
                table: "promotion_class",
                keyColumn: "id",
                keyValue: new Guid("8713bd36-48d6-43dd-94b9-407c3aff1528"),
                columns: new[] { "created_date", "updated_date" },
                values: new object[] { new DateTime(2023, 12, 6, 16, 7, 47, 806, DateTimeKind.Local).AddTicks(5492), new DateTime(2023, 12, 6, 16, 7, 47, 806, DateTimeKind.Local).AddTicks(5493) });

            migrationBuilder.UpdateData(
                table: "promotion_class",
                keyColumn: "id",
                keyValue: new Guid("c386c5f1-d3d2-4e7f-ad6a-34b4f185325c"),
                columns: new[] { "created_date", "updated_date" },
                values: new object[] { new DateTime(2023, 12, 6, 16, 7, 47, 806, DateTimeKind.Local).AddTicks(5497), new DateTime(2023, 12, 6, 16, 7, 47, 806, DateTimeKind.Local).AddTicks(5498) });

            migrationBuilder.UpdateData(
                table: "promotion_class",
                keyColumn: "id",
                keyValue: new Guid("dbf358cb-f43b-4d69-9176-8ee63ac8953f"),
                columns: new[] { "created_date", "updated_date" },
                values: new object[] { new DateTime(2023, 12, 6, 16, 7, 47, 806, DateTimeKind.Local).AddTicks(5494), new DateTime(2023, 12, 6, 16, 7, 47, 806, DateTimeKind.Local).AddTicks(5495) });

            migrationBuilder.UpdateData(
                table: "promotion_type",
                keyColumn: "id",
                keyValue: new Guid("1f57489b-cca0-4392-ae00-3d145012d375"),
                columns: new[] { "created_date", "updated_date" },
                values: new object[] { new DateTime(2023, 12, 6, 16, 7, 47, 806, DateTimeKind.Local).AddTicks(5614), new DateTime(2023, 12, 6, 16, 7, 47, 806, DateTimeKind.Local).AddTicks(5614) });

            migrationBuilder.UpdateData(
                table: "promotion_type",
                keyColumn: "id",
                keyValue: new Guid("2524251a-565a-46c0-93d5-deea80c63ff5"),
                columns: new[] { "created_date", "updated_date" },
                values: new object[] { new DateTime(2023, 12, 6, 16, 7, 47, 806, DateTimeKind.Local).AddTicks(5626), new DateTime(2023, 12, 6, 16, 7, 47, 806, DateTimeKind.Local).AddTicks(5627) });

            migrationBuilder.UpdateData(
                table: "promotion_type",
                keyColumn: "id",
                keyValue: new Guid("3c7ed57d-8235-453f-8f97-ba93b3747b4f"),
                columns: new[] { "created_date", "updated_date" },
                values: new object[] { new DateTime(2023, 12, 6, 16, 7, 47, 806, DateTimeKind.Local).AddTicks(5629), new DateTime(2023, 12, 6, 16, 7, 47, 806, DateTimeKind.Local).AddTicks(5630) });

            migrationBuilder.UpdateData(
                table: "promotion_type",
                keyColumn: "id",
                keyValue: new Guid("57ae0d50-1d3b-4a33-8d7c-a4cab863aa30"),
                columns: new[] { "created_date", "updated_date" },
                values: new object[] { new DateTime(2023, 12, 6, 16, 7, 47, 806, DateTimeKind.Local).AddTicks(5635), new DateTime(2023, 12, 6, 16, 7, 47, 806, DateTimeKind.Local).AddTicks(5636) });

            migrationBuilder.UpdateData(
                table: "promotion_type",
                keyColumn: "id",
                keyValue: new Guid("86ed449a-e4bc-4c28-a6e5-3ba18e491e63"),
                columns: new[] { "created_date", "updated_date" },
                values: new object[] { new DateTime(2023, 12, 6, 16, 7, 47, 806, DateTimeKind.Local).AddTicks(5623), new DateTime(2023, 12, 6, 16, 7, 47, 806, DateTimeKind.Local).AddTicks(5624) });

            migrationBuilder.UpdateData(
                table: "promotion_type",
                keyColumn: "id",
                keyValue: new Guid("886470d3-5e0b-41ed-baa7-10cd94511e10"),
                columns: new[] { "created_date", "updated_date" },
                values: new object[] { new DateTime(2023, 12, 6, 16, 7, 47, 806, DateTimeKind.Local).AddTicks(5617), new DateTime(2023, 12, 6, 16, 7, 47, 806, DateTimeKind.Local).AddTicks(5618) });

            migrationBuilder.UpdateData(
                table: "promotion_type",
                keyColumn: "id",
                keyValue: new Guid("bd4f0c46-7d03-45fa-b33c-77028218593a"),
                columns: new[] { "created_date", "updated_date" },
                values: new object[] { new DateTime(2023, 12, 6, 16, 7, 47, 806, DateTimeKind.Local).AddTicks(5620), new DateTime(2023, 12, 6, 16, 7, 47, 806, DateTimeKind.Local).AddTicks(5621) });

            migrationBuilder.UpdateData(
                table: "promotion_type",
                keyColumn: "id",
                keyValue: new Guid("dda43968-95bd-4d94-8737-fd621d0a5895"),
                columns: new[] { "created_date", "updated_date" },
                values: new object[] { new DateTime(2023, 12, 6, 16, 7, 47, 806, DateTimeKind.Local).AddTicks(5632), new DateTime(2023, 12, 6, 16, 7, 47, 806, DateTimeKind.Local).AddTicks(5633) });

            migrationBuilder.UpdateData(
                table: "promotion_type",
                keyColumn: "id",
                keyValue: new Guid("e0d70f81-6a25-434d-9055-e50554ef585c"),
                columns: new[] { "created_date", "updated_date" },
                values: new object[] { new DateTime(2023, 12, 6, 16, 7, 47, 806, DateTimeKind.Local).AddTicks(5610), new DateTime(2023, 12, 6, 16, 7, 47, 806, DateTimeKind.Local).AddTicks(5611) });

            migrationBuilder.UpdateData(
                table: "promotion_type",
                keyColumn: "id",
                keyValue: new Guid("fac8e236-2fb7-4b4a-b644-0680f60fd0a0"),
                columns: new[] { "created_date", "updated_date" },
                values: new object[] { new DateTime(2023, 12, 6, 16, 7, 47, 806, DateTimeKind.Local).AddTicks(5605), new DateTime(2023, 12, 6, 16, 7, 47, 806, DateTimeKind.Local).AddTicks(5606) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "site_code",
                table: "zone_view_model",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true)
                .Annotation("Relational:ColumnOrder", 2);

            migrationBuilder.UpdateData(
                table: "promotion_class",
                keyColumn: "id",
                keyValue: new Guid("302be9cd-5e08-454d-b8e5-582d336750d7"),
                columns: new[] { "created_date", "updated_date" },
                values: new object[] { new DateTime(2023, 12, 6, 14, 24, 46, 613, DateTimeKind.Local).AddTicks(629), new DateTime(2023, 12, 6, 14, 24, 46, 613, DateTimeKind.Local).AddTicks(638) });

            migrationBuilder.UpdateData(
                table: "promotion_class",
                keyColumn: "id",
                keyValue: new Guid("8713bd36-48d6-43dd-94b9-407c3aff1528"),
                columns: new[] { "created_date", "updated_date" },
                values: new object[] { new DateTime(2023, 12, 6, 14, 24, 46, 613, DateTimeKind.Local).AddTicks(641), new DateTime(2023, 12, 6, 14, 24, 46, 613, DateTimeKind.Local).AddTicks(643) });

            migrationBuilder.UpdateData(
                table: "promotion_class",
                keyColumn: "id",
                keyValue: new Guid("c386c5f1-d3d2-4e7f-ad6a-34b4f185325c"),
                columns: new[] { "created_date", "updated_date" },
                values: new object[] { new DateTime(2023, 12, 6, 14, 24, 46, 613, DateTimeKind.Local).AddTicks(651), new DateTime(2023, 12, 6, 14, 24, 46, 613, DateTimeKind.Local).AddTicks(652) });

            migrationBuilder.UpdateData(
                table: "promotion_class",
                keyColumn: "id",
                keyValue: new Guid("dbf358cb-f43b-4d69-9176-8ee63ac8953f"),
                columns: new[] { "created_date", "updated_date" },
                values: new object[] { new DateTime(2023, 12, 6, 14, 24, 46, 613, DateTimeKind.Local).AddTicks(645), new DateTime(2023, 12, 6, 14, 24, 46, 613, DateTimeKind.Local).AddTicks(646) });

            migrationBuilder.UpdateData(
                table: "promotion_type",
                keyColumn: "id",
                keyValue: new Guid("1f57489b-cca0-4392-ae00-3d145012d375"),
                columns: new[] { "created_date", "updated_date" },
                values: new object[] { new DateTime(2023, 12, 6, 14, 24, 46, 613, DateTimeKind.Local).AddTicks(810), new DateTime(2023, 12, 6, 14, 24, 46, 613, DateTimeKind.Local).AddTicks(811) });

            migrationBuilder.UpdateData(
                table: "promotion_type",
                keyColumn: "id",
                keyValue: new Guid("2524251a-565a-46c0-93d5-deea80c63ff5"),
                columns: new[] { "created_date", "updated_date" },
                values: new object[] { new DateTime(2023, 12, 6, 14, 24, 46, 613, DateTimeKind.Local).AddTicks(822), new DateTime(2023, 12, 6, 14, 24, 46, 613, DateTimeKind.Local).AddTicks(823) });

            migrationBuilder.UpdateData(
                table: "promotion_type",
                keyColumn: "id",
                keyValue: new Guid("3c7ed57d-8235-453f-8f97-ba93b3747b4f"),
                columns: new[] { "created_date", "updated_date" },
                values: new object[] { new DateTime(2023, 12, 6, 14, 24, 46, 613, DateTimeKind.Local).AddTicks(825), new DateTime(2023, 12, 6, 14, 24, 46, 613, DateTimeKind.Local).AddTicks(826) });

            migrationBuilder.UpdateData(
                table: "promotion_type",
                keyColumn: "id",
                keyValue: new Guid("57ae0d50-1d3b-4a33-8d7c-a4cab863aa30"),
                columns: new[] { "created_date", "updated_date" },
                values: new object[] { new DateTime(2023, 12, 6, 14, 24, 46, 613, DateTimeKind.Local).AddTicks(831), new DateTime(2023, 12, 6, 14, 24, 46, 613, DateTimeKind.Local).AddTicks(832) });

            migrationBuilder.UpdateData(
                table: "promotion_type",
                keyColumn: "id",
                keyValue: new Guid("86ed449a-e4bc-4c28-a6e5-3ba18e491e63"),
                columns: new[] { "created_date", "updated_date" },
                values: new object[] { new DateTime(2023, 12, 6, 14, 24, 46, 613, DateTimeKind.Local).AddTicks(819), new DateTime(2023, 12, 6, 14, 24, 46, 613, DateTimeKind.Local).AddTicks(820) });

            migrationBuilder.UpdateData(
                table: "promotion_type",
                keyColumn: "id",
                keyValue: new Guid("886470d3-5e0b-41ed-baa7-10cd94511e10"),
                columns: new[] { "created_date", "updated_date" },
                values: new object[] { new DateTime(2023, 12, 6, 14, 24, 46, 613, DateTimeKind.Local).AddTicks(813), new DateTime(2023, 12, 6, 14, 24, 46, 613, DateTimeKind.Local).AddTicks(814) });

            migrationBuilder.UpdateData(
                table: "promotion_type",
                keyColumn: "id",
                keyValue: new Guid("bd4f0c46-7d03-45fa-b33c-77028218593a"),
                columns: new[] { "created_date", "updated_date" },
                values: new object[] { new DateTime(2023, 12, 6, 14, 24, 46, 613, DateTimeKind.Local).AddTicks(816), new DateTime(2023, 12, 6, 14, 24, 46, 613, DateTimeKind.Local).AddTicks(817) });

            migrationBuilder.UpdateData(
                table: "promotion_type",
                keyColumn: "id",
                keyValue: new Guid("dda43968-95bd-4d94-8737-fd621d0a5895"),
                columns: new[] { "created_date", "updated_date" },
                values: new object[] { new DateTime(2023, 12, 6, 14, 24, 46, 613, DateTimeKind.Local).AddTicks(828), new DateTime(2023, 12, 6, 14, 24, 46, 613, DateTimeKind.Local).AddTicks(829) });

            migrationBuilder.UpdateData(
                table: "promotion_type",
                keyColumn: "id",
                keyValue: new Guid("e0d70f81-6a25-434d-9055-e50554ef585c"),
                columns: new[] { "created_date", "updated_date" },
                values: new object[] { new DateTime(2023, 12, 6, 14, 24, 46, 613, DateTimeKind.Local).AddTicks(760), new DateTime(2023, 12, 6, 14, 24, 46, 613, DateTimeKind.Local).AddTicks(761) });

            migrationBuilder.UpdateData(
                table: "promotion_type",
                keyColumn: "id",
                keyValue: new Guid("fac8e236-2fb7-4b4a-b644-0680f60fd0a0"),
                columns: new[] { "created_date", "updated_date" },
                values: new object[] { new DateTime(2023, 12, 6, 14, 24, 46, 613, DateTimeKind.Local).AddTicks(755), new DateTime(2023, 12, 6, 14, 24, 46, 613, DateTimeKind.Local).AddTicks(756) });
        }
    }
}
