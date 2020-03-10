
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MyStore.Models;
using MyStore.Services.Context;

namespace MyStore.Services
{
	public class DataService
	{
		public readonly DataContext _context;
		public DataService(DataContext dataContext)
		{
			_context = dataContext;
		}
		public int InsertInventoryItem(InventoryItem item)
		{
			var trackedItem = _context.Add(item);
			return trackedItem.Entity.Id;
		}

		public IEnumerable<InventoryItem> GetInventoryItems()
		{
			return _context.InventoryItems;
		}

		#region Inventory Items
		public InventoryItem GetInventoryItemById(int id)
		{
			return _context.InventoryItems
				.SingleOrDefault(x => x.Id == id);
		}

		public IEnumerable<InventoryItem> GetItemsLessThan(double price)
		{
			return _context.InventoryItems
				.Where(x => x.Price < price);
		}

		public IEnumerable<ChangeNameRequest> GetNameAndIdsInStorageLocation(string location)
		{
			return GetInventoryItems()
				.Where(x => x.StorageLocation == location)
				.Select(x => new ChangeNameRequest()
				{
					Id = x.Id,
					Name = x.Name
				});
		}

		public bool UpdateInventoryItem(InventoryItem item)
		{
			_context.Update<InventoryItem>(item);
			return _context.SaveChanges() != 0;
		}

		public bool UpdateInventoryItemName(ChangeNameRequest request)
		{
			var item = GetInventoryItemById(request.Id);
			item.Name = request.Name; // changes to the fetch object are tracked by default.
			// calling save changes will update all tracked items with their new data.
			return _context.SaveChanges() != 0;
		}

		public bool DeleteInventoryItem(int id)
		{
			var item = GetInventoryItemById(id);
			_context.Remove(item);

			return _context.SaveChanges() != 0;
		}
		#endregion

		#region Purchase Orders
		public int InsertPurchaseOrder(PurchaseOrder order)
		{
			var trackedItem = _context.Add(order);
			return trackedItem.Entity.Id;
		}

		public IEnumerable<PurchaseOrder> GetPurchaseOrders()
		{
			return _context.PurchaseOrders
				.Include(x => x.Item)
				.Include(x => x.PaymentType);
		}

		public PurchaseOrder GetPurchaseOrderById(int id)
		{
			return GetPurchaseOrders().SingleOrDefault(x => x.Id == id);
		}

		public bool SavePurchaseOrder(PurchaseOrder order)
		{
			_context.Update(order);
			return _context.SaveChanges() != 0;
		}

		public bool DeletePurchaseOrder(int id)
		{
			var order = _context.PurchaseOrders.Single(x => x.Id == id);
			_context.PurchaseOrders.Remove(order);
			return _context.SaveChanges() != 0;
		}
		#endregion

		#region PaymentTypes
		public IEnumerable<PaymentType> GetPaymentTypes()
		{
			return _context.PaymentTypes;
		}

		// Create your own upsert. By knowing if the incoming item Id is 0, it can
		// be determined if its a new item or if an item should be updated.
		public bool UpsertPaymentTypes(PaymentType paymentType)
		{
			if (paymentType.Id == 0)
				_context.Add(paymentType);
			else
				_context.Update(paymentType);
			return _context.SaveChanges() != 0;
		}
		#endregion
	}
}
