﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WorkordersNotes.ViewModel.Commands
{
    public class NewCustomerCommand : ICommand
    {
        public NotesVM VM { get; set; }

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public NewCustomerCommand(NotesVM vm)
        {
            VM = vm;
        }

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            VM.CreateNotebook();
        }
    }
}
