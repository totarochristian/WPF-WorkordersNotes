using WorkordersNotes.Model;
using WorkordersNotes.ViewModel.Commands;
using WorkordersNotes.ViewModel.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static SQLite.SQLite3;

namespace WorkordersNotes.ViewModel
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

        private string email;
        public string Email
        {
            get { return email; }
            set
            {
                email = value;
                //Re-define the user
                User = new User()
                {
                    Email = email,
                    Name = this.name,
                    Lastname = this.lastname,
                    Password = this.password,
                    ConfirmPassword = this.ConfirmPassword
                };
                //Call the event to change the Email assigned
                OnPropertyChanged("Email");
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
                    Email = this.email,
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
                    Email = this.email,
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
                    Email = this.email,
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
                    Email = this.email,
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

        public event EventHandler Authenticated;

        public event EventHandler LanguageChanged;

        public RegisterCommand RegisterCommand { get; set; }
		public LoginCommand LoginCommand { get; set; }
		public ShowRegisterCommand ShowRegisterCommand { get; set; }
        public ChangeLoginWindowLanguageCommand ChangeLanguageCommand { get; set; }

		public LoginVM()
		{
            //Define the commands related to the user view model
            RegisterCommand = new RegisterCommand(this);
			LoginCommand = new LoginCommand(this);
			ShowRegisterCommand = new ShowRegisterCommand(this);
            ChangeLanguageCommand = new ChangeLoginWindowLanguageCommand(this);

            //Define initial values
			LoginVisibility = Visibility.Visible;
			RegisterVisibility = Visibility.Collapsed;
            User = new User();

            //Assign the method to be called when the language change event of ChangeLanguageCommand will be invoked
            ChangeLanguageCommand.LanguageChanged += ChangeLanguageCommand_LanguageChanged;
        }

        private void ChangeLanguageCommand_LanguageChanged(object? sender, EventArgs e)
        {
            LanguageChanged?.Invoke(sender, e);
        }

        public void SwitchViews()
		{
			//Switch the value
			isShowingRegister = !isShowingRegister;
			//Assign the visibility for each the visibilities property (login and register)
			LoginVisibility = !isShowingRegister ? Visibility.Visible : Visibility.Collapsed;
            RegisterVisibility = isShowingRegister ? Visibility.Visible : Visibility.Collapsed;
        }

		public async void Login()
		{
            //Call the Login method of the firebase auth helper class passing the user data binded in the login stack panel
            bool result = await FirebaseAuthHelper.Login(User);
            //If login is done correctly, invoke the authenticated event (generally will close the login dialog)
            if(result)
            {
                Authenticated?.Invoke(this, new EventArgs());
            }
        }

        public async void Register()
        {
            //Call the Register method of the firebase auth helper class passing the user data binded in the register stack panel
            bool result = await FirebaseAuthHelper.Register(User);
            //If register is done correctly, invoke the authenticated event (generally will close the login dialog)
            if (result)
            {
                Authenticated?.Invoke(this, new EventArgs());
            }
        }

        private void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
