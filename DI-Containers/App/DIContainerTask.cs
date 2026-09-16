using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using FluentAssertions;
using FractalPainting.App.Fractals;
using FractalPainting.Infrastructure.Common;
using FractalPainting.Infrastructure.UiActions;
using Ninject;
using Ninject.Extensions.Factory;
using System;
using System.Drawing;
using System.Linq;
using SettingsForm = FractalPainting.UI.SettingsForm;

namespace FractalPainting.App;

public static class DIContainerTask
{
    public static MainWindow CreateMainWindow()
    {
        return ConfigureContainer().Get<MainWindow>();
    }

    public static StandardKernel ConfigureContainer()
    {
        var container = new StandardKernel();
        container.Bind<IImageController, AvaloniaImageController>().To<AvaloniaImageController>().InSingletonScope();
        container.Bind<Palette>().ToSelf().InSingletonScope();
        ConfigureSettings(container);
        ConfigureActions(container);
        container.Bind<Window>().To<MainWindow>().InSingletonScope();
        container.Bind<MainWindow>().ToSelf().InSingletonScope();
        container.Bind<Func<Window>>().ToMethod(c => () => c.Kernel.Get<MainWindow>());
        container.Bind<IDragonPainterFactory>().ToFactory();
        return container;
    }

    public static void ConfigureActions(StandardKernel container)
    {
        container.Bind<IUiAction>().To<SaveImageAction>();
        container.Bind<IUiAction>().To<DragonFractalAction>();
        container.Bind<IUiAction>().To<KochFractalAction>();
        container.Bind<IUiAction>().To<ImageSettingsAction>();
        container.Bind<IUiAction>().To<PaletteSettingsAction>();
    }

    public static void ConfigureSettings(StandardKernel container)
    {
        container.Bind<IBlobStorage>().To<FileBlobStorage>();
        container.Bind<IObjectSerializer>().To<XmlObjectSerializer>();
        container.Bind<AppSettings>().ToMethod(c => c.Kernel.Get<SettingsManager>().Load()).InSingletonScope();
        container.Bind<ImageSettings>().ToMethod(c => c.Kernel.Get<AppSettings>().ImageSettings).InSingletonScope();
    }
}

public interface IDragonPainterFactory
{
    DragonPainter CreateDragonPainter(DragonSettings settings);
}

public class DragonFractalAction : IUiAction
{
    public MenuCategory Category => MenuCategory.Fractals;
    public string Name => "Дракон";
    public event EventHandler? CanExecuteChanged;
    private readonly Func<Window> window;
    private readonly IDragonPainterFactory factory;

    public DragonFractalAction(IDragonPainterFactory factory, Func<Window> window)
    {
        this.window = window;
        this.factory = factory;
    }

    public bool CanExecute(object? parameter)
    {
        return true;
    }

    public async void Execute(object? parameter)
    {
        var dragonSettings = CreateRandomSettings();
        await new SettingsForm(dragonSettings).ShowDialog(window());
        var painter = factory.CreateDragonPainter(dragonSettings);
        painter.Paint();
    }

    private static DragonSettings CreateRandomSettings()
    {
        return new DragonSettingsGenerator(new Random()).Generate();
    }
}

public class KochFractalAction : IUiAction
{
    private readonly Lazy<KochPainter> painter;
    public MenuCategory Category => MenuCategory.Fractals;
    public string Name => "Кривая Коха";
    public event EventHandler? CanExecuteChanged;

    public KochFractalAction(Lazy<KochPainter> painter)
    {
        this.painter = painter;
    }

    public bool CanExecute(object? parameter)
    {
        return true;
    }

    public void Execute(object? parameter)
    {
        painter.Value.Paint();
    }
}

public class DragonPainter
{
    private readonly IImageController imageController;
    private readonly DragonSettings settings;
    private readonly Palette palette;

    public DragonPainter(IImageController imageController, DragonSettings settings, Palette palette)
    {
        this.imageController = imageController;
        this.settings = settings;
        this.palette = palette;
    }

    public void Paint()
    {
        using var ctx = imageController.CreateDrawingContext();
        var imageSize = imageController.GetImageSize();
        var size = Math.Min(imageSize.Width, imageSize.Height) / 2.1f;
        ctx.FillRectangle(new SolidColorBrush(palette.BackgroundColor),
            new Rect(0, 0, imageSize.Width, imageSize.Height));
        var r = new Random();
        var cosa = (float)Math.Cos(settings.Angle1);
        var sina = (float)Math.Sin(settings.Angle1);
        var cosb = (float)Math.Cos(settings.Angle2);
        var sinb = (float)Math.Sin(settings.Angle2);
        var shiftX = settings.ShiftX * size * 0.8f;
        var shiftY = settings.ShiftY * size * 0.8f;
        var scale = settings.Scale;
        var p = new Point(0, 0);
        foreach (var i in Enumerable.Range(0, settings.IterationsCount))
        {
            ctx.FillRectangle(new SolidColorBrush(palette.BackgroundColor),
                new Rect(imageSize.Width / 3f + p.X, imageSize.Height / 2f + p.Y, 1, 1));

            if (r.Next(0, 2) == 0)
                p = new Point(scale * (p.X * cosa - p.Y * sina), scale * (p.X * sina + p.Y * cosa));
            else
                p = new Point(scale * (p.X * cosb - p.Y * sinb) + (float)shiftX,
                    scale * (p.X * sinb + p.Y * cosb) + (float)shiftY);
        }
        imageController.UpdateUi();
    }
}