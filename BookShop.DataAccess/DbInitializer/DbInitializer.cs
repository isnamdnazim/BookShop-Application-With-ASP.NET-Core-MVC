using BookShop.Models;
using BookShop.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShop.DataAccess.DbInitializer
{
	public class DbInitializer : IDbInitializer
	{

        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _db;

        public DbInitializer(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext db)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _db = db;
        }
		public void Initialize()
		{
			//migrations if they are not applied
			try
			{
				if (_db.Database.GetPendingMigrations().Count() > 0)
				{
					_db.Database.Migrate();
				}
			}
			catch(Exception ex)
			{

			}

			//create roles if they are not created

			if (!_roleManager.RoleExistsAsync(StaticDetails.Role_Admin).GetAwaiter().GetResult())
			{
				_roleManager.CreateAsync(new IdentityRole(StaticDetails.Role_Admin)).GetAwaiter().GetResult();
				_roleManager.CreateAsync(new IdentityRole(StaticDetails.Role_Employee)).GetAwaiter().GetResult();
				_roleManager.CreateAsync(new IdentityRole(StaticDetails.Role_User_Company)).GetAwaiter().GetResult();
				_roleManager.CreateAsync(new IdentityRole(StaticDetails.Role_User_Individual)).GetAwaiter().GetResult();

				//if roles are not created, then er eill create admin user as wll

				_userManager.CreateAsync(new ApplicationUser
				{
					UserName = "admin@bookshop.com",
					Email = "admin@bookshop.com",
					Name = "Book Shop",
					PhoneNumber = "0123456789",
					StreetAddress = "Road#8,Ranavola,Uttara",
					State = "Dhaka",
					PostalCode = "1230",
					City = "Dhaka"
				}, "Admin@123").GetAwaiter().GetResult();

				ApplicationUser user = _db.ApplicationUsers.FirstOrDefault(u => u.Email == "admin@bookshop.com");

				_userManager.AddToRoleAsync(user, StaticDetails.Role_Admin).GetAwaiter().GetResult();
			}
			return;

		}
	}
}
