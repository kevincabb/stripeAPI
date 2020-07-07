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
				new InventoryItem( 1, "stripe/Images/hoody.png", "Hoody", "Apparel", 50.99, 1, false),
				new InventoryItem( 2, "stripe/Images/slippers.png", "Jordan Slides", "Shoes", 70.99, 1, false),
				new InventoryItem( 3, "stripe/Images/watch.png", "Mr. Nice Watch", "Misc", 99.99, 1, false),
				new InventoryItem( 4, "stripe/Images/watch.png", "Broken Watch", "Misc", 9.99, 1, false),
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
					Subtotal = 59.99,
					SalesTax = .45,
					PaymentTypeId = 2,
					NameOfBuyer = "John Doe"
				},
				new PurchaseOrder() {
					Id = 2,
					Datetime = DateTime.Now.AddDays(-2),
					Subtotal = 70.99,
					SalesTax = 106.35,
					PaymentTypeId = 4,
					NameOfBuyer = "Mil cab"
				},
				new PurchaseOrder() {
					Id = 3,
					Datetime = DateTime.Now.AddDays(-2),
					Subtotal = 99.99,
					SalesTax = 106.35,
					PaymentTypeId = 4,
					NameOfBuyer = "Mildred Smith"
				},
				new PurchaseOrder() {
					Id = 4,
					Datetime = DateTime.Now.AddDays(-2),
					Subtotal = 9.99,
					SalesTax = 106.35,
					PaymentTypeId = 4,
					NameOfBuyer = "Bob Pryt"
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
					Quantity = 1
				}
			};
			builder.Entity<PurchaseOrderItem>().HasData(orderItems);
		}
	}
}
