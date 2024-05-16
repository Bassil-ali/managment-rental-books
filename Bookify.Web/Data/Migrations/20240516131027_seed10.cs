using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookify.Web.Data.Migrations
{
    public partial class seed10 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedById", "CreatedOn" },
                values: new object[] { "138db5fe-7a06-4c85-9a62-b1c6bd936b57", new DateTime(2024, 5, 16, 16, 10, 26, 986, DateTimeKind.Local).AddTicks(4031) });

            migrationBuilder.UpdateData(
                table: "Governorates",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedById", "CreatedOn", "Name" },
                values: new object[] { "138db5fe-7a06-4c85-9a62-b1c6bd936b57", new DateTime(2024, 5, 16, 16, 10, 26, 986, DateTimeKind.Local).AddTicks(3466), "Example" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Areas",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedById", "CreatedOn" },
                values: new object[] { null, new DateTime(2024, 5, 16, 16, 0, 4, 671, DateTimeKind.Local).AddTicks(8428) });

            migrationBuilder.UpdateData(
                table: "Governorates",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedById", "CreatedOn", "Name" },
                values: new object[] { null, new DateTime(2024, 5, 16, 16, 0, 4, 671, DateTimeKind.Local).AddTicks(8296), "Example Governorate" });
        }
    }
}
