using WorkordersNotes.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WorkordersNotes.ViewModel.Commands
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
            if(parameter.GetType() == typeof(Notebook)) {
                //Cast the parameter as Notebook object
                Notebook notebook = parameter as Notebook;
                //If the notebook retrieved isn't null
                if (notebook != null)
                {
                    //Call the stop editing method of VM, updating the casted notebook
                    VM.StopEditing(notebook);
                }
            }
            else if(parameter.GetType() == typeof(Note)) {
                //Cast the parameter as Note object
                Note note = parameter as Note;
                //If the note retrieved isn't null
                if (note != null)
                {
                    //Call the stop editing method of VM, updating the casted note
                    VM.StopEditing(note);
                }
            }
        }
    }
}
