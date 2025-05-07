using System.Collections.ObjectModel;
using CSharpFunctionalExtensions;
using Mapster;
using Mediator;
using Microsoft.AspNetCore.Components;
using RentalControl.Components.Models;

namespace RentalControl.Components.Pages;

public partial class Tenants(ISender sender) : ComponentBase
{
    private ObservableCollection<Tenant>? Items { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var result = await sender.Send(new Endpoints.Tenant.List.Query());
        result.Match(SetItems, Console.WriteLine);
    }
    
    private void SetItems(RentalControl.Models.Get.Tenant[] items)
    {
        Items = items.Adapt<ObservableCollection<Tenant>>();
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