using WorkordersNotes.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WorkordersNotes.ViewModel.Commands
{
    public class DeleteCustomerCommand : ICommand
    {
        public NotesVM VM { get; set; }

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public DeleteCustomerCommand(NotesVM vm)
        {
            VM = vm;
        }

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            //Cast the parameter as Customer object
            Customer customer = parameter as Customer;
            //If the customer retrieved isn't null
            if (customer != null)
            {
                //Call the delete method of VM, deleting the casted customer
                VM.DeleteNotebook(customer);
            }
        }
    }
}
