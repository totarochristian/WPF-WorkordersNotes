using WorkordersNotes.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WorkordersNotes.ViewModel.Commands;

namespace WorkordersNotes.View
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        LoginVM viewModel;

        public LoginWindow()
        {
            InitializeComponent();

            //Retrieve the login view model from the resources instantiated in the window
            viewModel = Resources["vm"] as LoginVM;
            //Assign the event to call when the Authenticated event will be invoked
            viewModel.Authenticated += ViewModel_Authenticated;
            //Assign the method to be called when the notes view model language changed event will be called
            viewModel.LanguageChanged += ViewModel_LanguageChanged;

            //Set the owner of the window
            Owner = Application.Current.MainWindow;
            //Set the startup location equals to the center of the owner
            WindowStartupLocation = WindowStartupLocation.CenterOwner;

            //Call the change language command to update the language to the active language of the app
            viewModel.ChangeLanguageCommand.Execute(Properties.Settings.Default.ActiveLanguage);
        }

        private void ViewModel_LanguageChanged(object? sender, EventArgs e)
        {
            //Cast the sender as ChangeLanguageCommand
            ChangeLoginWindowLanguageCommand languageCommand = sender as ChangeLoginWindowLanguageCommand;
            //If the language command isn't null, add the dictionary inside of it in the merged dictionaries (before add, clear it)
            if (languageCommand != null)
            {
                this.Resources.MergedDictionaries.Clear();
                this.Resources.MergedDictionaries.Add(languageCommand.Dictionary);
            }   
        }

        private void ViewModel_Authenticated(object? sender, EventArgs e)
        {
            //Close the dialog (this happen when will be invoked the authenticated event in the login view model, so when user login or register)
            this.Close();
        }
    }
}
