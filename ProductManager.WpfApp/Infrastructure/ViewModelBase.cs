using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ProductManager.WpfApp.Infrastructure;

/// <summary>
/// Base class for all WPF ViewModels.
/// Implements <see cref="INotifyPropertyChanged"/> and exposes a helper
/// <see cref="SetProperty{T}"/> method that only raises the event when
/// the value actually changes.
/// </summary>
public abstract class ViewModelBase : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    /// Sets <paramref name="field"/> to <paramref name="value"/> and raises
    /// <see cref="PropertyChanged"/> if the value changed.
    /// </summary>
    /// <returns>True if the value changed and the event was raised.</returns>
    protected bool SetProperty<T>(
        ref T field,
        T value,
        [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value))
            return false;

        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
