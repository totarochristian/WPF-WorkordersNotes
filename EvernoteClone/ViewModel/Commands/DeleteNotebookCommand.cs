using WorkordersNotes.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WorkordersNotes.ViewModel.Commands
{
    public class DeleteNotebookCommand : ICommand
    {
        public NotesVM VM { get; set; }

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public DeleteNotebookCommand(NotesVM vm)
        {
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
            //If the notebook retrieved isn't null
            if (notebook != null)
            {
                //Call the delete method of VM, deleting the casted notebook
                VM.DeleteNotebook(notebook);
            }
        }
    }
}
