using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyStore.Models;
using Stripe;
using Stripe.Checkout;

namespace MyStore.Services
{
	public class StripeHelperService
	{
		readonly DataService _dataService;
		readonly SessionService sessions;
		readonly PaymentIntentService paymentIntents;
		public StripeHelperService(
			DataService dataService,
			SessionService sessionService,
			PaymentIntentService paymentIntentService)
		{
			_dataService = dataService;
			sessions = sessionService;
			paymentIntents = paymentIntentService;
		}

		public async Task<Session> InitSessionAsync(PurchaseOrderRequest[] request)
		{
			var requestEnum = request.AsEnumerable();
			var itemList = request.Join(_dataService.GetInventoryItems(),
			x => x.InventoryItemId, y => y.Id, (x, y) => new PurchaseOrderRequest()
			{
				InventoryItemId = x.InventoryItemId,
				Quantity = x.Quantity,
				Price = y.Price,
				Name = y.Name,
				Description = y.Description
			});


			var po = new PurchaseOrder(itemList);
			var poId = await _dataService.InsertPurchaseOrder(po);
			var metaValues = new Dictionary<string, string>();
			metaValues.Add("PurchaseOrderId", $"{poId}");

			var options = new SessionCreateOptions
			{
				PaymentMethodTypes = new List<string> {
							"card", // other 2 options are ideal and FPT (appears to be country driven)
					},
				LineItems = itemList.Select(x =>
				{
					return new SessionLineItemOptions
					{
						Name = $"{x.Name}",
						Description = $"{x.Description}",
						Amount = (long)(x.Price * 100),
						Currency = "usd",
						Quantity = x.Quantity
					};
				}).Append(new SessionLineItemOptions
				{
					Name = "Sales Tax",
					Description = "9.00%",
					Amount = (long)(po.SalesTax * 100),
					Currency = "usd",
					Quantity = 1
				}).ToList(),
				SuccessUrl = "http://localhost:4200/checkout?session_id={CHECKOUT_SESSION_ID}",
				CancelUrl = $"http://localhost:4200/checkout?cancel={poId}",
				Metadata = metaValues
			};
			var session = sessions.Create(options);
			po.StripeCheckoutSessionId = session.Id;
			_dataService.SavePurchaseOrder(po);
			return session;
		}

		public Session GetSessionBySessionId(string sessionId)
		{
			var res = sessions.Get(sessionId);
			res.PaymentIntent = paymentIntents.Get(res.PaymentIntentId);
			return res;
		}

		public PurchaseOrder GetPurchaseOrderBySessionId(string sessionId)
		{
			var res = sessions.Get(sessionId);
			var poId = Int32.Parse(res.Metadata.GetValueOrDefault("PurchaseOrderId"));
			var po = _dataService.GetPurchaseOrderById(poId);

			res.PaymentIntent = paymentIntents.Get(res.PaymentIntentId);

			po.StripeCheckoutSession = res;

			return po;
		}
	}
}
