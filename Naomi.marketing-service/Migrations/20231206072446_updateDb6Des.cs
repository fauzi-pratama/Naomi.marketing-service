using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Naomi.marketing_service.Migrations
{
    /// <inheritdoc />
    public partial class updateDb6Des : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_date",
                table: "site_view_model",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone")
                .Annotation("Relational:ColumnOrder", 7)
                .OldAnnotation("Relational:ColumnOrder", 6);

            migrationBuilder.AlterColumn<string>(
                name: "updated_by",
                table: "site_view_model",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true)
                .Annotation("Relational:ColumnOrder", 8)
                .OldAnnotation("Relational:ColumnOrder", 7);

            migrationBuilder.AlterColumn<string>(
                name: "site_description",
                table: "site_view_model",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200,
                oldNullable: true)
                .Annotation("Relational:ColumnOrder", 4)
                .OldAnnotation("Relational:ColumnOrder", 3);

            migrationBuilder.AlterColumn<string>(
                name: "site_code",
                table: "site_view_model",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true)
                .Annotation("Relational:ColumnOrder", 3)
                .OldAnnotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_date",
                table: "site_view_model",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone")
                .Annotation("Relational:ColumnOrder", 5)
                .OldAnnotation("Relational:ColumnOrder", 4);

            migrationBuilder.AlterColumn<string>(
                name: "created_by",
                table: "site_view_model",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true)
                .Annotation("Relational:ColumnOrder", 6)
                .OldAnnotation("Relational:ColumnOrder", 5);

            migrationBuilder.AlterColumn<bool>(
                name: "active_flag",
                table: "site_view_model",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean")
                .Annotation("Relational:ColumnOrder", 9)
                .OldAnnotation("Relational:ColumnOrder", 8);

            migrationBuilder.AddColumn<string>(
                name: "zone_code",
                table: "site_view_model",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true)
                .Annotation("Relational:ColumnOrder", 2);

            migrationBuilder.AlterColumn<string>(
                name: "value",
                table: "promotion_header",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .Annotation("Relational:ColumnOrder", 28)
                .OldAnnotation("Relational:ColumnOrder", 27);

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_date",
                table: "promotion_header",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone")
                .Annotation("Relational:ColumnOrder", 44)
                .OldAnnotation("Relational:ColumnOrder", 43);

            migrationBuilder.AlterColumn<string>(
                name: "updated_by",
                table: "promotion_header",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true)
                .Annotation("Relational:ColumnOrder", 45)
                .OldAnnotation("Relational:ColumnOrder", 44);

            migrationBuilder.AlterColumn<string>(
                name: "short_desc",
                table: "promotion_header",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .Annotation("Relational:ColumnOrder", 39)
                .OldAnnotation("Relational:ColumnOrder", 38);

            migrationBuilder.AlterColumn<string>(
                name: "result_exp",
                table: "promotion_header",
                type: "character varying(10)",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(10)",
                oldMaxLength: 10,
                oldNullable: true)
                .Annotation("Relational:ColumnOrder", 25)
                .OldAnnotation("Relational:ColumnOrder", 24);

            migrationBuilder.AlterColumn<string>(
                name: "requirement_exp",
                table: "promotion_header",
                type: "character varying(10)",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(10)",
                oldMaxLength: 10,
                oldNullable: true)
                .Annotation("Relational:ColumnOrder", 24)
                .OldAnnotation("Relational:ColumnOrder", 23);

            migrationBuilder.AlterColumn<string>(
                name: "promo_terms_condition",
                table: "promotion_header",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .Annotation("Relational:ColumnOrder", 38)
                .OldAnnotation("Relational:ColumnOrder", 37);

            migrationBuilder.AlterColumn<string>(
                name: "promo_displayed",
                table: "promotion_header",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .Annotation("Relational:ColumnOrder", 41)
                .OldAnnotation("Relational:ColumnOrder", 40);

            migrationBuilder.AlterColumn<string>(
                name: "nip_entertain",
                table: "promotion_header",
                type: "character varying(10)",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(10)",
                oldMaxLength: 10,
                oldNullable: true)
                .Annotation("Relational:ColumnOrder", 36)
                .OldAnnotation("Relational:ColumnOrder", 35);

            migrationBuilder.AlterColumn<bool>(
                name: "new_member",
                table: "promotion_header",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean")
                .Annotation("Relational:ColumnOrder", 31)
                .OldAnnotation("Relational:ColumnOrder", 30);

            migrationBuilder.AlterColumn<string>(
                name: "mop_promo_selection_name",
                table: "promotion_header",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .Annotation("Relational:ColumnOrder", 35)
                .OldAnnotation("Relational:ColumnOrder", 34);

            migrationBuilder.AlterColumn<string>(
                name: "mop_promo_selection_id",
                table: "promotion_header",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .Annotation("Relational:ColumnOrder", 33)
                .OldAnnotation("Relational:ColumnOrder", 32);

            migrationBuilder.AlterColumn<string>(
                name: "mop_promo_selection_code",
                table: "promotion_header",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .Annotation("Relational:ColumnOrder", 34)
                .OldAnnotation("Relational:ColumnOrder", 33);

            migrationBuilder.AlterColumn<double>(
                name: "min_transaction",
                table: "promotion_header",
                type: "double precision",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double precision")
                .Annotation("Relational:ColumnOrder", 26)
                .OldAnnotation("Relational:ColumnOrder", 25);

            migrationBuilder.AlterColumn<string>(
                name: "members",
                table: "promotion_header",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .Annotation("Relational:ColumnOrder", 32)
                .OldAnnotation("Relational:ColumnOrder", 31);

            migrationBuilder.AlterColumn<bool>(
                name: "member_only",
                table: "promotion_header",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean")
                .Annotation("Relational:ColumnOrder", 30)
                .OldAnnotation("Relational:ColumnOrder", 29);

            migrationBuilder.AlterColumn<double>(
                name: "max_transaction",
                table: "promotion_header",
                type: "double precision",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double precision")
                .Annotation("Relational:ColumnOrder", 27)
                .OldAnnotation("Relational:ColumnOrder", 26);

            migrationBuilder.AlterColumn<string>(
                name: "max_disc",
                table: "promotion_header",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .Annotation("Relational:ColumnOrder", 29)
                .OldAnnotation("Relational:ColumnOrder", 28);

            migrationBuilder.AlterColumn<decimal>(
                name: "entertain_budget",
                table: "promotion_header",
                type: "numeric",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric",
                oldNullable: true)
                .Annotation("Relational:ColumnOrder", 37)
                .OldAnnotation("Relational:ColumnOrder", 36);

            migrationBuilder.AlterColumn<bool>(
                name: "display_on_app",
                table: "promotion_header",
                type: "boolean",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true)
                .Annotation("Relational:ColumnOrder", 40)
                .OldAnnotation("Relational:ColumnOrder", 39);

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_date",
                table: "promotion_header",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone")
                .Annotation("Relational:ColumnOrder", 42)
                .OldAnnotation("Relational:ColumnOrder", 41);

            migrationBuilder.AlterColumn<string>(
                name: "created_by",
                table: "promotion_header",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true)
                .Annotation("Relational:ColumnOrder", 43)
                .OldAnnotation("Relational:ColumnOrder", 42);

            migrationBuilder.AlterColumn<bool>(
                name: "active_flag",
                table: "promotion_header",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean")
                .Annotation("Relational:ColumnOrder", 46)
                .OldAnnotation("Relational:ColumnOrder", 45);

            migrationBuilder.AddColumn<int>(
                name: "max_qty_promo",
                table: "promotion_header",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Relational:ColumnOrder", 23);

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_date",
                table: "mop_view_model",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone")
                .Annotation("Relational:ColumnOrder", 8)
                .OldAnnotation("Relational:ColumnOrder", 7);

            migrationBuilder.AlterColumn<string>(
                name: "updated_by",
                table: "mop_view_model",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true)
                .Annotation("Relational:ColumnOrder", 9)
                .OldAnnotation("Relational:ColumnOrder", 8);

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_date",
                table: "mop_view_model",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone")
                .Annotation("Relational:ColumnOrder", 6)
                .OldAnnotation("Relational:ColumnOrder", 5);

            migrationBuilder.AlterColumn<string>(
                name: "created_by",
                table: "mop_view_model",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true)
                .Annotation("Relational:ColumnOrder", 7)
                .OldAnnotation("Relational:ColumnOrder", 6);

            migrationBuilder.AlterColumn<bool>(
                name: "active_flag",
                table: "mop_view_model",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean")
                .Annotation("Relational:ColumnOrder", 10)
                .OldAnnotation("Relational:ColumnOrder", 9);

            migrationBuilder.AddColumn<bool>(
                name: "is_promotion",
                table: "mop_view_model",
                type: "boolean",
                nullable: false,
                defaultValue: false)
                .Annotation("Relational:ColumnOrder", 5);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "zone_code",
                table: "site_view_model");

            migrationBuilder.DropColumn(
                name: "max_qty_promo",
                table: "promotion_header");

            migrationBuilder.DropColumn(
                name: "is_promotion",
                table: "mop_view_model");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_date",
                table: "site_view_model",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone")
                .Annotation("Relational:ColumnOrder", 6)
                .OldAnnotation("Relational:ColumnOrder", 7);

            migrationBuilder.AlterColumn<string>(
                name: "updated_by",
                table: "site_view_model",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true)
                .Annotation("Relational:ColumnOrder", 7)
                .OldAnnotation("Relational:ColumnOrder", 8);

            migrationBuilder.AlterColumn<string>(
                name: "site_description",
                table: "site_view_model",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200,
                oldNullable: true)
                .Annotation("Relational:ColumnOrder", 3)
                .OldAnnotation("Relational:ColumnOrder", 4);

            migrationBuilder.AlterColumn<string>(
                name: "site_code",
                table: "site_view_model",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true)
                .Annotation("Relational:ColumnOrder", 2)
                .OldAnnotation("Relational:ColumnOrder", 3);

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_date",
                table: "site_view_model",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone")
                .Annotation("Relational:ColumnOrder", 4)
                .OldAnnotation("Relational:ColumnOrder", 5);

            migrationBuilder.AlterColumn<string>(
                name: "created_by",
                table: "site_view_model",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true)
                .Annotation("Relational:ColumnOrder", 5)
                .OldAnnotation("Relational:ColumnOrder", 6);

            migrationBuilder.AlterColumn<bool>(
                name: "active_flag",
                table: "site_view_model",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean")
                .Annotation("Relational:ColumnOrder", 8)
                .OldAnnotation("Relational:ColumnOrder", 9);

            migrationBuilder.AlterColumn<string>(
                name: "value",
                table: "promotion_header",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .Annotation("Relational:ColumnOrder", 27)
                .OldAnnotation("Relational:ColumnOrder", 28);

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_date",
                table: "promotion_header",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone")
                .Annotation("Relational:ColumnOrder", 43)
                .OldAnnotation("Relational:ColumnOrder", 44);

            migrationBuilder.AlterColumn<string>(
                name: "updated_by",
                table: "promotion_header",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true)
                .Annotation("Relational:ColumnOrder", 44)
                .OldAnnotation("Relational:ColumnOrder", 45);

            migrationBuilder.AlterColumn<string>(
                name: "short_desc",
                table: "promotion_header",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .Annotation("Relational:ColumnOrder", 38)
                .OldAnnotation("Relational:ColumnOrder", 39);

            migrationBuilder.AlterColumn<string>(
                name: "result_exp",
                table: "promotion_header",
                type: "character varying(10)",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(10)",
                oldMaxLength: 10,
                oldNullable: true)
                .Annotation("Relational:ColumnOrder", 24)
                .OldAnnotation("Relational:ColumnOrder", 25);

            migrationBuilder.AlterColumn<string>(
                name: "requirement_exp",
                table: "promotion_header",
                type: "character varying(10)",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(10)",
                oldMaxLength: 10,
                oldNullable: true)
                .Annotation("Relational:ColumnOrder", 23)
                .OldAnnotation("Relational:ColumnOrder", 24);

            migrationBuilder.AlterColumn<string>(
                name: "promo_terms_condition",
                table: "promotion_header",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .Annotation("Relational:ColumnOrder", 37)
                .OldAnnotation("Relational:ColumnOrder", 38);

            migrationBuilder.AlterColumn<string>(
                name: "promo_displayed",
                table: "promotion_header",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .Annotation("Relational:ColumnOrder", 40)
                .OldAnnotation("Relational:ColumnOrder", 41);

            migrationBuilder.AlterColumn<string>(
                name: "nip_entertain",
                table: "promotion_header",
                type: "character varying(10)",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(10)",
                oldMaxLength: 10,
                oldNullable: true)
                .Annotation("Relational:ColumnOrder", 35)
                .OldAnnotation("Relational:ColumnOrder", 36);

            migrationBuilder.AlterColumn<bool>(
                name: "new_member",
                table: "promotion_header",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean")
                .Annotation("Relational:ColumnOrder", 30)
                .OldAnnotation("Relational:ColumnOrder", 31);

            migrationBuilder.AlterColumn<string>(
                name: "mop_promo_selection_name",
                table: "promotion_header",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .Annotation("Relational:ColumnOrder", 34)
                .OldAnnotation("Relational:ColumnOrder", 35);

            migrationBuilder.AlterColumn<string>(
                name: "mop_promo_selection_id",
                table: "promotion_header",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .Annotation("Relational:ColumnOrder", 32)
                .OldAnnotation("Relational:ColumnOrder", 33);

            migrationBuilder.AlterColumn<string>(
                name: "mop_promo_selection_code",
                table: "promotion_header",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .Annotation("Relational:ColumnOrder", 33)
                .OldAnnotation("Relational:ColumnOrder", 34);

            migrationBuilder.AlterColumn<double>(
                name: "min_transaction",
                table: "promotion_header",
                type: "double precision",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double precision")
                .Annotation("Relational:ColumnOrder", 25)
                .OldAnnotation("Relational:ColumnOrder", 26);

            migrationBuilder.AlterColumn<string>(
                name: "members",
                table: "promotion_header",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .Annotation("Relational:ColumnOrder", 31)
                .OldAnnotation("Relational:ColumnOrder", 32);

            migrationBuilder.AlterColumn<bool>(
                name: "member_only",
                table: "promotion_header",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean")
                .Annotation("Relational:ColumnOrder", 29)
                .OldAnnotation("Relational:ColumnOrder", 30);

            migrationBuilder.AlterColumn<double>(
                name: "max_transaction",
                table: "promotion_header",
                type: "double precision",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double precision")
                .Annotation("Relational:ColumnOrder", 26)
                .OldAnnotation("Relational:ColumnOrder", 27);

            migrationBuilder.AlterColumn<string>(
                name: "max_disc",
                table: "promotion_header",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .Annotation("Relational:ColumnOrder", 28)
                .OldAnnotation("Relational:ColumnOrder", 29);

            migrationBuilder.AlterColumn<decimal>(
                name: "entertain_budget",
                table: "promotion_header",
                type: "numeric",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric",
                oldNullable: true)
                .Annotation("Relational:ColumnOrder", 36)
                .OldAnnotation("Relational:ColumnOrder", 37);

            migrationBuilder.AlterColumn<bool>(
                name: "display_on_app",
                table: "promotion_header",
                type: "boolean",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true)
                .Annotation("Relational:ColumnOrder", 39)
                .OldAnnotation("Relational:ColumnOrder", 40);

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_date",
                table: "promotion_header",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone")
                .Annotation("Relational:ColumnOrder", 41)
                .OldAnnotation("Relational:ColumnOrder", 42);

            migrationBuilder.AlterColumn<string>(
                name: "created_by",
                table: "promotion_header",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true)
                .Annotation("Relational:ColumnOrder", 42)
                .OldAnnotation("Relational:ColumnOrder", 43);

            migrationBuilder.AlterColumn<bool>(
                name: "active_flag",
                table: "promotion_header",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean")
                .Annotation("Relational:ColumnOrder", 45)
                .OldAnnotation("Relational:ColumnOrder", 46);

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_date",
                table: "mop_view_model",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone")
                .Annotation("Relational:ColumnOrder", 7)
                .OldAnnotation("Relational:ColumnOrder", 8);

            migrationBuilder.AlterColumn<string>(
                name: "updated_by",
                table: "mop_view_model",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true)
                .Annotation("Relational:ColumnOrder", 8)
                .OldAnnotation("Relational:ColumnOrder", 9);

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_date",
                table: "mop_view_model",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone")
                .Annotation("Relational:ColumnOrder", 5)
                .OldAnnotation("Relational:ColumnOrder", 6);

            migrationBuilder.AlterColumn<string>(
                name: "created_by",
                table: "mop_view_model",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true)
                .Annotation("Relational:ColumnOrder", 6)
                .OldAnnotation("Relational:ColumnOrder", 7);

            migrationBuilder.AlterColumn<bool>(
                name: "active_flag",
                table: "mop_view_model",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean")
                .Annotation("Relational:ColumnOrder", 9)
                .OldAnnotation("Relational:ColumnOrder", 10);

            migrationBuilder.UpdateData(
                table: "promotion_class",
                keyColumn: "id",
                keyValue: new Guid("302be9cd-5e08-454d-b8e5-582d336750d7"),
                columns: new[] { "created_date", "updated_date" },
                values: new object[] { new DateTime(2023, 11, 21, 10, 16, 16, 148, DateTimeKind.Local).AddTicks(459), new DateTime(2023, 11, 21, 10, 16, 16, 148, DateTimeKind.Local).AddTicks(473) });

            migrationBuilder.UpdateData(
                table: "promotion_class",
                keyColumn: "id",
                keyValue: new Guid("8713bd36-48d6-43dd-94b9-407c3aff1528"),
                columns: new[] { "created_date", "updated_date" },
                values: new object[] { new DateTime(2023, 11, 21, 10, 16, 16, 148, DateTimeKind.Local).AddTicks(482), new DateTime(2023, 11, 21, 10, 16, 16, 148, DateTimeKind.Local).AddTicks(484) });

            migrationBuilder.UpdateData(
                table: "promotion_class",
                keyColumn: "id",
                keyValue: new Guid("c386c5f1-d3d2-4e7f-ad6a-34b4f185325c"),
                columns: new[] { "created_date", "updated_date" },
                values: new object[] { new DateTime(2023, 11, 21, 10, 16, 16, 148, DateTimeKind.Local).AddTicks(493), new DateTime(2023, 11, 21, 10, 16, 16, 148, DateTimeKind.Local).AddTicks(495) });

            migrationBuilder.UpdateData(
                table: "promotion_class",
                keyColumn: "id",
                keyValue: new Guid("dbf358cb-f43b-4d69-9176-8ee63ac8953f"),
                columns: new[] { "created_date", "updated_date" },
                values: new object[] { new DateTime(2023, 11, 21, 10, 16, 16, 148, DateTimeKind.Local).AddTicks(488), new DateTime(2023, 11, 21, 10, 16, 16, 148, DateTimeKind.Local).AddTicks(489) });

            migrationBuilder.UpdateData(
                table: "promotion_type",
                keyColumn: "id",
                keyValue: new Guid("1f57489b-cca0-4392-ae00-3d145012d375"),
                columns: new[] { "created_date", "updated_date" },
                values: new object[] { new DateTime(2023, 11, 21, 10, 16, 16, 148, DateTimeKind.Local).AddTicks(727), new DateTime(2023, 11, 21, 10, 16, 16, 148, DateTimeKind.Local).AddTicks(728) });

            migrationBuilder.UpdateData(
                table: "promotion_type",
                keyColumn: "id",
                keyValue: new Guid("2524251a-565a-46c0-93d5-deea80c63ff5"),
                columns: new[] { "created_date", "updated_date" },
                values: new object[] { new DateTime(2023, 11, 21, 10, 16, 16, 148, DateTimeKind.Local).AddTicks(759), new DateTime(2023, 11, 21, 10, 16, 16, 148, DateTimeKind.Local).AddTicks(761) });

            migrationBuilder.UpdateData(
                table: "promotion_type",
                keyColumn: "id",
                keyValue: new Guid("3c7ed57d-8235-453f-8f97-ba93b3747b4f"),
                columns: new[] { "created_date", "updated_date" },
                values: new object[] { new DateTime(2023, 11, 21, 10, 16, 16, 148, DateTimeKind.Local).AddTicks(768), new DateTime(2023, 11, 21, 10, 16, 16, 148, DateTimeKind.Local).AddTicks(770) });

            migrationBuilder.UpdateData(
                table: "promotion_type",
                keyColumn: "id",
                keyValue: new Guid("57ae0d50-1d3b-4a33-8d7c-a4cab863aa30"),
                columns: new[] { "created_date", "updated_date" },
                values: new object[] { new DateTime(2023, 11, 21, 10, 16, 16, 148, DateTimeKind.Local).AddTicks(782), new DateTime(2023, 11, 21, 10, 16, 16, 148, DateTimeKind.Local).AddTicks(783) });

            migrationBuilder.UpdateData(
                table: "promotion_type",
                keyColumn: "id",
                keyValue: new Guid("86ed449a-e4bc-4c28-a6e5-3ba18e491e63"),
                columns: new[] { "created_date", "updated_date" },
                values: new object[] { new DateTime(2023, 11, 21, 10, 16, 16, 148, DateTimeKind.Local).AddTicks(753), new DateTime(2023, 11, 21, 10, 16, 16, 148, DateTimeKind.Local).AddTicks(754) });

            migrationBuilder.UpdateData(
                table: "promotion_type",
                keyColumn: "id",
                keyValue: new Guid("886470d3-5e0b-41ed-baa7-10cd94511e10"),
                columns: new[] { "created_date", "updated_date" },
                values: new object[] { new DateTime(2023, 11, 21, 10, 16, 16, 148, DateTimeKind.Local).AddTicks(735), new DateTime(2023, 11, 21, 10, 16, 16, 148, DateTimeKind.Local).AddTicks(737) });

            migrationBuilder.UpdateData(
                table: "promotion_type",
                keyColumn: "id",
                keyValue: new Guid("bd4f0c46-7d03-45fa-b33c-77028218593a"),
                columns: new[] { "created_date", "updated_date" },
                values: new object[] { new DateTime(2023, 11, 21, 10, 16, 16, 148, DateTimeKind.Local).AddTicks(745), new DateTime(2023, 11, 21, 10, 16, 16, 148, DateTimeKind.Local).AddTicks(747) });

            migrationBuilder.UpdateData(
                table: "promotion_type",
                keyColumn: "id",
                keyValue: new Guid("dda43968-95bd-4d94-8737-fd621d0a5895"),
                columns: new[] { "created_date", "updated_date" },
                values: new object[] { new DateTime(2023, 11, 21, 10, 16, 16, 148, DateTimeKind.Local).AddTicks(775), new DateTime(2023, 11, 21, 10, 16, 16, 148, DateTimeKind.Local).AddTicks(776) });

            migrationBuilder.UpdateData(
                table: "promotion_type",
                keyColumn: "id",
                keyValue: new Guid("e0d70f81-6a25-434d-9055-e50554ef585c"),
                columns: new[] { "created_date", "updated_date" },
                values: new object[] { new DateTime(2023, 11, 21, 10, 16, 16, 148, DateTimeKind.Local).AddTicks(719), new DateTime(2023, 11, 21, 10, 16, 16, 148, DateTimeKind.Local).AddTicks(720) });

            migrationBuilder.UpdateData(
                table: "promotion_type",
                keyColumn: "id",
                keyValue: new Guid("fac8e236-2fb7-4b4a-b644-0680f60fd0a0"),
                columns: new[] { "created_date", "updated_date" },
                values: new object[] { new DateTime(2023, 11, 21, 10, 16, 16, 148, DateTimeKind.Local).AddTicks(709), new DateTime(2023, 11, 21, 10, 16, 16, 148, DateTimeKind.Local).AddTicks(712) });
        }
    }
}
