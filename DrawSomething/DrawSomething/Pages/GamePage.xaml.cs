using DrawSomething.ViewModels;

namespace DrawSomething.Pages;

public partial class GamePage : ContentPage
{
	public GamePage(GamePageViewModel gamePageViewModel)
	{
		InitializeComponent();

		BindingContext = gamePageViewModel;
	}
}
