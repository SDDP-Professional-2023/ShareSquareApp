using Microsoft.AspNetCore.Mvc;
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
    public class FavoriteItemService : IFavoriteItemService
    {

        private readonly IFavoriteItemDOA _favoriteItemDOA;
        public FavoriteItemService(IFavoriteItemDOA favoriteItemDOA)
        {
            _favoriteItemDOA = favoriteItemDOA;
        }
        public void AddToFavorites(FavoriteItem favoriteItem)
        {
            _favoriteItemDOA.Add(favoriteItem);
        }

        public IEnumerable<FavoriteItem> GetAllFavorites()
        {
            var favoriteItem = _favoriteItemDOA.GetItems();
            return favoriteItem;
        }

        public Task<List<FavoriteItem>> GetFavoriteItems(string userId)
        {
            return _favoriteItemDOA.GetFavoriteItems(userId);
        }

        public Task RemoveFavoriteItem(int id)
        {
            return _favoriteItemDOA.RemoveFavoriteItem(id);
        }
    }
}
