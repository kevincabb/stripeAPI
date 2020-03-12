using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyStore.Models;
using MyStore.Services;
using Stripe;
using Stripe.Checkout;

namespace MyStore.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class PurchaseOrderController
	{
		readonly DataService _service;
		readonly StripeHelperService _stripeHelper;
		public PurchaseOrderController(DataService dataService, StripeHelperService stripeHelper)
		{
			_service = dataService;
			_stripeHelper = stripeHelper;
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

		[HttpPost("newSession")]
		public async Task<Session> InitiatePaymentWithStripeAsync(PurchaseOrderRequest[] request)
		{
			return await _stripeHelper.InitSessionAsync(request);
		}

		[HttpGet("bySession/{sessionId}")]
		public PurchaseOrder ProcessItem(string sessionId)
		{
			return _stripeHelper.GetPurchaseOrderBySessionId(sessionId);
		}
	}
}
