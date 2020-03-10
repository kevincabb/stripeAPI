using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using MyStore.Models;
using MyStore.Services;

namespace MyStore.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class PurchaseOrderController
	{
		readonly DataService _service;
		public PurchaseOrderController(DataService dataService)
		{
			_service = dataService;
		}

		[HttpGet]
		public IEnumerable<PurchaseOrder> GetPurchaseOrders()
		{
			return _service.GetPurchaseOrders();
		}

		[HttpGet("{id}")]
		public PurchaseOrder GetPurchaseOrderById(int id)
		{
			return _service.GetPurchaseOrderById(id);
		}

		[HttpPost("{id}")]
		public bool SavePurchaseOrder(int id, PurchaseOrder order)
		{
			return _service.SavePurchaseOrder(order);
		}

		[HttpDelete("{id}")]
		public bool DeletePurchaseOrder(int id)
		{
			return _service.DeletePurchaseOrder(id);
		}
	}
}
