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
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUserDOA _userDOA;
        public UserService(IUserDOA userDOA, UserManager<IdentityUser> userManager)
        {
            _userDOA = userDOA;
            _userManager = userManager;
        }
        public async Task<List<IdentityUser>> GetUsers()
        {
            var allUsers = await _userDOA.Users();
            var nonAdminUsers = new List<IdentityUser>();

            foreach (var user in allUsers)
            {
                if (!await _userManager.IsInRoleAsync(user, "Admin"))
                {
                    nonAdminUsers.Add(user);
                }
            }
            return nonAdminUsers;
        }

        public async Task<int> GetUsersCount()
        {
            return await _userDOA.UsersCount();
        }
    }
}
