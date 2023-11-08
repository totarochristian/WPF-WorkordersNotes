using EvernoteClone.Model;
using EvernoteClone.ViewModel.Commands;
using EvernoteClone.ViewModel.Helpers;
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
			set { 
				user = value;
                //Call the event to change the user assigned
                OnPropertyChanged("User");
            }
		}

        private string username;
        public string Username
        {
            get { return username; }
            set
            {
                username = value;
                //Re-define the user
                User = new User()
                {
                    Username = username,
                    Name = this.name,
                    Lastname = this.lastname,
                    Password = this.password,
                    ConfirmPassword = this.ConfirmPassword
                };
                //Call the event to change the Username assigned
                OnPropertyChanged("Username");
            }
        }

        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                //Re-define the user
                User = new User()
                {
                    Username = this.username,
                    Name = name,
                    Lastname = this.lastname,
                    Password = this.password,
                    ConfirmPassword = this.ConfirmPassword
                };
                //Call the event to change the Name assigned
                OnPropertyChanged("Name");
            }
        }

        private string lastname;
        public string Lastname
        {
            get { return lastname; }
            set
            {
                lastname = value;
                //Re-define the user
                User = new User()
                {
                    Username = this.username,
                    Name = this.name,
                    Lastname = lastname,
                    Password = this.password,
                    ConfirmPassword = this.ConfirmPassword
                };
                //Call the event to change the Lastname assigned
                OnPropertyChanged("Lastname");
            }
        }

        private string password;
        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                //Re-define the user
                User = new User()
                {
                    Username = this.username,
                    Name = this.name,
                    Lastname = this.lastname,
                    Password = password,
                    ConfirmPassword = this.ConfirmPassword
                };
                //Call the event to change the Password assigned
                OnPropertyChanged("Password");
            }
        }

        private string confirmPassword;
        public string ConfirmPassword
        {
            get { return confirmPassword; }
            set
            {
                confirmPassword = value;
                //Re-define the user
                User = new User()
                {
                    Username = this.username,
                    Name = this.name,
                    Lastname = this.lastname,
                    Password = this.password,
                    ConfirmPassword = ConfirmPassword
                };
                //Call the event to change the ConfirmPassword assigned
                OnPropertyChanged("ConfirmPassword");
            }
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
		public ShowRegisterCommand ShowRegisterCommand { get; set; }

		public LoginVM()
		{
            //Define the commands related to the user view model
            RegisterCommand = new RegisterCommand(this);
			LoginCommand = new LoginCommand(this);
			ShowRegisterCommand = new ShowRegisterCommand(this);

            //Define initial values
			LoginVisibility = Visibility.Visible;
			RegisterVisibility = Visibility.Collapsed;
            User = new User();
        }

        public void SwitchViews()
		{
			//Switch the value
			isShowingRegister = !isShowingRegister;
			//Assign the visibility for each the visibilities property (login and register)
			LoginVisibility = !isShowingRegister ? Visibility.Visible : Visibility.Collapsed;
            RegisterVisibility = isShowingRegister ? Visibility.Visible : Visibility.Collapsed;
        }

		public void Login()
		{
			//TODO: login functionality
		}

        public async void Register()
        {
            //Call the Register method of the firebase auth helper class passing the user data binded in the register stack panel
            await FirebaseAuthHelper.Register(User);
        }

        private void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
