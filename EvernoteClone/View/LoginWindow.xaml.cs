using EvernoteClone.ViewModel;
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

namespace EvernoteClone.View
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

            //Set the owner of the window
            Owner = Application.Current.MainWindow;
            //Set the startup location equals to the center of the owner
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
        }
    }
}
