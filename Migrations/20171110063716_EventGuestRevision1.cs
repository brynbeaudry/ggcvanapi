using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ggcvan.Migrations
{
    public partial class EventGuestRevision1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventGuests_AspNetUsers_ApplicationUserId",
                table: "EventGuests");

            migrationBuilder.DropForeignKey(
                name: "FK_Events_Games_GameId",
                table: "Events");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EventGuests",
                table: "EventGuests");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "EventGuests");

            migrationBuilder.AlterColumn<int>(
                name: "GameId",
                table: "Events",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GuestId",
                table: "EventGuests",
                type: "varchar(127)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EventGuests",
                table: "EventGuests",
                columns: new[] { "GuestId", "EventId" });

            migrationBuilder.AddForeignKey(
                name: "FK_EventGuests_AspNetUsers_GuestId",
                table: "EventGuests",
                column: "GuestId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Games_GameId",
                table: "Events",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventGuests_AspNetUsers_GuestId",
                table: "EventGuests");

            migrationBuilder.DropForeignKey(
                name: "FK_Events_Games_GameId",
                table: "Events");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EventGuests",
                table: "EventGuests");

            migrationBuilder.DropColumn(
                name: "GuestId",
                table: "EventGuests");

            migrationBuilder.AlterColumn<int>(
                name: "GameId",
                table: "Events",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "EventGuests",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EventGuests",
                table: "EventGuests",
                columns: new[] { "ApplicationUserId", "EventId" });

            migrationBuilder.AddForeignKey(
                name: "FK_EventGuests_AspNetUsers_ApplicationUserId",
                table: "EventGuests",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Games_GameId",
                table: "Events",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
