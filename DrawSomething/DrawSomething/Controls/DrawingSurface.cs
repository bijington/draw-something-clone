using System.Windows.Input;

namespace DrawSomething.Controls;

public class DrawingSurface : GraphicsView, IDrawable
{
    private DrawingPath currentPath;
    private readonly IList<DrawingPath> paths = new List<DrawingPath>();

    public DrawingSurface()
	{
        Drawable = this;

        DragInteraction += DrawingSurface_DragInteraction;
        StartInteraction += DrawingSurface_StartInteraction;
        EndInteraction += DrawingSurface_EndInteraction;
	}

    public static readonly BindableProperty LineThicknessProperty =
        BindableProperty.Create(
            nameof(LineThickness),
            typeof(float),
            typeof(DrawingSurface),
            10f);

    public float LineThickness
    {
        get => (float)GetValue(LineThicknessProperty);
        set => SetValue(LineThicknessProperty, value);
    }

    public static readonly BindableProperty DrawingColorProperty =
        BindableProperty.Create(
            nameof(DrawingColor),
            typeof(Color),
            typeof(DrawingSurface),
            Colors.Black);

    public Color DrawingColor
    {
        get => (Color)GetValue(DrawingColorProperty);
        set => SetValue(DrawingColorProperty, value);
    }

    public static readonly BindableProperty ClearCommandProperty =
        BindableProperty.CreateReadOnly(
            nameof(ClearCommand),
            typeof(ICommand),
            typeof(DrawingSurface),
            default,
            BindingMode.OneWayToSource,
            defaultValueCreator: CreateClearCommand).BindableProperty;

    public ICommand ClearCommand => (ICommand)GetValue(ClearCommandProperty);

    static object CreateClearCommand(BindableObject bindable)
        => new Command(() => ((DrawingSurface)bindable).Clear());

    public static readonly BindableProperty UndoCommandProperty =
        BindableProperty.CreateReadOnly(
            nameof(UndoCommand),
            typeof(ICommand),
            typeof(DrawingSurface),
            default,
            BindingMode.OneWayToSource,
            defaultValueCreator: CreateUndoCommand).BindableProperty;

    public ICommand UndoCommand => (ICommand)GetValue(UndoCommandProperty);

    static object CreateUndoCommand(BindableObject bindable)
        => new Command(() => ((DrawingSurface)bindable).Undo());

    private void DrawingSurface_DragInteraction(object sender, TouchEventArgs e)
    {
        currentPath.Add(e.Touches.First());

        Invalidate();
    }

    private void DrawingSurface_EndInteraction(object sender, TouchEventArgs e)
    {
        currentPath.Add(e.Touches.First());

        Invalidate();
    }

    private void DrawingSurface_StartInteraction(object sender, TouchEventArgs e)
    {
        currentPath = new DrawingPath(DrawingColor, LineThickness);
        currentPath.Add(e.Touches.First());
        paths.Add(currentPath);

        Invalidate();
    }

    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        foreach (var path in paths)
        {
            DrawPath(path, canvas);
        }
    }

    private void Clear()
    {
        paths.Clear();

        Invalidate();
    }

    private void Undo()
    {
        if (paths.Any() is false)
        {
            return;
        }

        paths.RemoveAt(paths.Count - 1);

        Invalidate();
    }

    private void DrawPath(DrawingPath path, ICanvas canvas)
    {
        canvas.StrokeColor = path.Color;
        canvas.StrokeSize = path.Thickness;
        canvas.StrokeLineCap = LineCap.Round;
        canvas.DrawPath(path.Path);
    }
}
