using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication30.Data.Managers;
using WebApplication30.Models;
using WebApplication30.Service.Contract;

namespace WebApplication30.Service.Implementation
{
    public class GlobalUserService : IGlobalUserService
    {
        // Consts
        public const string GmailAccountEmail = "pokemonmaster@interia.pl";
        public const string GmailAccountPassword = "Pokemonki123!";

        // Services
        public AppUserManager UserManager { get; set; }
        public IAppRoleService AppRoleService { get; set; }
        public AppRoleManager RoleManager { get; set; }

        // Properties
        public virtual string GlobalAdminPassword { get; set; } = "aaaaaaaa";
        public virtual AppUser GlobalAdministrator { get; set; } = new AppUser()
        {
            Id = "6d77fdff-4de7-4595-a07f-dc553e04f8c9",
            Email = GmailAccountEmail,
            UserName = GmailAccountEmail,
            EmailConfirmed = true,
        };

        // Constructor
        public GlobalUserService(AppUserManager userManager, IAppRoleService appRoleService, AppRoleManager appRoleManager)
        {
            UserManager = userManager;
            RoleManager = appRoleManager;
            AppRoleService = appRoleService;
        }

        // Methods
        public async Task EnsureGlobalUserExists()
        {
            var user = await UserManager.FindByIdAsync(GlobalAdministrator.Id);
            if (user == null)
            {
                var result = await UserManager.CreateAsync(GlobalAdministrator, GlobalAdminPassword);

                if (!result.Succeeded)
                {
                    throw new Exception("Cannot create Global Administrator account when it not exists!");
                }
            }
        }

        public async Task EnsureGlobalUserHasGlobalRoles()
        {
            foreach (var role in AppRoleService.AppRoles)
            {
                var user = UserManager.Users.FirstOrDefault(u => u.Id == GlobalAdministrator.Id);
                bool isIncurrentRole = await UserManager.IsInRoleAsync(user, role.Name);
                if (!isIncurrentRole)
                {
                    var result = await UserManager.AddToRoleAsync(user, role.Name);

                    if (!result.Succeeded)
                    {
                        throw new Exception($"Cannot assign role [{role.Name}] to Global administrator!");
                    }
                }
            }
        }
    }
}
