using WorkordersNotes.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WorkordersNotes.ViewModel.Commands
{
    public class NewNoteCommand : ICommand
    {
        public NotesVM VM { get; set; }

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public NewNoteCommand(NotesVM vm)
        {
            VM = vm;
        }

        public bool CanExecute(object? parameter)
        {
            Customer selectedNotebook = parameter as Customer;
            if(selectedNotebook != null)
                return true;
            return false;
        }

        public void Execute(object? parameter)
        {
            Customer selectedNotebook = parameter as Customer;
            VM.CreateNote(selectedNotebook.Id);
        }
    }
}
