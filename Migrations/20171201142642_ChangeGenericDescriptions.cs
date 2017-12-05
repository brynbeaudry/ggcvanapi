using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ggcvan.Migrations
{
    public partial class ChangeGenericDescriptions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Games",
                newName: "GameDescription");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Events",
                newName: "EventDescription");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "GameDescription",
                table: "Games",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "EventDescription",
                table: "Events",
                newName: "Description");
        }
    }
}
