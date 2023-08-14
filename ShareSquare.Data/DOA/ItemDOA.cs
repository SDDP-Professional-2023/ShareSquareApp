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
    public class ItemDOA : IItemDOA
    {
        private readonly ShareSquareDbContext _context;

        public ItemDOA(ShareSquareDbContext context)
        {
            _context = context;
        }
        public void Add(Item item)
        {
            _context.Items.Add(item);
            _context.SaveChanges();
        }

        public void Delete(Item item)
        {
            _context.Items.Remove(item);
            _context.SaveChanges();
        }

        public async Task<Item?> GetItem(int id)
        {
            var item = await _context.Items.FindAsync(id);
            return item ?? null;
        }

        public async Task<List<Item>> GetItems(string currentUserId = null)
        {
            if (currentUserId == null)
            {
                return await _context.Items.Where(i => i.Status == ItemStatus.Active).ToListAsync();
            }
            else
            {
                return await _context.Items.Where(i => i.User.Id != currentUserId && i.Status == ItemStatus.Active).ToListAsync();
            }
        }

        public async Task<List<Item>> GetAllItems()
        {
             return await _context.Items.ToListAsync();
        }

        public async Task<int> GetItemsCount()
        {
            return await _context.Items.CountAsync();
        }

        public Item? GetUserItem(int id)
        {
            var item = _context.Items.FirstOrDefault(i => i.ItemId == id && i.Status != ItemStatus.Deleted);
            return item;
        }

        public List<Item> GetUserItems(string currentUserId = null)
        {
            var items = _context.Items.Where(i => i.Status != ItemStatus.Deleted);

            if (currentUserId != null)
            {
                items = items.Where(i => i.User.Id == currentUserId);
            }

            return items.ToList();
        }


        public void Update(Item item)
        {
            _context.Items.Update(item);
            _context.SaveChanges();
       }

        public async Task UpdateItem(Item item)
        {
            _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}