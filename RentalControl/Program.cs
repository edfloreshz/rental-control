using Carter;
using MudBlazor.Services;
using QuestPDF.Infrastructure;
using RentalControl.Components;
using RentalControl.Services;
using Scalar.AspNetCore;
using Supabase.Postgrest;

var builder = WebApplication.CreateBuilder(args);

QuestPDF.Settings.License = LicenseType.Community;

// Add services to the container.
builder.Services.AddOpenApi();
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddMediator();
builder.Services.AddCarter();
builder.Services.AddMudServices();

builder.Services.AddSingleton(new Client(builder.Configuration["Postgrest:Url"]!));
builder.Services.AddSingleton<TenantService>();
builder.Services.AddSingleton<ContractService>();
builder.Services.AddSingleton<GuarantorService>();
builder.Services.AddSingleton<AddressService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.MapOpenApi();
app.MapScalarApiReference();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapCarter();

app.Run();
