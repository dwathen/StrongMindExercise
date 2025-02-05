using Microsoft.AspNetCore.Components;
using MudBlazor;
using StrongMindExercise.Application.Pizzas.DTOs;
using StrongMindExercise.Application.Errors;

namespace StrongMindExercise.WebUI.Pizzas.Pages;

public partial class Pizzas
{
    private List<PizzaReadDTO> pizzas;

    [Inject]
    private PizzaWebService PizzaWebService { get; set; }

    [Inject]
    private IDialogService DialogService { get; set; }

    [Inject]
    private ISnackbar Snackbar { get; set; }

    protected async override Task OnInitializedAsync()
    {
        pizzas = await PizzaWebService.GetPizzasAsync();
    }

    private async Task OpenCreateDialog()
    {
        var parameters = new DialogParameters { ["IsNew"] = true };
        var dialog = DialogService.Show<PizzaDialog>("Create Pizza", parameters);
        var result = await dialog.Result;
        if (!result.Canceled)
        {
            Snackbar.Add("Pizza created successfully!", Severity.Success);
            pizzas = await PizzaWebService.GetPizzasAsync();
        }
    }

    private async Task OpenEditDialog(PizzaReadDTO pizza)
    {
        var parameters = new DialogParameters
        {
            ["IsNew"] = false,
            ["Pizza"] = pizza
        };
        var dialog = DialogService.Show<PizzaDialog>("Edit Pizza", parameters);
        var result = await dialog.Result;
        if (!result.Canceled)
        {
            Snackbar.Add("Pizza updated successfully!", Severity.Success);
            pizzas = await PizzaWebService.GetPizzasAsync();
        }
    }

    private async Task DeletePizza(PizzaReadDTO pizza)
    {
        bool? result = await DialogService.ShowMessageBox(
            "Confirm Delete",
            $"Are you sure you want to delete pizza '{pizza.Name}'?",
            yesText: "Yes", cancelText: "Cancel");

        if (result == true)
        {
            var response = await PizzaWebService.DeletePizzaAsync(pizza.Id);
            if (response == Error.None)
            {
                Snackbar.Add("Pizza deleted successfully!", Severity.Success);
                pizzas.Remove(pizza);
                StateHasChanged();
            }
            else
            {
                Snackbar.Add(response.Description, Severity.Error);
            }
        }
    }
}
