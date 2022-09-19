using System.Data;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.IdentityModel.Tokens;
using MySql.Data.MySqlClient;
using System.Security.Cryptography.X509Certificates;
using webapi_bilheteria_c.Domain.Interface;
using webapi_bilheteria_c.Domain.Models;
using webapi_bilheteria_c.Infra.Client;
using webapi_bilheteria_c.Infra.Repository;
using webapi_bilheteria_c.Services;
using webapi_bilheteria_c.Services.Provider;

namespace webapi_bilheteria_c.Properties
{
    public static class DIConfig
    {        
        public static ParametersProvider _parametersProvider;
        public static CredentialsProvider _credentialsProvider;

        public static IServiceCollection ConfigureServiceDependence(this IServiceCollection services){
            AddServices(services);
            AddRepository(services);
            AddProvider(services);
            AddClient(services);
            AddAppSettings(services);
            return services;
        }

        private static void AddAppSettings(this IServiceCollection services){
            var configurationKeys = new ConfigurationKeys();
            var settingsPath = 
                Path.Combine(PlatformServices.Default.Application.ApplicationBasePath, 
                $@"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json");                                                                 

            var configuration = new ConfigurationBuilder()
                .AddJsonFile(settingsPath, false, true)
                .Build();

            new ConfigureFromConfigurationOptions<ConfigurationKeys>(configuration)
                .Configure(configurationKeys);
            
            //add Database
            services.AddTransient<IDbConnection>(s => new MySqlConnection(configurationKeys.ConnectionStrings?.BilheteriaCDB)); 

            //starting Providers                       
            _parametersProvider = services.BuildServiceProvider().GetService<ParametersProvider>();
            _credentialsProvider = services.BuildServiceProvider().GetService<CredentialsProvider>();

            configurationKeys.Parameters = GetParametersFromDB();                    
            configurationKeys.Credentials = GetCredentialsFromDB();

            AddAuthorization(services, 
                Encoding.ASCII.GetBytes(configurationKeys.Parameters.FirstOrDefault(s => s.Code == "CONF1").Value));
            
            //add GerenciaNet certified
            configurationKeys.CertificateGerenciaNet = new X509Certificate2($@"./Certs/{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.p12", "");            

            services.AddSingleton(configurationKeys);
        }

        private static void AddServices(IServiceCollection services){            
            services.AddScoped<IUsersService, UsersService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<IEventsService, EventsService>(); 
            services.AddScoped<ILoggerService, LoggerService>();       
        }

        private static void AddRepository(IServiceCollection services){
            services.AddScoped<IUsersRepository, UsersRepository>();
            services.AddScoped<IParametersRepository, ParametersRepository>();
            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddScoped<IEventsRepository, EventsRepository>();
            services.AddScoped<ICredentialsRepository, CredentialsRepository>();
            services.AddScoped<IAuthorizationRepository, AuthorizationRepository>();
        }

        private static void AddProvider(IServiceCollection services){
            services.AddScoped<ParametersProvider>();
            services.AddScoped<CredentialsProvider>();
        }

        private static void AddClient(IServiceCollection services){
            services.AddScoped<IMessageProducer, RabbitMQClient>();
            services.AddScoped<IPixClient, GerenciaNetClient>();
        }

        private static List<Parameters> GetParametersFromDB(){
            return _parametersProvider.GetParametersFromDB();
        }

        private static List<Credentials> GetCredentialsFromDB(){
            return _credentialsProvider.GetCredentialsFromDB();
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