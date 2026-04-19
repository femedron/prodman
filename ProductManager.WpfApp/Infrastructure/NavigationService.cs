using System.Windows.Controls;

namespace ProductManager.WpfApp.Infrastructure;

/// <summary>
/// Abstraction for in-app navigation.
/// ViewModels depend on this interface, never on the concrete WPF Frame —
/// keeps ViewModels testable and platform-agnostic.
/// </summary>
public interface INavigationService
{
    void NavigateTo(Page page);
    void GoBack();
    bool CanGoBack { get; }
}

//public interface INavigatedTo
//{
//    Task OnNavigatedTo();
//}

/// <summary>
/// Concrete WPF implementation backed by a <see cref="Frame"/> control.
/// Registered as singleton in the IoC container.
/// The Frame reference is set from MainWindow once the window is initialised.
/// </summary>
public class NavigationService : INavigationService
{
    private Frame? _frame;

    /// <summary>
    /// Called by MainWindow to inject the hosting Frame.
    /// Done here (not in constructor) because the Frame exists only after
    /// the XAML tree is built.
    /// </summary>
    public void SetFrame(Frame frame) => _frame = frame;

    public bool CanGoBack => _frame?.CanGoBack ?? false;

    public void NavigateTo(Page page)
    {
        if (_frame is null)
            throw new InvalidOperationException("NavigationService frame is not set.");
        _frame.Navigate(page);

        //if (page.DataContext is INavigatedTo vm)
        //{
        //    _ = vm.OnNavigatedTo();
        //}
    }

    public void GoBack()
    {
        if (_frame?.CanGoBack == true)
            _frame.GoBack();
    }
}
