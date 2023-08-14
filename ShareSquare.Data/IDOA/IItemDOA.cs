using ShareSquare.Data.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareSquare.Data.IDOA
{
    public interface IItemDOA
    {
        void Add(Item item);
        void Delete(Item item);
        Item? GetUserItem(int id);
        List<Item> GetUserItems(string currentUserId = null);
        Task<Item?> GetItem(int id);
        Task<List<Item>> GetItems(string currentUserId = null);
        Task UpdateItem(Item item);
        void Update(Item item);
        Task<List<Item>> GetAllItems();
        Task<int> GetItemsCount();
    }
}
