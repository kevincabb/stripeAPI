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
		
		readonly DataService _dataService;
		public InventoryController(
			DataService dataService
		)
		{
			_dataService = dataService;
		}

		// [HttpGet("populate")]
		// public int PopulateData()
		// {
		// 	var fixedData = _fixedService.fixedData;
		// 	return _liteDbService.PopulateData(fixedData);
		// }

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

		[HttpPost("add")]
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
		public bool Update(InventoryItem product)
		{
			List<InventoryItem> items =new List<InventoryItem>(_dataService.GetInventoryItems());
			foreach(var item in items)
            {
                if(product.id == item.id){
                    return _dataService.UpdateInventoryItem(product);
                }
            }
			return false;
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

		// [HttpGet("ItemsInLocation/{location}")]
		// public IEnumerable<ChangeNameRequest> GetNameAndIdInLocation(string location)
		// {
		// 	// return _liteDbService.GetNameAndIdsInStorageLocation(location);
		// 	return _dataService.GetNameAndIdsInStorageLocation(location);
		// }

	}
}
