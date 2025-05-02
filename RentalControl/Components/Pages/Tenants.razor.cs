using System.Collections.ObjectModel;
using Mapster;
using Mediator;
using Microsoft.AspNetCore.Components;
using RentalControl.Components.Models;
using RentalControl.Services.Tenant;

namespace RentalControl.Components.Pages;

public partial class Tenants(ISender sender) : ComponentBase
{
    private ObservableCollection<Tenant>? Items { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var list = await sender.Send(new List.GetTenantsQuery());
        Items = list.Adapt<ObservableCollection<Tenant>>();
    }

    private void AddItem()
    {
        Items?.Add(new Tenant());
    }
    
    private void RemoveItem(Tenant? item)
    {
        if (item is null) return;
        Items?.Remove(item);
    }
}