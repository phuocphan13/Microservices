using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Worker.Migrations
{
    public partial class Add_JobExternalId_JobRunHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "JobExternalId",
                table: "JobRunHistories",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "JobExternalId",
                table: "JobRunHistories");
        }
    }
}
