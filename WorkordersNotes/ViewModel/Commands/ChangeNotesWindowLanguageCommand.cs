using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace WorkordersNotes.ViewModel.Commands
{
    public class ChangeNotesWindowLanguageCommand : ICommand
    {
        public NotesVM VM { get; set; }

        public ResourceDictionary Dictionary { get; private set; }

        public event EventHandler LanguageChanged;

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public ChangeNotesWindowLanguageCommand(NotesVM vm)
        {
            VM = vm;            
        }

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            //Teorically the parameter is the language name es: Italian
            string language = parameter as string;
            Dictionary = new ResourceDictionary();
            //If the language isn't null
            if (language != null)
            {
                //Define the dictionary source using the language passed as parameter
                switch(language)
                {
                    case "Italian" or "Italiano":
                        Dictionary.Source = new Uri("..\\..\\..\\StringResources.it.xaml", UriKind.Relative);
                        break;
                    case "English" or "Inglese":
                        Dictionary.Source = new Uri("..\\StringResources.en.xaml", UriKind.Relative);
                        break;
                    default:
                        Dictionary.Source = new Uri("..\\StringResources.it.xaml", UriKind.Relative);
                        break;
                }
                //Invoke the language changed event
                LanguageChanged?.Invoke(this, new EventArgs());
            }
        }
    }
}
