using Microsoft.AspNetCore.Identity;
using ShareSquare.Data.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShareSquare.Data.IDOA
{
    public interface IUserDOA
    {
        Task<List<IdentityUser>> Users();
        Task<int> UsersCount();
    }
}
