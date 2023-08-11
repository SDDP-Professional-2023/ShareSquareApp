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
        Task<IEnumerable<Error>> GetAllErrorsAsync();
        Task<bool> UpdateErrorStatusAsync(int id, string status);
    }
}
