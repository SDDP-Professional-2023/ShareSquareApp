using ShareSquare.Data.DOA;
using ShareSquare.Data.IDOA;
using ShareSquare.Data.Models.Domain;
using ShareSquareApp.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareSquareApp.Services
{
    public class UserItemService : IUserItemService
    {
        private readonly IItemDOA _itemDoa;

        public UserItemService(IItemDOA itemDoa)
        {
            _itemDoa = itemDoa;
        }

        public async Task<List<Item>> GetUserItemsAsync(string userId, ItemStatus? status = null)
        {
            var items = _itemDoa.GetUserItems(userId);

            if (status.HasValue)
            {
                items = items.Where(i => i.Status == status.Value).ToList();
            }

            return items;
        }

        public async Task SoftDeleteItemAsync(int itemId)
        {
            var item = _itemDoa.GetUserItem(itemId);
            item.Status = ItemStatus.Deleted;
            _itemDoa.Update(item);
        }

        public async Task UpdateItemStatusAsync(int itemId, ItemStatus status)
        {
            var item = _itemDoa.GetUserItem(itemId);
            item.Status = status;
            _itemDoa.Update(item);
        }
    }
}
