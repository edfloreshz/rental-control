using Microsoft.AspNetCore.Components;

namespace RentalControl.Components.Pages;

public partial class Home(HttpClient http) : ComponentBase
{
    private IEnumerable<Models.Contract>? Contracts { get; set; } = [];

    protected override async Task OnInitializedAsync()
    {
        Contracts = await http.GetFromJsonAsync<Models.Contract[]>("/api/v1/contracts");
    }
}