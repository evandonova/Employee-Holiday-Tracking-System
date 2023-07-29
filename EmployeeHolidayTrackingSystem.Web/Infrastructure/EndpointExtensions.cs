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
    }
}
