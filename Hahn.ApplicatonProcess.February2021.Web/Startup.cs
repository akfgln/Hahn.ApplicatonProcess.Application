using Hahn.ApplicatonProcess.February2021.Data;
using Hahn.ApplicatonProcess.February2021.Data.Helpers;
using Hahn.ApplicatonProcess.February2021.Domain.Common;
using Hahn.ApplicatonProcess.February2021.Web.Filters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hahn.ApplicatonProcess.February2021.Web
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
            Configurations.Setup(services, Configuration);
            ConfigureJwtBearer(services, Configuration);
            services.AddControllers();
            services.AddMvc(options => { options.Filters.Add(new ApiExceptionFilter()); })
               .AddJsonOptions(o =>
               {
                   o.JsonSerializerOptions.PropertyNamingPolicy = null;
                   o.JsonSerializerOptions.DictionaryKeyPolicy = null;
               });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Hahn.ApplicatonProcess.February2021.Web", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                      {
                        {
                          new OpenApiSecurityScheme
                          {
                            Reference = new OpenApiReference
                              {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                              },
                              Scheme = "oauth2",
                              Name = "Bearer",
                              In = ParameterLocation.Header,

                            },
                            new System.Collections.Generic.List<string>()
                          }
                        });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Hahn.ApplicatonProcess.February2021.Web v1"));
            }

            InitDatabase(app);

            app.UseStaticFiles();
            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private static void ConfigureJwtBearer(IServiceCollection services, IConfiguration conf)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, (o) =>
            {
                o.TokenValidationParameters = new TokenValidationParameters()
                {
                    IssuerSigningKey = TokenAuthOption.Key,
                    ValidAudience = TokenAuthOption.Audience,
                    ValidIssuer = TokenAuthOption.Issuer,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ClockSkew = TimeSpan.FromMinutes(0)
                };
            });

            services.AddAuthorization(auth =>
            {
                auth.AddPolicy(JwtBearerDefaults.AuthenticationScheme, new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser().Build());
            });
        }

        private void InitDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<HahnDbContext>();
                context.Database.EnsureCreated();

                /* Seed Data For InMemory*/
                if (context.Database.IsInMemory())
                {
                    var testRoles = context.Roles.FirstOrDefaultAsync(x => x.DefaultRoleName == "Administrator");
                    if (testRoles.Result == null)
                    {
                        context.Roles.AddRange(new List<Roles>() {
                    new Roles {  DefaultRoleName = "Administrator", CreateDate = DateTime.UtcNow, IsActive = true, IsDeleted = false, ModifiedDate = DateTime.UtcNow },
                    new Roles {  DefaultRoleName = "Manager", CreateDate = DateTime.UtcNow, IsActive = true, IsDeleted = false, ModifiedDate = DateTime.UtcNow  },
                    new Roles {  DefaultRoleName = "Administrator,Manager", CreateDate = DateTime.UtcNow, IsActive = true, IsDeleted = false, ModifiedDate = DateTime.UtcNow  }});
                        context.SaveChanges();
                    }
                    var testAdmin = context.Users.FirstOrDefaultAsync(x => x.EMail == "admin@hahn.com");
                    if (testAdmin.Result == null)
                    {
                        context.Users.Add(new Users { EMail = "admin@hahn.com", Password = "admin".WithBCrypt(), FirstName = "admin", LastName = "admin" });
                        context.SaveChanges();
                        testAdmin = context.Users.FirstOrDefaultAsync(x => x.EMail == "admin@hahn.com");
                        var roles = context.Roles.ToListAsync();
                        if (testAdmin != null && testAdmin.Result != null && testRoles.Result != null && testRoles.Result != null)
                        {
                            foreach (var item in roles.Result)
                            {
                                testAdmin.Result.Roles.Add(new UserRoles { Role = item, IsActive = true, User = testAdmin.Result });
                            }
                            context.SaveChanges();
                        }
                    }
                }
                else
                {
                    context.Database.Migrate();
                }
            }
        }
    }
}
