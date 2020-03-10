using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MyStore.Models
{
	public class PurchaseOrder
	{
		[Key] // informing ef that this is the tables unique identifier
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)] // informing ef that the db will provide this value
		public int Id { get; set; }
		public int InventoryItemId { get; set; }
		public int PaymentTypeId { get; set; }
		public DateTime Datetime { get; set; }
		public int Quantity { get; set; }
		public double Subtotal { get; set; }
		public double SalesTax { get; set; }
		public string NameOfBuyer { get; set; }
		[NotMapped] public string ItemName => Item?.Name;
		// we added JsonIgnore in lecture to show how item can be hidden from client
		[JsonIgnore, ForeignKey("InventoryItemId")] // referencing the InventoryItemId above
		public virtual InventoryItem Item { get; set; } // virtual allows ef to lazy load the property for efficieny
		[ForeignKey("PaymentTypeId")]
		public virtual PaymentType PaymentType { get; set; }

		[NotMapped] public double Total => Subtotal + SalesTax; // tells ef not to map this to database
	}
}
