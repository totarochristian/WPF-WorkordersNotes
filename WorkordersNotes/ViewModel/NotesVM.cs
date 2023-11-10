using Azure.Storage.Blobs;
using WorkordersNotes.Model;
using WorkordersNotes.ViewModel.Commands;
using WorkordersNotes.ViewModel.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;

namespace WorkordersNotes.ViewModel
{
    public class NotesVM : INotifyPropertyChanged
    {
        public ObservableCollection<Customer> Customers { get; set; }

		private Customer selectedNotebook;
        public Customer SelectedNotebook
		{
			get { return selectedNotebook; }
			set { 
				selectedNotebook = value;
				//Call the event to change the selected customer in the list view
				OnPropertyChanged("SelectedNotebook");
                //Update notes in the collection using the id of the new selected customer
                GetNotes();
			}
		}

		private Note selectedNote;

		public Note SelectedNote
		{
			get { return selectedNote; }
			set { 
				selectedNote = value;
                //Call the event to change the selected note in the list view
                OnPropertyChanged("SelectedNote");
				//Invoke the selected note changed event
				SelectedNoteChanged?.Invoke(this, new EventArgs());
            }
		}

		private Visibility isVisibleNotebook;
        public Visibility IsVisibleNotebook
        {
            get { return isVisibleNotebook; }
            set { 
				isVisibleNotebook = value;

                //Call the event to change the visibility of the text box in the grid of the customer element in the list view
                OnPropertyChanged("IsVisibleNotebook");
            }
        }

        private Visibility isVisibleNote;
        public Visibility IsVisibleNote
        {
            get { return isVisibleNote; }
            set
            {
                isVisibleNote = value;

                //Call the event to change the visibility of the text box in the grid of the note element in the list view
                OnPropertyChanged("IsVisibleNote");
            }
        }

        public ObservableCollection<Note> Notes { get; set; }

		public NewCustomerCommand NewCustomerCommand { get; set; }
		public NewNoteCommand NewNoteCommand { get; set; }
		public EditCommand EditCommand { get; set; }
		public EndEditingCommand EndEditingCommand { get; set; }
        public DeleteCustomerCommand DeleteCustomerCommand { get; set; }
        public DeleteNoteCommand DeleteNoteCommand { get; set; }
        public ChangeNotesWindowLanguageCommand ChangeLanguageCommand { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

		public event EventHandler SelectedNoteChanged;

        public event EventHandler LanguageChanged;

        public NotesVM()
		{
			//Define the commands related to the notes view model
			NewCustomerCommand = new NewCustomerCommand(this);
			NewNoteCommand = new NewNoteCommand(this);
			EditCommand = new EditCommand(this);
			EndEditingCommand = new EndEditingCommand(this);
            DeleteCustomerCommand = new DeleteCustomerCommand(this);
            DeleteNoteCommand = new DeleteNoteCommand(this);
            ChangeLanguageCommand = new ChangeNotesWindowLanguageCommand(this);

            //Define initial values inside the collections displayed in the list view
            Customers = new ObservableCollection<Customer>();
			Notes = new ObservableCollection<Note>();
			IsVisibleNotebook = Visibility.Collapsed;
			IsVisibleNote = Visibility.Collapsed;

			//Update customers in the collection adding the values saved previously in the database
			GetNotebooks();

            //Assign the method to be called when the language change event of ChangeLanguageCommand will be invoked
            ChangeLanguageCommand.LanguageChanged += ChangeLanguageCommand_LanguageChanged;
        }

        private void ChangeLanguageCommand_LanguageChanged(object? sender, EventArgs e)
        {
            LanguageChanged?.Invoke(sender, e);
        }

        public async void CreateNotebook()
		{
			Customer newNotebook = new Customer()
			{
				Name = "New customer",
				UserId = App.UserId
			};
			await DatabaseHelper.Insert(newNotebook);

            //Update customers in the collection adding the values saved in the database
            GetNotebooks();
		}

		public async void CreateNote(string notebookId)
		{
			Note newNote = new Note()
			{
				CustomerId = notebookId,
				CreatedAt = DateTime.Now,
				UpdatedAt = DateTime.Now,
				Title = $"Note for {DateTime.Now.ToString("yyyy/MM/dd")}",
			};
			await DatabaseHelper.Insert(newNote);

            //Update notes in the collection adding the values saved in the database
            GetNotes();
        }

		public async void GetNotebooks()
		{
			//Read customers from the database
			var customers = await DatabaseHelper.Read<Customer>();
			//If customers founded in the database
			if(customers != null)
			{
                //Filter customers and gets only the customers related to the current user logged
                var notebooksFiltered = customers.Where(n => n.UserId == App.UserId);
                //Clear the collection
                Customers.Clear();
                //Add the customers readed in the collection
                foreach (var customer in notebooksFiltered)
                {
                    Customers.Add(customer);
                }
			}
			else
			{
                //Clear the collection
                Customers.Clear();
            }
		}

        public async void GetNotes()
        {
			//If there is a selected customer
			if(SelectedNotebook != null)
			{
				//Read notes from the database that are related to the customer selected
				var notes = (await DatabaseHelper.Read<Note>());
				//If notes founded in the database
				if(notes != null)
				{
                    //Filter the notes using the selected customer
                    var notesFiltered = notes.Where(n => n.CustomerId == SelectedNotebook.Id).ToList();
                    //Clear the collection
                    Notes.Clear();
                    //Add the notes readed in the collection
                    foreach (var note in notesFiltered)
                    {
                        Notes.Add(note);
                    }
				}
				else
				{
                    //Clear the collection
                    Notes.Clear();
                }
            }
        }

		private void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		public void StartEditing<T>(T parameter)
		{
            if (parameter.GetType().Name == typeof(Customer).Name)
				IsVisibleNotebook = Visibility.Visible;
			else if (parameter.GetType() == typeof(Note))
                IsVisibleNote = Visibility.Visible;
        }

        public void StopEditing(Customer customer)
        {
			//Hide the text box setting the visibility to collapsed
            IsVisibleNotebook = Visibility.Collapsed;
			//Update the customer passed to the method
			DatabaseHelper.Update(customer);
            //Update customers in the collection adding the values saved in the database
            GetNotebooks();
        }

        public void StopEditing(Note note)
        { 
            //Hide the text box setting the visibility to collapsed
            IsVisibleNote = Visibility.Collapsed;
            //Update the note passed to the method
            DatabaseHelper.Update(note);
            //Update note in the collection adding the values saved in the database
            GetNotes();
        }

        public async void DeleteNotebook(Customer customer)
        {
            //Clear the notes and the customers, also the selected items
            Notes.Clear();
            Customers.Clear();
            SelectedNote = null;
            SelectedNotebook = null;
            //Before delete the customer, is mandatory delete all the related notes, so retrieve all the notes linked to this customer
            List<Note> notes = (await DatabaseHelper.Read<Note>()).Where(n=>n.CustomerId == customer.Id).ToList();
            //For each note in the notes list founded, call the DeleteNode method
            foreach (Note note in notes)
                DeleteNote(note, false);
            //Delete the customer passed to the method
            DatabaseHelper.Delete(customer);
            //Update customers list to fill it with updated values
            GetNotebooks();
        }

        public void DeleteNote(Note note, bool updateNotes)
        {
            //Clear the notes and the customers, also the selected items
            Notes.Clear();
            SelectedNote = null;
            //Delete blob file using the id of the note
            DeleteBlobFile(note.Id);
            //Delete the note passed to the method
            DatabaseHelper.Delete(note);
            //If update notes bool is setted
            if (updateNotes)
                GetNotes();
        }

        private void DeleteBlobFile(string id)
        {
            BlobServiceClient blobServiceClient = new BlobServiceClient(App.connectionString);
            BlobContainerClient cont = blobServiceClient.GetBlobContainerClient(App.containerName);
            cont.GetBlobClient($"{id}.rtf").DeleteIfExists();
        }
    }
}
