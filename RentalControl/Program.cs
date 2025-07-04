using Carter;
using QuestPDF.Infrastructure;
using RentalControl.Services;
using Scalar.AspNetCore;
using Supabase.Postgrest;

var builder = WebApplication.CreateBuilder(args);

QuestPDF.Settings.License = LicenseType.Community;

builder.Services.AddOpenApi();
builder.Services.AddMediator();
builder.Services.AddCarter();

builder.Services.AddSingleton(new Client(builder.Configuration["Postgrest:Url"]!));
builder.Services.AddSingleton<TenantService>();
builder.Services.AddSingleton<ContractService>();
builder.Services.AddSingleton<GuarantorService>();
builder.Services.AddSingleton<AddressService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

app.MapOpenApi();
app.MapScalarApiReference();


app.MapCarter();

app.Run();
