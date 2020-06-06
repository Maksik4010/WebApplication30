using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication30.Data;
using WebApplication30.Models;

namespace WebApplication30.Repositories
{
    public class RoleRepository : RoleStore<AppRole, ApplicationDbContext, string, IdentityUserRole<string>, IdentityRoleClaim<string>>
    {
        // Properties

        // Constructor
        public RoleRepository(ApplicationDbContext dbContext)
            : base(dbContext)
        {
        }

        // Methods
    }
}
