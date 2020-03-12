using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using Stripe.Checkout;

namespace MyStore.Models
{
	public class PurchaseOrder
	{
		[Key] // informing ef that this is the tables unique identifier
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)] // informing ef that the db will provide this value
		public int Id { get; set; }
		public int PaymentTypeId { get; set; }
		public DateTime Datetime { get; set; }
		public double Subtotal { get; set; }
		public double SalesTax { get; set; }
		public string NameOfBuyer { get; set; }

		public ICollection<PurchaseOrderItem> PurchaseOrderItems { get; set; }
		[ForeignKey("PaymentTypeId")] public virtual PaymentType PaymentType { get; set; }

		public string StripeCheckoutSessionId { get; set; }

		[NotMapped] public Session StripeCheckoutSession { get; set; }

		[NotMapped] public double Total => Subtotal + SalesTax; // tells ef not to map this to database

		public PurchaseOrder() { }

		public PurchaseOrder(IEnumerable<PurchaseOrderRequest> itemList)
		{
			PaymentTypeId = 1; // None
			PurchaseOrderItems = itemList.Select(x => new PurchaseOrderItem()
			{
				InventoryItemId = x.InventoryItemId,
				Quantity = x.Quantity
			}).ToList();

			Subtotal = itemList.Sum(x =>
				x.Price * x.Quantity);
			SalesTax = Subtotal * .09;
		}
	}

	public class PurchaseOrderRequest
	{
		public int InventoryItemId { get; set; }
		public int Quantity { get; set; }
		[JsonIgnore] public double Price { get; set; }
		[JsonIgnore] public string Name {get;set;}
		[JsonIgnore] public string Description {get;set;}
	}

}
