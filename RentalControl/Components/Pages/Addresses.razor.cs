using System.Collections.ObjectModel;
using Mapster;
using Mediator;
using Microsoft.AspNetCore.Components;
using RentalControl.Components.Models;
using RentalControl.Services.Addresses;

namespace RentalControl.Components.Pages;

public partial class Addresses(ISender sender) : ComponentBase
{
    private ObservableCollection<Address>? Items { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var list = await sender.Send(new List.GetAddressQuery());
        Items = list.Adapt<ObservableCollection<Address>>();
    }

    private void AddItem()
    {
        Items?.Add(new Address());
    }
    
    private void RemoveItem(Address? item)
    {
        if (item is null) return;
        Items?.Remove(item);
    }
}