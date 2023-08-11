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
    public class ErrorDOA : IErrorDOA
    {
        private readonly ShareSquareDbContext _context;

        public ErrorDOA(ShareSquareDbContext context)
        {
            _context = context;
        }

        public async Task<Error> AddErrorAsync(Error error)
        {
            await _context.Errors.AddAsync(error);
            await _context.SaveChangesAsync();
            return error;
        }

        public async Task<Error> GetErrorByIdAsync(int id)
        {
            return await _context.Errors.FindAsync(id);
        }

        public async Task<IEnumerable<Error>> GetAllErrorsAsync()
        {
            return await _context.Errors.ToListAsync();
        }

        public async Task<bool> UpdateErrorStatusAsync(int id, string status)
        {
            var error = await GetErrorByIdAsync(id);
            if (error == null) return false;

            error.Status = status;
            _context.Errors.Update(error);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
