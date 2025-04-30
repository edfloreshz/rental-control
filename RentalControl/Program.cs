using Carter;
using Radzen;
using RentalControl.Components;
using Supabase.Postgrest;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddMediator();
builder.Services.AddCarter();
builder.Services.AddRadzenComponents();
builder.Services.AddScoped(_ => new HttpClient { BaseAddress = new Uri(builder.Configuration["Api:Url"]!) });
builder.Services.AddSingleton(new Client(builder.Configuration["Postgrest:Url"]!));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapCarter();

app.Run();