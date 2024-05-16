using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookify.Web.Data.Migrations
{
    public partial class seed2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedOn",
                value: new DateTime(2024, 5, 16, 15, 42, 40, 4, DateTimeKind.Local).AddTicks(9120));

            migrationBuilder.UpdateData(
                table: "Governorates",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedById", "CreatedOn", "Name" },
                values: new object[] { "138db5fe-7a06-4c85-9a62-b1c6bd936b57", new DateTime(2024, 5, 16, 15, 35, 43, 628, DateTimeKind.Local).AddTicks(7635), "Example Governorate" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedOn",
                value: new DateTime(2024, 5, 16, 15, 38, 43, 891, DateTimeKind.Local).AddTicks(3670));

            migrationBuilder.UpdateData(
                table: "Governorates",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedById", "CreatedOn", "Name" },
                values: new object[] { "1", new DateTime(2024, 5, 16, 15, 38, 43, 891, DateTimeKind.Local).AddTicks(3545), "sanaa" });
        }
    }
}
