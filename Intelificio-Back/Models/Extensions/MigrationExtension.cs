using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Backend.Models.Extensions
{
    public static class MigrationExtension
    {
        public static async Task<IHost> UseMigrations(this IHost app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<IntelificioDbContext>();
                var passwordHasher = new PasswordHasher<User>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();

                if (!await context.Roles.AnyAsync())
                {
                    _ = await roleManager.CreateAsync(new Role
                    {
                        Name = "Administrador",
                    });
                    _ = await roleManager.CreateAsync(new Role
                    {
                        Name = "Usuario",
                    });
                    _ = await context.SaveChangesAsync();

                }
                if (!await context.Users.AnyAsync())
                {
                    var admin = new User
                    {
                        Email = "admin@outlook.com",
                        UserName = "admin@outlook.com",
                        FirstName = "Admin",
                        LastName = "Admin",
                        Password = "Admin.123",
                        PhoneNumber = "1234567890",
                        Role = (await roleManager.FindByNameAsync("Administrador"))!,
                        Rut = "123456789"
                    };
                    var user = new User
                    {
                        Email = "user@outlook.com",
                        UserName = "user@outlook.com",
                        FirstName = "User",
                        LastName = "User",
                        Password = "User.123",
                        PhoneNumber = "1234567890",
                        Role = (await roleManager.FindByNameAsync("Usuario"))!,
                        Rut = "123456789"
                    };
                    _ = await userManager.CreateAsync(user, user.Password);
                    _ = await userManager.CreateAsync(admin, admin.Password);
                    _ = await context.SaveChangesAsync();

                }

                if (!await context.Regions.AnyAsync())
                {
                    var region = new Region
                    {
                        Name = "Metropolitana",
                    };

                    _ = context.Regions.AddAsync(region);
                    _ = await context.SaveChangesAsync();

                }

                if (!await context.City.AnyAsync())
                {
                    var region = (await context.Regions.FirstOrDefaultAsync(x => x.Name == "Metropolitana"))!;
                    var city = new City
                    {
                        Name = "Santiago",
                        Region = region
                    };

                    _ = context.City.AddAsync(city);
                    _ = await context.SaveChangesAsync();

                }

                if (!await context.Municipality.AnyAsync())
                {
                    var city = (await context.City.FirstOrDefaultAsync(x => x.Name == "Santiago"))!;
                    var municipality = new Municipality
                    {
                        Name = "La Florida",
                        City = city
                    };

                    _ = context.Municipality.AddAsync(municipality);
                    _ = await context.SaveChangesAsync();

                }

                if (!await context.Community.AnyAsync())
                {
                    var municipality = (await context.Municipality.FirstOrDefaultAsync(x => x.Name == "La Florida"))!;
                    var community = new List<Community>
                    {
                        new Community
                        {
                            Address = "Calle 123",
                            Municipality = municipality,
                            Name = "Comunidad 1"
                        },
                        new Community
                        {
                            Address = "Calle 123",
                            Municipality = municipality,
                            Name = "Comunidad 2"
                        }
                };

                    context.Community.AddRange(community);
                    _ = await context.SaveChangesAsync();

                }

                if (!await context.Buildings.AnyAsync())
                {
                    var community1 = (await context.Community.FirstOrDefaultAsync(x => x.Name == "Comunidad 1"))!;
                    var community2 = (await context.Community.FirstOrDefaultAsync(x => x.Name == "Comunidad 2"))!;
                    var buildings = new List<Building>
                    {
                        new Building
                        {
                            Community = community1,
                            Name = "Torre 1",
                        },
                        new Building
                        {
                            Community = community1,
                            Name = "Torre 2",
                        },
                        new Building
                        {
                            Community = community2,
                            Name = "Torre 1",
                        },
                        new Building
                        {
                            Community = community2,
                            Name = "Torre 2",
                        },
                    };

                    await context.Buildings.AddRangeAsync(buildings);
                    _ = await context.SaveChangesAsync();

                }

                if (!await context.UnitTypes.AnyAsync())
                {
                    var unitTypes = new List<UnitType>
                    {
                        new UnitType
                        {
                            Description = "Unidad",
                        },
                        new UnitType
                        {
                            Description = "Estacionamiento",
                        },
                        new UnitType
                        {
                            Description = "Bodega",
                        },
                    };

                    context.UnitTypes.AddRange(unitTypes);
                    _ = await context.SaveChangesAsync();

                }

                if (!await context.Units.AnyAsync())
                {
                    var towers1 = context.Buildings.Where(x => x.Community.Name == "Comunidad 1").ToList();
                    var towers2 = context.Buildings.Where(x => x.Community.Name == "Comunidad 2").ToList();
                    var unitTypes = await context.UnitTypes.ToListAsync();

                    var unidades = new List<Unit>
                    {
                        new Unit
                        {
                            Building = towers1.FirstOrDefault(x => x.Name == "Torre 1")!,
                            Type = unitTypes.Where(x => x.Description == "Unidad").FirstOrDefault()!,
                            Number = "101"
                        },
                        new Unit
                        {
                            Building = towers1.FirstOrDefault(x => x.Name == "Torre 1")!,
                            Type = unitTypes.Where(x => x.Description == "Unidad").FirstOrDefault()!,
                            Number = "102"
                        },
                        new Unit
                        {
                            Building = towers1.FirstOrDefault(x => x.Name == "Torre 1")!,
                            Type = unitTypes.Where(x => x.Description == "Unidad").FirstOrDefault()!,
                            Number = "103"
                        },
                        new Unit
                        {
                            Building = towers1.FirstOrDefault(x => x.Name == "Torre 1")!,
                            Type = unitTypes.Where(x => x.Description == "Estacionamiento").FirstOrDefault()!,
                            Number = "01"
                        },

                        new Unit
                        {
                            Building = towers1.FirstOrDefault(x => x.Name == "Torre 2")!,
                            Type = (await context.UnitTypes.FirstOrDefaultAsync(x => x.Description == "Unidad"))!,
                            Number = "101"
                        },
                        new Unit
                        {
                            Building = towers1.FirstOrDefault(x => x.Name == "Torre 2")!,
                            Type = (await context.UnitTypes.FirstOrDefaultAsync(x => x.Description == "Unidad"))!,
                            Number = "102"
                        },
                        new Unit
                        {
                            Building = towers1.FirstOrDefault(x => x.Name == "Torre 2")!,
                            Type = unitTypes.Where(x => x.Description == "Unidad").FirstOrDefault()!,
                            Number = "103"
                        },
                        new Unit
                        {
                            Building = towers1.FirstOrDefault(x => x.Name == "Torre 2")!,
                            Type = unitTypes.Where(x => x.Description == "Estacionamiento").FirstOrDefault()!,
                            Number = "01"
                        },





                        new Unit
                        {
                            Building = towers2.FirstOrDefault(x => x.Name == "Torre 1")!,
                            Type = unitTypes.Where(x => x.Description == "Unidad").FirstOrDefault()!,
                            Number = "101"
                        },
                        new Unit
                        {
                            Building = towers2.FirstOrDefault(x => x.Name == "Torre 1")!,
                            Type = unitTypes.Where(x => x.Description == "Unidad").FirstOrDefault()!,
                            Number = "102"
                        },
                        new Unit
                        {
                            Building = towers2.FirstOrDefault(x => x.Name == "Torre 1")!,
                            Type = unitTypes.Where(x => x.Description == "Unidad").FirstOrDefault()!,
                            Number = "103"
                        },
                        new Unit
                        {
                            Building = towers2.FirstOrDefault(x => x.Name == "Torre 1")!,
                            Type = unitTypes.Where(x => x.Description == "Estacionamiento").FirstOrDefault() !,
                            Number = "01"
                        },

                        new Unit
                        {
                            Building = towers2.FirstOrDefault(x => x.Name == "Torre 2")!,
                            Type = unitTypes.Where(x => x.Description == "Unidad").FirstOrDefault()!,
                            Number = "101"
                        },
                        new Unit
                        {
                            Building = towers2.FirstOrDefault(x => x.Name == "Torre 2")!,
                            Type = unitTypes.Where(x => x.Description == "Unidad").FirstOrDefault()!,
                            Number = "102"
                        },
                        new Unit
                        {
                            Building = towers2.FirstOrDefault(x => x.Name == "Torre 2")!,
                            Type = unitTypes.Where(x => x.Description == "Unidad").FirstOrDefault()!,
                            Number = "103"
                        },
                        new Unit
                        {
                            Building = towers2.FirstOrDefault(x => x.Name == "Torre 2")!,
                            Type = unitTypes.Where(x => x.Description == "Estacionamiento").FirstOrDefault() !,
                            Number = "01"
                        }
                    };

                    context.Units.AddRange(unidades);
                    _ = await context.SaveChangesAsync();
                }
            }
            return app;
        }
    }
}
