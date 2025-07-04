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

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

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

app.UseCors("AllowFrontend");

// Add global OPTIONS handler for preflight requests
app.Use(async (context, next) =>
{
    if (context.Request.Method == "OPTIONS")
    {
        context.Response.StatusCode = 204;
        return;
    }
    await next();
});

app.UseHttpsRedirection();

app.MapOpenApi();
app.MapScalarApiReference();

app.MapCarter();

app.Run();
