using Microsoft.AspNetCore.Components;

namespace StrongMindExercise.WebUI.Components.Pages;

public partial class Home
{
    [Inject]
    private NavigationManager NavigationManager { get; set; }

    public void NavigateToToppings()
    {
        NavigationManager.NavigateTo("/Toppings");
    }

    public void NavigateToPizzas()
    {
        NavigationManager.NavigateTo("/Pizzas");
    }
}
