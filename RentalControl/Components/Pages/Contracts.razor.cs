using Microsoft.AspNetCore.Components;

namespace RentalControl.Components.Pages;

public partial class Contracts(HttpClient http) : ComponentBase
{
    private IEnumerable<Models.Contract>? ContractList { get; set; } = [];
    private IList<Models.Contract> _selectedContracts = [];

    protected override async Task OnInitializedAsync()
    {
        ContractList = await http.GetFromJsonAsync<Models.Contract[]>("/api/v1/contracts");
    }
}