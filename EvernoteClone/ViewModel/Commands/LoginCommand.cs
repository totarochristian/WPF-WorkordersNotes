using EvernoteClone.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EvernoteClone.ViewModel.Commands
{
    public class LoginCommand : ICommand
    {
        public LoginVM VM { get; set; }
        public event EventHandler? CanExecuteChanged;

        public LoginCommand(LoginVM vm)
        {
            VM = vm;
        }

        public bool CanExecute(object? parameter)
        {
            //Cast the parameter as user object
            User user = parameter as User;
            //If user is null, return false
            if (user == null)
                return false;
            //If username is null or empty, return false
            if(string.IsNullOrEmpty(user.Username))
                return false;
            //If password is null or empty, return false
            if (string.IsNullOrEmpty(user.Password))
                return false;
            //If arrive here, the username and the password are inserted and so the user could try the login
            return true;
        }

        public void Execute(object? parameter)
        {
            //Call the Login method from the user view model
            VM.Login();
        }
    }
}
