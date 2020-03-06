using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MyStore.Context;
using MyStore.Models;

namespace MyStore.Services
{
	public class DataService
	{
		private readonly StoreDataContext context;
		public DataService(StoreDataContext context)
		{
			this.context = context;
		}
		#region InventoryItems
		public IEnumerable<InventoryItem> GetInventoryList()
		{
			return context.InventoryItems;
		}

		public InventoryItem GetInventoryItemById(int id)
		{
			return context.InventoryItems.FirstOrDefault(x => x.Id == id);
		}

		public int InsertInventoryItem(InventoryItem item)
		{
			var trackedVersionOfItem = context.Add(item);
			context.SaveChanges();
			return trackedVersionOfItem.Entity.Id;
		}

		public InventoryItem UpdateInventoryItem(InventoryItem item)
		{
			var trackedVersionOfItem = context.Update(item);
			context.SaveChanges();
			return trackedVersionOfItem.Entity;
		}

		public bool DeleteInventoryItem(int id)
		{
			var item = context.InventoryItems.FirstOrDefault(x => x.Id == id);
			context.Remove(item);
			return context.SaveChanges() != 0;
		}

		#endregion

		#region PurchaseOrder
		public IEnumerable<PurchaseOrder> GetPurchaseOrdersList()
		{
			return context.PurchaseOrders
				.Include(x => x.Item)
				.Include(x => x.PaymentType);
		}

		public PurchaseOrder GetPurchaseOrderById(int id)
		{
			return context.PurchaseOrders.FirstOrDefault(x => x.Id == id);
		}
		#endregion


	}

}
