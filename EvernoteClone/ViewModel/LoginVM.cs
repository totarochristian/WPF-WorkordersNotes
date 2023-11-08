using EvernoteClone.Model;
using EvernoteClone.ViewModel.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EvernoteClone.ViewModel
{
    public class LoginVM : INotifyPropertyChanged
    {
		private bool isShowingRegister = false;
		private User user;

		public User User
		{
			get { return user; }
			set { user = value; }
		}

		private Visibility loginVisibility;
        public Visibility LoginVisibility
		{
			get { return loginVisibility; }
			set { 
				loginVisibility = value;
                //Call the event to change the login visibility in the list view
                OnPropertyChanged("LoginVisibility");
			}
		}

        private Visibility registerVisibility;
        public Visibility RegisterVisibility
        {
            get { return registerVisibility; }
            set
            {
                registerVisibility = value;
                //Call the event to change the register visibility in the list view
                OnPropertyChanged("RegisterVisibility");
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public RegisterCommand RegisterCommand { get; set; }
		public LoginCommand LoginCommand { get; set; }

		public LoginVM()
		{
            //Define the commands related to the user view model
            RegisterCommand = new RegisterCommand(this);
			LoginCommand = new LoginCommand(this);

            //Define initial values
			LoginVisibility = Visibility.Visible;
			RegisterVisibility = Visibility.Collapsed;
        }

        public void SwitchViews()
		{
			//Switch the value
			isShowingRegister = !isShowingRegister;
			//Assign the visibility for each the visibilities property (login and register)
			LoginVisibility = !isShowingRegister ? Visibility.Visible : Visibility.Collapsed;
            RegisterVisibility = isShowingRegister ? Visibility.Visible : Visibility.Collapsed;
        }

		private void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
