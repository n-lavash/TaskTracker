using Hangfire;
using Microsoft.AspNetCore.Authentication.Cookies;
using TaskTracker.BLL;
using TaskTracker.BLL.Interfaces;
using TaskTracker.DAL;
using TaskTracker.DAL.Interfaces;
using TaskTracker.PL.BackgroundTasks;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var dbHost = Environment.GetEnvironmentVariable("DB_HOST");
var dbName = Environment.GetEnvironmentVariable("DB_NAME");
var dbPassword = Environment.GetEnvironmentVariable("DB_SA_PASSWORD");
//var connectionString = builder.Configuration.GetConnectionString("TaskTrackerConnectStr");
var connectionString = $"Data Source={dbHost};Initial Catalog={dbName};Connect Timeout=30;User ID=sa;Password={dbPassword}";

builder.Services.AddHangfire(c =>
    c.UseSqlServerStorage(connectionString));
builder.Services.AddHangfireServer();
builder.Services.AddSingleton(new BackgroundJobServerOptions
{
    WorkerCount = 1
});

builder.Services.AddSingleton(connectionString);
builder.Services.AddScoped<ITaskTrackerBll, TaskTrackerBll>();
builder.Services.AddSingleton<ITaskTrackerDao>(provider => new TaskTrackerDao(connectionString));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options => //CookieAuthenticationOptions
    {
        options.LoginPath = "/Auth/Login";
        options.AccessDeniedPath = "/Auth/Login";
        options.ExpireTimeSpan = new TimeSpan(7, 0, 0, 0);

    });

builder.Services.AddCors(options =>
{
	options.AddPolicy(name: "CORSPolicy",
		policy =>
		{
			policy.AllowAnyOrigin();
			policy.AllowAnyMethod();
			policy.AllowAnyHeader();
		});
});

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.None;
    options.Secure = CookieSecurePolicy.Always;
});

builder.Services.AddAuthorization();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseDeveloperExceptionPage();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseCors("CORSPolicy");

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseHangfireDashboard("/dashboard");

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=GetUsersTask}/{id?}");
});

BackgroundJob.Enqueue<CacheInitiator>(x => x.StartUserTasks());
BackgroundJob.Enqueue<CacheInitiator>(x => x.StartUsers());

RecurringJob.AddOrUpdate<CheckTasks>("CompareDataFromDbAndRedis",
    x => x.StartCheck(),
    Cron.Minutely);

app.Run();
