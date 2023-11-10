using WorkordersNotes.Model;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WorkordersNotes.View.UserControls
{
    /// <summary>
    /// Interaction logic for DisplayCustomer.xaml
    /// </summary>
    public partial class DisplayCustomer : UserControl
    {
        public Customer Customer
        {
            get { return (Customer)GetValue(NotebookProperty); }
            set { SetValue(NotebookProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NotebookProperty =
            DependencyProperty.Register("Customer", typeof(Customer), typeof(DisplayCustomer), new PropertyMetadata(null, SetValues));

        private static void SetValues(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //Cast the dependency object to this type of user control
            DisplayCustomer notebookUserControl = d as DisplayCustomer;
            //When change the property, re-assign it to the user control if is not null
            if(notebookUserControl != null)
            {
                notebookUserControl.DataContext = notebookUserControl.Customer;
            }
        }

        public DisplayCustomer()
        {
            InitializeComponent();
        }
    }
}
