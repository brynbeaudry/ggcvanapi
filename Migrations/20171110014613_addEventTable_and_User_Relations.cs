using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ggcvan.Migrations
{
    public partial class addEventTable_and_User_Relations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Game",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(type: "longtext", nullable: true),
                    ImageUrl = table.Column<string>(type: "longtext", nullable: true),
                    NumberOfPlayers = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "longtext", nullable: true),
                    Url = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Game", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(type: "longtext", nullable: true),
                    EndTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    EventCreator = table.Column<string>(type: "varchar(127)", nullable: true),
                    GameId = table.Column<int>(type: "int", nullable: true),
                    Host = table.Column<string>(type: "longtext", nullable: true),
                    Latitude = table.Column<double>(type: "double", nullable: false),
                    LocationDescription = table.Column<string>(type: "longtext", nullable: true),
                    Longitude = table.Column<double>(type: "double", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Title = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Events_AspNetUsers_EventCreator",
                        column: x => x.EventCreator,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Events_Game_GameId",
                        column: x => x.GameId,
                        principalTable: "Game",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EventGuests",
                columns: table => new
                {
                    ApplicationUserId = table.Column<string>(type: "varchar(127)", nullable: false),
                    EventId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventGuests", x => new { x.ApplicationUserId, x.EventId });
                    table.ForeignKey(
                        name: "FK_EventGuests_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventGuests_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EventGuests_EventId",
                table: "EventGuests",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_EventCreator",
                table: "Events",
                column: "EventCreator");

            migrationBuilder.CreateIndex(
                name: "IX_Events_GameId",
                table: "Events",
                column: "GameId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventGuests");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "Game");
        }
    }
}
