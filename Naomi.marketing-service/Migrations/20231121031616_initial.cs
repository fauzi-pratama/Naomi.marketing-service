using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Naomi.marketing_service.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "approval_mapping",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    company_id = table.Column<Guid>(type: "uuid", nullable: true),
                    company_code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    updated_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_by = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    active_flag = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_approval_mapping", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "brand_view_model",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    brand_code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    brand_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    updated_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_by = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    active_flag = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_brand_view_model", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "company_view_model",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    company_code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    company_description = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    updated_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_by = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    active_flag = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_company_view_model", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "mop_view_model",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    company_code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    site_code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    mop_code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    mop_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    updated_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_by = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    active_flag = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mop_view_model", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "promotion_app_display",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    app_code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    app_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    bucket_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    region = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    secret_key = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    access_key = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    base_directory = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    updated_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_by = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    active_flag = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_promotion_app_display", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "promotion_approval",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    promotion_header_id = table.Column<Guid>(type: "uuid", nullable: true),
                    approval_mapping_id = table.Column<Guid>(type: "uuid", nullable: true),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    updated_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_by = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    active_flag = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_promotion_approval", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "promotion_approval_detail",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    promotion_approval_id = table.Column<Guid>(type: "uuid", nullable: true),
                    approval_level = table.Column<int>(type: "integer", nullable: false),
                    approver_id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    job_position = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    approve = table.Column<bool>(type: "boolean", nullable: false),
                    approval_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    approval_notes = table.Column<string>(type: "text", nullable: true),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    updated_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_by = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    active_flag = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_promotion_approval_detail", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "promotion_channel",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    channel_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    updated_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_by = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    active_flag = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_promotion_channel", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "promotion_class",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    promotion_class_key = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    promotion_class_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    line_num = table.Column<int>(type: "integer", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    updated_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_by = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    active_flag = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_promotion_class", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "promotion_entertain",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    employee_nip = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    employee_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    job_position = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    start_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    end_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    entertain_budget = table.Column<decimal>(type: "numeric", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    updated_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_by = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    active_flag = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_promotion_entertain", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "promotion_header",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    promotion_class_id = table.Column<Guid>(type: "uuid", nullable: false),
                    promotion_type_id = table.Column<Guid>(type: "uuid", nullable: true),
                    promotion_status_id = table.Column<Guid>(type: "uuid", nullable: false),
                    company_id = table.Column<Guid>(type: "uuid", nullable: false),
                    company_code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    company_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    promotion_code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    redemption_code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    promotion_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    depts = table.Column<string>(type: "text", nullable: true),
                    start_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    end_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    promotion_channel = table.Column<string>(type: "text", nullable: true),
                    objective = table.Column<string>(type: "text", nullable: true),
                    promotion_material = table.Column<string>(type: "text", nullable: true),
                    zones = table.Column<string>(type: "text", nullable: true),
                    sites = table.Column<string>(type: "text", nullable: true),
                    target_sales = table.Column<int>(type: "integer", nullable: false),
                    max_promo_used_type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    max_promo_used_qty = table.Column<int>(type: "integer", nullable: false),
                    multiple_promo = table.Column<bool>(type: "boolean", nullable: false),
                    multiple_promo_max_qty = table.Column<int>(type: "integer", nullable: false),
                    requirement_exp = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    result_exp = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    min_transaction = table.Column<double>(type: "double precision", nullable: false),
                    max_transaction = table.Column<double>(type: "double precision", nullable: false),
                    value = table.Column<string>(type: "text", nullable: true),
                    max_disc = table.Column<string>(type: "text", nullable: true),
                    member_only = table.Column<bool>(type: "boolean", nullable: false),
                    new_member = table.Column<bool>(type: "boolean", nullable: false),
                    members = table.Column<string>(type: "text", nullable: true),
                    mop_promo_selection_id = table.Column<string>(type: "text", nullable: true),
                    mop_promo_selection_code = table.Column<string>(type: "text", nullable: true),
                    mop_promo_selection_name = table.Column<string>(type: "text", nullable: true),
                    nip_entertain = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    entertain_budget = table.Column<decimal>(type: "numeric", nullable: true),
                    promo_terms_condition = table.Column<string>(type: "text", nullable: true),
                    short_desc = table.Column<string>(type: "text", nullable: true),
                    display_on_app = table.Column<bool>(type: "boolean", nullable: true),
                    promo_displayed = table.Column<string>(type: "text", nullable: true),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    updated_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_by = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    active_flag = table.Column<bool>(type: "boolean", nullable: false),
                    brand_id = table.Column<Guid>(type: "uuid", nullable: true),
                    brand = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_promotion_header", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "promotion_history",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    promotion_header_id = table.Column<Guid>(type: "uuid", nullable: false),
                    promotion_status_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    updated_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_by = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    active_flag = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_promotion_history", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "promotion_material",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    material_name = table.Column<string>(type: "text", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    updated_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_by = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    active_flag = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_promotion_material", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "promotion_status",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    promotion_status_key = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    promotion_status_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    updated_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_by = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    active_flag = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_promotion_status", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "promotion_type",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    promotion_class_id = table.Column<Guid>(type: "uuid", nullable: false),
                    promotion_type_key = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    promotion_type_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    line_num = table.Column<int>(type: "integer", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    updated_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_by = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    active_flag = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_promotion_type", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "site_view_model",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    company_code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    site_code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    site_description = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    updated_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_by = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    active_flag = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_site_view_model", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "zone_view_model",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    company_code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    site_code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    zone_code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    zone_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    updated_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_by = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    active_flag = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_zone_view_model", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "approval_mapping_detail",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    approval_mapping_id = table.Column<Guid>(type: "uuid", nullable: false),
                    approval_level = table.Column<int>(type: "integer", nullable: false),
                    approver_id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    job_position = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    updated_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_by = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    active_flag = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_approval_mapping_detail", x => x.id);
                    table.ForeignKey(
                        name: "FK_approval_mapping_detail_approval_mapping_approval_mapping_id",
                        column: x => x.approval_mapping_id,
                        principalTable: "approval_mapping",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "promotion_entertain_email",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    promotion_entertain_id = table.Column<Guid>(type: "uuid", nullable: true),
                    email = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    default_email = table.Column<bool>(type: "boolean", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    updated_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_by = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    active_flag = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_promotion_entertain_email", x => x.id);
                    table.ForeignKey(
                        name: "FK_promotion_entertain_email_promotion_entertain_promotion_ent~",
                        column: x => x.promotion_entertain_id,
                        principalTable: "promotion_entertain",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "promotion_app_image",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    promotion_header_id = table.Column<Guid>(type: "uuid", nullable: false),
                    app_code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    image_link = table.Column<string>(type: "text", nullable: true),
                    file_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    updated_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_by = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    active_flag = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_promotion_app_image", x => x.id);
                    table.ForeignKey(
                        name: "FK_promotion_app_image_promotion_header_promotion_header_id",
                        column: x => x.promotion_header_id,
                        principalTable: "promotion_header",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "promotion_rule_mop_group",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    promotion_header_id = table.Column<Guid>(type: "uuid", nullable: false),
                    line_num = table.Column<int>(type: "integer", nullable: false),
                    mop_group_id = table.Column<string>(type: "text", nullable: true),
                    mop_group_code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    mop_group_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    updated_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_by = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    active_flag = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_promotion_rule_mop_group", x => x.id);
                    table.ForeignKey(
                        name: "FK_promotion_rule_mop_group_promotion_header_promotion_header_~",
                        column: x => x.promotion_header_id,
                        principalTable: "promotion_header",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "promotion_rule_requirement",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    promotion_header_id = table.Column<Guid>(type: "uuid", nullable: false),
                    line_num = table.Column<int>(type: "integer", nullable: false),
                    stock_code_id = table.Column<Guid>(type: "uuid", nullable: false),
                    stock_code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    stock_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    qty = table.Column<int>(type: "integer", nullable: false),
                    price = table.Column<string>(type: "text", nullable: true),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    updated_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_by = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    active_flag = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_promotion_rule_requirement", x => x.id);
                    table.ForeignKey(
                        name: "FK_promotion_rule_requirement_promotion_header_promotion_heade~",
                        column: x => x.promotion_header_id,
                        principalTable: "promotion_header",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "promotion_rule_result",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    promotion_header_id = table.Column<Guid>(type: "uuid", nullable: false),
                    line_num = table.Column<int>(type: "integer", nullable: false),
                    stock_code_id = table.Column<Guid>(type: "uuid", nullable: false),
                    stock_code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    stock_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    qty = table.Column<int>(type: "integer", nullable: false),
                    value = table.Column<string>(type: "text", nullable: true),
                    max_disc = table.Column<string>(type: "text", nullable: true),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    created_by = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    updated_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    updated_by = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    active_flag = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_promotion_rule_result", x => x.id);
                    table.ForeignKey(
                        name: "FK_promotion_rule_result_promotion_header_promotion_header_id",
                        column: x => x.promotion_header_id,
                        principalTable: "promotion_header",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "promotion_class",
                columns: new[] { "id", "active_flag", "created_by", "created_date", "line_num", "promotion_class_key", "promotion_class_name", "updated_by", "updated_date" },
                values: new object[,]
                {
                    { new Guid("302be9cd-5e08-454d-b8e5-582d336750d7"), true, "System", new DateTime(2023, 11, 21, 10, 16, 16, 148, DateTimeKind.Local).AddTicks(459), 1, "ITEM", "ITEM", "System", new DateTime(2023, 11, 21, 10, 16, 16, 148, DateTimeKind.Local).AddTicks(473) },
                    { new Guid("8713bd36-48d6-43dd-94b9-407c3aff1528"), true, "System", new DateTime(2023, 11, 21, 10, 16, 16, 148, DateTimeKind.Local).AddTicks(482), 2, "CART", "CART", "System", new DateTime(2023, 11, 21, 10, 16, 16, 148, DateTimeKind.Local).AddTicks(484) },
                    { new Guid("c386c5f1-d3d2-4e7f-ad6a-34b4f185325c"), true, "System", new DateTime(2023, 11, 21, 10, 16, 16, 148, DateTimeKind.Local).AddTicks(493), 4, "Entertain", "Entertain", "System", new DateTime(2023, 11, 21, 10, 16, 16, 148, DateTimeKind.Local).AddTicks(495) },
                    { new Guid("dbf358cb-f43b-4d69-9176-8ee63ac8953f"), true, "System", new DateTime(2023, 11, 21, 10, 16, 16, 148, DateTimeKind.Local).AddTicks(488), 3, "MOP", "MOP", "System", new DateTime(2023, 11, 21, 10, 16, 16, 148, DateTimeKind.Local).AddTicks(489) }
                });

            migrationBuilder.InsertData(
                table: "promotion_type",
                columns: new[] { "id", "active_flag", "created_by", "created_date", "line_num", "promotion_class_id", "promotion_type_key", "promotion_type_name", "updated_by", "updated_date" },
                values: new object[,]
                {
                    { new Guid("1f57489b-cca0-4392-ae00-3d145012d375"), true, "System", new DateTime(2023, 11, 21, 10, 16, 16, 148, DateTimeKind.Local).AddTicks(727), 2, new Guid("302be9cd-5e08-454d-b8e5-582d336750d7"), "AMOUNT", "DISCOUNT AMOUNT ITEM", "System", new DateTime(2023, 11, 21, 10, 16, 16, 148, DateTimeKind.Local).AddTicks(728) },
                    { new Guid("2524251a-565a-46c0-93d5-deea80c63ff5"), true, "System", new DateTime(2023, 11, 21, 10, 16, 16, 148, DateTimeKind.Local).AddTicks(759), 2, new Guid("8713bd36-48d6-43dd-94b9-407c3aff1528"), "PERCENT", "DISCOUNT PERCENT CART", "System", new DateTime(2023, 11, 21, 10, 16, 16, 148, DateTimeKind.Local).AddTicks(761) },
                    { new Guid("3c7ed57d-8235-453f-8f97-ba93b3747b4f"), true, "System", new DateTime(2023, 11, 21, 10, 16, 16, 148, DateTimeKind.Local).AddTicks(768), 2, new Guid("dbf358cb-f43b-4d69-9176-8ee63ac8953f"), "AMOUNT", "DISCOUNT AMOUNT MOP", "System", new DateTime(2023, 11, 21, 10, 16, 16, 148, DateTimeKind.Local).AddTicks(770) },
                    { new Guid("57ae0d50-1d3b-4a33-8d7c-a4cab863aa30"), true, "System", new DateTime(2023, 11, 21, 10, 16, 16, 148, DateTimeKind.Local).AddTicks(782), 2, new Guid("c386c5f1-d3d2-4e7f-ad6a-34b4f185325c"), "PERCENT", "DISCOUNT PERCENT Entertain", "System", new DateTime(2023, 11, 21, 10, 16, 16, 148, DateTimeKind.Local).AddTicks(783) },
                    { new Guid("86ed449a-e4bc-4c28-a6e5-3ba18e491e63"), true, "System", new DateTime(2023, 11, 21, 10, 16, 16, 148, DateTimeKind.Local).AddTicks(753), 2, new Guid("8713bd36-48d6-43dd-94b9-407c3aff1528"), "AMOUNT", "DISCOUNT AMOUNT CART", "System", new DateTime(2023, 11, 21, 10, 16, 16, 148, DateTimeKind.Local).AddTicks(754) },
                    { new Guid("886470d3-5e0b-41ed-baa7-10cd94511e10"), true, "System", new DateTime(2023, 11, 21, 10, 16, 16, 148, DateTimeKind.Local).AddTicks(735), 2, new Guid("302be9cd-5e08-454d-b8e5-582d336750d7"), "PERCENT", "DISCOUNT PERCENT ITEM", "System", new DateTime(2023, 11, 21, 10, 16, 16, 148, DateTimeKind.Local).AddTicks(737) },
                    { new Guid("bd4f0c46-7d03-45fa-b33c-77028218593a"), true, "System", new DateTime(2023, 11, 21, 10, 16, 16, 148, DateTimeKind.Local).AddTicks(745), 2, new Guid("302be9cd-5e08-454d-b8e5-582d336750d7"), "BUNDLE", "DISCOUNT BUNDLING ITEM", "System", new DateTime(2023, 11, 21, 10, 16, 16, 148, DateTimeKind.Local).AddTicks(747) },
                    { new Guid("dda43968-95bd-4d94-8737-fd621d0a5895"), true, "System", new DateTime(2023, 11, 21, 10, 16, 16, 148, DateTimeKind.Local).AddTicks(775), 2, new Guid("dbf358cb-f43b-4d69-9176-8ee63ac8953f"), "PERCENT", "DISCOUNT PERCENT MOP", "System", new DateTime(2023, 11, 21, 10, 16, 16, 148, DateTimeKind.Local).AddTicks(776) },
                    { new Guid("e0d70f81-6a25-434d-9055-e50554ef585c"), true, "System", new DateTime(2023, 11, 21, 10, 16, 16, 148, DateTimeKind.Local).AddTicks(719), 1, new Guid("302be9cd-5e08-454d-b8e5-582d336750d7"), "SP", "SPECIAL PRICE ITEM", "System", new DateTime(2023, 11, 21, 10, 16, 16, 148, DateTimeKind.Local).AddTicks(720) },
                    { new Guid("fac8e236-2fb7-4b4a-b644-0680f60fd0a0"), true, "System", new DateTime(2023, 11, 21, 10, 16, 16, 148, DateTimeKind.Local).AddTicks(709), 1, new Guid("302be9cd-5e08-454d-b8e5-582d336750d7"), "ITEM", "BUY X GET Y ITEM", "System", new DateTime(2023, 11, 21, 10, 16, 16, 148, DateTimeKind.Local).AddTicks(712) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_approval_mapping_detail_approval_mapping_id",
                table: "approval_mapping_detail",
                column: "approval_mapping_id");

            migrationBuilder.CreateIndex(
                name: "IX_promotion_app_image_promotion_header_id",
                table: "promotion_app_image",
                column: "promotion_header_id");

            migrationBuilder.CreateIndex(
                name: "IX_promotion_entertain_email_promotion_entertain_id",
                table: "promotion_entertain_email",
                column: "promotion_entertain_id");

            migrationBuilder.CreateIndex(
                name: "IX_promotion_header_promotion_code",
                table: "promotion_header",
                column: "promotion_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_promotion_rule_mop_group_promotion_header_id",
                table: "promotion_rule_mop_group",
                column: "promotion_header_id");

            migrationBuilder.CreateIndex(
                name: "IX_promotion_rule_requirement_promotion_header_id",
                table: "promotion_rule_requirement",
                column: "promotion_header_id");

            migrationBuilder.CreateIndex(
                name: "IX_promotion_rule_result_promotion_header_id",
                table: "promotion_rule_result",
                column: "promotion_header_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "approval_mapping_detail");

            migrationBuilder.DropTable(
                name: "brand_view_model");

            migrationBuilder.DropTable(
                name: "company_view_model");

            migrationBuilder.DropTable(
                name: "mop_view_model");

            migrationBuilder.DropTable(
                name: "promotion_app_display");

            migrationBuilder.DropTable(
                name: "promotion_app_image");

            migrationBuilder.DropTable(
                name: "promotion_approval");

            migrationBuilder.DropTable(
                name: "promotion_approval_detail");

            migrationBuilder.DropTable(
                name: "promotion_channel");

            migrationBuilder.DropTable(
                name: "promotion_class");

            migrationBuilder.DropTable(
                name: "promotion_entertain_email");

            migrationBuilder.DropTable(
                name: "promotion_history");

            migrationBuilder.DropTable(
                name: "promotion_material");

            migrationBuilder.DropTable(
                name: "promotion_rule_mop_group");

            migrationBuilder.DropTable(
                name: "promotion_rule_requirement");

            migrationBuilder.DropTable(
                name: "promotion_rule_result");

            migrationBuilder.DropTable(
                name: "promotion_status");

            migrationBuilder.DropTable(
                name: "promotion_type");

            migrationBuilder.DropTable(
                name: "site_view_model");

            migrationBuilder.DropTable(
                name: "zone_view_model");

            migrationBuilder.DropTable(
                name: "approval_mapping");

            migrationBuilder.DropTable(
                name: "promotion_entertain");

            migrationBuilder.DropTable(
                name: "promotion_header");
        }
    }
}
