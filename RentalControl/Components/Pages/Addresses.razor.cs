using Microsoft.AspNetCore.Components;

namespace RentalControl.Components.Pages;

public partial class Addresses(HttpClient http) : ComponentBase
{
    private IEnumerable<Models.Address>? AddressesList { get; set; }

    protected override async Task OnInitializedAsync()
    {
        AddressesList = await http.GetFromJsonAsync<Models.Address[]>("/api/v1/addresses");
    }
}