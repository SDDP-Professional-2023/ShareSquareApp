using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using ShareSquare.Data;
using ShareSquare.Data.DOA;
using ShareSquare.Data.IDOA;
using ShareSquare.Data.Models;
using ShareSquare.Data.Models.Repository;
using ShareSquare.Hubs;
using ShareSquare.Mapping;
using ShareSquareApp.Services;
using ShareSquareApp.Services.IServices;

namespace ShareSquare
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ShareSquareDbContext>(options =>
                options.UseSqlServer(connectionString, b => b.MigrationsAssembly("ShareSquare")));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            //builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
            //    .AddEntityFrameworkStores<ApplicationDbContext>();

            builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<ShareSquareDbContext>()
                .AddDefaultTokenProviders();

            builder.Services.Configure<IdentityOptions>(options =>
            {
                // requires at least one numeric character in the password
                options.Password.RequireDigit = true;
                // password must be at least 8 characters long
                options.Password.RequiredLength = 8;
                // requires at least one special character in the password
                options.Password.RequireNonAlphanumeric = true;
                // requires at least one uppercase character in the password
                options.Password.RequireUppercase = true;
                // requires at least one lowercase character in the password
                options.Password.RequireLowercase = true;
                // requires a certain number of unique characters in the password
                options.Password.RequiredUniqueChars = 4;

                // sets how long a user is locked out when a lockout occurs (we set to 5mins) 
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                // sets the number of failed access attempts allowed before a user is locked out
                options.Lockout.MaxFailedAccessAttempts = 5;
                // sets whether lockouts are enabled for new users
                options.Lockout.AllowedForNewUsers = true;

                // sets the characters that are allowed in the user names
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                // ensures that registered email addresses are unique across users
                options.User.RequireUniqueEmail = true;

            });
            builder.Services.AddAuthentication().AddFacebook(options =>
            {
                var Facebook = builder.Configuration.GetSection("Facebook").Get<FacebookOptions>();
                options.AppId = Facebook.AppId;
                options.AppSecret = Facebook.AppSecret;
            });

            builder.Services.AddAuthentication().AddGoogle(options =>
            {
                var Google = builder.Configuration.GetSection("Google").Get<GoogleOptions>();

                options.ClientId = Google.ClientId;
                options.ClientSecret = Google.ClientSecret;
            });

            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();

            // This part utilize dependency injection setup
            // Transient lifetime services are created each time they're requested
            // Scoped service is created once per request within the scope
            // Singleton services are created the first time they are requested 

            // An instance of MailJetEmailSender will be created each time the IEmailSender interface is used
            builder.Services.AddTransient<IEmailSender, MailJetEmailSender>();
            builder.Services.AddTransient<IEmailService, EmailService>();

            builder.Services.AddScoped<IItemDOA, ItemDOA>();
            builder.Services.AddScoped<IItemService, ItemService>();

            builder.Services.AddScoped<IFavoriteItemDOA, FavoriteItemDOA>();
            builder.Services.AddScoped<IFavoriteItemService, FavoriteItemService>();

            builder.Services.AddScoped<IReviewDOA, ReviewDOA>();
            builder.Services.AddScoped<IReviewService, ReviewService>();

            builder.Services.AddScoped<IUserItemService, UserItemService>();

            builder.Services.AddScoped<IMessageDOA, MessageDOA>();
            builder.Services.AddScoped<IMessageService, MessageService>();

            builder.Services.AddScoped<IErrorDOA, ErrorDOA>();
            builder.Services.AddScoped<IErrorService, ErrorService>();

            builder.Services.AddScoped<IUserDOA, UserDOA>();
            builder.Services.AddScoped<IUserService, UserService>();

            builder.Services.AddAutoMapper(typeof(MappingProfile));

            builder.Services.AddSignalR();
            builder.Services.AddHttpContextAccessor();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.Use(async (context, next) =>
            {
                if (string.IsNullOrEmpty(context.Request.Path.Value) || context.Request.Path.Value == "/")
                {
                    if (context.User.Identity.IsAuthenticated && context.User.IsInRole("Admin"))
                    {
                        context.Response.Redirect("/Admin/Index");
                        return;
                    }
                    else
                    {
                        context.Response.Redirect("/Item/Index");
                        return;
                    }
                }

                await next.Invoke();
            });

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Item}/{action=Index}/{id?}");
            app.MapRazorPages();
            app.MapHub<UserHub>("/hubs/userCount");
            app.MapHub<MessageHub>("/hubs/messageHub");

            app.Run();
        }
    }
}