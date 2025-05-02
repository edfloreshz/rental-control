using System.Collections.ObjectModel;
using Mapster;
using Mediator;
using Microsoft.AspNetCore.Components;
using RentalControl.Components.Models;

namespace RentalControl.Components.Pages;

public partial class Contracts(ISender sender) : ComponentBase
{
    private ObservableCollection<Contract>? Items { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var items = await sender.Send(new Endpoints.Contract.List.Query());
        Items = items.Adapt<ObservableCollection<Contract>>();
    }

    private void AddItem()
    {
        Items?.Add(new Contract());
    }
    
    private void RemoveItem(Contract? item)
    {
        if (item is null) return;
        Items?.Remove(item);
    }
}