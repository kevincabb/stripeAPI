using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyStore.Migrations
{
    public partial class UpdatePurchaseOrderWithStripeId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StripeCheckoutSessionId",
                table: "PurchaseOrders",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "PurchaseOrders",
                keyColumn: "Id",
                keyValue: 1,
                column: "Datetime",
                value: new DateTime(2020, 3, 9, 19, 4, 52, 257, DateTimeKind.Local).AddTicks(8489));

            migrationBuilder.UpdateData(
                table: "PurchaseOrders",
                keyColumn: "Id",
                keyValue: 2,
                column: "Datetime",
                value: new DateTime(2020, 3, 8, 19, 4, 52, 258, DateTimeKind.Local).AddTicks(822));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StripeCheckoutSessionId",
                table: "PurchaseOrders");

            migrationBuilder.UpdateData(
                table: "PurchaseOrders",
                keyColumn: "Id",
                keyValue: 1,
                column: "Datetime",
                value: new DateTime(2020, 3, 8, 11, 57, 37, 343, DateTimeKind.Local).AddTicks(8561));

            migrationBuilder.UpdateData(
                table: "PurchaseOrders",
                keyColumn: "Id",
                keyValue: 2,
                column: "Datetime",
                value: new DateTime(2020, 3, 7, 11, 57, 37, 344, DateTimeKind.Local).AddTicks(762));
        }
    }
}
