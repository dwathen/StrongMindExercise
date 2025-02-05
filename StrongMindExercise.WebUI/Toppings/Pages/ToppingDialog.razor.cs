using Microsoft.AspNetCore.Components;
using MudBlazor;
using StrongMindExercise.Application.Errors;
using StrongMindExercise.Application.Toppings.DTOs;

namespace StrongMindExercise.WebUI.Toppings.Pages;

public partial class ToppingDialog
{
    [Parameter] 
    public bool IsNew { get; set; }
    [Parameter] 
    public ToppingReadDTO Topping { get; set; } = new();

    private string toppingName;

    [Inject]
    private ISnackbar Snackbar { get; set; }

    [Inject]
    private ToppingWebService ToppingWebService { get; set; }

    [CascadingParameter]
    private IMudDialogInstance Dialog { get; set; }

    protected override void OnInitialized()
    {
        if (!IsNew && Topping != null)
        {
            toppingName = Topping.Name;
        }
    }

    private async Task Save()
    {
        if (string.IsNullOrWhiteSpace(toppingName))
        {
            Snackbar.Add("Topping name is required.", Severity.Error);
            return;
        }

        if (IsNew)
        {
            var createDTO = new ToppingCreateDTO { Name = toppingName };
            var response = await ToppingWebService.CreateToppingAsync(createDTO);

            HandleResponse(response);
        }
        else
        {
            var updateDTO = new ToppingUpdateDTO { Id = Topping.Id, Name = toppingName };
            var response = await ToppingWebService.UpdateToppingAsync(updateDTO);

            HandleResponse(response);
        }
    }

    private void HandleResponse(Error response)
    {
        if (response == Error.None)
        {
            Dialog.Close(DialogResult.Ok(true));
        }
        else
        {
            Snackbar.Add(response.Description, Severity.Error);
        }
    }

    private void Cancel() => Dialog.Cancel();
}
