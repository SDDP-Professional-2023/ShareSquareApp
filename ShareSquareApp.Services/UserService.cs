using Microsoft.AspNetCore.Identity;
using ShareSquare.Data.IDOA;
using ShareSquareApp.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareSquareApp.Services
{
    public class UserService : IUserService
    {
        private readonly IUserDOA _userDOA;
        public UserService(IUserDOA userDOA)
        {
            _userDOA = userDOA;
        }
        public async Task<List<IdentityUser>> GetUsers()
        {
            return await _userDOA.Users();
        }
    }
}
