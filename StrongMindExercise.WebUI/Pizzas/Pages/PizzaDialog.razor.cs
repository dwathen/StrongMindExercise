using Microsoft.AspNetCore.Components;
using MudBlazor;
using StrongMindExercise.Application.Pizzas.DTOs;
using StrongMindExercise.Application.Errors;
using StrongMindExercise.Application.Toppings.DTOs;
using StrongMindExercise.WebUI.Toppings;

namespace StrongMindExercise.WebUI.Pizzas.Pages;

public partial class PizzaDialog
{
    [Parameter] 
    public bool IsNew { get; set; }
    [Parameter] 
    public PizzaReadDTO Pizza { get; set; } = new();

    private List<ToppingReadDTO> Toppings { get; set; }
    private IEnumerable<string> SelectedToppings { get; set; } = new HashSet<string>();
    private string pizzaName;
    private string SelectedTopping { get; set; } = "Nothing Selected";

    [Inject]
    private ISnackbar Snackbar { get; set; }

    [Inject]
    private PizzaWebService PizzaWebService { get; set; }

    [Inject]
    private ToppingWebService ToppingWebService { get; set; }

    [CascadingParameter]
    private IMudDialogInstance Dialog { get; set; }

    protected override async Task OnInitializedAsync()
    {
        Toppings = await ToppingWebService.GetToppingsAsync();

        if (!IsNew && Pizza != null)
        {
            pizzaName = Pizza.Name;
            SelectedToppings = Pizza.Toppings.Select(t => t.Name).ToHashSet();
        }
    }

    private async Task Save()
    {
        if (string.IsNullOrWhiteSpace(pizzaName))
        {
            Snackbar.Add("Pizza name is required.", Severity.Error);
            return;
        }

        if (IsNew)
        {
            var createDTO = new PizzaCreateDTO { Name = pizzaName, ToppingIds = GetToppingIds() };
            var response = await PizzaWebService.CreatePizzaAsync(createDTO);

            HandleResponse(response);
        }
        else
        {
            var updateDTO = new PizzaUpdateDTO { Id = Pizza.Id, Name = pizzaName, ToppingIds = GetToppingIds() };
            var response = await PizzaWebService.UpdatePizzaAsync(updateDTO);

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

    private List<int> GetToppingIds()
    {
        List<int> toppingIds = new();

        foreach (var topping in SelectedToppings)
        {
            var toppingId = Toppings.FirstOrDefault(t => t.Name == topping)?.Id;
            if (toppingId.HasValue)
            {
                toppingIds.Add(toppingId.Value);
            }
        }

        return toppingIds;
    }

    private void Cancel() => Dialog.Cancel();
}
