# .Net Api with Entity Framework

Last updated: 3/13/2020

Starting out with the project we've already created
https://github.com/jfoleysjcoe/dotnetlitedb
and
https://github.com/jfoleysjcoe/dotnetefcore

## Setting up your project

Assuming you've caught up with the last state of the Entity Framework project, lets now add what we need for Stripe Api communication.

### Add Stripe package

First we'll need to add the Stripe dotnet package

`dotnet add package stripe.net`

### Set up key

##### appsettings.json

```json
"Stripe": {
	"ApiKey":"YourStripeSecretKey"
}
```

This key can be found at [Stripe Test Api Keys](https://dashboard.stripe.com/test/apikeys)

### Updating our Models and Context

Since we want purchase orders to contain a variety of inventory items with different quantities, we will have to update our database schema.

Purchase orders can't contain InventoryItemId or Quantity any more, since these will vary per item in the order.

I have added a new model called `PurchaseOrderItem`

```c#
public class PurchaseOrderItem
{
	[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public int Id { get; set; }

	public int InventoryItemId { get; set; } // which item is being bought
	public int PurchaseOrderId { get; set; } // which purchase order is this for
	public int Quantity { get; set; }	// how many items were bought

	[ForeignKey("InventoryItemId")]
	public virtual InventoryItem InventoryItem { get; set; }

	[JsonIgnore][ForeignKey("PurchaseOrderId")] // why did we JsonIgnore?
	public virtual PurchaseOrder PurchaseOrder { get; set; }
}
```

Having `PurchaseOrderItem` class allows us to clean up the `PurchaseOrder` class

##### PurchaseOrder.cs

```C#
public class PurchaseOrder
{
	[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public int Id { get; set; }

	public int PaymentTypeId { get; set; }
	public DateTime Datetime { get; set; }
	public double Subtotal { get; set; }
	public double SalesTax { get; set; }
	public string NameOfBuyer { get; set; } // do we still need this?

	// now a PO has a list of items in the sale each with its own Id and Quantity
	public ICollection<PurchaseOrderItem> PurchaseOrderItems { get; set; }

	[ForeignKey("PaymentTypeId")] public virtual PaymentType PaymentType { get; set; }

	// each PO request generates a sessionId that is unique in Stripe, it would be wise to
	// to store this with the PO in case we need to reference it.
	public string StripeCheckoutSessionId { get; set; }

	[NotMapped] public Session StripeCheckoutSession { get; set; }
	[NotMapped] public double Total => Subtotal + SalesTax;
}
```

##### PurchaseOrder.cs

```c#
public class PurchaseOrderRequest
{
	public int InventoryItemId { get; set; }
	public int Quantity { get; set; }
	[JsonIgnore] public double Price { get; set; } // JsonIgnore?
	[JsonIgnore] public string Name { get; set; } // we'll see later how we're using this
	[JsonIgnore] public string Description { get; set; } // and why it is helpful
}
```

Great, we've changed a bunch of models. Now what? Well, to start, we had some seed data that we created that is now, very much, broken and giving us compilation errors. So you'll have to update the context to properly seed build and seed the database.

Lucky for you, I'll toss that code in here

##### DataContext.cs

```C#
// we need to add the DbSet of the new class we made
public DbSet<PurchaseOrderItem> PurchaseOrderItems { get; set; }

// update the seedData function
private void SeedData(ModelBuilder builder)
{
	// Inventory Fixed Data was not pasted in since it didn't change

	// New Payment Type "None" was added as first record
	var types = new List<PaymentType> {
		new PaymentType() {
			Id = 1,
			Name = "None"
		},
		new PaymentType() {
			Id = 2,
			Name = "Cash"
		},
		new PaymentType() {
			Id = 3,
			Name = "Credit"
		},
		new PaymentType() {
			Id = 4,
			Name = "Debit"
		},
		new PaymentType() {
			Id = 5,
			Name = "Check"
		},
		new PaymentType() {
			Id = 6,
			Name = "GiftCard"
		}
	};
	builder.Entity<PaymentType>().HasData(types);

	// Update mocking of purchase orders
	var orders = new List<PurchaseOrder> {
		new PurchaseOrder() {
			Id = 1,
			Datetime = DateTime.Now.AddDays(-1),
			Subtotal = 5.5,
			SalesTax = .45,
			PaymentTypeId = 2,
			NameOfBuyer = "John Doe"
		},
		new PurchaseOrder() {
			Id = 2,
			Datetime = DateTime.Now.AddDays(-2),
			Subtotal = 98.5,
			SalesTax = 106.35,
			PaymentTypeId = 4,
			NameOfBuyer = "Mildred Smith"
		}
	};
	builder.Entity<PurchaseOrder>().HasData(orders);

	// Mock the Item list for the purchase orders above
	var orderItems = new List<PurchaseOrderItem> {
		new PurchaseOrderItem() {
			Id = 1,
			InventoryItemId = 1,
			PurchaseOrderId = 1,
			Quantity = 2
		},new PurchaseOrderItem() {
			Id = 2,
			InventoryItemId = 3,
			PurchaseOrderId = 1,
			Quantity = 1
		},new PurchaseOrderItem() {
			Id = 3,
			InventoryItemId = 1,
			PurchaseOrderId = 2,
			Quantity = 1
		},new PurchaseOrderItem() {
			Id = 4,
			InventoryItemId = 4,
			PurchaseOrderId = 2,
			Quantity = 2
		}
	};
	builder.Entity<PurchaseOrderItem>().HasData(orderItems);
}
```

### Are we ready now?

No, we still need to record the migration and update the dataabase.

So we'll run our migration commands

`dotnet ef migrations UpdatePurchaseOrderModel`

and

`dotnet ef database update`

If that was successful, we're good to continue.

---

## Stripe Session Service

### _This section coorelates to "Init Stripe Session with Redirect" in the client ReadMe_

According to [Stripe Api Doc - Sessions](https://stripe.com/docs/api/checkout/sessions/create) we need to use the `SessionService`. They even provide a snippet of code as an example and the response expected.

Since `SessionService` is a service, we should add it to the services provided by the .net framework by adding it in `Startup.cs`

##### Startup.cs

```c#
// we need to set the Private key from config
Stripe.StripeConfiguration.ApiKey = Configuration
	.GetSection("Stripe").GetValue("ApiKey", "");

services
	.AddTransient<SessionService>();

```

### Helper Service and usage

I decided to create a service that handles all Stripe related calls. This lets me focus Stripe work in one service.

##### StripeHelperService.cs

```C#
public class StripeHelperService
{
  readonly DataService _dataService;
  readonly SessionService sessions;
  public StripeHelperService(
    DataService dataService,
    SessionService sessionService)
  {
    _dataService = dataService;
    sessions = sessionService;
  }

  public async Task<Session> InitSessionAsync(PurchaseOrderRequest[] request)
  {
    var itemList = request.Join(
      _dataService.GetInventoryItems(),
      x => x.InventoryItemId,
      y => y.Id,
      (x, y) => new PurchaseOrderRequest() {
        InventoryItemId = x.InventoryItemId,
        Quantity = x.Quantity,
        Price = y.Price,
        Name = y.Name,
        Description = y.Description
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
      SuccessUrl = "http://localhost:4200/result?session_id={CHECKOUT_SESSION_ID}",
      CancelUrl = $"http://localhost:4200/checkout?cancel={poId}",
      Metadata = metaValues
    };
    var session = sessions.Create(options);
    po.StripeCheckoutSessionId = session.Id;
    _dataService.SavePurchaseOrder(po);
    return session;
  }
}
```

And of course we'll add it to Startup

##### Startup.cs

```c#
	.AddTransient<StripeHelperService>()
```

### Updating our PurchaseOrder Controller

##### PurchaseOrderController.cs

```c#
[HttpPost("newSession")]
public async Task<Session> InitiatePaymentWithStripeAsync(PurchaseOrderRequest[] request)
{
	return await _stripeHelper.InitSessionAsync(request);
}
```

With a successful session created. Our client can redirect to Stripe with the session Id and present the payment form.

---

## After Payment Made

### _This section coorelates to "After Redirect" in the client ReadMe_

On successful payment, we know we receive the sessionId back from stripe. We know this ecause of the SuccessUrl we assigned in the creation of the Session.

```c#
  SuccessUrl = "http://localhost:4200/result?session_id={CHECKOUT_SESSION_ID}",
```

Knowing this we'll create a function in our helper service to fetch the session from the Stripe API

##### StripeHelperService.cs
```c#
public Session GetSessionBySessionId(string sessionId)
{
  return sessions.Get(sessionId);
}
```

##### PurchaseOrderController.cs
```c#
[HttpGet("session/{sessionId}")]
public Session GetSession(string sessionId)
{
  return _stripeHelper.GetSessionBySessionId(sessionId);
}
```

Great!
---

But is this result useful to us?

Nope.

### Improving Session Response
You might not have noticed, but this is the same exact session object we had created except 1 field has been updated. `PaymentIntentId`.

The Payment Intent Id provides information about the actual payment. Let see how to fetch the PaymentIntent.

According to [Stripe Api Doc - PaymentIntents](https://stripe.com/docs/api/payment_intents/retrieve) we need to use the `PaymentIntentService`. They provide a snippet of code as an example and the response expected.

So, just like `SessionService`, we'll add this in Startup

##### Startup.cs
```c#
services
  .AddTransient<StripeHelperService>()
  .AddTransient<SessionService>()
  .AddTransient<PaymentIntentService>();
// we can just append the Add function to simplify the code.
```

```c#
public Session GetSessionBySessionId(string sessionId)
{
  var res = sessions.Get(sessionId);
  // remember to add paymentIntents to the contructor!
  res.PaymentIntent = paymentIntents.Get(res.PaymentIntentId);
  return res;
}
```

_Vommit Data Much?_
--

---
## Side Quest!
### Json Response Clean up

When dealing with objects with a pluthera of null values, we can clean up the response by asking Json to ignore them.
##### Startup.cs
```c#
services.AddControllers() // must be appended to AddControllers()
  .AddJsonOptions(options => options.JsonSerializerOptions.IgnoreNullValues = true);
```
---

## Returning meaningful data

Its up to the Api to return meaningful data. Since you're also building the client, you will have the insight to know what that data is.

In our case, we don't need the `session`, as is. We want the `PurchaseOrder`, which can include the `session`. This would be useful to show a printable receipt.

So we'll add a request to use the `sessionId`, to fetch the `PurchaseOrder`.

##### StripeHelperService.cs
```c#
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
```

## Whats left?

The only part left is to decide what to do with a cancel request. You can use the poId to do whatever you want!

Good luck!
