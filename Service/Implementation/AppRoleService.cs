using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication30.Data.Managers;
using WebApplication30.Models;
using WebApplication30.Repositories;
using WebApplication30.Service.Contract;

namespace WebApplication30.Service.Implementation
{
    public class AppRoleService : IAppRoleService
    {
        // Properties
        public AppRoleManager RoleManager { get; set; }
        public List<AppRole> AppRoles { get; set; }
        public RoleRepository RoleStore { get; set; }

        // Roles Names Constants
        public const string Administrator = "Administrator";
        public const string Moderator = "Moderator";
        public const string User = "User";

        // Roles
        public AppRole AdministratorRole { get; set; }  // Global privigiles
        public AppRole ModeratorRole { get; set; }      // Can edit some data
        public AppRole UserRole { get; set; }    // Normal user with normal rights

        // Constructor
        public AppRoleService(AppRoleManager roleManager)
        {
            RoleManager = roleManager;
            DefineRoles();
        }

        // Methods
        public void DefineRoles()
        {
            AdministratorRole = new AppRole()
            {
                Id = "48254cf1-2750-4fd4-9a8e-15f491ba8262",
                Name = Administrator,
            };

            ModeratorRole = new AppRole()
            {
                Id = "824e7c64-4825-4c3d-b6da-55c0b3f462ce",
                Name = Moderator,
            };

            UserRole = new AppRole()
            {
                Id = "ad31d0fd-c4aa-485c-ab4a-c778ec897b63",
                Name = User,
            };

            AppRoles = new List<AppRole>()
            {
                AdministratorRole,
                ModeratorRole,
                UserRole,
            };
        }

        public async Task EnsureAppRolesExists()
        {
            foreach (var role in AppRoles)
            {
                bool roleExists = await RoleManager.RoleExistsAsync(role.Name);
                if (!roleExists)
                {
                    var result = await RoleManager.CreateAsync(role);
                }
            }
        }
    }
}
