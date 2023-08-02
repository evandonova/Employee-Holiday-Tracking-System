namespace EmployeeHolidayTrackingSystem.Web.Infrastructure
{
    public static class EndpointExtensions
    {
        public static void MapRedirect(this IEndpointRouteBuilder endpoints,
            string sourcePath, string destinationPath)
        {
            endpoints.MapGet(sourcePath, context => 
                    Task.Factory.StartNew(() 
                        => context.Response.Redirect(destinationPath, true, true)));
        }

        public static void MapDefaultAreaRoute(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapControllerRoute(
                name: "Areas",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
        }
    }
}
