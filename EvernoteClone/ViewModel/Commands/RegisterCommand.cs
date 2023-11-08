using EvernoteClone.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EvernoteClone.ViewModel.Commands
{
    public class RegisterCommand : ICommand
    {
        public LoginVM VM { get; set; }
        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public RegisterCommand(LoginVM vm) {
            VM = vm;
        }

        public bool CanExecute(object? parameter)
        {
            //Cast the parameter as user object
            User user = parameter as User;
            //If user is null, return false
            if (user == null)
                return false;
            //If email is null or empty, return false
            if (string.IsNullOrEmpty(user.Email))
                return false;
            //If password or confirm password is null or empty, return false
            if (string.IsNullOrEmpty(user.Password) || string.IsNullOrEmpty(user.ConfirmPassword))
                return false;
            //If password and confirm password aren't equals, return false
            if (!user.Password.Equals(user.ConfirmPassword))
                return false;
            //If arrive here, the username, the password and the confirm password are inserted and the password and the confirm password are equals, so the user could try the register
            return true;
        }

        public void Execute(object? parameter)
        {
            //Call the Register method from the user view model
            VM.Register();
        }
    }
}
