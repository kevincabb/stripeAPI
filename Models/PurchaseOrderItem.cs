using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MyStore.Models
{
	public class PurchaseOrderItem
	{
		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		public int InventoryItemId { get; set; }
		public int PurchaseOrderId { get; set; }
		public int Quantity { get; set; }

		[ForeignKey("InventoryItemId")] public virtual InventoryItem InventoryItem { get; set; }
		[JsonIgnore][ForeignKey("PurchaseOrderId")] public virtual PurchaseOrder PurchaseOrder { get; set; }
	}
}
