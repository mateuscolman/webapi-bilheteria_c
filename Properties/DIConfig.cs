using System.Data;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.IdentityModel.Tokens;
using MySql.Data.MySqlClient;
using webapi_bilheteria_c.Domain.Interface;
using webapi_bilheteria_c.Domain.Models;
using webapi_bilheteria_c.Infra.Repository;
using webapi_bilheteria_c.Services;
using webapi_bilheteria_c.Services.Provider;

namespace webapi_bilheteria_c.Properties
{
    public static class DIConfig
    {

        public static ParametersProvider _parametersProvider;

        public static IServiceCollection ConfigureServiceDependence(this IServiceCollection services){
            AddAppSettings(services);
            return services;
        }

        public static void AddAppSettings(this IServiceCollection services){
            var configurationKeys = new ConfigurationKeys();
            var filePath = 
                Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, 
                $@"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json");
            
            var configuration = new ConfigurationBuilder()
                .AddJsonFile(filePath, false, true)
                .Build();

            new ConfigureFromConfigurationOptions<ConfigurationKeys>(configuration)
                .Configure(configurationKeys);
            
            //add Database
            services.AddTransient<IDbConnection>(s => new MySqlConnection(configurationKeys.ConnectionStrings?.BilheteriaCDB));

            //add Services
            services.AddScoped<IUsersService, UsersService>();
            services.AddScoped<ITokenService, TokenService>();

            //add Repository
            services.AddScoped<IUsersRepository, UsersRepository>();
            services.AddScoped<IParametersRepository, ParametersRepository>();

            //add Providers
            services.AddScoped<ParametersProvider>();

            _parametersProvider = services.BuildServiceProvider().GetService<ParametersProvider>();

            configurationKeys.Parameters = GetParametersFromDB();
            AddAuthorization(services, 
                Encoding.ASCII.GetBytes(configurationKeys.Parameters.FirstOrDefault(s => s.Code == "CONF1").Value));

            services.AddSingleton(configurationKeys);
        }

        private  static List<Parameters> GetParametersFromDB(){
            return _parametersProvider.GetParametersFromDB();
        }

        private static void AddAuthorization(IServiceCollection services, byte[] secret){
            services.AddAuthentication(x =>
            {
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(secret),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
        }
    }
}