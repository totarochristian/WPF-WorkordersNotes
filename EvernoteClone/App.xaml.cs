using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace WorkordersNotes
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static string UserId = string.Empty;
        //Define the connection string (this is in the access key of the azure storage account)
        public static string connectionString = "DefaultEndpointsProtocol=https;AccountName=evernotestoragetest;AccountKey=DvMu326ln4lS6ii9tGAoYvuzHAMlhsxBo5ou1eiGg9aiym4X411ebC6tUhlGuXmjkp5myXCFRwUK+AStakRBFw==;EndpointSuffix=core.windows.net";
        //Define the name of the container defined in the azure storage account
        public static string containerName = "notes";
    }
}
