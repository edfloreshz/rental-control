using System.Collections.ObjectModel;
using CSharpFunctionalExtensions;
using Mapster;
using Mediator;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using RentalControl.Components.Models;

namespace RentalControl.Components.Pages;

public partial class Contracts(ISender sender, IDialogService dialogService) : ComponentBase
{
    private ObservableCollection<Contract>? Items { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var result = await sender.Send(new Endpoints.Contract.List.Query());
        result.Match(SetItems, Console.WriteLine);
    }

    private Task<IDialogReference> OpenAddDialog()
    {
        var options = new DialogOptions { CloseOnEscapeKey = true };
        return dialogService.ShowAsync<Dialogs.Contract.Add>("Add Contract", options);
    }
    
    private void SetItems(RentalControl.Models.Get.Contract[] items)
    {
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