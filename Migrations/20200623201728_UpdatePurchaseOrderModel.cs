using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyStore.Migrations
{
    public partial class UpdatePurchaseOrderModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InventoryItems",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    img = table.Column<string>(nullable: true),
                    itemName = table.Column<string>(nullable: true),
                    type = table.Column<string>(nullable: true),
                    itemPrice = table.Column<double>(nullable: false),
                    quantity = table.Column<int>(nullable: false),
                    sold = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryItems", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "PaymentTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseOrders",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PaymentTypeId = table.Column<int>(nullable: false),
                    Datetime = table.Column<DateTime>(nullable: false),
                    Subtotal = table.Column<double>(nullable: false),
                    SalesTax = table.Column<double>(nullable: false),
                    NameOfBuyer = table.Column<string>(nullable: true),
                    StripeCheckoutSessionId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchaseOrders_PaymentTypes_PaymentTypeId",
                        column: x => x.PaymentTypeId,
                        principalTable: "PaymentTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PurchaseOrderItems_PurchaseOrders_PurchaseOrderId",
                        column: x => x.PurchaseOrderId,
                        principalTable: "PurchaseOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "InventoryItems",
                columns: new[] { "id", "img", "itemName", "itemPrice", "quantity", "sold", "type" },
                values: new object[,]
                {
                    { 1, "stripe/Images/hoody.png", "Hoody", 50.990000000000002, 1, false, "Apparel" },
                    { 2, "stripe/Images/slippers.png", "Jordan Slides", 70.989999999999995, 1, false, "Shoes" },
                    { 3, "stripe/Images/watch.png", "Mr. Nice Watch", 99.989999999999995, 1, false, "Misc" },
                    { 4, "stripe/Images/watch.png", "Broken Watch", 9.9900000000000002, 1, false, "Misc" }
                });

            migrationBuilder.InsertData(
                table: "PaymentTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "None" },
                    { 2, "Cash" },
                    { 3, "Credit" },
                    { 4, "Debit" },
                    { 5, "Check" },
                    { 6, "GiftCard" }
                });

            migrationBuilder.InsertData(
                table: "PurchaseOrders",
                columns: new[] { "Id", "Datetime", "NameOfBuyer", "PaymentTypeId", "SalesTax", "StripeCheckoutSessionId", "Subtotal" },
                values: new object[,]
                {
                    { 1, new DateTime(2020, 6, 22, 13, 17, 28, 149, DateTimeKind.Local).AddTicks(2540), "John Doe", 2, 0.45000000000000001, null, 59.990000000000002 },
                    { 2, new DateTime(2020, 6, 21, 13, 17, 28, 171, DateTimeKind.Local).AddTicks(9110), "Mil cab", 4, 106.34999999999999, null, 70.989999999999995 },
                    { 3, new DateTime(2020, 6, 21, 13, 17, 28, 171, DateTimeKind.Local).AddTicks(9170), "Mildred Smith", 4, 106.34999999999999, null, 99.989999999999995 },
                    { 4, new DateTime(2020, 6, 21, 13, 17, 28, 171, DateTimeKind.Local).AddTicks(9180), "Bob Pryt", 4, 106.34999999999999, null, 9.9900000000000002 }
                });

            migrationBuilder.InsertData(
                table: "PurchaseOrderItems",
                columns: new[] { "Id", "InventoryItemId", "PurchaseOrderId", "Quantity" },
                values: new object[,]
                {
                    { 1, 1, 1, 2 },
                    { 2, 3, 1, 1 },
                    { 3, 1, 2, 1 },
                    { 4, 4, 2, 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderItems_InventoryItemId",
                table: "PurchaseOrderItems",
                column: "InventoryItemId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderItems_PurchaseOrderId",
                table: "PurchaseOrderItems",
                column: "PurchaseOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_PaymentTypeId",
                table: "PurchaseOrders",
                column: "PaymentTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PurchaseOrderItems");

            migrationBuilder.DropTable(
                name: "InventoryItems");

            migrationBuilder.DropTable(
                name: "PurchaseOrders");

            migrationBuilder.DropTable(
                name: "PaymentTypes");
        }
    }
}
