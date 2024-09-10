namespace Backend.Models.Extensions
{
    public static class MigrationExtension
    {
        public static async Task<IHost> UseMigrations(this IHost app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<IntelificioDbContext>();
                //if (context.Providers.Count() > 0) return app;

                //var providerList = new List<Provider> {
                //    new Provider { Name = "Azure", ImageURL = "/app/Common/Images/Azure.png" },
                //    new Provider { Name = "AWS", ImageURL = "/app/Common/Images/AWS.png" },
                //    new Provider { Name = "GCP", ImageURL = "/app/Common/Images/GCP.png" }
                //};

                //context.Providers.AddRange(providerList);
                //_ = context.SaveChanges();

                //var test = await context.Providers.Where(e => e.Name == "AWS").FirstOrDefaultAsync();
                //_ = context.Providers.Remove(test);
                //_ = await context.SaveChangesAsync();
            }
            return app;
        }
    }
}
