using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication30.Models;

namespace WebApplication30.Service.Contract
{
    public interface IAppRoleService
    {
        List<AppRole> AppRoles { get; }

        void DefineRoles();
        Task EnsureAppRolesExists();
    }
}
