using Store.Components;
using Store.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<ProductService>();
builder.Services.AddHttpClient<ProductService>(c =>
{
    var url = builder.Configuration["ProductEndpoint"] ?? throw new InvalidOperationException("ProductEndpoint is not set");

    c.BaseAddress = new(url);
});

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.SetMinimumLevel(LogLevel.Debug);

var app = builder.Build();

// var logger = app.Services.GetRequiredService<ILoggerFactory>()
//     .CreateLogger<Program>();

var configuration = app.Services.GetRequiredService<IConfiguration>();
Console.WriteLine("toto");

app.Logger.LogInformation($"Logged after the app is built in the Program file.");

var imagePrefix = configuration["ImagePrefix"];

app.Logger.LogInformation("Configuration ImagePrefix = " + imagePrefix);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
