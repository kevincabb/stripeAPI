using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyStore.Migrations
{
    public partial class AddPurchaseOrderItems : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrders_InventoryItems_InventoryItemId",
                table: "PurchaseOrders");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseOrders_InventoryItemId",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "InventoryItemId",
                table: "PurchaseOrders");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "PurchaseOrders");

            migrationBuilder.CreateTable(
                name: "PurchaseOrderItems",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InventoryItemId = table.Column<int>(nullable: false),
                    PurchaseOrderId = table.Column<int>(nullable: false),
                    Quantity = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseOrderItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchaseOrderItems_InventoryItems_InventoryItemId",
                        column: x => x.InventoryItemId,
                        principalTable: "InventoryItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PurchaseOrderItems_PurchaseOrders_PurchaseOrderId",
                        column: x => x.PurchaseOrderId,
                        principalTable: "PurchaseOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "PaymentTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "None");

            migrationBuilder.UpdateData(
                table: "PaymentTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "Cash");

            migrationBuilder.UpdateData(
                table: "PaymentTypes",
                keyColumn: "Id",
                keyValue: 3,
                column: "Name",
                value: "Credit");

            migrationBuilder.UpdateData(
                table: "PaymentTypes",
                keyColumn: "Id",
                keyValue: 4,
                column: "Name",
                value: "Debit");

            migrationBuilder.UpdateData(
                table: "PaymentTypes",
                keyColumn: "Id",
                keyValue: 5,
                column: "Name",
                value: "Check");

            migrationBuilder.InsertData(
                table: "PaymentTypes",
                columns: new[] { "Id", "Name" },
                values: new object[] { 6, "GiftCard" });

            migrationBuilder.InsertData(
                table: "PurchaseOrderItems",
                columns: new[] { "Id", "InventoryItemId", "PurchaseOrderId", "Quantity" },
                values: new object[,]
                {
                    { 1, 1, 1, 2 },
                    { 2, 3, 1, 1 },
                    { 3, 1, 2, 1 },
                    { 4, 4, 2, 2 }
                });

            migrationBuilder.UpdateData(
                table: "PurchaseOrders",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Datetime", "PaymentTypeId", "SalesTax", "Subtotal" },
                values: new object[] { new DateTime(2020, 3, 10, 12, 19, 7, 55, DateTimeKind.Local).AddTicks(1612), 2, 0.45000000000000001, 5.5 });

            migrationBuilder.UpdateData(
                table: "PurchaseOrders",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Datetime", "PaymentTypeId", "SalesTax", "Subtotal" },
                values: new object[] { new DateTime(2020, 3, 9, 12, 19, 7, 55, DateTimeKind.Local).AddTicks(4128), 4, 106.34999999999999, 98.5 });

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderItems_InventoryItemId",
                table: "PurchaseOrderItems",
                column: "InventoryItemId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderItems_PurchaseOrderId",
                table: "PurchaseOrderItems",
                column: "PurchaseOrderId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PurchaseOrderItems");

            migrationBuilder.DeleteData(
                table: "PaymentTypes",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.AddColumn<int>(
                name: "InventoryItemId",
                table: "PurchaseOrders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "PurchaseOrders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "PaymentTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "Cash");

            migrationBuilder.UpdateData(
                table: "PaymentTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "Credit");

            migrationBuilder.UpdateData(
                table: "PaymentTypes",
                keyColumn: "Id",
                keyValue: 3,
                column: "Name",
                value: "Debit");

            migrationBuilder.UpdateData(
                table: "PaymentTypes",
                keyColumn: "Id",
                keyValue: 4,
                column: "Name",
                value: "Check");

            migrationBuilder.UpdateData(
                table: "PaymentTypes",
                keyColumn: "Id",
                keyValue: 5,
                column: "Name",
                value: "GiftCard");

            migrationBuilder.UpdateData(
                table: "PurchaseOrders",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Datetime", "InventoryItemId", "PaymentTypeId", "Quantity", "SalesTax", "Subtotal" },
                values: new object[] { new DateTime(2020, 3, 9, 19, 4, 52, 257, DateTimeKind.Local).AddTicks(8489), 1, 1, 2, 0.25, 1.0 });

            migrationBuilder.UpdateData(
                table: "PurchaseOrders",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Datetime", "InventoryItemId", "PaymentTypeId", "Quantity", "SalesTax", "Subtotal" },
                values: new object[] { new DateTime(2020, 3, 8, 19, 4, 52, 258, DateTimeKind.Local).AddTicks(822), 3, 3, 4, 4.5, 18.0 });

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_InventoryItemId",
                table: "PurchaseOrders",
                column: "InventoryItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrders_InventoryItems_InventoryItemId",
                table: "PurchaseOrders",
                column: "InventoryItemId",
                principalTable: "InventoryItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
