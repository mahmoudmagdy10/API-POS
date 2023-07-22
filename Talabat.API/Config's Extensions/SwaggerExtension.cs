namespace Talabat.API.Config_s_Extensions
{
    public static class SwaggerExtension
    {
        public static IServiceCollection AddSwaggerService(this IServiceCollection service)
        {
            service.AddEndpointsApiExplorer();
            service.AddSwaggerGen();
            return service;
        }
        
        public static WebApplication AddSwaggerMiddleware(this WebApplication app)
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            return app;
        }
    }
}
