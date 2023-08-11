using Microsoft.EntityFrameworkCore;
using ShareSquare.Data.IDOA;
using ShareSquare.Data.Models.Domain;
using ShareSquare.Data.Models.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareSquare.Data.DOA
{
    public class FavoriteItemDOA : IFavoriteItemDOA
    {
        private readonly ShareSquareDbContext _context;

        public FavoriteItemDOA(ShareSquareDbContext context)
        {
            _context = context;
        }

        public void Add(FavoriteItem item)
        {
            _context.FavoriteItems.Add(item);
            _context.SaveChanges();
        }

        public void Delete(FavoriteItem item)
        {
            _context.FavoriteItems.Remove(item);
            _context.SaveChanges();
        }

        public List<FavoriteItem> GetItems()
        {
            return _context.FavoriteItems.ToList();
        }

        public async Task<List<FavoriteItem>> GetFavoriteItems(string userId)
        {
            return await _context.FavoriteItems
                .Include(fi => fi.Item)
                .Where(fi => fi.UserId == userId)
                .ToListAsync();
        }

        public async Task RemoveFavoriteItem(int id)
        {
            var favoriteItem = await _context.FavoriteItems.FindAsync(id);
            _context.FavoriteItems.Remove(favoriteItem);
            await _context.SaveChangesAsync();
        }
    }
}
