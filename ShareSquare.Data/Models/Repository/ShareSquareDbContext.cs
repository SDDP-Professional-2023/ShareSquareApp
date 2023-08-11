using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ShareSquare.Data.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareSquare.Data.Models.Repository
{
    public class ShareSquareDbContext : IdentityDbContext
    {
        public ShareSquareDbContext(DbContextOptions<ShareSquareDbContext> options)
            : base(options)
        {


        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Error> Errors { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<FavoriteItem> FavoriteItems { get; set; }
        public DbSet<Message> Messages { get; set; }

    }
}
