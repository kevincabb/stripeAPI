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
			var itemList = request.Join(
				_dataService.GetInventoryItems(),
				x => x.InventoryItemId,
				y => y.id,
				(x, y) => new PurchaseOrderRequest()
				{
					InventoryItemId = x.InventoryItemId,
					Quantity = x.Quantity,
					Price = y.itemPrice,
					Name = y.itemName,
					Description = y.type
				});

			var po = new PurchaseOrder(itemList);
			var poId = await _dataService.InsertPurchaseOrder(po);
			// we had to convert InsertPurchaseOrder to async method because we need
			// to wait until the database saves and returns us an Id, so we use "await" to pause
			// the thread until the Task is complete.

			// metaValues, Stripe lets you add meta data to your sessions/payments/transactions.
			var metaValues = new Dictionary<string, string>();
			metaValues.Add("PurchaseOrderId", $"{poId}");

			var options = new SessionCreateOptions
			{
				PaymentMethodTypes = new List<string> {
					"card", // other 2 options are ideal and FPT (appears to be country driven)
				},
				LineItems = itemList.Select(x =>
				{
					// first make a LineItem for each item in request list.
					return new SessionLineItemOptions
					{
						Name = $"{x.Name}",
						Description = $"{x.Description}",
						Amount = (long)(x.Price * 100), // Stripe uses long for price
						Currency = "usd",
						Quantity = x.Quantity
					};
					// after list is generated, we'll append the sales tax line
				}).Append(new SessionLineItemOptions
				{
					Name = "Sales Tax",
					Description = "9.00%",
					Amount = (long)(po.SalesTax * 100),
					Currency = "usd",
					Quantity = 1
				}).ToList(),
				SuccessUrl = "http://kevin.azurewebsites.net/webstore/result?session_id={CHECKOUT_SESSION_ID}",
				CancelUrl = $"http://kevin.azurewebsites.net/webstore//home",
				Metadata = metaValues
			};
			var session = sessions.Create(options); // Create session
																							// Save sessionId in the purchase order request
			po.StripeCheckoutSessionId = session.Id;
			_dataService.SavePurchaseOrder(po);
			// return the session, could just respond with sessionId

			// So the Session knows the Purchase Order its referring to and the PO also has the sessionId.
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
			var res = GetSessionBySessionId(sessionId);

			var poId = Int32.Parse(res.Metadata.GetValueOrDefault("PurchaseOrderId"));
			var po = _dataService.GetPurchaseOrderById(poId);

			// we can also query the db to find the PO that matches the property sessionId
			var poAlt = _dataService.GetPurchaseOrders()
				.SingleOrDefault(x => x.StripeCheckoutSessionId == sessionId);

			if (po == null || poAlt == null)
				return null;

			po.StripeCheckoutSession = res;
			return po;
		}
	}
}
