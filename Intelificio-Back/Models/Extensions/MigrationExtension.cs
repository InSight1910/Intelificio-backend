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
                        Name = "Propietario",
                    });
                    _ = await roleManager.CreateAsync(new Role
                    {
                        Name = "Residente",
                    });
                    _ = await roleManager.CreateAsync(new Role
                    {
                        Name = "Arrendatario",
                    });
                    _ = await roleManager.CreateAsync(new Role
                    {
                        Name = "Conserje",
                    });
                    _ = await context.SaveChangesAsync();

                }
                if (!await context.Users.AnyAsync())
                {
                    var adminRole = await roleManager.FindByNameAsync("Administrador");
                    var userRole = await roleManager.FindByNameAsync("Propietario");

                    var admin = new User
                    {
                        Email = "admin@outlook.com",
                        UserName = "admin@outlook.com",
                        FirstName = "Admin",
                        LastName = "Admin",
                        PhoneNumber = "1234567890",
                        Rut = "123456789",
                        //Role = adminRole
                    };
                    var user = new User
                    {
                        Email = "user@outlook.com",
                        UserName = "user@outlook.com",
                        FirstName = "User",
                        LastName = "User",
                        PhoneNumber = "1234567890",
                        Rut = "123456789",
                        //Role = userRole
                    };
                    _ = await userManager.CreateAsync(user, "User.1234");
                    _ = await userManager.CreateAsync(admin, "Admin.123");
                    _ = await userManager.AddToRoleAsync(admin, "Administrador");
                    _ = await userManager.AddToRoleAsync(user, "Propietario");
                    _ = await context.SaveChangesAsync();

                }

                await SeedLocation(context);

                if (!await context.Community.AnyAsync())
                {
                    var municipality = (await context.Municipality.FirstOrDefaultAsync(x => x.Name == "La Florida"))!;
                    var community = new List<Community>
                    {
                        new Community
                        {
                            Address = "Calle 123",
                            Municipality = municipality,
                            Name = "Comunidad 1",
                            Rut = "123123"
                        },
                        new Community
                        {
                            Address = "Calle 123",
                            Municipality = municipality,
                            Name = "Comunidad 2",
                            Rut = "123123"
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
                            Floors = 10
                        },
                        new Building
                        {
                            Community = community1,
                            Name = "Torre 2",
                            Floors = 10
                        },
                        new Building
                        {
                            Community = community2,
                            Name = "Torre 1",
                            Floors = 10
                        },
                        new Building
                        {
                            Community = community2,
                            Name = "Torre 2",
                            Floors = 10
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
                            Description = "Departamento",
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
                            BuildingId = towers1.FirstOrDefault(x => x.Name == "Torre 1")!.ID,
                            UnitTypeId = unitTypes.Where(x => x.Description == "Departamento").FirstOrDefault()!.ID,
                            Number = "101",
                            Floor = 1,
                            Surface = 50.1F
                        },
                        new Unit
                        {
                            BuildingId = towers1.FirstOrDefault(x => x.Name == "Torre 1")!.ID,
                            UnitTypeId = unitTypes.Where(x => x.Description == "Departamento").FirstOrDefault().ID!,
                            Number = "102",
                            Floor = 1,
                            Surface = 50.1F
                        },
                        new Unit
                        {
                            BuildingId = towers1.FirstOrDefault(x => x.Name == "Torre 1")!.ID,
                            UnitTypeId = unitTypes.Where(x => x.Description == "Departamento").FirstOrDefault()!.ID,
                            Number = "103",
                            Floor = 1,
                            Surface = 50.1F
                        },
                        new Unit
                        {
                            BuildingId = towers1.FirstOrDefault(x => x.Name == "Torre 1")!.ID,
                            UnitTypeId = unitTypes.Where(x => x.Description == "Estacionamiento").FirstOrDefault()!.ID,
                            Number = "01",
                            Floor = 1,
                            Surface = 50.1F
                        },

                        new Unit
                        {
                            BuildingId = towers1.FirstOrDefault(x => x.Name == "Torre 2")!.ID,
                            UnitTypeId = (await context.UnitTypes.FirstOrDefaultAsync(x => x.Description == "Departamento"))!.ID,
                            Number = "101",
                            Floor = 1,
                            Surface = 50.1F
                        },
                        new Unit
                        {
                            BuildingId = towers1.FirstOrDefault(x => x.Name == "Torre 2")!.ID,
                            UnitTypeId = (await context.UnitTypes.FirstOrDefaultAsync(x => x.Description == "Departamento"))!.ID,
                            Number = "102",
                            Floor = 1,
                            Surface = 50.1F
                        },
                        new Unit
                        {
                            BuildingId = towers1.FirstOrDefault(x => x.Name == "Torre 2")!.ID,
                            UnitTypeId = unitTypes.Where(x => x.Description == "Departamento").FirstOrDefault()!.ID,
                            Number = "103",
                            Floor = 1,
                            Surface = 50.1F
                        },
                        new Unit
                        {
                            BuildingId = towers1.FirstOrDefault(x => x.Name == "Torre 2") !.ID,
                            UnitTypeId = unitTypes.Where(x => x.Description == "Estacionamiento").FirstOrDefault()!.ID,
                            Number = "01",
                            Floor = 1,
                            Surface = 50.1F
                        },





                        new Unit
                        {
                            BuildingId = towers2.FirstOrDefault(x => x.Name == "Torre 1") !.ID,
                            UnitTypeId = unitTypes.Where(x => x.Description == "Departamento").FirstOrDefault() !.ID,
                            Number = "101",
                            Floor = 1,
                            Surface = 50.1F
                        },
                        new Unit
                        {
                            BuildingId = towers2.FirstOrDefault(x => x.Name == "Torre 1") !.ID,
                            UnitTypeId = unitTypes.Where(x => x.Description == "Departamento").FirstOrDefault() !.ID,
                            Number = "102",
                            Floor = 1,
                            Surface = 50.1F
                        },
                        new Unit
                        {
                            BuildingId = towers2.FirstOrDefault(x => x.Name == "Torre 1") !.ID,
                            UnitTypeId = unitTypes.Where(x => x.Description == "Departamento").FirstOrDefault() !.ID,
                            Number = "103",
                            Floor = 1,
                            Surface = 50.1F
                        },
                        new Unit
                        {
                            BuildingId = towers2.FirstOrDefault(x => x.Name == "Torre 1") !.ID,
                            UnitTypeId = unitTypes.Where(x => x.Description == "Estacionamiento").FirstOrDefault() !.ID,
                            Number = "01",
                            Floor = 1,
                            Surface = 50.1F
                        },

                        new Unit
                        {
                            BuildingId = towers2.FirstOrDefault(x => x.Name == "Torre 2") !.ID,
                            UnitTypeId = unitTypes.Where(x => x.Description == "Departamento").FirstOrDefault() !.ID,
                            Number = "101",
                            Floor = 1,
                            Surface = 50.1F
                        },
                        new Unit
                        {
                            BuildingId = towers2.FirstOrDefault(x => x.Name == "Torre 2") !.ID,
                            UnitTypeId = unitTypes.Where(x => x.Description == "Departamento").FirstOrDefault() !.ID,
                            Number = "102",
                            Floor = 1,
                            Surface = 50.1F
                        },
                        new Unit
                        {
                            BuildingId = towers2.FirstOrDefault(x => x.Name == "Torre 2") !.ID,
                            UnitTypeId = unitTypes.Where(x => x.Description == "Departamento").FirstOrDefault() !.ID,
                            Number = "103",
                            Floor = 1,
                            Surface = 50.1F
                        },
                        new Unit
                        {
                            BuildingId = towers2.FirstOrDefault(x => x.Name == "Torre 2") !.ID,
                            UnitTypeId = unitTypes.Where(x => x.Description == "Estacionamiento").FirstOrDefault() !.ID,
                            Number = "01",
                            Floor = 1,
                            Surface = 50.1F
                        }
                    };

                    context.Units.AddRange(unidades);
                    _ = await context.SaveChangesAsync();
                }
            }
            return app;
        }

        private static async Task SeedLocation(IntelificioDbContext _context)
        {
            #region Regiones
            var arica = new Region { Name = "Arica y Parinacota" };
            var tarapaca = new Region { Name = "Tarapacá" };
            var antofagasta = new Region { Name = "Antofagasta" };
            var atacama = new Region { Name = "Atacama" };
            var coquimbo = new Region { Name = "Coquimbo" };
            var valparaiso = new Region { Name = "Valparaíso" };
            var metropolitana = new Region { Name = "Metropolitana de Santiago" };
            var ohiggins = new Region { Name = "O'Higgins" };
            var maule = new Region { Name = "Maule" };
            var nuble = new Region { Name = "Ñuble" };
            var biobio = new Region { Name = "Biobío" };
            var araucania = new Region { Name = "La Araucanía" };
            var losRios = new Region { Name = "Los Ríos" };
            var losLagos = new Region { Name = "Los Lagos" };
            var aysen = new Region { Name = "Aysén" };
            var magallanes = new Region { Name = "Magallanes y de la Antártica Chilena" };
            #endregion
            if (!await _context.Regions.AnyAsync())
            {
                await _context.Regions.AddRangeAsync(new List<Region>
                {
                    arica,
                    tarapaca,
                    antofagasta,
                    atacama,
                    coquimbo,
                    valparaiso,
                    metropolitana,
                    ohiggins,
                    maule,
                    nuble,
                    biobio,
                    araucania,
                    losRios,
                    losLagos,
                    aysen,
                    magallanes
                });

                _ = await _context.SaveChangesAsync();
            }

            #region Ciudades
            var aricaCity = new City { Name = "Arica", Region = arica };
            var parinacota = new City { Name = "Parinacota", Region = arica };
            var iquique = new City { Name = "Iquique", Region = tarapaca };
            var tamarugal = new City { Name = "Tamarugal", Region = tarapaca };
            var antofagastaCity = new City { Name = "Antofagasta", Region = antofagasta };
            var elLoa = new City { Name = "El Loa", Region = antofagasta };
            var tocopilla = new City { Name = "Tocopilla", Region = antofagasta };
            var chañaral = new City { Name = "Chañaral", Region = atacama };
            var copiapo = new City { Name = "Copiapó", Region = atacama };
            var huasco = new City { Name = "Huasco", Region = atacama };
            var elqui = new City { Name = "Elqui", Region = coquimbo };
            var limari = new City { Name = "Limarí", Region = coquimbo };
            var choapa = new City { Name = "Choapa", Region = coquimbo };
            var valparaisoCity = new City { Name = "Valparaíso", Region = valparaiso };
            var sanAntonio = new City { Name = "San Antonio", Region = valparaiso };
            var sanFelipe = new City { Name = "San Felipe", Region = valparaiso };
            var losAndes = new City { Name = "Los Andes", Region = valparaiso };
            var petorca = new City { Name = "Petorca", Region = valparaiso };
            var quillota = new City { Name = "Quillota", Region = valparaiso };
            var margaMarga = new City { Name = "Marga Marga", Region = valparaiso };
            var metropolitanaCity = new City { Name = "Santiago", Region = metropolitana };
            var cordillera = new City { Name = "Cordillera", Region = metropolitana };
            var maipo = new City { Name = "Maipo", Region = metropolitana };
            var melipilla = new City { Name = "Melipilla", Region = metropolitana };
            var talagante = new City { Name = "Talagante", Region = metropolitana };
            var chacabuco = new City { Name = "Chacabuco", Region = metropolitana };
            var cachapoal = new City { Name = "Cachapoal", Region = ohiggins };
            var colchagua = new City { Name = "Colchagua", Region = ohiggins };
            var cardenalCaro = new City { Name = "Cardenal Caro", Region = ohiggins };
            var curico = new City { Name = "Curicó", Region = maule };
            var talca = new City { Name = "Talca", Region = maule };
            var linares = new City { Name = "Linares", Region = maule };
            var cauquenes = new City { Name = "Cauquenes", Region = maule };
            var diguillin = new City { Name = "Diguillín", Region = nuble };
            var itata = new City { Name = "Itata", Region = nuble };
            var punilla = new City { Name = "Punilla", Region = nuble };
            var biobioCity = new City { Name = "Biobío", Region = biobio };
            var concepcion = new City { Name = "Concepción", Region = biobio };
            var arauco = new City { Name = "Arauco", Region = biobio };
            var malleco = new City { Name = "Malleco", Region = araucania };
            var cautin = new City { Name = "Cautín", Region = araucania };
            var valdivia = new City { Name = "Valdivia", Region = losRios };
            var ranco = new City { Name = "Ranco", Region = losRios };
            var osorno = new City { Name = "Osorno", Region = losLagos };
            var llanquihue = new City { Name = "Llanquihue", Region = losLagos };
            var chiloe = new City { Name = "Chiloé", Region = losLagos };
            var palena = new City { Name = "Palena", Region = losLagos };
            var aysenCity = new City { Name = "Aysén", Region = aysen };
            var capitanPrat = new City { Name = "Capitán Prat", Region = aysen };
            var generalCarrera = new City { Name = "General Carrera", Region = aysen };
            var coyhaique = new City { Name = "Coyhaique", Region = aysen };
            var magallanesCity = new City { Name = "Magallanes", Region = magallanes };
            var ultimaEsperanza = new City { Name = "Última Esperanza", Region = magallanes };
            var tierraDelFuego = new City { Name = "Tierra del Fuego", Region = magallanes };
            var antartica = new City { Name = "Antártica", Region = magallanes };
            #endregion 
            if (!await _context.City.AnyAsync())
            {
                await _context.City.AddRangeAsync(new List<City>
                {
                    aricaCity,
                    parinacota,
                    iquique,
                    tamarugal,
                    antofagastaCity,
                    elLoa,
                    tocopilla,
                    chañaral,
                    copiapo,
                    huasco,
                    elqui,
                    limari,
                    choapa,
                    valparaisoCity,
                    sanAntonio,
                    sanFelipe,
                    losAndes,
                    petorca,
                    quillota,
                    margaMarga,
                    metropolitanaCity,
                    cordillera,
                    maipo,
                    melipilla,
                    talagante,
                    chacabuco,
                    cachapoal,
                    colchagua,
                    cardenalCaro,
                    curico,
                    talca,
                    linares,
                    cauquenes,
                    diguillin,
                    itata,
                    punilla,
                    biobioCity,
                    concepcion,
                    arauco,
                    malleco,
                    cautin,
                    valdivia,
                    ranco,
                    osorno,
                    llanquihue,
                    chiloe,
                    palena,
                    aysenCity,
                    capitanPrat,
                    generalCarrera,
                    coyhaique,
                    magallanesCity,
                    ultimaEsperanza,
                    tierraDelFuego,
                    antartica
                });

                _ = await _context.SaveChangesAsync();
            }

            #region Comunas

            var aricaComuna = new Municipality { Name = "Arica", City = aricaCity };
            var camarones = new Municipality { Name = "Camarones", City = aricaCity };

            var generalLagos = new Municipality { Name = "General Lagos", City = parinacota };
            var putre = new Municipality { Name = "Putre", City = parinacota };

            var iquiqueComuna = new Municipality { Name = "Iquique", City = iquique };
            var altoHospicio = new Municipality { Name = "Alto Hospicio", City = iquique };

            var camina = new Municipality { Name = "Camiña", City = tamarugal };
            var colchane = new Municipality { Name = "Colchane", City = tamarugal };
            var huara = new Municipality { Name = "Huara", City = tamarugal };
            var pica = new Municipality { Name = "Pica", City = tamarugal };
            var pozoAlmonte = new Municipality { Name = "Pozo Almonte", City = tamarugal };

            var antofagastaComuna = new Municipality { Name = "Antofagasta", City = antofagastaCity };
            var mejillones = new Municipality { Name = "Mejillones", City = antofagastaCity };
            var sierraGorda = new Municipality { Name = "Sierra Gorda", City = antofagastaCity };
            var taltal = new Municipality { Name = "Taltal", City = antofagastaCity };

            var tocopillaComuna = new Municipality { Name = "Tocopilla", City = tocopilla };
            var mariaElena = new Municipality { Name = "María Elena", City = tocopilla };

            var calama = new Municipality { Name = "Calama", City = elLoa };
            var ollague = new Municipality { Name = "Ollagüe", City = elLoa };
            var sanPedroDeAtacama = new Municipality { Name = "San Pedro de Atacama", City = elLoa };

            var chañaralComuna = new Municipality { Name = "Chañaral", City = chañaral };
            var diegoDeAlmagro = new Municipality { Name = "Diego de Almagro", City = chañaral };

            var caldera = new Municipality { Name = "Caldera", City = copiapo };
            var tierraAmarilla = new Municipality { Name = "Tierra Amarilla", City = copiapo };
            var copiapoComuna = new Municipality { Name = "Copiapó", City = copiapo };

            var altoDelCarmen = new Municipality { Name = "Alto del Carmen", City = huasco };
            var freirina = new Municipality { Name = "Freirina", City = huasco };
            var huascoComuna = new Municipality { Name = "Huasco", City = huasco };
            var vallenar = new Municipality { Name = "Vallenar", City = huasco };

            var andacollo = new Municipality { Name = "Andacollo", City = elqui };
            var coquimboComuna = new Municipality { Name = "Coquimbo", City = elqui };
            var laHiguera = new Municipality { Name = "La Higuera", City = elqui };
            var laSerena = new Municipality { Name = "La Serena", City = elqui };
            var paihuano = new Municipality { Name = "Paihuano", City = elqui };
            var vicuna = new Municipality { Name = "Vicuña", City = elqui };

            var canela = new Municipality { Name = "Canela", City = choapa };
            var illapel = new Municipality { Name = "Illapel", City = choapa };
            var losVilos = new Municipality { Name = "Los Vilos", City = choapa };
            var salamanca = new Municipality { Name = "Salamanca", City = choapa };

            var combarbala = new Municipality { Name = "Combarbalá", City = limari };
            var montePatria = new Municipality { Name = "Monte Patria", City = limari };
            var ovalle = new Municipality { Name = "Ovalle", City = limari };
            var punitaqui = new Municipality { Name = "Punitaqui", City = limari };
            var rioHurtado = new Municipality { Name = "Río Hurtado", City = limari };

            var casablanca = new Municipality { Name = "Casablanca", City = valparaisoCity };
            var concon = new Municipality { Name = "Concón", City = valparaisoCity };
            var juanFernandez = new Municipality { Name = "Juan Fernández", City = valparaisoCity };
            var puchuncavi = new Municipality { Name = "Puchuncaví", City = valparaisoCity };
            var quintero = new Municipality { Name = "Quintero", City = valparaisoCity };
            var valparaisoComuna = new Municipality { Name = "Valparaíso", City = valparaisoCity };
            var vinaDelMar = new Municipality { Name = "Viña del Mar", City = valparaisoCity };

            var algarrobo = new Municipality { Name = "Algarrobo", City = sanAntonio };
            var cartagena = new Municipality { Name = "Cartagena", City = sanAntonio };
            var elQuisco = new Municipality { Name = "El Quisco", City = sanAntonio };
            var elTabo = new Municipality { Name = "El Tabo", City = sanAntonio };
            var sanAntonioComuna = new Municipality { Name = "San Antonio", City = sanAntonio };
            var santoDomingo = new Municipality { Name = "Santo Domingo", City = sanAntonio };

            var catemu = new Municipality { Name = "Catemu", City = sanFelipe };
            var llailay = new Municipality { Name = "Llaillay", City = sanFelipe };
            var panquehue = new Municipality { Name = "Panquehue", City = sanFelipe };
            var putaendo = new Municipality { Name = "Putaendo", City = sanFelipe };
            var sanFelipeComuna = new Municipality { Name = "San Felipe", City = sanFelipe };
            var santaMaria = new Municipality { Name = "Santa María", City = sanFelipe };

            var calleLarga = new Municipality { Name = "Calle Larga", City = losAndes };
            var losAndesComuna = new Municipality { Name = "Los Andes", City = losAndes };
            var rinconada = new Municipality { Name = "Rinconada", City = losAndes };
            var sanEsteban = new Municipality { Name = "San Esteban", City = losAndes };

            var cabildo = new Municipality { Name = "Cabildo", City = petorca };
            var laLigua = new Municipality { Name = "La Ligua", City = petorca };
            var papudo = new Municipality { Name = "Papudo", City = petorca };
            var petorcaComuna = new Municipality { Name = "Petorca", City = petorca };
            var zapallar = new Municipality { Name = "Zapallar", City = petorca };

            var hijuelas = new Municipality { Name = "Hijuelas", City = quillota };
            var laCruz = new Municipality { Name = "La Cruz", City = quillota };
            var nogales = new Municipality { Name = "Nogales", City = quillota };
            var quillotaComuna = new Municipality { Name = "Quillota", City = quillota };
            var calera = new Municipality { Name = "La Calera", City = quillota };

            var olmue = new Municipality { Name = "Olmué", City = margaMarga };
            var limache = new Municipality { Name = "Limache", City = margaMarga };
            var villaAlemana = new Municipality { Name = "Villa Alemana", City = margaMarga };
            var margaMargaComuna = new Municipality { Name = "Marga Marga", City = margaMarga };

            var cerrillos = new Municipality { Name = "Cerrillos", City = metropolitanaCity };
            var cerroNavia = new Municipality { Name = "Cerro Navia", City = metropolitanaCity };
            var conchali = new Municipality { Name = "Conchalí", City = metropolitanaCity };
            var elBosque = new Municipality { Name = "El Bosque", City = metropolitanaCity };
            var estacionCentral = new Municipality { Name = "Estación Central", City = metropolitanaCity };
            var huechuraba = new Municipality { Name = "Huechuraba", City = metropolitanaCity };
            var independencia = new Municipality { Name = "Independencia", City = metropolitanaCity };
            var laCisterna = new Municipality { Name = "La Cisterna", City = metropolitanaCity };
            var laFlorida = new Municipality { Name = "La Florida", City = metropolitanaCity };
            var laGranja = new Municipality { Name = "La Granja", City = metropolitanaCity };
            var laPintana = new Municipality { Name = "La Pintana", City = metropolitanaCity };
            var laReina = new Municipality { Name = "La Reina", City = metropolitanaCity };
            var lasCondes = new Municipality { Name = "Las Condes", City = metropolitanaCity };
            var loBarnechea = new Municipality { Name = "Lo Barnechea", City = metropolitanaCity };
            var loEspejo = new Municipality { Name = "Lo Espejo", City = metropolitanaCity };
            var loPrado = new Municipality { Name = "Lo Prado", City = metropolitanaCity };
            var macul = new Municipality { Name = "Macul", City = metropolitanaCity };
            var maipu = new Municipality { Name = "Maipú", City = metropolitanaCity };
            var nunoa = new Municipality { Name = "Ñuñoa", City = metropolitanaCity };
            var pedroAguirreCerda = new Municipality { Name = "Pedro Aguirre Cerda", City = metropolitanaCity };
            var penalolen = new Municipality { Name = "Peñalolén", City = metropolitanaCity };
            var providencia = new Municipality { Name = "Providencia", City = metropolitanaCity };
            var pudahuel = new Municipality { Name = "Pudahuel", City = metropolitanaCity };

            var quilicura = new Municipality { Name = "Quilicura", City = metropolitanaCity };
            var quintaNormal = new Municipality { Name = "Quinta Normal", City = metropolitanaCity };
            var recoleta = new Municipality { Name = "Recoleta", City = metropolitanaCity };

            var renca = new Municipality { Name = "Renca", City = metropolitanaCity };
            var sanJoakin = new Municipality { Name = "San Joaquín", City = metropolitanaCity };
            var sanMiguel = new Municipality { Name = "San Miguel", City = metropolitanaCity };
            var sanRamon = new Municipality { Name = "San Ramón", City = metropolitanaCity };
            var santiagoComuna = new Municipality { Name = "Santiago", City = metropolitanaCity };
            var vitacura = new Municipality { Name = "Vitacura", City = metropolitanaCity };

            var padreHurtado = new Municipality { Name = "Padre Hurtado", City = talagante };
            var penaflor = new Municipality { Name = "Peñaflor", City = talagante };
            var talaganteComuna = new Municipality { Name = "Talagante", City = talagante };
            var elMonte = new Municipality { Name = "El Monte", City = talagante };
            var islaDeMaipo = new Municipality { Name = "Isla de Maipo", City = talagante };

            var buin = new Municipality { Name = "Buin", City = maipo };
            var caleraDeTango = new Municipality { Name = "Calera de Tango", City = maipo };
            var paine = new Municipality { Name = "Paine", City = maipo };
            var sanBernardo = new Municipality { Name = "San Bernardo", City = maipo };

            var alhue = new Municipality { Name = "Alhué", City = melipilla };
            var curacavi = new Municipality { Name = "Curacaví", City = melipilla };
            var mariaPinto = new Municipality { Name = "María Pinto", City = melipilla };
            var melipillaComuna = new Municipality { Name = "Melipilla", City = melipilla };
            var sanPedro = new Municipality { Name = "San Pedro", City = melipilla };

            var colina = new Municipality { Name = "Colina", City = chacabuco };
            var lampa = new Municipality { Name = "Lampa", City = chacabuco };
            var tiltil = new Municipality { Name = "Tiltil", City = chacabuco };

            var codegua = new Municipality { Name = "Codegua", City = cachapoal };
            var coinco = new Municipality { Name = "Coinco", City = cachapoal };
            var coltauco = new Municipality { Name = "Coltauco", City = cachapoal };
            var donihue = new Municipality { Name = "Doñihue", City = cachapoal };
            var graneros = new Municipality { Name = "Graneros", City = cachapoal };
            var lasCabras = new Municipality { Name = "Las Cabras", City = cachapoal };
            var machali = new Municipality { Name = "Machalí", City = cachapoal };
            var malloa = new Municipality { Name = "Malloa", City = cachapoal };
            var olivar = new Municipality { Name = "Olivar", City = cachapoal };
            var peumo = new Municipality { Name = "Peumo", City = cachapoal };
            var pichidegua = new Municipality { Name = "Pichidegua", City = cachapoal };
            var quintaDeTilcoco = new Municipality { Name = "Quinta de Tilcoco", City = cachapoal };
            var rancagua = new Municipality { Name = "Rancagua", City = cachapoal };
            var rengo = new Municipality { Name = "Rengo", City = cachapoal };
            var requinoa = new Municipality { Name = "Requínoa", City = cachapoal };
            var sanVicente = new Municipality { Name = "San Vicente de Tagua Tagua", City = cachapoal };
            var sanFrancisco = new Municipality { Name = "San Francisco de Mostazal", City = cachapoal };

            var chepica = new Municipality { Name = "Chepica", City = colchagua };
            var chimbarongo = new Municipality { Name = "Chimbarongo", City = colchagua };
            var lolol = new Municipality { Name = "Lolol", City = colchagua };
            var nancagua = new Municipality { Name = "Nancagua", City = colchagua };
            var palmilla = new Municipality { Name = "Palmilla", City = colchagua };
            var peralillo = new Municipality { Name = "Peralillo", City = colchagua };
            var placilla = new Municipality { Name = "Placilla", City = colchagua };
            var pumanque = new Municipality { Name = "Pumanque", City = colchagua };
            var sanFernando = new Municipality { Name = "San Fernando", City = colchagua };
            var santaCruz = new Municipality { Name = "Santa Cruz", City = colchagua };

            var laEstrella = new Municipality { Name = "La Estrella", City = cardenalCaro };
            var litueche = new Municipality { Name = "Litueche", City = cardenalCaro };
            var marchihue = new Municipality { Name = "Marchihue", City = cardenalCaro };
            var navidad = new Municipality { Name = "Navidad", City = cardenalCaro };
            var peredones = new Municipality { Name = "Paredones", City = cardenalCaro };
            var pichelemu = new Municipality { Name = "Pichilemu", City = cardenalCaro };

            var colbun = new Municipality { Name = "Colbún", City = linares };
            var longavi = new Municipality { Name = "Longaví", City = linares };
            var parral = new Municipality { Name = "Parral", City = linares };
            var retiro = new Municipality { Name = "Retiro", City = linares };
            var sanJavier = new Municipality { Name = "San Javier", City = linares };
            var villaAlegre = new Municipality { Name = "Villa Alegre", City = linares };
            var yerbasBuenas = new Municipality { Name = "Yerbas Buenas", City = linares };
            var linaresComuna = new Municipality { Name = "Linares", City = linares };

            var cauquenesComuna = new Municipality { Name = "Cauquenes", City = cauquenes };
            var chanco = new Municipality { Name = "Chanco", City = cauquenes };
            var pelluhue = new Municipality { Name = "Pelluhue", City = cauquenes };

            var curicoComuna = new Municipality { Name = "Curicó", City = curico };
            var hualane = new Municipality { Name = "Hualañé", City = curico };
            var licanten = new Municipality { Name = "Licantén", City = curico };
            var molina = new Municipality { Name = "Molina", City = curico };
            var rauco = new Municipality { Name = "Rauco", City = curico };
            var romeral = new Municipality { Name = "Romeral", City = curico };
            var sagradaFamilia = new Municipality { Name = "Sagrada Familia", City = curico };
            var teno = new Municipality { Name = "Teno", City = curico };
            var vichuquen = new Municipality { Name = "Vichuquén", City = curico };

            var constitucion = new Municipality { Name = "Constitución", City = talca };
            var curepto = new Municipality { Name = "Curepto", City = talca };
            var empedrado = new Municipality { Name = "Empedrado", City = talca };
            var mauleComuna = new Municipality { Name = "Maule", City = talca };
            var pelarco = new Municipality { Name = "Pelarco", City = talca };
            var pencahue = new Municipality { Name = "Pencahue", City = talca };
            var rioClaro = new Municipality { Name = "Río Claro", City = talca };
            var sanClemente = new Municipality { Name = "San Clemente", City = talca };
            var sanRafael = new Municipality { Name = "San Rafael", City = talca };
            var talcaComuna = new Municipality { Name = "Talca", City = talca };

            var bulnes = new Municipality { Name = "Bulnes", City = diguillin };
            var chillanComuna = new Municipality { Name = "Chillán", City = diguillin };
            var chillanViejo = new Municipality { Name = "Chillán Viejo", City = diguillin };
            var elCarmen = new Municipality { Name = "El Carmen", City = diguillin };
            var pemuco = new Municipality { Name = "Pemuco", City = diguillin };
            var pinto = new Municipality { Name = "Pinto", City = diguillin };
            var quillon = new Municipality { Name = "Quillón", City = diguillin };
            var sanIgnacio = new Municipality { Name = "San Ignacio", City = diguillin };
            var yungay = new Municipality { Name = "Yungay", City = diguillin };

            var cobquecura = new Municipality { Name = "Cobquecura", City = itata };
            var coelemu = new Municipality { Name = "Coelemu", City = itata };
            var ninhue = new Municipality { Name = "Ninhue", City = itata };
            var portezuelo = new Municipality { Name = "Portezuelo", City = itata };
            var quirihue = new Municipality { Name = "Quirihue", City = itata };
            var ranquil = new Municipality { Name = "Ránquil", City = itata };
            var treguaco = new Municipality { Name = "Treguaco", City = itata };

            var coihueco = new Municipality { Name = "Coihueco", City = punilla };
            var niquen = new Municipality { Name = "Ñiquén", City = punilla };
            var sanCarlos = new Municipality { Name = "San Carlos", City = punilla };
            var sanFabian = new Municipality { Name = "San Fabián", City = punilla };
            var sannicolas = new Municipality { Name = "San Nicolás", City = punilla };

            var araucoComuna = new Municipality { Name = "Arauco", City = arauco };
            var canete = new Municipality { Name = "Cañete", City = arauco };
            var contulmo = new Municipality { Name = "Contulmo", City = arauco };
            var curanilahue = new Municipality { Name = "Curanilahue", City = arauco };
            var lebu = new Municipality { Name = "Lebu", City = arauco };
            var losAlamos = new Municipality { Name = "Los Álamos", City = arauco };
            var tirua = new Municipality { Name = "Tirúa", City = arauco };

            var altoBiobio = new Municipality { Name = "Alto Biobío", City = biobioCity };
            var antuco = new Municipality { Name = "Antuco", City = biobioCity };
            var cabrero = new Municipality { Name = "Cabrero", City = biobioCity };
            var laja = new Municipality { Name = "Laja", City = biobioCity };
            var losAngeles = new Municipality { Name = "Los Ángeles", City = biobioCity };
            var mulchen = new Municipality { Name = "Mulchén", City = biobioCity };
            var nacimiento = new Municipality { Name = "Nacimiento", City = biobioCity };
            var negrete = new Municipality { Name = "Negrete", City = biobioCity };
            var quilleco = new Municipality { Name = "Quilleco", City = biobioCity };
            var quilaco = new Municipality { Name = "Quilaco", City = biobioCity };
            var sanRosendo = new Municipality { Name = "San Rosendo", City = biobioCity };
            var santaBarbara = new Municipality { Name = "Santa Bárbara", City = biobioCity };
            var tucapel = new Municipality { Name = "Tucapel", City = biobioCity };
            var yumbel = new Municipality { Name = "Yumbel", City = biobioCity };

            var concepcionComuna = new Municipality { Name = "Concepción", City = concepcion };
            var chiguayante = new Municipality { Name = "Chiguayante", City = concepcion };
            var coronel = new Municipality { Name = "Coronel", City = concepcion };
            var florida = new Municipality { Name = "Florida", City = concepcion };
            var hualpen = new Municipality { Name = "Hualpén", City = concepcion };
            var hualqui = new Municipality { Name = "Hualqui", City = concepcion };
            var lota = new Municipality { Name = "Lota", City = concepcion };
            var penco = new Municipality { Name = "Penco", City = concepcion };
            var sanPedroDeLaPaz = new Municipality { Name = "San Pedro de la Paz", City = concepcion };
            var santaJuana = new Municipality { Name = "Santa Juana", City = concepcion };
            var talcahuano = new Municipality { Name = "Talcahuano", City = concepcion };
            var tome = new Municipality { Name = "Tomé", City = concepcion };

            var carahue = new Municipality { Name = "Carahue", City = cautin };
            var cholchol = new Municipality { Name = "Cholchol", City = cautin };
            var cunco = new Municipality { Name = "Cunco", City = cautin };
            var curarrehue = new Municipality { Name = "Curarrehue", City = cautin };
            var freire = new Municipality { Name = "Freire", City = cautin };
            var galvarino = new Municipality { Name = "Galvarino", City = cautin };
            var gorbea = new Municipality { Name = "Gorbea", City = cautin };
            var lautarocomuna = new Municipality { Name = "Lautaro", City = cautin };
            var loncoche = new Municipality { Name = "Loncoche", City = cautin };
            var melipeuco = new Municipality { Name = "Melipeuco", City = cautin };
            var nuevaImperial = new Municipality { Name = "Nueva Imperial", City = cautin };
            var padrelasCasas = new Municipality { Name = "Padre las Casas", City = cautin };
            var perquenco = new Municipality { Name = "Perquenco", City = cautin };
            var pitrufquen = new Municipality { Name = "Pitrufquén", City = cautin };
            var pucon = new Municipality { Name = "Pucón", City = cautin };
            var saavedra = new Municipality { Name = "Saavedra", City = cautin };
            var temucoComuna = new Municipality { Name = "Temuco", City = cautin };
            var teodoroSchmidt = new Municipality { Name = "Teodoro Schmidt", City = cautin };
            var tolten = new Municipality { Name = "Toltén", City = cautin };
            var villarrica = new Municipality { Name = "Villarrica", City = cautin };
            var vilcun = new Municipality { Name = "Vilcún", City = cautin };

            var angol = new Municipality { Name = "Angol", City = malleco };
            var collipulli = new Municipality { Name = "Collipulli", City = malleco };
            var curacautin = new Municipality { Name = "Curacautín", City = malleco };
            var ercilla = new Municipality { Name = "Ercilla", City = malleco };
            var lonquimay = new Municipality { Name = "Lonquimay", City = malleco };
            var losSauces = new Municipality { Name = "Los Sauces", City = malleco };
            var lumaco = new Municipality { Name = "Lumaco", City = malleco };
            var puren = new Municipality { Name = "Purén", City = malleco };
            var renaico = new Municipality { Name = "Renaico", City = malleco };
            var traiguen = new Municipality { Name = "Traiguén", City = malleco };
            var victoria = new Municipality { Name = "Victoria", City = malleco };

            var fultron = new Municipality { Name = "Futrono", City = ranco };
            var laUnion = new Municipality { Name = "La Unión", City = ranco };
            var lagoRanco = new Municipality { Name = "Lago Ranco", City = ranco };
            var rioBueno = new Municipality { Name = "Río Bueno", City = ranco };

            var corral = new Municipality { Name = "Corral", City = valdivia };
            var lanco = new Municipality { Name = "Lanco", City = valdivia };
            var losLagosMunicipality = new Municipality { Name = "Los Lagos", City = valdivia };
            var mafil = new Municipality { Name = "Máfil", City = valdivia };
            var mariquina = new Municipality { Name = "Mariquina", City = valdivia };
            var paillaco = new Municipality { Name = "Paillaco", City = valdivia };
            var panguipulli = new Municipality { Name = "Panguipulli", City = valdivia };
            var valdiviaComuna = new Municipality { Name = "Valdivia", City = valdivia };

            var ancud = new Municipality { Name = "Ancud", City = chiloe };
            var castro = new Municipality { Name = "Castro", City = chiloe };
            var chonchi = new Municipality { Name = "Chonchi", City = chiloe };
            var curacoDeVelez = new Municipality { Name = "Curaco de Vélez", City = chiloe };
            var dalcahue = new Municipality { Name = "Dalcahue", City = chiloe };
            var puqueldon = new Municipality { Name = "Puqueldón", City = chiloe };
            var queilen = new Municipality { Name = "Queilén", City = chiloe };
            var quellon = new Municipality { Name = "Quellón", City = chiloe };
            var quemchi = new Municipality { Name = "Quemchi", City = chiloe };
            var quimchao = new Municipality { Name = "Quinchao", City = chiloe };

            var calbuco = new Municipality { Name = "Calbuco", City = llanquihue };
            var cochamo = new Municipality { Name = "Cochamó", City = llanquihue };
            var fresia = new Municipality { Name = "Fresia", City = llanquihue };
            var frutillar = new Municipality { Name = "Frutillar", City = llanquihue };
            var llanquihueComuna = new Municipality { Name = "Llanquihue", City = llanquihue };
            var losMuermos = new Municipality { Name = "Los Muermos", City = llanquihue };
            var maullin = new Municipality { Name = "Maullín", City = llanquihue };
            var puertoMontt = new Municipality { Name = "Puerto Montt", City = llanquihue };
            var puertoVaras = new Municipality { Name = "Puerto Varas", City = llanquihue };

            var osornoComuna = new Municipality { Name = "Osorno", City = osorno };
            var puertoOctay = new Municipality { Name = "Puerto Octay", City = osorno };
            var purranque = new Municipality { Name = "Purranque", City = osorno };
            var puyehue = new Municipality { Name = "Puyehue", City = osorno };
            var rioNegro = new Municipality { Name = "Río Negro", City = osorno };
            var sanPablo = new Municipality { Name = "San Pablo", City = osorno };
            var sanJuanDeLaCosta = new Municipality { Name = "San Juan de la Costa", City = osorno };

            var chaiten = new Municipality { Name = "Chaitén", City = palena };
            var futaleufu = new Municipality { Name = "Futaleufú", City = palena };
            var hualaihue = new Municipality { Name = "Hualaihué", City = palena };
            var palenaMunicipality = new Municipality { Name = "Palena", City = palena };

            var aysenMunicipality = new Municipality { Name = "Aysén", City = aysenCity };
            var cisnes = new Municipality { Name = "Cisnes", City = aysenCity };
            var guaitecas = new Municipality { Name = "Guaitecas", City = aysenCity };

            var cochrane = new Municipality { Name = "Cocharne", City = capitanPrat };
            var ohigginsMunicipality = new Municipality { Name = "O'Higgins", City = capitanPrat };
            var tortel = new Municipality { Name = "Tortel", City = capitanPrat };

            var coyhaiqueMunicipality = new Municipality { Name = "Coyhaique", City = coyhaique };
            var lagoVerde = new Municipality { Name = "Lago Verde", City = coyhaique };

            var chileChico = new Municipality { Name = "Chile Chico", City = generalCarrera };
            var rioIbanez = new Municipality { Name = "Río Ibáñez", City = generalCarrera };

            var antarticaMunicipality = new Municipality { Name = "Antártica", City = antartica };
            var caboDeHornos = new Municipality { Name = "Cabo de Hornos", City = antartica };

            var lagunaBlanca = new Municipality { Name = "Laguna Blanca", City = magallanesCity };
            var puntaArenas = new Municipality { Name = "Punta Arenas", City = magallanesCity };
            var rioVerde = new Municipality { Name = "Río Verde", City = magallanesCity };
            var sanGregorio = new Municipality { Name = "San Gregorio", City = magallanesCity };

            var porvenir = new Municipality { Name = "Porvenir", City = tierraDelFuego };
            var primavera = new Municipality { Name = "Primavera", City = tierraDelFuego };
            var timaukel = new Municipality { Name = "Timaukel", City = tierraDelFuego };

            var natales = new Municipality { Name = "Natales", City = ultimaEsperanza };
            var torresDelPaine = new Municipality { Name = "Torres del Paine", City = ultimaEsperanza };
            #endregion
            if (!await _context.Municipality.AnyAsync())
            {
                await _context.Municipality.AddRangeAsync(new List<Municipality> {
                    aricaComuna,
                    camarones,
                    generalLagos,
                    putre,
                    iquiqueComuna,
                    altoHospicio,
                    camina,
                    colchane,
                    huara,
                    pica,
                    pozoAlmonte,
                    antofagastaComuna,
                    mejillones,
                    sierraGorda,
                    taltal,
                    tocopillaComuna,
                    mariaElena,
                    calama,
                    ollague,
                    sanPedroDeAtacama,
                    chañaralComuna,
                    diegoDeAlmagro,
                    caldera,
                    tierraAmarilla,
                    copiapoComuna,
                    altoDelCarmen,
                    freirina,
                    huascoComuna,
                    vallenar,
                    andacollo,
                    coquimboComuna,
                    laHiguera,
                    laSerena,
                    paihuano,
                    vicuna,
                    canela,
                    illapel,
                    losVilos,
                    salamanca,
                    combarbala,
                    montePatria,
                    ovalle,
                    punitaqui,
                    rioHurtado,
                    casablanca,
                    concon,
                    juanFernandez,
                    puchuncavi,
                    quintero,
                    valparaisoComuna,
                    vinaDelMar,
                    algarrobo,
                    cartagena,
                    elQuisco,
                    elTabo,
                    sanAntonioComuna,
                    santoDomingo,
                    catemu,
                    llailay,
                    panquehue,
                    putaendo,
                    sanFelipeComuna,
                    santaMaria,
                    calleLarga,
                    losAndesComuna,
                    rinconada,
                    sanEsteban,
                    cabildo,
                    laLigua,
                    papudo,
                    petorcaComuna,
                    zapallar,
                    hijuelas,
                    laCruz,
                    nogales,
                    quillotaComuna,
                    calera,
                    olmue,
                    limache,
                    villaAlemana,
                    margaMargaComuna,
                    cerrillos,
                    cerroNavia,
                    conchali,
                    elBosque,
                    estacionCentral,
                    huechuraba,
                    independencia,
                    laCisterna,
                    laFlorida,
                    laGranja,
                    laPintana,
                    laReina,
                    lasCondes,
                    loBarnechea,
                    loEspejo,
                    loPrado,
                    macul,
                    maipu,
                    nunoa,
                    pedroAguirreCerda,
                    penalolen,
                    providencia,
                    pudahuel,
                    quilicura,
                    quintaNormal,
                    recoleta,
                    renca,
                    sanJoakin,
                    sanMiguel,
                    sanRamon,
                    santiagoComuna,
                    vitacura,
                    padreHurtado,
                    penaflor,
                    talaganteComuna,
                    elMonte,
                    islaDeMaipo,
                    buin,
                    caleraDeTango,
                    paine,
                    sanBernardo,
                    alhue,
                    curacavi,
                    mariaPinto,
                    melipillaComuna,
                    sanPedro,
                    colina,
                    lampa,
                    tiltil,
                    codegua,
                    coinco,
                    coltauco,
                    donihue,
                    graneros,
                    lasCabras,
                    machali,
                    malloa,
                    olivar,
                    peumo,
                    pichidegua,
                    quintaDeTilcoco,
                    rancagua,
                    rengo,
                    requinoa,
                    sanVicente,
                    sanFrancisco,
                    chepica,
                    chimbarongo,
                    lolol,
                    nancagua,
                    palmilla,
                    peralillo,
                    placilla,
                    pumanque,
                    sanFernando,
                    santaCruz,
                    laEstrella,
                    litueche,
                    marchihue,
                    navidad,
                    peredones,
                    pichelemu,
                    colbun,
                    longavi,
                    parral,
                    retiro,
                    sanJavier,
                    villaAlegre,
                    yerbasBuenas,
                    linaresComuna,
                    cauquenesComuna,
                    chanco,
                    pelluhue,
                    curicoComuna,
                    hualane,
                    licanten,
                    molina,
                    rauco,
                    romeral,
                    sagradaFamilia,
                    teno,
                    vichuquen,
                    constitucion,
                    curepto,
                    empedrado,
                    mauleComuna,
                    pelarco,
                    pencahue,
                    rioClaro,
                    sanClemente,
                    sanRafael,
                    talcaComuna,
                    bulnes,
                    chillanComuna,
                    chillanViejo,
                    elCarmen,
                    pemuco,
                    pinto,
                    quillon,
                    sanIgnacio,
                    yungay,
                    cobquecura,
                    coelemu,
                    ninhue,
                    portezuelo,
                    quirihue,
                    ranquil,
                    treguaco,
                    coihueco,
                    niquen,
                    sanCarlos,
                    sanFabian,
                    sannicolas,
                    araucoComuna,
                    canete,
                    contulmo,
                    curanilahue,
                    lebu,
                    losAlamos,
                    tirua,
                    altoBiobio,
                    antuco,
                    cabrero,
                    laja,
                    losAngeles,
                    mulchen,
                    nacimiento,
                    negrete,
                    quilleco,
                    quilaco,
                    sanRosendo,
                    santaBarbara,
                    tucapel,
                    yumbel,
                    concepcionComuna,
                    chiguayante,
                    coronel,
                    florida,
                    hualpen,
                    hualqui,
                    lota,
                    penco,
                    sanPedroDeLaPaz,
                    santaJuana,
                    talcahuano,
                    tome,
                    carahue,
                    cholchol,
                    cunco,
                    curarrehue,
                    freire,
                    galvarino,
                    gorbea,
                    lautarocomuna,
                    loncoche,
                    melipeuco,
                    nuevaImperial,
                    padrelasCasas,
                    perquenco,
                    pitrufquen,
                    pucon,
                    saavedra,
                    temucoComuna,
                    teodoroSchmidt,
                    tolten,
                    villarrica,
                    vilcun,
                    angol,
                    collipulli,
                    curacautin,
                    ercilla,
                    lonquimay,
                    losSauces,
                    lumaco,
                    puren,
                    renaico,
                    traiguen,
                    victoria,
                    fultron,
                    laUnion,
                    lagoRanco,
                    rioBueno,
                    corral,
                    lanco,
                    losLagosMunicipality,
                    mafil,
                    mariquina,
                    paillaco,
                    panguipulli,
                    valdiviaComuna,
                    ancud,
                    castro,
                    chonchi,
                    curacoDeVelez,
                    dalcahue,
                    puqueldon,
                    queilen,
                    quellon,
                    quemchi,
                    quimchao,
                    calbuco,
                    cochamo,
                    fresia,
                    frutillar,
                    llanquihueComuna,
                    losMuermos,
                    maullin,
                    puertoMontt,
                    puertoVaras,
                    osornoComuna,
                    puertoOctay,
                    purranque,
                    puyehue,
                    rioNegro,
                    sanPablo,
                    sanJuanDeLaCosta,
                    chaiten,
                    futaleufu,
                    hualaihue,
                    palenaMunicipality,
                    aysenMunicipality,
                    cisnes,
                    guaitecas,
                    cochrane,
                    ohigginsMunicipality,
                    tortel,
                    coyhaiqueMunicipality,
                    lagoVerde,
                    chileChico,
                    rioIbanez,
                    antarticaMunicipality,
                    caboDeHornos,
                    lagunaBlanca,
                    puntaArenas,
                    rioVerde,
                    sanGregorio,
                    porvenir,
                    primavera,
                    timaukel,
                    natales,
                    torresDelPaine
                });
                _ = await _context.SaveChangesAsync();
            }
        }
    }
}
