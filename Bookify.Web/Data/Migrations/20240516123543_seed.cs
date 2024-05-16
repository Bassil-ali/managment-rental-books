using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookify.Web.Data.Migrations
{
    public partial class seed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Governorates",
                columns: new[] { "Id", "CreatedById", "CreatedOn", "IsDeleted", "LastUpdatedById", "LastUpdatedOn", "Name" },
                values: new object[] { 1, "1", new DateTime(2024, 5, 16, 15, 35, 43, 628, DateTimeKind.Local).AddTicks(7635), false, null, null, "Example Governorate" });

            migrationBuilder.InsertData(
                table: "Areas",
                columns: new[] { "Id", "CreatedById", "CreatedOn", "GovernorateId", "IsDeleted", "LastUpdatedById", "LastUpdatedOn", "Name" },
                values: new object[] { 1, null, new DateTime(2024, 5, 16, 15, 35, 43, 628, DateTimeKind.Local).AddTicks(7834), 1, false, null, null, "Example Area" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Governorates",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
