using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ggcvan.Migrations
{
    public partial class UserNameIsntUnique : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "NonNormalizedUserNameIndex",
                table: "AspNetUsers");

            migrationBuilder.CreateIndex(
                name: "NonNormalizedUserNameIndex",
                table: "AspNetUsers",
                column: "UserName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "NonNormalizedUserNameIndex",
                table: "AspNetUsers");

            migrationBuilder.CreateIndex(
                name: "NonNormalizedUserNameIndex",
                table: "AspNetUsers",
                column: "UserName",
                unique: true);
        }
    }
}
