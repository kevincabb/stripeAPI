using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using LiteDB;

namespace MyStore.Models
{
	public class InventoryItem
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int id {get; set;}
        public string img {get; set;}
        public string itemName {get; set;}
        public string type {get; set;}
        public double itemPrice {get; set;}
        public int quantity {get; set; }
        public Boolean sold {get; set;}
        
        public InventoryItem(){}

        public InventoryItem(int id, string img, string itemName, string type, double itemPrice, int quantity, Boolean sold){
            this.id = id;
            this.img = img;
            this.itemName = itemName;
            this.type = type;
            this.itemPrice = itemPrice;
            this.quantity = quantity;
            this.sold = sold;
        }
	}
}
