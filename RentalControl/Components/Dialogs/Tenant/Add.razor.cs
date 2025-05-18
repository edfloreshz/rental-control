using System.Collections.ObjectModel;
using CSharpFunctionalExtensions;
using Mapster;
using Mediator;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using RentalControl.Components.Models;

namespace RentalControl.Components.Dialogs.Tenant;

public partial class Add(ISender sender, IDialogService dialogService) : ComponentBase
{
    [CascadingParameter]
    public required IMudDialogInstance MudDialog { get; set; }
    private MudForm Form { get; set; } = new();
    private Models.Tenant Model { get; set; } = new();
    private Models.Address[] Addresses { get; set; } = [];
    private bool Success { get; set; }
    private string[] Errors { get; set; } = [];
    
    protected override async Task OnInitializedAsync()
    {
        var addressesResult = await sender.Send(new Endpoints.Addresses.List.Query());
        
        addressesResult.Match(SetAddresses, Console.WriteLine);
    }
  
    private void SetAddresses(RentalControl.Models.Get.Address[] items)
    {
        Addresses = items.Adapt<Models.Address[]>();
    }
    
    private void Submit() => MudDialog.Close(DialogResult.Ok(true));

    private void Cancel() => MudDialog.Cancel();
    
    private Task OpenAddAddressDialog()
    {
        return dialogService.ShowAsync<Dialogs.Address.Add>("Agregar Dirección");
    }
}