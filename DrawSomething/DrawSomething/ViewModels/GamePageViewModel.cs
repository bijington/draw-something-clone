namespace DrawSomething.ViewModels;

public class GamePageViewModel
{
    public GamePageViewModel()
    {
        AvailableColors = new List<Color>
        {
            Colors.Black,
            Colors.Red,
            Colors.Orange,
            Colors.Yellow,
            Colors.Green,
            Colors.Blue,
            Colors.Indigo,
            Colors.Violet,
            Colors.White
        };
    }

    public string Subject { get; } = "GLOBE";

    public IList<Color> AvailableColors { get; }
}