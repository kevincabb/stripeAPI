using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using MyStore.Models;
using MyStore.Services;

namespace MyStore.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class InventoryController : ControllerBase
	{
		readonly InventoryFixedDataService _fixedService;
		readonly InventoryLiteDbService _liteDbService;
		readonly DataService _dataService;
		public InventoryController(
			InventoryFixedDataService fixedService,
			InventoryLiteDbService liteDbService,
			DataService dataService
		)
		{
			_fixedService = fixedService;
			_liteDbService = liteDbService;
			_dataService = dataService;
		}

		[HttpGet("populate")]
		public int PopulateData()
		{
			var fixedData = _fixedService.fixedData;
			return _liteDbService.PopulateData(fixedData);
		}

		[HttpGet]
		public IEnumerable<InventoryItem> GetInventoryItems()
		{
			// return _fixedService.fixedData;
			return _dataService.GetInventoryItems();
		}

		[HttpGet("{id}")]
		public InventoryItem GetInventoryItem(int id)
		{
			// return _fixedService.fixedData.AsEnumerable().First(x => x.Id == id);
			// return _liteDbService.GetInventoryItemById(id);
			return _dataService.GetInventoryItemById(id);
		}

		[HttpPost]
		public int AddInventoryItem(InventoryItem item)
		{
			// return _fixedService.Insert(item);
			// return _liteDbService.Insert(item);
			return _dataService.InsertInventoryItem(item);
		}

		[HttpDelete("{id}")]
		public bool DeleteInventoryItem(int id)
		{
			//return _fixedService.Delete(id);
			return _dataService.DeleteInventoryItem(id); ;
		}

		[HttpPost("update")]
		public bool Update(InventoryItem item)
		{
			//return _fixedService.Update(item);
			// return _liteDbService.Update(item);
			return _dataService.UpdateInventoryItem(item);
		}

		[HttpGet("findBelowPrice/{price}")]
		public IEnumerable<InventoryItem> FindBelowPrice(double price)
		{
			// return _fixedService.GetItemsLessThan(price);
			// return _liteDbService.GetItemsLessThan(price);
			return _dataService.GetItemsLessThan(price);
		}

		[HttpPost("update/name")]
		public bool UpdateName(ChangeNameRequest request)
		{
			// return _liteDbService.UpdateName(request);
			return _dataService.UpdateInventoryItemName(request);
		}

		[HttpGet("ItemsInLocation/{location}")]
		public IEnumerable<ChangeNameRequest> GetNameAndIdInLocation(string location)
		{
			// return _liteDbService.GetNameAndIdsInStorageLocation(location);
			return _dataService.GetNameAndIdsInStorageLocation(location);
		}

	}
}
