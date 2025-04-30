using Microsoft.AspNetCore.Components;

namespace RentalControl.Components.Pages;

public partial class Contracts(HttpClient http) : ComponentBase
{
    private IEnumerable<Models.Contract>? ContractsList { get; set; }

    protected override async Task OnInitializedAsync()
    {
        ContractsList = await http.GetFromJsonAsync<Models.Contract[]>("/api/v1/contracts");
    }
}