using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace dotnetefc.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InventoryItems",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Price = table.Column<double>(nullable: false),
                    Sku = table.Column<string>(nullable: true),
                    StorageLocation = table.Column<string>(nullable: true),
                    Quantity = table.Column<int>(nullable: false),
                    IsSaleItem = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryItems", x => x.Id);
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
                    InventoryItemId = table.Column<int>(nullable: false),
                    PaymentTypeId = table.Column<int>(nullable: false),
                    Datetime = table.Column<DateTime>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    Subtotal = table.Column<double>(nullable: false),
                    SalesTax = table.Column<double>(nullable: false),
                    NameOfBuyer = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchaseOrders_InventoryItems_InventoryItemId",
                        column: x => x.InventoryItemId,
                        principalTable: "InventoryItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PurchaseOrders_PaymentTypes_PaymentTypeId",
                        column: x => x.PaymentTypeId,
                        principalTable: "PaymentTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                values: new object[] { 1, new DateTime(2020, 3, 5, 13, 34, 3, 587, DateTimeKind.Local).AddTicks(4572), 1, "John Doe", 1, 2, 0.25, 1.0 });

            migrationBuilder.InsertData(
                table: "PurchaseOrders",
                columns: new[] { "Id", "Datetime", "InventoryItemId", "NameOfBuyer", "PaymentTypeId", "Quantity", "SalesTax", "Subtotal" },
                values: new object[] { 2, new DateTime(2020, 3, 4, 13, 34, 3, 587, DateTimeKind.Local).AddTicks(7024), 3, "Mildred Smith", 3, 4, 4.5, 18.0 });

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_InventoryItemId",
                table: "PurchaseOrders",
                column: "InventoryItemId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrders_PaymentTypeId",
                table: "PurchaseOrders",
                column: "PaymentTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PurchaseOrders");

            migrationBuilder.DropTable(
                name: "InventoryItems");

            migrationBuilder.DropTable(
                name: "PaymentTypes");
        }
    }
}
