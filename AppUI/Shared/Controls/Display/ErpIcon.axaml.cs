using AppUI.Core.Icons;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;

namespace AppUI.Shared.Controls.Display;

public partial class ErpIcon : UserControl
{
    public static readonly StyledProperty<LucideIconKind> KindProperty =
        AvaloniaProperty.Register<ErpIcon, LucideIconKind>(nameof(Kind), LucideIconKind.Info);

    public static readonly StyledProperty<double> SizeProperty =
        AvaloniaProperty.Register<ErpIcon, double>(nameof(Size), 16.0);

    public static readonly StyledProperty<double> StrokeThicknessProperty =
        AvaloniaProperty.Register<ErpIcon, double>(nameof(StrokeThickness), 2.0);

    public static readonly StyledProperty<Geometry?> GeometryProperty =
        AvaloniaProperty.Register<ErpIcon, Geometry?>(nameof(Geometry));

    static ErpIcon()
    {
        KindProperty.Changed.AddClassHandler<ErpIcon>((icon, _) => icon.UpdateGeometry());
    }

    public ErpIcon()
    {
        InitializeComponent();
        UpdateGeometry();
    }

    public LucideIconKind Kind
    {
        get => GetValue(KindProperty);
        set => SetValue(KindProperty, value);
    }

    public double Size
    {
        get => GetValue(SizeProperty);
        set => SetValue(SizeProperty, value);
    }

    public double StrokeThickness
    {
        get => GetValue(StrokeThicknessProperty);
        set => SetValue(StrokeThicknessProperty, value);
    }

    public Geometry? Geometry
    {
        get => GetValue(GeometryProperty);
        private set => SetValue(GeometryProperty, value);
    }

    private void UpdateGeometry()
    {
        Geometry = LucideIconProvider.GetGeometry(Kind);
    }
}
