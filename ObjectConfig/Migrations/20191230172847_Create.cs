using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ObjectConfig.Migrations
{
    public partial class Create : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence(
                name: "EntityFrameworkHiLoSequence",
                incrementBy: 10);

            migrationBuilder.CreateTable(
                name: "Applications",
                columns: table => new
                {
                    ApplicationId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    Code = table.Column<string>(maxLength: 64, nullable: false),
                    Description = table.Column<string>(maxLength: 512, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Applications", x => x.ApplicationId);
                });

            migrationBuilder.CreateTable(
                name: "TypeElements",
                columns: table => new
                {
                    TypeElementId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: false),
                    Description = table.Column<string>(maxLength: 512, nullable: true),
                    Type = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeElements", x => x.TypeElementId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false),
                    ExternalId = table.Column<string>(maxLength: 256, nullable: true),
                    DisplayName = table.Column<string>(maxLength: 128, nullable: false),
                    Email = table.Column<string>(maxLength: 128, nullable: false),
                    IsGlobalAdmin = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Environments",
                columns: table => new
                {
                    EnvironmentId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    Code = table.Column<string>(maxLength: 64, nullable: false),
                    Description = table.Column<string>(maxLength: 512, nullable: true),
                    ApplicationId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Environments", x => x.EnvironmentId);
                    table.ForeignKey(
                        name: "FK_Environments_Applications_ApplicationId",
                        column: x => x.ApplicationId,
                        principalTable: "Applications",
                        principalColumn: "ApplicationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ConfigElements",
                columns: table => new
                {
                    ConfigElementId = table.Column<int>(nullable: false),
                    TypeElementId = table.Column<int>(nullable: true),
                    ParrentConfigElementId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfigElements", x => x.ConfigElementId);
                    table.ForeignKey(
                        name: "FK_ConfigElements_ConfigElements_ParrentConfigElementId",
                        column: x => x.ParrentConfigElementId,
                        principalTable: "ConfigElements",
                        principalColumn: "ConfigElementId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ConfigElements_TypeElements_TypeElementId",
                        column: x => x.TypeElementId,
                        principalTable: "TypeElements",
                        principalColumn: "TypeElementId",
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
                        name: "FK_UsersTypes_TypeElements_ValueTypeId",
                        column: x => x.ValueTypeId,
                        principalTable: "TypeElements",
                        principalColumn: "TypeElementId",
                        onDelete: ReferentialAction.Cascade);
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
                name: "Configs",
                columns: table => new
                {
                    ConfigId = table.Column<int>(nullable: false),
                    Code = table.Column<string>(maxLength: 128, nullable: false),
                    DateFrom = table.Column<DateTimeOffset>(nullable: false),
                    DateTo = table.Column<DateTimeOffset>(nullable: true),
                    VersionFrom = table.Column<string>(maxLength: 23, nullable: false),
                    VersionTo = table.Column<string>(maxLength: 23, nullable: true),
                    Description = table.Column<string>(maxLength: 512, nullable: true),
                    EnvironmentId = table.Column<int>(nullable: false),
                    ConfigElementId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Configs", x => x.ConfigId);
                    table.ForeignKey(
                        name: "FK_Configs_ConfigElements_ConfigElementId",
                        column: x => x.ConfigElementId,
                        principalTable: "ConfigElements",
                        principalColumn: "ConfigElementId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Configs_Environments_EnvironmentId",
                        column: x => x.EnvironmentId,
                        principalTable: "Environments",
                        principalColumn: "EnvironmentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ValueElements",
                columns: table => new
                {
                    ValueElementId = table.Column<int>(nullable: false),
                    Value = table.Column<string>(maxLength: 2147483647, nullable: true),
                    Comment = table.Column<string>(maxLength: 2147483647, nullable: true),
                    DateFrom = table.Column<DateTimeOffset>(nullable: false),
                    DateTo = table.Column<DateTimeOffset>(nullable: true),
                    TypeElementId = table.Column<int>(nullable: true),
                    ChangeOwnerUserId = table.Column<int>(nullable: true),
                    ConfigElementId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ValueElements", x => x.ValueElementId);
                    table.ForeignKey(
                        name: "FK_ValueElements_Users_ChangeOwnerUserId",
                        column: x => x.ChangeOwnerUserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ValueElements_ConfigElements_ConfigElementId",
                        column: x => x.ConfigElementId,
                        principalTable: "ConfigElements",
                        principalColumn: "ConfigElementId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ValueElements_TypeElements_TypeElementId",
                        column: x => x.TypeElementId,
                        principalTable: "TypeElements",
                        principalColumn: "TypeElementId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "DisplayName", "Email", "ExternalId", "IsGlobalAdmin" },
                values: new object[] { 1, "GlobalAdmin", "admin@global.net", "0701ea11-3386-4bcb-9726-49d7619ea1a5", true });

            migrationBuilder.CreateIndex(
                name: "IX_Applications_Code",
                table: "Applications",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ConfigElements_ParrentConfigElementId",
                table: "ConfigElements",
                column: "ParrentConfigElementId");

            migrationBuilder.CreateIndex(
                name: "IX_ConfigElements_TypeElementId",
                table: "ConfigElements",
                column: "TypeElementId");

            migrationBuilder.CreateIndex(
                name: "IX_Configs_ConfigElementId",
                table: "Configs",
                column: "ConfigElementId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Configs_EnvironmentId",
                table: "Configs",
                column: "EnvironmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Configs_Code_VersionFrom_EnvironmentId",
                table: "Configs",
                columns: new[] { "Code", "VersionFrom", "EnvironmentId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Environments_ApplicationId",
                table: "Environments",
                column: "ApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_Environments_Code_ApplicationId",
                table: "Environments",
                columns: new[] { "Code", "ApplicationId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_ExternalId",
                table: "Users",
                column: "ExternalId",
                unique: true,
                filter: "[ExternalId] IS NOT NULL");

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
                name: "IX_ValueElements_ChangeOwnerUserId",
                table: "ValueElements",
                column: "ChangeOwnerUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ValueElements_ConfigElementId",
                table: "ValueElements",
                column: "ConfigElementId");

            migrationBuilder.CreateIndex(
                name: "IX_ValueElements_TypeElementId",
                table: "ValueElements",
                column: "TypeElementId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Configs");

            migrationBuilder.DropTable(
                name: "UsersApplications");

            migrationBuilder.DropTable(
                name: "UsersEnvironments");

            migrationBuilder.DropTable(
                name: "UsersTypes");

            migrationBuilder.DropTable(
                name: "ValueElements");

            migrationBuilder.DropTable(
                name: "Environments");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "ConfigElements");

            migrationBuilder.DropTable(
                name: "Applications");

            migrationBuilder.DropTable(
                name: "TypeElements");

            migrationBuilder.DropSequence(
                name: "EntityFrameworkHiLoSequence");
        }
    }
}
