using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;
using FractalPainting.Infrastructure.Common;
using FractalPainting.Infrastructure.UiActions;
using FractalPainting.UI;
using System;
using System.Drawing;

namespace FractalPainting.App;

public class ImageSettingsAction : IUiAction
{
    private readonly ImageSettings imageSettings;
    private readonly IImageController imageController;
    private readonly Func<Window> window;
    public ImageSettingsAction(IImageController imageController, ImageSettings imageSettings, Func<Window> window)
    {
        this.imageSettings = imageSettings;
        this.imageController = imageController;
        this.window = window;
    }

    public MenuCategory Category => MenuCategory.Settings;
    public event EventHandler? CanExecuteChanged;
    public string Name => "Изображение...";

    public bool CanExecute(object? parameter)
    {
        return true;
    }

    public async void Execute(object? parameter)
    {
        await new SettingsForm(imageSettings).ShowDialog(window());
        imageController.RecreateImage(imageSettings);
    }
}

public class SaveImageAction : IUiAction
{
    private readonly Func<Window> window;
    private readonly IImageController imageController;
    public SaveImageAction(IImageController imageController, Func<Window> window)
    {
        this.imageController = imageController;
        this.window = window;
    }

    public MenuCategory Category => MenuCategory.File;
    public event EventHandler? CanExecuteChanged;
    public string Name => "Сохранить...";

    public bool CanExecute(object? parameter)
    {
        return true;
    }

    public async void Execute(object? settings)
    {
        var topLevel = TopLevel.GetTopLevel(window());
        if (topLevel is null) return;

        var options = new FilePickerSaveOptions
        {
            Title = "Сохранить изображение",
            SuggestedFileName = "image.bmp",
        };
        var saveFile = await topLevel.StorageProvider.SaveFilePickerAsync(options);
        if (saveFile is not null)
            imageController.SaveImage(saveFile.Path.AbsolutePath);
    }
}

public class PaletteSettingsAction : IUiAction
{
    private readonly Func<Window> window;
    private readonly Palette palette;
    public PaletteSettingsAction(Func<Window> window, Palette palette)
    {
        this.window = window;
        this.palette = palette;
    }

    public MenuCategory Category => MenuCategory.Settings;
    public event EventHandler? CanExecuteChanged;
    public string Name => "Палитра...";

    public bool CanExecute(object? parameter)
    {
        return true;
    }

    public async void Execute(object? parameter)
    {
        await new SettingsForm(palette).ShowDialog(window());
    }
}

public partial class MainWindow : Window
{
    private const int MenuSize = 32;
    // Контролы из авалонии
    private Menu? menu;
    private ImageControl? image;

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);

        menu = this.FindNameScope()?.Find<Menu>("Menu");
        image = this.FindNameScope()?.Find<ImageControl>("Image");
    }

    public MainWindow(IUiAction[] actions, AvaloniaImageController avaloniaImageController)
    {
        InitializeComponent();
        var imageSettings = CreateSettingsManager().Load().ImageSettings;
        ClientSize = new Size(imageSettings.Width, imageSettings.Height + MenuSize);
        menu.ItemsSource = actions.ToMenuItems();
        Title = "Fractal Painter";
        //Services.SetMainWindow(this);

        avaloniaImageController.SetControl(image);
        avaloniaImageController.RecreateImage(imageSettings);
    }

    private static SettingsManager CreateSettingsManager()
    {
        return new SettingsManager(new XmlObjectSerializer(), new FileBlobStorage());
    }
}