using FilmesAPI.Business;

namespace FilmesAPI
{
    public static class InjectionDependency
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IFilmeBusiness, FilmeBusiness>();

            return services;
        }
    }
}
