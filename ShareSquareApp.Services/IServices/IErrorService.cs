using ShareSquare.Data.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareSquareApp.Services.IServices
{
    public interface IErrorService
    {
        Task<Error> CreateErrorAsync(Error error);
        Task<Error> GetErrorByIdAsync(int id);
        Task<IEnumerable<Error>> GetAllErrorsAsync();
        Task<bool> SetErrorStatusAsync(int id, string status);
        Task LogErrorAsync(Exception ex);
    }
}
