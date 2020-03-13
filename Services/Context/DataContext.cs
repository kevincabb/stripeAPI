using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using MyStore.Models;

namespace MyStore.Services.Context
{
	public class DataContext : DbContext
	{
		public DbSet<InventoryItem> InventoryItems { get; set; }
		public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
		public DbSet<PaymentType> PaymentTypes { get; set; }
		public DbSet<PurchaseOrderItem> PurchaseOrderItems { get; set; }
		public DataContext(DbContextOptions<DataContext> options) : base(options)
		{

		}
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			SeedData(modelBuilder);
		}

		private void SeedData(ModelBuilder builder)
		{
			//took fixed data straight from fixedService
			var fixedData = new List<InventoryItem> {
				new InventoryItem(1, "#2 pencil", "Pencil", .50, "38830982031", "A1", 100),
				new InventoryItem(2, "spiral notebook", "Notebook", 1.50, "3881111131", "A2", 50),
				new InventoryItem(3, "3 ring binder with dividers", "Binder", 4.50, "54830982031", "A2", 5),
				new InventoryItem(4, "Scientific calculator", "Ti83+ Calculator", 49.00, "3889462031", "A4", 100),
				new InventoryItem(5, "black ball point pen", "Pen", .50, "388309867", "A1", 10),
				new InventoryItem(6, "metallic coaster", "Coaster", 5.50, "388309212", "A6", 1),
				new InventoryItem(7, "Fuzzy backpack", "Backpack", 25.49, "388309987", "A5", 100)
			};
			builder.Entity<InventoryItem>().HasData(fixedData);

			// also initializing the accepted payment types
			var types = new List<PaymentType> {
				new PaymentType() {
					Id = 1,
					Name = "None"
				},
				new PaymentType() {
					Id = 2,
					Name = "Cash"
				},
				new PaymentType() {
					Id = 3,
					Name = "Credit"
				},
				new PaymentType() {
					Id = 4,
					Name = "Debit"
				},
				new PaymentType() {
					Id = 5,
					Name = "Check"
				},
				new PaymentType() {
					Id = 6,
					Name = "GiftCard"
				}
			};
			builder.Entity<PaymentType>().HasData(types);

			// and mocking some purchase orders
			var orders = new List<PurchaseOrder> {
				new PurchaseOrder() {
					Id = 1,
					Datetime = DateTime.Now.AddDays(-1),
					Subtotal = 5.5,
					SalesTax = .45,
					PaymentTypeId = 2,
					NameOfBuyer = "John Doe"
				},
				new PurchaseOrder() {
					Id = 2,
					Datetime = DateTime.Now.AddDays(-2),
					Subtotal = 98.5,
					SalesTax = 106.35,
					PaymentTypeId = 4,
					NameOfBuyer = "Mildred Smith"
				}
			};
			builder.Entity<PurchaseOrder>().HasData(orders);

			var orderItems = new List<PurchaseOrderItem> {
				new PurchaseOrderItem() {
					Id = 1,
					InventoryItemId = 1,
					PurchaseOrderId = 1,
					Quantity = 2
				},new PurchaseOrderItem() {
					Id = 2,
					InventoryItemId = 3,
					PurchaseOrderId = 1,
					Quantity = 1
				},new PurchaseOrderItem() {
					Id = 3,
					InventoryItemId = 1,
					PurchaseOrderId = 2,
					Quantity = 1
				},new PurchaseOrderItem() {
					Id = 4,
					InventoryItemId = 4,
					PurchaseOrderId = 2,
					Quantity = 2
				}
			};
			builder.Entity<PurchaseOrderItem>().HasData(orderItems);
		}
	}
}
