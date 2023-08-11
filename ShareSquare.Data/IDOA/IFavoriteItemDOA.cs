using ShareSquare.Data.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareSquare.Data.IDOA
{
    public interface IFavoriteItemDOA
    {
        void Add(FavoriteItem item);
        void Delete(FavoriteItem item);
        Task<List<FavoriteItem>> GetFavoriteItems(string userId);
        List<FavoriteItem> GetItems();
        Task RemoveFavoriteItem(int id);
    }
}
