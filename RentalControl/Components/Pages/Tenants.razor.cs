using Microsoft.AspNetCore.Components;

namespace RentalControl.Components.Pages;

public partial class Tenants(HttpClient http) : ComponentBase
{
    private IEnumerable<Models.Tenant>? TenantList { get; set; }

    protected override async Task OnInitializedAsync()
    {
        TenantList = await http.GetFromJsonAsync<Models.Tenant[]>("/api/v1/tenants");
    }
}