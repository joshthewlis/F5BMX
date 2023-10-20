using System;
using System.Windows.Input;

namespace F5BMX.Core;

internal sealed class RelayCommand<T> : ICommand
{

    private readonly Action<T> execute;
    private readonly Func<bool>? canExecute;

    public event EventHandler? CanExecuteChanged
    {
        add { CommandManager.RequerySuggested += value; }
        remove { CommandManager.RequerySuggested -= value; }
    }

    public RelayCommand(Action<T> execute) : this(execute, null) { }

    public RelayCommand(Action<T> execute, Func<bool>? canExecute)
    {
        this.execute = execute;
        this.canExecute = canExecute;
    }

    public bool CanExecute(object? parameter)
    {
        return this.canExecute?.Invoke() != false;
    }

    public void Execute(T parameter)
    {
        this.execute(parameter);
    }

    public void Execute(object? parameter)
    {
        // FIX : Better Exception
        if (!TryGetCommandArgument(parameter, out T? result))
            throw new ArgumentException("Invalid Parameter");

        this.execute((T)parameter);
    }

    internal bool TryGetCommandArgument(object? parameter, out T? result)
    {
        if (parameter is null && default(T) == null)
        {
            result = default;
            return true;
        }

        if (parameter is T argument)
        {
            result = argument;
            return true;
        }

        result = default;
        return false;
    }

}