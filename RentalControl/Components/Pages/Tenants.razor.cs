using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Components;
using RentalControl.Components.Models;

namespace RentalControl.Components.Pages;

public partial class Tenants(HttpClient http) : ComponentBase
{
    private ObservableCollection<Tenant>? Items { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var list = await http.GetFromJsonAsync<Tenant[]>("/api/v1/tenants");
        Items = new ObservableCollection<Tenant>(list);
    }
    
    void AddItem()
    {
        Items?.Add(new Tenant());
    }
}