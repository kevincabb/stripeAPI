using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyStore.Migrations
{
    public partial class seeddata : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "InventoryItems",
                columns: new[] { "Id", "Description", "IsSaleItem", "Name", "Price", "Quantity", "Sku", "StorageLocation" },
                values: new object[,]
                {
                    { 1, "#2 pencil", false, "Pencil", 0.5, 100, "38830982031", "A1" },
                    { 2, "spiral notebook", false, "Notebook", 1.5, 50, "3881111131", "A2" },
                    { 3, "3 ring binder with dividers", false, "Binder", 4.5, 5, "54830982031", "A2" },
                    { 4, "Scientific calculator", false, "Ti83+ Calculator", 49.0, 100, "3889462031", "A4" },
                    { 5, "black ball point pen", false, "Pen", 0.5, 10, "388309867", "A1" },
                    { 6, "metallic coaster", false, "Coaster", 5.5, 1, "388309212", "A6" },
                    { 7, "Fuzzy backpack", false, "Backpack", 25.489999999999998, 100, "388309987", "A5" }
                });

            migrationBuilder.InsertData(
                table: "PaymentTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Cash" },
                    { 2, "Credit" },
                    { 3, "Debit" },
                    { 4, "Check" },
                    { 5, "GiftCard" }
                });

            migrationBuilder.InsertData(
                table: "PurchaseOrders",
                columns: new[] { "Id", "Datetime", "InventoryItemId", "NameOfBuyer", "PaymentTypeId", "Quantity", "SalesTax", "Subtotal" },
                values: new object[] { 1, new DateTime(2020, 3, 8, 11, 57, 37, 343, DateTimeKind.Local).AddTicks(8561), 1, "John Doe", 1, 2, 0.25, 1.0 });

            migrationBuilder.InsertData(
                table: "PurchaseOrders",
                columns: new[] { "Id", "Datetime", "InventoryItemId", "NameOfBuyer", "PaymentTypeId", "Quantity", "SalesTax", "Subtotal" },
                values: new object[] { 2, new DateTime(2020, 3, 7, 11, 57, 37, 344, DateTimeKind.Local).AddTicks(762), 3, "Mildred Smith", 3, 4, 4.5, 18.0 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "InventoryItems",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "InventoryItems",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "InventoryItems",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "InventoryItems",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "InventoryItems",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "PaymentTypes",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "PaymentTypes",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "PaymentTypes",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "PurchaseOrders",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "PurchaseOrders",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "InventoryItems",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "InventoryItems",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "PaymentTypes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "PaymentTypes",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
