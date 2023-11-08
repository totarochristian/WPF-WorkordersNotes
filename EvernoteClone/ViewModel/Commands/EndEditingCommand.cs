using EvernoteClone.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EvernoteClone.ViewModel.Commands
{
    public class EndEditingCommand : ICommand
    {
        public NotesVM VM { get; set; }

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public EndEditingCommand(NotesVM vm) {
            VM = vm;
        }

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            //Cast the parameter as Notebook object
            Notebook notebook = parameter as Notebook;
            //If the notebook retrieve isn't null
            if(notebook != null)
            {
                //Call the stop editing method of VM, updating the casted notebook
                VM.StopEditing(notebook);
            }
        }
    }
}
