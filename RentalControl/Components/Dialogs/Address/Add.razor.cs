using System.Collections.ObjectModel;
using CSharpFunctionalExtensions;
using Mapster;
using Mediator;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using RentalControl.Components.Models;

namespace RentalControl.Components.Dialogs.Address;

public partial class Add(ISender sender) : ComponentBase
{
    [CascadingParameter]
    public required IMudDialogInstance MudDialog { get; set; }
    private MudForm Form { get; set; } = new();
    private Models.Address Model { get; set; } = new();
    private bool Success { get; set; }
    private string[] Errors { get; set; } = [];
  
    private async Task Submit()
    {
        if (!Form.IsValid) return;

        var model = Model.Adapt<RentalControl.Models.Create.Address>();
        var result = await sender.Send(new Endpoints.Addresses.Add.Command(model));
        if (result.IsSuccess)
        {
            Success = true;
            MudDialog.Close(DialogResult.Ok(result.Value));
        }
        else
        {
            Errors = result.Error.Split(Environment.NewLine);
        }
    }

    private void Cancel() => MudDialog.Cancel();
}