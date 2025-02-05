using Microsoft.AspNetCore.Components;

namespace StrongMindExercise.WebUI.Components.Layout;

public partial class MainLayout
{
    [Inject]
    private NavigationManager NavigationManager { get; set; }

    private void NavigateToHome()
    {
        NavigationManager.NavigateTo("/");
    }

    private void NavigateToGitHub()
    {
        NavigationManager.NavigateTo("https://github.com/dwathen/StrongMindExercise");
    }
}
