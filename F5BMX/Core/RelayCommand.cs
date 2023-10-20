using System;
using System.Windows.Input;

namespace F5BMX.Core;

internal sealed class RelayCommand : ICommand
{

    private readonly Action execute;
    private readonly Func<bool>? canExecute;

    public event EventHandler? CanExecuteChanged
    {
        add { CommandManager.RequerySuggested += value; }
        remove { CommandManager.RequerySuggested -= value; }
    }

    public RelayCommand(Action execute) : this(execute, null) { }

    public RelayCommand(Action execute, Func<bool>? canExecute)
    {
        this.execute = execute;
        this.canExecute = canExecute;
    }

    public bool CanExecute(object? parameter)
    {
        return this.canExecute?.Invoke() != false;
    }

    public void Execute(object? parameter)
    {
        this.execute();
    }

}