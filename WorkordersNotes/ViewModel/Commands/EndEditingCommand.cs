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
            if(parameter.GetType() == typeof(Customer)) {
                //Cast the parameter as Customer object
                Customer customer = parameter as Customer;
                //If the customer retrieved isn't null
                if (customer != null)
                {
                    //Call the stop editing method of VM, updating the casted customer
                    VM.StopEditing(customer);
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
