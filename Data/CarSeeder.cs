using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace CarInfoFromDatabase.Data
{
    public class CarSeeder
    {
        private readonly CarContext _ctx;
        private readonly IWebHostEnvironment _env;
        private readonly UserManager<StoreUsers> _userManager;

        public CarSeeder(CarContext ctx, IWebHostEnvironment env, UserManager<StoreUsers> userManager)
        {
            this._ctx = ctx;
            this._env = env;
            this._userManager = userManager;
        }


        public async Task  SeedAsync()
        {
            _ctx.Database.EnsureCreated();

            StoreUsers user = await _userManager.FindByEmailAsync("mait@gmail.com");

            if (user == null)
            {
                user = new StoreUsers()
                {
                    FirstName = "reda",
                    LastName = "Hamm",
                    Email = "mait@gmail.com",
                    UserName = "mait@gmail.com"
                };
                var result = await _userManager.CreateAsync(user, "P@ssw0rd!");
                //var result = await _userManager.AddPasswordAsync(user, "Password");


                if (result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Could not find user store");
                }
            }

            if (!_ctx.root.Any())
            {
                //need to create sample data
                var filePath = Path.Combine(_env.ContentRootPath, "Data/car.json");
                var json = File.ReadAllText(filePath);
                var showCar = JsonSerializer.Deserialize<IEnumerable<Root>>(json);
                _ctx.root.AddRange(showCar);
                _ctx.SaveChanges();
            }
        }
    }
}
