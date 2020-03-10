# .Net Api with Entity Framework

Last updated: 3/10/2020

Starting out with the project we've already created

https://github.com/jfoleysjcoe/dotnetlitedb

## EF Core Cli

### Setup

`dotnet add package Microsoft.EntityFrameworkCore --version 3.1.2`

`dotnet add package Microsoft.EntityFrameworkCore.Tools --version 3.1.2`

`dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 3.1.2`

`dotnet add package Microsoft.EntityFrameworkCore.Design --version 3.1.2`

If `dotnet ef` doesn't give you a unicorn. Run the following command.

`dotnet tool install -g dotnet-ef`

---
## Command line helper

Tell Entity Framework that you've added a change to migrate. Migrations will only be added if the .net build succeeds.

`dotnet ef migrations add [name]`

\* [name] should be descriptive. -- For example: `Initialize`, `UpdateInventoryModel`, `AddPurchaseOrderModel`

Tell Entity Framekwork to sync your migrations with the database.

`dotnet ef database update`

Tell Entity Framework to undo your last migration

`dotnet ef migrations remove`

### Important!

1. Entity Framework can be frustrating at time when you're dealing with your migrations. This is normal :)
2. Do not rename or move your models or context once you run `migrations add` because the Migrations folder references these models and locations. If this is something you want to do, please ping me and I'll explain the process to accomplish this correctly.
3. Always Stop your api when running EF commands. This will ensure EF will use the latest code.

---

## Setting up your project

Models and Context are the key classes for Entity Framework.

### Models

We have a model `InventoryItem.cs`

Lets add a couple more and take a look at some Entity Framework related decorators.

Don't forget to add similar modifiers to `InventoryItem.cs`

##### PurchaseOrder.cs
```C#
public class PurchaseOrder
{
	[Key] // informing ef that this is the tables unique identifier
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)] // informing ef that the db will provide this value
	public int Id { get; set; }
	public int InventoryItemId { get; set; }
	public int PaymentTypeId { get; set; }
	public DateTime Datetime { get; set; }
	public int Quantity { get; set; }
	public double Subtotal {get; set; }
	public double SalesTax { get; set; }
	public string NameOfBuyer { get; set; }

	[ForeignKey("InventoryItemId")] // referencing the InventoryItemId above
	public virtual InventoryItem Item { get; set; } // virtual allows ef to lazy load the property for efficieny
	[ForeignKey("PaymentTypeId")]
	public virtual PaymentType PaymentType { get; set; }

	[NotMapped] public double Total => Subtotal + SalesTax; // tells ef not to map this to database
}
```
##### PaymentType.cs
```C#
public class PaymentType
{
	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	public int Id { get; set; }
	public string Name { get; set; }
}
```

### Context

A context class is an important piece of working with the Entity Framework. It represents a session of the underlying database so that you can perform tasks on that database.

The context class is used to query or save data to the database. It is also used to configure domain classes, database related mappings, change tracking settings, caching, transaction etc.

We will create a single context for this demo.

As you can see below, every context created will inherit from `DbContext`. We also included entity sets for `InventoryItem`, `PurchaseOrder` and `PaymentType`

##### DataContext.cs
```C#
public class DataContext : DbContext
{
	public DbSet<InventoryItem> InventoryItems { get; set; }
	public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
	public DbSet<PaymentType> PaymentTypes { get; set; }

	public DataContext(DbContextOptions<DataContext> options) : base(options)
	{ }

}
```

With the context created, we need to add it to the services provided within `Startup.cs`.

##### Startup.cs
```C#
public void ConfigureServices(IServiceCollection services)
{
	...
	var connectionString = Configuration.GetConnectionString("NameOfMyConnectionString"); // from app settings
	services.AddDbContext<DataContext>(options => options.UseSqlServer(connectionString));
	...
}
```

Your connection string will be stored within your `appsettings.json`. We will go over how to obtain this string from you database.

##### appsettings.json
```json
"ConnectionStrings": {
	"NameOfMyConnectionString": "Data Source=<server-name>;Initial Catalog=<dbname>;Persist Security Info=True;User ID=<username>;Password=<password>"
}
```

### Seeding Data

Now that you have you context started with your models ready, we can request Entity Framework to initialize the database with some data if it is empty. We do this by overriding the `OnModelCreating` within the context.

##### DataContext.cs
```C#
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
	base.OnModelCreating(modelBuilder);
	SeedData(modelBuilder);
}

private void SeedData(ModelBuilder builder)
{
	//took fixed data straight from fixedService
	var fixedData = new List<InventoryItem> {
		new InventoryItem(1, "#2 pencil", "Pencil", .50, "38830982031", "A1", 100),
		new InventoryItem(2, "spiral notebook", "Notebook", 1.50, "3881111131", "A2", 50),
		new InventoryItem(3, "3 ring binder with dividers", "Binder", 4.50, "54830982031", "A2", 5),
		new InventoryItem(4, "Scientific calculator", "Ti83+ Calculator", 49.00, "3889462031", "A4", 100),
		new InventoryItem(5, "black ball point pen", "Pen", .50, "388309867", "A1", 10),
		new InventoryItem(6, "metallic coaster", "Coaster", 5.50, "388309212", "A6", 1),
		new InventoryItem(7, "Fuzzy backpack", "Backpack", 25.49, "388309987", "A5", 100)
	};
	builder.Entity<InventoryItem>().HasData(fixedData);

	// also initializing the accepted payment types
	var types = new List<PaymentType> {
		new PaymentType() {
			Name = "Cash"
		},
		new PaymentType() {
			Name = "Credit"
		},
		new PaymentType() {
			Name = "Debit"
		},
		new PaymentType() {
			Name = "Check"
		},
		new PaymentType() {
			Name = "GiftCard"
		}
	};
	builder.Entity<PaymentType>().HasData(types);

	// and mocking some purchase orders
	var orders = new List<PurchaseOrder> {
		new PurchaseOrder() {
			Id = 1,
			InventoryItemId = 1,
			Quantity = 2,
			Datetime = DateTime.Now.AddDays(-1),
			Subtotal = 1,
			SalesTax = .25,
			PaymentTypeId = 1,
			NameOfBuyer = "John Doe"
		},
		new PurchaseOrder() {
			Id = 2,
			InventoryItemId = 3,
			Quantity = 4,
			Datetime = DateTime.Now.AddDays(-2),
			Subtotal = 18,
			SalesTax = 4.5,
			PaymentTypeId = 3,
			NameOfBuyer = "Mildred Smith"
		}
	};
	builder.Entity<PurchaseOrder>().HasData(orders);
}
```

## All the CRUD

Everything should now be set up for you to start `C`reating, `R`eading, `U`pdating and `D`eleting data, except of course we haven't created a service yet.

So that's the next step.

We have already created `InventoryFixedDataService` and the `InventoryLiteDbService`. Both of those had a single responsibility. In this demo we'll be creating a more broad service called `DataService`.

The only different between this service and the others is the context property.

```C#
public class DataService
{
	private readonly DataContext context;
	public DataService(DataContext context) // this is provided because we defined it in startup
	{
		this.context = context;
	}


}
```

### Using the context
#### Creating
```C#
context.Add(item);

context.SaveChanges(); // remember to save changes
```
#### Reading
```C#
var fullList = context.InventoryItems;
var listOfSaleItems = context.InventoryItems.Where(x => x.IsSaleItem);
```

#### Updating
```C#
context.Update(item); // if item.Id != 0, will update all fields to match the new object

// alternatively
var fetchedItem = context.InventoryItems.FindOrDefault(x => x.Id == item.Id);
fetchedItem.Name = "New Name"; // just update a single field

// remember to save changes
context.SaveChanges();
```
#### Deleting
```C#
var item = context.InventoryItems.FirstOrDefault(x => x.Id == id);
context.Remove(item);

// save changes!
context.SaveChanges();
```

### Including Virtual objects.
```C#
var listOfSaleItems = context.PurchaseOrders
	.Include(x => x.Item) // asking entity framework to use InventoryItemId (foreignkey) to map this object with the query
	.Include(x => x.PaymentType); // asking entity framework to use PaymentTypeId (foreignkey) to map this object with the query
```
~ FIN
