using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication30.Service.Contract
{
    public interface IGlobalUserService
    {
        Task EnsureGlobalUserExists();
        Task EnsureGlobalUserHasGlobalRoles();
    }
}
