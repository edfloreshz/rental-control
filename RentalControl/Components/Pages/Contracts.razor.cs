using System.Collections.ObjectModel;
using Mapster;
using Mediator;
using Microsoft.AspNetCore.Components;
using RentalControl.Components.Models;
using RentalControl.Services.Contract;

namespace RentalControl.Components.Pages;

public partial class Contracts(ISender sender) : ComponentBase
{
    private ObservableCollection<Contract>? Items { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var list = await sender.Send(new List.GetContractsQuery());
        Items = list.Adapt<ObservableCollection<Contract>>();
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