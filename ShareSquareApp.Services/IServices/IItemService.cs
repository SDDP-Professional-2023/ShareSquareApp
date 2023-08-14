using ShareSquare.Data.Models;
using ShareSquare.Data.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareSquareApp.Services.IServices
{
    public interface IItemService
    {
        Item CreateNewItem(Item item);
        Task<List<Item>> GetAllItems();
        Task<Item?> GetItemById(int id);
        Task<List<Item>?> GetItems(FilterModel filter, string currentUserId = null);
        Task<List<Item>> GetItems();
        Task<int> GetItemsCount();
        Task UpdateItemAsync(Item item, DateTime? created_at);
    }
}
