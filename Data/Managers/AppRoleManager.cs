using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication30.Models;

namespace WebApplication30.Data.Managers
{
    public class AppRoleManager : RoleManager<AppRole>
    {
        // Constructor
        public AppRoleManager(IRoleStore<AppRole> store, IEnumerable<IRoleValidator<AppRole>> roleValidators,
            ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, ILogger<RoleManager<AppRole>> logger)
            : base(store, roleValidators, keyNormalizer, errors, logger)
        {
        }

        // Methods
    }
}
