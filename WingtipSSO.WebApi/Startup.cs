using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Runtime;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WingtipSSO.BusinessLogicLayer;
using WingtipSSO.BusinessLogicLayer.Identity;
using WingtipSSO.DataAccessLayer;
using WingtipSSO.DynamoDBAccess;
using WingtipSSO.MongoDBDataAccess;

namespace WingtipSSO.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            #region Configuration4JWT
            var jwtOptions = new JwtOptions();
            Configuration.GetSection("jwt").Bind(jwtOptions);
            services.AddSingleton(jwtOptions);
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = jwtOptions.JwtIssuer,
                    ValidAudience = jwtOptions.JwtAudience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.JwtKey))
                };
            });
            #endregion

            services.AddControllers();

            #region Coniguration4AutoMapper 
            services.AddAutoMapper(typeof(DynamoWingtipProfile));
            services.AddAutoMapper(typeof(MongoWingtipProfile));
            services.AddAutoMapper(typeof(WingtipDtoProfile));
            #endregion


            string DBType = Configuration["DatabaseSettings:DBType"];
            if (DBType.Equals("MongoDB"))
            {
                services.Configure<DatabaseSettings>(Configuration.GetSection(nameof(DatabaseSettings)));
                services.AddSingleton<IDatabaseSettings>(x => x.GetRequiredService<IOptions<DatabaseSettings>>().Value);

                #region Configuration4MongoDBRepository
                services.AddSingleton<IUsersRepository, MongoUsersRepository>();
                services.AddSingleton<IUserLoginLogsRepository, MongoUserLoginLogsRepository>();
                services.AddSingleton<IUserHistoriesRepository, MongoUserHistoriesRepository>();
                services.AddSingleton<IUserWrongPasswordRepository, MongoUserWrongPasswordRepository>();
                #endregion
            }
            else
            {
                #region Configuration4DynamoDB
                AmazonDynamoDBConfig clientConfig = new AmazonDynamoDBConfig();
                //    Configuration.GetSection("DatabaseSettings:DynamoDB").Bind(clientConfig);
                clientConfig.RegionEndpoint = RegionEndpoint.USEast2;
                clientConfig.ServiceURL = "https://dynamodb.us-east-2.amazonaws.com";

                var credentials = new BasicAWSCredentials(Configuration["DatabaseSettings:DynamoDB:AccessKey"], Configuration["DatabaseSettings:DynamoDB:SecretKey"]);

                AmazonDynamoDBClient client = new AmazonDynamoDBClient(credentials, clientConfig);
                DynamoDBContext context = new DynamoDBContext(client);
                services.AddSingleton(context);
                #endregion
                #region Configuration4DynamoDBRepository
                services.AddSingleton<IUsersRepository, DynamoUsersRepository>();
                services.AddSingleton<IUserLoginLogsRepository, DynamoUserLoginLogsRepository>();
                services.AddSingleton<IUserHistoriesRepository, DynamoUserHistoriesRepository>();
                services.AddSingleton<IUserWrongPasswordRepository, DynamoUserWrongPasswordRepository>();
                #endregion
            }

            services.AddScoped<IUserService, UserService>();

            services.AddScoped<IJwtProvider, JwtProvider>();

            services.AddCors(options =>
            {
                options.AddPolicy("FrontEndClient", builder => builder.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200"));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("FrontEndClient");

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
