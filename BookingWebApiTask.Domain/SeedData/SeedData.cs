using BookingWebApiTask.Domain.Data;
using BookingWebApiTask.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingWebApiTask.Domain.SeedData
{
    public static class SeedData
    {
        public static async Task Seed(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            await context.Database.MigrateAsync();

            await SeedRolesAsync(roleManager);

            await SeedUsersAsync(userManager);

            await SeedTripsAsync(context);

        }

        public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            string[] roles = { "Admin", "User" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
        }


        public static async Task SeedUsersAsync(UserManager<ApplicationUser> userManager)
        {
            var users = new[]
            {
                new ApplicationUser
                {
                    Name = "Ahmed Gamal",
                    Email = "ahmedgamal52001@gmail.com",
                    UserName = "ahmedgamal52001@gmail.com",
                    EmailConfirmed = true,
                    PasswordHash = "12345",
                    PhoneNumber = "01147893607"
                },
                new ApplicationUser
                {
                    Name = "Ahmed Yassen",
                    Email = "ahmedyassen@gmail.com",
                    UserName = "ahmedyassen@gmail.com",
                    EmailConfirmed = true,
                    PasswordHash = "12345",
                    PhoneNumber = "01147893607"
                },
                new ApplicationUser
                {
                    Name = "Ahmed Ali",
                    Email = "ahmedali@gmail.com",
                    UserName = "ahmedali@gmail.com",
                    EmailConfirmed = true,
                    PasswordHash = "12345",
                    PhoneNumber = "01147893607"
                }
            };

            if (userManager.Users.Count() < users.Count())
            {
                foreach (var user in users)
                {
                    var existingUser = await userManager.FindByEmailAsync(user.Email!);
                    if (existingUser == null)
                    {
                        IdentityResult result = await userManager.CreateAsync(user, user.PasswordHash!);
                        if (result.Succeeded)
                        {
                            await userManager.AddToRoleAsync(user, "User");
                        }
                    }
                }
            }
        }

        public static async Task SeedTripsAsync(ApplicationDbContext dbContext)
        {
            if (!dbContext.Trips.Any())
            {
                var trips = new[]
                {
                    new Trip { 
                        Name = "Trip 1",
                        CityName = "Cairo",
                        ImageUrl = "images/trip_1.jpg",
                        Price = 1000,
                        Content = "Cairo is the capital of Egypt",
                        Creation_date = new DateTime(2024, 03, 24) 
                    },
                    new Trip { 
                        Name = "Trip 2",
                        CityName = "Alexandria",
                        ImageUrl = "images/trip_2.jpg",
                        Price = 2000,
                        Content = "Alexandria is the second capital of Egypt",
                        Creation_date = new DateTime(2024, 03, 24)
                    },
                    new Trip { 
                        Name = "Trip 3",
                        CityName = "Aswan", 
                        ImageUrl = "images/trip_3.jpg",                    
                        Price = 3000, 
                        Content = "Aswan is the third capital of Egypt", 
                        Creation_date = new DateTime(2024, 03, 24) 
                    }
                };

                await dbContext.Trips.AddRangeAsync(trips);
                await dbContext.SaveChangesAsync();
            }
        }

    }
}

