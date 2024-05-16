using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookify.Web.Data.Migrations
{
    public partial class seed8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedOn", "Name" },
                values: new object[] { new DateTime(2024, 5, 16, 15, 58, 25, 364, DateTimeKind.Local).AddTicks(6319), "Example Area" });

            migrationBuilder.UpdateData(
                table: "Governorates",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedById", "CreatedOn", "Name" },
                values: new object[] { null, new DateTime(2024, 5, 16, 15, 58, 25, 364, DateTimeKind.Local).AddTicks(6106), "Example Governorate" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedOn", "Name" },
                values: new object[] { new DateTime(2024, 5, 16, 15, 50, 53, 270, DateTimeKind.Local).AddTicks(2148), "betboss" });

            migrationBuilder.UpdateData(
                table: "Governorates",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedById", "CreatedOn", "Name" },
                values: new object[] { "138db5fe-7a06-4c85-9a62-b1c6bd936b57", new DateTime(2024, 5, 16, 15, 35, 43, 628, DateTimeKind.Local).AddTicks(7635), "sanaa" });
        }
    }
}
