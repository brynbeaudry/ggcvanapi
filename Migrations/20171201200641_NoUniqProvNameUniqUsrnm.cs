using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ggcvan.Migrations
{
    public partial class NoUniqProvNameUniqUsrnm : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ProviderNameIndex",
                table: "AspNetUsers");

            migrationBuilder.CreateIndex(
                name: "ProviderNameIndex",
                table: "AspNetUsers",
                column: "ProviderName");

            migrationBuilder.CreateIndex(
                name: "NonNormalizedUserNameIndex",
                table: "AspNetUsers",
                column: "UserName",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ProviderNameIndex",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "NonNormalizedUserNameIndex",
                table: "AspNetUsers");

            migrationBuilder.CreateIndex(
                name: "ProviderNameIndex",
                table: "AspNetUsers",
                column: "ProviderName",
                unique: true);
        }
    }
}
