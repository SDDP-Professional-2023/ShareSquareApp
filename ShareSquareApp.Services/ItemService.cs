using Microsoft.EntityFrameworkCore;
using ShareSquare.Data.DOA;
using ShareSquare.Data.IDOA;
using ShareSquare.Data.Models;
using ShareSquare.Data.Models.Domain;
using ShareSquare.SD;
using ShareSquareApp.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareSquareApp.Services
{
    public class ItemService : IItemService
    {
        private readonly IItemDOA _itemDOA;
        public ItemService(IItemDOA itemDOA)
        {
            _itemDOA = itemDOA;
        }

        public Item CreateNewItem(Item item)
        {
            item.Created_at = DateTime.UtcNow;
            item.Updated_at = null;
            _itemDOA.Add(item);
            return item;
        }

        public async Task<List<Item>?> GetItems(FilterModel filter, string currentUserId = null)
        {
            var items = await _itemDOA.GetItems(currentUserId);

            if (filter.StartYear != 0 && filter.EndYear != 0)
            {
                items = items.Where(i => i.PublicationYearOrReleaseYear >= filter.StartYear && i.PublicationYearOrReleaseYear <= filter.EndYear).ToList();
            }

            if (!string.IsNullOrEmpty(filter.Language))
            {
                items = items.Where(i => i.Language == filter.Language).ToList();
            }

            if (!string.IsNullOrEmpty(filter.Condition))
            {
                items = items.Where(i => i.Condition == filter.Condition).ToList();
            }

            if (filter.Price != 0)
            {
                items = items.Where(i => i.Price <= filter.Price).ToList();
            }

            return items;
        }

        public async Task<Item?> GetItemById(int id)
        {
            var item = await _itemDOA.GetItem(id);
            return item;
        }

        public async Task UpdateItemAsync(Item item, DateTime? created_at)
        {
            item.Created_at = created_at;
            item.Updated_at = DateTime.UtcNow;
            await _itemDOA.UpdateItem(item);
        }
        public async Task<List<Item>> GetItems()
        {
            var items = await _itemDOA.GetItems();

            return items;
        }

        public async Task<List<Item>> GetAllItems()
        {
            var items = await _itemDOA.GetAllItems();

            return items;
        }

        public async Task<int> GetItemsCount()
        {
            return await _itemDOA.GetItemsCount();
        }
    }
}
