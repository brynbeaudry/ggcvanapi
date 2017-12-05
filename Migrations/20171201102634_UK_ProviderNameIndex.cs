using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ggcvan.Migrations
{
    public partial class UK_ProviderNameIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameIndex(
                name: "IX_AspNetUsers_ProviderId",
                table: "AspNetUsers",
                newName: "ProviderIdIndex");

            migrationBuilder.AlterColumn<string>(
                name: "ProviderName",
                table: "AspNetUsers",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "ProviderNameIndex",
                table: "AspNetUsers",
                column: "ProviderName",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ProviderNameIndex",
                table: "AspNetUsers");

            migrationBuilder.RenameIndex(
                name: "ProviderIdIndex",
                table: "AspNetUsers",
                newName: "IX_AspNetUsers_ProviderId");

            migrationBuilder.AlterColumn<string>(
                name: "ProviderName",
                table: "AspNetUsers",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
