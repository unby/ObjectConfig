using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ObjectConfig.Data.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Applications",
                columns: table => new
                {
                    ApplicationId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    Code = table.Column<string>(maxLength: 64, nullable: false),
                    Description = table.Column<string>(maxLength: 512, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Applications", x => x.ApplicationId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExternalId = table.Column<string>(nullable: false),
                    DisplayName = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    IsGlobalAdmin = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "ValueTypes",
                columns: table => new
                {
                    ValueTypeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 256, nullable: false),
                    Description = table.Column<string>(maxLength: 512, nullable: true),
                    Type = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ValueTypes", x => x.ValueTypeId);
                });

            migrationBuilder.CreateTable(
                name: "Environments",
                columns: table => new
                {
                    EnvironmentId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    Code = table.Column<string>(maxLength: 64, nullable: false),
                    Description = table.Column<string>(maxLength: 512, nullable: true),
                    ApplicationId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Environments", x => x.EnvironmentId);
                    table.ForeignKey(
                        name: "FK_Environments_Applications_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "Applications",
                        principalColumn: "ApplicationId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UsersApplications",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false),
                    ApplicationId = table.Column<int>(nullable: false),
                    AccessRole = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersApplications", x => new { x.UserId, x.ApplicationId });
                    table.ForeignKey(
                        name: "FK_UsersApplications_Applications_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "Applications",
                        principalColumn: "ApplicationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsersApplications_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UsersTypes",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false),
                    ValueTypeId = table.Column<int>(nullable: false),
                    AccessRole = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersTypes", x => new { x.UserId, x.ValueTypeId });
                    table.ForeignKey(
                        name: "FK_UsersTypes_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsersTypes_ValueTypes_ValueTypeId",
                        column: x => x.ValueTypeId,
                        principalTable: "ValueTypes",
                        principalColumn: "ValueTypeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ValueObjects",
                columns: table => new
                {
                    ValueTypeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<string>(maxLength: 2147483647, nullable: false),
                    DateFrom = table.Column<DateTimeOffset>(nullable: false),
                    DateTo = table.Column<DateTimeOffset>(nullable: true),
                    TypeValueTypeId = table.Column<int>(nullable: true),
                    ChangeOwnerUserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ValueObjects", x => x.ValueTypeId);
                    table.ForeignKey(
                        name: "FK_ValueObjects_Users_ChangeOwnerUserId",
                        column: x => x.ChangeOwnerUserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ValueObjects_ValueTypes_TypeValueTypeId",
                        column: x => x.TypeValueTypeId,
                        principalTable: "ValueTypes",
                        principalColumn: "ValueTypeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Configs",
                columns: table => new
                {
                    ConfigId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(maxLength: 128, nullable: false),
                    DateFrom = table.Column<DateTimeOffset>(nullable: false),
                    DateTo = table.Column<DateTimeOffset>(nullable: true),
                    VersionFrom = table.Column<string>(maxLength: 23, nullable: false),
                    VersionTo = table.Column<string>(maxLength: 23, nullable: true),
                    Description = table.Column<string>(maxLength: 512, nullable: true),
                    EnvironmentId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Configs", x => x.ConfigId);
                    table.ForeignKey(
                        name: "FK_Configs_Environments_EnvironmentId",
                        column: x => x.EnvironmentId,
                        principalTable: "Environments",
                        principalColumn: "EnvironmentId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UsersEnvironments",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false),
                    EnvironmentId = table.Column<int>(nullable: false),
                    AccessRole = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersEnvironments", x => new { x.UserId, x.EnvironmentId });
                    table.ForeignKey(
                        name: "FK_UsersEnvironments_Environments_EnvironmentId",
                        column: x => x.EnvironmentId,
                        principalTable: "Environments",
                        principalColumn: "EnvironmentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UsersEnvironments_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ValueConfigs",
                columns: table => new
                {
                    ValueConfigId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParrentValueConfigId = table.Column<int>(nullable: false),
                    ConfigId = table.Column<int>(nullable: true),
                    TypeValueTypeId = table.Column<int>(nullable: true),
                    ParrentPropertyValueConfigId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ValueConfigs", x => x.ValueConfigId);
                    table.ForeignKey(
                        name: "FK_ValueConfigs_Configs_ConfigId",
                        column: x => x.ConfigId,
                        principalTable: "Configs",
                        principalColumn: "ConfigId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ValueConfigs_ValueConfigs_ParrentPropertyValueConfigId",
                        column: x => x.ParrentPropertyValueConfigId,
                        principalTable: "ValueConfigs",
                        principalColumn: "ValueConfigId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ValueConfigs_ValueTypes_TypeValueTypeId",
                        column: x => x.TypeValueTypeId,
                        principalTable: "ValueTypes",
                        principalColumn: "ValueTypeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Configs_EnvironmentId",
                table: "Configs",
                column: "EnvironmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Environments_ApplicationId",
                table: "Environments",
                column: "ApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersApplications_ApplicationId",
                table: "UsersApplications",
                column: "ApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersEnvironments_EnvironmentId",
                table: "UsersEnvironments",
                column: "EnvironmentId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersTypes_ValueTypeId",
                table: "UsersTypes",
                column: "ValueTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ValueConfigs_ConfigId",
                table: "ValueConfigs",
                column: "ConfigId");

            migrationBuilder.CreateIndex(
                name: "IX_ValueConfigs_ParrentPropertyValueConfigId",
                table: "ValueConfigs",
                column: "ParrentPropertyValueConfigId");

            migrationBuilder.CreateIndex(
                name: "IX_ValueConfigs_TypeValueTypeId",
                table: "ValueConfigs",
                column: "TypeValueTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ValueObjects_ChangeOwnerUserId",
                table: "ValueObjects",
                column: "ChangeOwnerUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ValueObjects_TypeValueTypeId",
                table: "ValueObjects",
                column: "TypeValueTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UsersApplications");

            migrationBuilder.DropTable(
                name: "UsersEnvironments");

            migrationBuilder.DropTable(
                name: "UsersTypes");

            migrationBuilder.DropTable(
                name: "ValueConfigs");

            migrationBuilder.DropTable(
                name: "ValueObjects");

            migrationBuilder.DropTable(
                name: "Configs");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "ValueTypes");

            migrationBuilder.DropTable(
                name: "Environments");

            migrationBuilder.DropTable(
                name: "Applications");
        }
    }
}
