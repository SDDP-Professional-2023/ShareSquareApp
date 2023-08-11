using Microsoft.AspNetCore.Identity;
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
    public class UserDOA : IUserDOA
    {
        private readonly ShareSquareDbContext _context;
        public UserDOA(ShareSquareDbContext context)
        {
            _context = context;
        }
        public async Task<List<IdentityUser>> Users()
        {
            return await _context.Users.ToListAsync();
        }
    }
}
