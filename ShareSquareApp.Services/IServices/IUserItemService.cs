using ShareSquare.Data.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareSquareApp.Services.IServices
{
    public interface IUserItemService
    {
        Task<List<Item>> GetUserItemsAsync(string userId, ItemStatus? status = null);
        Task SoftDeleteItemAsync(int itemId);
        Task UpdateItemStatusAsync(int itemId, ItemStatus status);
    }
}
