using System.Collections.ObjectModel;
using Mapster;
using Mediator;
using Microsoft.AspNetCore.Components;
using RentalControl.Components.Models;
using CSharpFunctionalExtensions;
using MudBlazor;

namespace RentalControl.Components.Pages;

public partial class Addresses(ISender sender, IDialogService dialogService) : ComponentBase
{
    private ObservableCollection<Address>? Items { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var result = await sender.Send(new Endpoints.Addresses.List.Query());
        result.Match(SetItems, Console.WriteLine);
    }
    
    private Task<IDialogReference> OpenAddDialog()
    {
        var options = new DialogOptions { CloseOnEscapeKey = true };
        return dialogService.ShowAsync<Dialogs.Address.Add>("Add Address", options);
    }
    
    private void SetItems(RentalControl.Models.Get.Address[] items)
    {
        Items = items.Adapt<ObservableCollection<Address>>();
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