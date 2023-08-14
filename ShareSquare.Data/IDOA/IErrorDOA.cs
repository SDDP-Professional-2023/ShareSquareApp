using ShareSquare.Data.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareSquare.Data.IDOA
{
    public interface IErrorDOA
    {
        Task<Error> AddErrorAsync(Error error);
        Task<Error> GetErrorByIdAsync(int id);
        Task<bool> UpdateErrorStatusAsync(int id, string status);
        Task<List<Error>> GetAllErrorsAsync();
        Task<int> GetErrorsCount();
    }
}
