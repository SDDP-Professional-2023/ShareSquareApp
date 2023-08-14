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
    public class ErrorService : IErrorService
    {
        private readonly IErrorDOA _errorDOA;

        public ErrorService(IErrorDOA errorDOA)
        {
            _errorDOA = errorDOA;
        }

        public async Task<Error> CreateErrorAsync(Error error)
        {
            return await _errorDOA.AddErrorAsync(error);
        }

        public async Task<Error> GetErrorByIdAsync(int id)
        {
            return await _errorDOA.GetErrorByIdAsync(id);
        }

        public async Task<List<Error>> GetAllErrorsAsync()
        {
            return await _errorDOA.GetAllErrorsAsync();
        }

        public async Task<int> GetErrorsCount()
        {
            return await _errorDOA.GetErrorsCount();
        }

        public async Task<bool> SetErrorStatusAsync(int id, string status)
        {
            return await _errorDOA.UpdateErrorStatusAsync(id, status);
        }

        public async Task LogErrorAsync(Exception ex)
        {
            var error = new Error
            {
                Text = ex.Message,
                Type = ex.GetType().Name,
                Timestamp = DateTime.UtcNow,
                Status = "Unresolved",
                Source = ex.Source,
                StackTrace = ex.StackTrace,
                Data = ex.Data.ToString() 
            };

            await CreateErrorAsync(error);
        }
    }
}
