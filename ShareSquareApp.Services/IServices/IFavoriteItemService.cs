using ShareSquare.Data.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareSquareApp.Services.IServices
{
    public interface IFavoriteItemService
    {
        void AddToFavorites(FavoriteItem favoriteItem);
        IEnumerable<FavoriteItem> GetAllFavorites();
        Task<List<FavoriteItem>> GetFavoriteItems(string userId);
        Task RemoveFavoriteItem(int id);
    }
}
