using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using Newtonsoft.Json.Serialization;
using quan_ly_kho.common.Common;
using quan_ly_kho.common.Helpers;
using quan_ly_kho.common.Models;
using quan_ly_kho.common.Services;
using quan_ly_kho.DataBase.Provider;
using Quartz;
using StackExchange.Redis;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tusdotnet;
using tusdotnet.Models;
using tusdotnet.Models.Configuration;
using tusdotnet.Models.Expiration;
using tusdotnet.Stores;

namespace quan_ly_kho.zapp
{
    public class Startup
    {
        private IMemoryCache _cache;

        // Đặt tên cho chính sách CORS để dễ quản lý
        public string MyAllowSpecificOrigins;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

        }
        public IConfiguration Configuration { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            BsonSerializer.RegisterSerializer(new DecimalSerializer(BsonType.Decimal128));
            BsonSerializer.RegisterSerializer(typeof(DateTime), MongoDB.Bson.Serialization.Serializers.DateTimeSerializer.LocalInstance);
            services.AddDbContext<DataContext>(options =>
                options.UseNpgsql(
                    Configuration.GetConnectionString("DefaultConnection")));
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);
            //  Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NBaF5cXmZCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdnWXtccXZUQmlZV0NxXUo=");

            //Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Mgo+DSMBPh8sVXJ8S0d+X1JPd11dXmJWd1p/THNYflR1fV9DaUwxOX1dQl9mSX1RdkVhWnZdcn1VQWk=;Mgo+DSMBMAY9C3t2XVhhQlJHfV5AQmBIYVp/TGpJfl96cVxMZVVBJAtUQF1hTH5QdEVjW3xXc3ZcRGNZ");
            var appSettings = appSettingsSection.Get<AppSettings>();

            var key_excel = appSettings.key_excel;
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(key_excel);
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);


            //JOB
            // base configuration from appsettings.json
            services.Configure<QuartzOptions>(Configuration.GetSection("Quartz"));

            // if you are using persistent job store, you might want to alter some options
            services.Configure<QuartzOptions>(options =>
            {
                options.Scheduling.IgnoreDuplicates = true; // default: false
                options.Scheduling.OverWriteExistingData = true; // default: true
            });
            services.AddQuartz(q =>
            {

                q.SchedulerId = "Scheduler-Core";
                q.UseMicrosoftDependencyInjectionJobFactory();

                //var jobKey = new JobKey("AutoCrawDataJob", "group1");
                //q.AddJob<AutoCrawDataJob>(jobKey, q => q.WithIdentity(jobKey));


                //q.AddTrigger(t => t
                //  .WithIdentity("AutoCrawDataJobTrigger")
                //    .ForJob(jobKey)
                //   .StartNow()
                //    .WithSimpleSchedule(x => x.WithInterval(TimeSpan.FromMinutes(2)).RepeatForever()));


                //q.AddJob<AutoDongBoDuLieuJob>(jobKey, q => q.WithIdentity(jobKey));


                //q.AddTrigger(t => t
                //  .WithIdentity("AutoDongBoDuLieuJobTrigger")
                //    .ForJob(jobKey)
                //   .StartNow()
                //    .WithSimpleSchedule(x => x.WithInterval(TimeSpan.FromMinutes(2)).RepeatForever()));

            });

            // ASP.NET Core hosting
            services.AddQuartzServer(options =>
            {
                // when shutting down we want jobs to complete gracefully
                options.WaitForJobsToComplete = true;
            });


            services.AddDbContext<vnaisoftDefautContext>(options =>
           options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddControllers().AddNewtonsoftJson(setup =>
            {
                setup.SerializerSettings.ContractResolver = new DefaultContractResolver();
                setup.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                setup.SerializerSettings.DateTimeZoneHandling = Newtonsoft.Json.DateTimeZoneHandling.Local;
            });
            services.Configure<MailSettings>(Configuration.GetSection("MailSettings"));
            services.AddMvc(options =>
            {
                options.InputFormatters.Add(new RawRequestBodyFormatter());
            }).AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            //services.AddAuthentication(x =>
            //{
            //    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;


            //    x.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //    x.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //    x.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
            //}).AddGoogle(options =>
            //{
            //    options.ClientId = Configuration["Google:ClientId"];
            //    options.ClientSecret = Configuration["Google:ClientSecret"];
            //    options.CallbackPath = "/home";
            //})
            //.AddJwtBearer(x =>
            //{
            //    x.Events = new JwtBearerEvents
            //    {
            //        OnTokenValidated = context =>
            //        {
            //            //var userService = context.HttpContext.RequestServices.GetRequiredService<IUserService>();
            //            var userId = context.Principal.Identity.Name;
            //            if (userId == null)
            //            {
            //                // return unauthorized if user no longer exists
            //                context.Fail("Unauthorized");
            //            }
            //            return Task.CompletedTask;
            //        },
            //        OnMessageReceived = context =>
            //        {
            //            var accessToken = context.Request.Query["accesstoken"];
            //            var path = context.HttpContext.Request.Path;
            //            if (!string.IsNullOrEmpty(accessToken)
            //                && path.StartsWithSegments("/SignalChatGPTHub"))
            //            {
            //                context.Token = accessToken;
            //            }
            //            if (!string.IsNullOrEmpty(accessToken)
            //               && path.StartsWithSegments("/SignalSocketChatHub"))
            //            {
            //                context.Token = accessToken;
            //            }
            //            return Task.CompletedTask;
            //        }
            //    };

            //    x.RequireHttpsMetadata = false;
            //    x.SaveToken = true;
            //    x.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        ValidateIssuerSigningKey = true,
            //        IssuerSigningKey = new SymmetricSecurityKey(key),
            //        ValidateIssuer = false,
            //        ValidateAudience = false
            //    };
            //});


            services.AddAuthentication(options =>
            {
                // Thiết lập mặc định là JWT cho các API
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
           .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
           {
               options.Events = new JwtBearerEvents
               {
                   OnTokenValidated = context =>
                   {
                       var userId = context.Principal.Identity.Name;
                       if (string.IsNullOrEmpty(userId))
                       {
                           context.Fail("Unauthorized");
                       }
                       return Task.CompletedTask;
                   },
                   OnMessageReceived = context =>
                   {
                       var accessToken = context.Request.Query["accesstoken"];
                       var path = context.HttpContext.Request.Path;
                       if (!string.IsNullOrEmpty(accessToken) &&
                           (path.StartsWithSegments("/SignalChatGPTHub") || path.StartsWithSegments("/SignalSocketChatHub")))
                       {
                           context.Token = accessToken;
                       }
                       return Task.CompletedTask;
                   }
               };

               options.RequireHttpsMetadata = false;
               options.SaveToken = true;
               options.TokenValidationParameters = new TokenValidationParameters
               {
                   ValidateIssuerSigningKey = true,
                   IssuerSigningKey = new SymmetricSecurityKey(key),
                   ValidateIssuer = false,
                   ValidateAudience = false
               };
           })
           .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
           {
               options.LoginPath = "/login"; // Đường dẫn login
               options.LogoutPath = "/logout"; // Đường dẫn logout
           });
            //.AddGoogle(GoogleDefaults.AuthenticationScheme, options =>
            //{
            //    options.ClientId = Configuration["Google:ClientId"];
            //    options.ClientSecret = Configuration["Google:ClientSecret"];
            //    options.CallbackPath = "/signin-google"; // Đường dẫn callback từ Google
            //});


            // Configure the DI container to use the multi-tenant database service
            services.AddSingleton<IMongoClientFactory, MongoClientFactory>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped(s =>
            {
                var factory = s.GetRequiredService<IMongoClientFactory>();
                var httpContext = s.GetService<IHttpContextAccessor>()?.HttpContext;
                var host = httpContext?.Request.Host.Host;
                var subdomain = "";

                subdomain = appSettings.mongodb_database;
                return factory.CreateClientDatabase(subdomain);
            });


            services.AddTransient<quan_ly_khoAuthorize>();
            services.AddTransient<IMailService, MailService>();
            services.AddHttpClient();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(20);
                options.Cookie.HttpOnly = true;
            });
            services.AddAuthentication()
                .AddIdentityServerJwt();
            services.AddControllersWithViews();
            services.AddRazorPages();
            services.AddScoped<IUserService, UserService>();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddSingleton(CreateTusConfiguration);
            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });
            //services.AddSignalR();

            //// --- Add CORS services with a dynamic policy ---
            //services.AddCors(options =>
            //{
            //    options.AddPolicy(name: "MyAllowSpecificOrigins",
            //           policy =>
            //           {
            //               policy.WithOrigins(

            //                          "https://localhost:44398",
            //                         "https://localhost:44399", // Cho phép nguồn gốc từ ứng dụng front-end local
            //                         "https://*.vnaisoft.com",   // Cho phép TẤT CẢ các tên miền phụ của vnaisoft.com
            //                         "https://*.aisava.com",
            //                         "https://sodangbo.hcm.edu.vn"
            //                      )
            //                     .SetIsOriginAllowedToAllowWildcardSubdomains() // BẮT BUỘC khi dùng wildcard *.
            //                     .AllowAnyHeader()  // Cho phép bất kỳ header nào (VD: Content-Type, Authorization)
            //                     .AllowAnyMethod(); // Cho phép bất kỳ phương thức nào (GET, POST, PUT, DELETE, OPTIONS)
            //           });
            //});
            //services.Configure<IISServerOptions>(options =>
            //{

            //    options.MaxRequestBodySize = long.MaxValue;
            //    options.MaxRequestBodyBufferSize = int.MaxValue;
            //});

            //services.Configure<FormOptions>(options =>
            //{
            //    options.MultipartBodyLengthLimit = long.MaxValue;
            //    options.ValueLengthLimit = int.MaxValue;
            //    options.BufferBodyLengthLimit = long.MaxValue;
            //    options.MemoryBufferThreshold = int.MaxValue;
            //});

            //services.AddStackExchangeRedisCache(options =>
            //{
            //    options.Configuration = appSettings.redis_server;
            //    options.ConfigurationOptions = new ConfigurationOptions()
            //    {
            //        EndPoints = { appSettings.redis_server },
            //    };
            //    options.ConfigurationOptions.Password = appSettings.redis_password;
            //    options.ConfigurationOptions.ChannelPrefix = appSettings.redis_client_name + "channel";
            //    options.ConfigurationOptions.ClientName = appSettings.redis_client_name;


            //});
            //var serviceProvider = services.BuildServiceProvider();
            //ClearRedisCache(serviceProvider);

        }
        private void ClearRedisCache(IServiceProvider serviceProvider)
        {
            // Retrieve the Redis configuration
            var redisConfiguration = serviceProvider.GetService<IOptions<RedisCacheOptions>>().Value;
            var appsetting = serviceProvider.GetService<IOptions<AppSettings>>().Value;

            // Create connection multiplexer

            var options = ConfigurationOptions.Parse(redisConfiguration.Configuration);
            options.ConnectRetry = 5;
            options.AllowAdmin = true;
            options.ClientName = appsetting.redis_client_name;
            options.ChannelPrefix = appsetting.redis_client_name + "channel";
            options.Password = appsetting.redis_password;


            var connectionMultiplexer = ConnectionMultiplexer.Connect(options);

            // Get the database to clear
            var database = connectionMultiplexer.GetDatabase();

            // Clear the cache
            var endpoints = connectionMultiplexer.GetEndPoints();
            foreach (var endpoint in endpoints)
            {
                var server = connectionMultiplexer.GetServer(endpoint);
                server.FlushDatabase(database.Database);
            }
        }

        private DefaultTusConfiguration CreateTusConfiguration(IServiceProvider serviceProvider)
        {
            var appSettingsSection = Configuration.GetSection("AppSettings");

            var _appSettings = appSettingsSection.Get<AppSettings>();

            var env = (IWebHostEnvironment)serviceProvider.GetRequiredService(typeof(IWebHostEnvironment));
            var tusFiles = Path.Combine(Directory.GetCurrentDirectory(), "file_upload", "tempFile");
            if (!Directory.Exists(tusFiles))
                Directory.CreateDirectory(tusFiles);

            return new DefaultTusConfiguration
            {
                UrlPath = "/tusFiles",
                //File storage path
                Store = new TusDiskStore(tusFiles),
                //Does metadata allow null values
                MetadataParsingStrategy = MetadataParsingStrategy.AllowEmptyValues,
                //The file will not be updated after expiration
                Expiration = new AbsoluteExpiration(TimeSpan.FromMinutes(120)),
                //Event handling (various events, meet your needs)
                //Event handling (various events, meet your needs)
                Events = new Events
                {
                    //Upload completion event callback
                    OnFileCompleteAsync = async ctx =>
                    {

                        //Get upload file
                        var file = await ctx.GetFileAsync();

                        //Get upload file
                        var metadatas = await file.GetMetadataAsync(ctx.CancellationToken);
                        //Get the target file name in the above file metadata
                        var fileNameMetadata = metadatas["fileName"];
                        //The target file name is encoded in Base64, so it needs to be decoded here
                        var fileName = fileNameMetadata.GetString(Encoding.UTF8).Trim('"') + "";
                        var path = "";
                        var id = metadatas["id"].GetString(Encoding.UTF8);
                        var controller = metadatas["controller"].GetString(Encoding.UTF8) ?? "";
                        if (controller == "storage_file_manager")
                        {

                            var id_folder = metadatas["id_folder"].GetString(Encoding.UTF8);
                            id_folder = id_folder == "undefined" ? "" : id_folder;
                            if (string.IsNullOrEmpty(id_folder))
                            {
                                path = Path.Combine(_appSettings.folder_path, "file_upload", "storage_file_manager");
                            }
                            else
                            {
                                path = Path.Combine(_appSettings.folder_path, "file_upload", "storage_file_manager", id_folder);
                            }
                        }
                        else
                        {
                            path = Path.Combine(_appSettings.folder_path, "file_upload", controller, id);
                        }

                        if (!Directory.Exists(path))
                            Directory.CreateDirectory(path);
                        var tick = Guid.NewGuid().ToString();
                        var pathsave = Path.Combine(path, ctx.FileId + "." + fileName.Split(".").Last());
                        //Convert the uploaded file to the actual target file

                        File.Move(Path.Combine(tusFiles, ctx.FileId), pathsave);
                        File.Delete(Path.Combine(tusFiles, ctx.FileId));
                        File.Delete(Path.Combine(tusFiles, ctx.FileId) + ".chunkcomplete");
                        File.Delete(Path.Combine(tusFiles, ctx.FileId) + ".chunkstart");
                        File.Delete(Path.Combine(tusFiles, ctx.FileId) + ".expiration");
                        File.Delete(Path.Combine(tusFiles, ctx.FileId) + ".metadata");
                        File.Delete(Path.Combine(tusFiles, ctx.FileId) + ".uploadlength");
                    }
                }
            };
        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, DataContext dataContext)
        {
            //dataContext.Database.Migrate();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //   app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseTus(httpContext => Task.FromResult(httpContext.RequestServices.GetService<DefaultTusConfiguration>()));
            app.UseStaticFiles();
            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();

            }
            else
            {
                app.UseHttpsRedirection();
            }
            //app.Use(async (context, next) =>
            //{
            //    context.Response.Headers.Add("X-Frame-Options", "SAMEORIGIN");
            //    await next();
            //});
            app.UseRouting();
            // 2. SỬ DỤNG MIDDLEWARE CORS
            // !! QUAN TRỌNG: Đặt UseCors() ở ĐÚNG VỊ TRÍ !!
            // Phải đặt sau UseRouting (nếu có) nhưng TRƯỚC UseAuthorization và MapControllers/MapEndpoints.

            app.UseCors("MyAllowSpecificOrigins");

            //app.UseMiddleware<sys_thong_ke_truy_capController>();
            app.UseSession();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}.ctr/{action=Index}/{id?}");
                //endpoints.MapHub<signalROPCHub>("/signalROPCHub");
                //endpoints.MapHub<SignalChatGPTHub>("/SignalChatGPTHub");
                //endpoints.MapHub<SignalSocketChatHub>("/SignalSocketChatHub");
                endpoints.MapRazorPages();
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";
                if (env.IsDevelopment())
                {
                    //spa.UseAngularCliServer(npmScript: "start");
                    spa.UseProxyToSpaDevelopmentServer("http://localhost:4200");
                }
            });
        }

    }
}
