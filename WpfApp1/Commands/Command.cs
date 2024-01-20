﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ForecastDesign.Commands
{
    
    public class Command : ICommand
    {
        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        public Command(Action<object> action, Predicate<object> predicat=null)
        {
            this.action = action;
            this.predicat = predicat;
            if (this.predicat == null)
                this.predicat = (obj) => true;
        }


        public Action<object>? action { get; set; }
        public Predicate<object>? predicat { get; set; }

        public bool CanExecute(object? parameter)
        {
            return predicat?.Invoke(parameter!) ?? false;
        }

        public void Execute(object? parameter)
        {
            action?.Invoke(parameter!);
        }
        
    }
}
