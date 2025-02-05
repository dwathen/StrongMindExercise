using Microsoft.AspNetCore.Components;
using MudBlazor;
using StrongMindExercise.Application.Errors;
using StrongMindExercise.Application.Toppings.DTOs;

namespace StrongMindExercise.WebUI.Toppings.Pages;

public partial class Toppings
{
    private List<ToppingReadDTO> toppings;

    [Inject]
    private ToppingWebService ToppingWebService { get; set; }

    [Inject]
    private IDialogService DialogService { get; set; }

    [Inject]
    private ISnackbar Snackbar { get; set; }

    protected async override Task OnInitializedAsync()
    {
        toppings = await ToppingWebService.GetToppingsAsync();
    }

    private async Task OpenCreateDialog()
    {
        var parameters = new DialogParameters { ["IsNew"] = true };
        var dialog = DialogService.Show<ToppingDialog>("Create Topping", parameters);
        var result = await dialog.Result;
        if (!result.Canceled)
        {
            Snackbar.Add("Topping created successfully!", Severity.Success);
            toppings = await ToppingWebService.GetToppingsAsync();
        }
    }

    private async Task OpenEditDialog(ToppingReadDTO topping)
    {
        var parameters = new DialogParameters
        {
            ["IsNew"] = false,
            ["Topping"] = topping
        };
        var dialog = DialogService.Show<ToppingDialog>("Edit Topping", parameters);
        var result = await dialog.Result;
        if (!result.Canceled)
        {
            Snackbar.Add("Topping updated successfully!", Severity.Success);
            toppings = await ToppingWebService.GetToppingsAsync();
        }
    }

    private async Task DeleteTopping(ToppingReadDTO topping)
    {
        bool? result = await DialogService.ShowMessageBox(
            "Confirm Delete",
            $"Are you sure you want to delete topping '{topping.Name}'?",
            yesText: "Yes", cancelText: "Cancel");

        if (result == true)
        {
            var response = await ToppingWebService.DeleteToppingAsync(topping.Id);
            if (response == Error.None)
            {
                Snackbar.Add("Topping deleted successfully!", Severity.Success);
                toppings.Remove(topping);
                StateHasChanged();
            }
            else
            {
                Snackbar.Add(response.Description, Severity.Error);
            }
        }
    }
}
