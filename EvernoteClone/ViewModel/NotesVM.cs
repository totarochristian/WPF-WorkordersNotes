using Azure.Storage.Blobs;
using EvernoteClone.Model;
using EvernoteClone.ViewModel.Commands;
using EvernoteClone.ViewModel.Helpers;
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

namespace EvernoteClone.ViewModel
{
    public class NotesVM : INotifyPropertyChanged
    {
        public ObservableCollection<Notebook> Notebooks { get; set; }

		private Notebook selectedNotebook;
        public Notebook SelectedNotebook
		{
			get { return selectedNotebook; }
			set { 
				selectedNotebook = value;
				//Call the event to change the selected notebook in the list view
				OnPropertyChanged("SelectedNotebook");
                //Update notes in the collection using the id of the new selected notebook
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

                //Call the event to change the visibility of the text box in the grid of the notebook element in the list view
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

		public NewNotebookCommand NewNotebookCommand { get; set; }
		public NewNoteCommand NewNoteCommand { get; set; }
		public EditCommand EditCommand { get; set; }
		public EndEditingCommand EndEditingCommand { get; set; }
        public DeleteCommand DeleteCommand { get; set; }

		public event PropertyChangedEventHandler? PropertyChanged;

		public event EventHandler SelectedNoteChanged;

        public NotesVM()
		{
			//Define the commands related to the notes view model
			NewNotebookCommand = new NewNotebookCommand(this);
			NewNoteCommand = new NewNoteCommand(this);
			EditCommand = new EditCommand(this);
			EndEditingCommand = new EndEditingCommand(this);
            DeleteCommand = new DeleteCommand(this);

			//Define initial values inside the collections displayed in the list view
			Notebooks = new ObservableCollection<Notebook>();
			Notes = new ObservableCollection<Note>();
			IsVisibleNotebook = Visibility.Collapsed;
			IsVisibleNote = Visibility.Collapsed;

			//Update notebooks in the collection adding the values saved previously in the database
			GetNotebooks();
		}

		public async void CreateNotebook()
		{
			Notebook newNotebook = new Notebook()
			{
				Name = "New notebook",
				UserId = App.UserId
			};
			await DatabaseHelper.Insert(newNotebook);

            //Update notebooks in the collection adding the values saved in the database
            GetNotebooks();
		}

		public async void CreateNote(string notebookId)
		{
			Note newNote = new Note()
			{
				NotebookId = notebookId,
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
			//Read notebooks from the database
			var notebooks = await DatabaseHelper.Read<Notebook>();
			//If notebooks founded in the database
			if(notebooks != null)
			{
                //Filter notebooks and gets only the notebooks related to the current user logged
                var notebooksFiltered = notebooks.Where(n => n.UserId == App.UserId);
                //Clear the collection
                Notebooks.Clear();
                //Add the notebooks readed in the collection
                foreach (var notebook in notebooksFiltered)
                {
                    Notebooks.Add(notebook);
                }
			}
			else
			{
                //Clear the collection
                Notebooks.Clear();
            }
		}

        public async void GetNotes()
        {
			//If there is a selected notebook
			if(SelectedNotebook != null)
			{
				//Read notes from the database that are related to the notebook selected
				var notes = (await DatabaseHelper.Read<Note>());
				//If notes founded in the database
				if(notes != null)
				{
                    //Filter the notes using the selected notebook
                    var notesFiltered = notes.Where(n => n.NotebookId == SelectedNotebook.Id).ToList();
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
            if (parameter.GetType().Name == typeof(Notebook).Name)
				IsVisibleNotebook = Visibility.Visible;
			else if (parameter.GetType() == typeof(Note))
                IsVisibleNote = Visibility.Visible;
        }

        public void StopEditing(Notebook notebook)
        {
			//Hide the text box setting the visibility to collapsed
            IsVisibleNotebook = Visibility.Collapsed;
			//Update the notebook passed to the method
			DatabaseHelper.Update(notebook);
            //Update notebooks in the collection adding the values saved in the database
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

        public void DeleteNotebook(Notebook notebook)
        {
            //Before delete the notebook, is mandatory delete all the related notes, so retrieve all the notes linked to this notebook
            List<Note> notes = (await DatabaseHelper.Read<Note>()).Where(n=>n.NotebookId == notebook.Id).ToList();
            //For each note in the notes list founded, call the DeleteNode method
            foreach (Note note in notes)
                DeleteNote(note);
            //Delete the notebook passed to the method
            DatabaseHelper.Delete(notebook);
            //Update notebooks in the collection
            GetNotebooks();
        }

        public void DeleteNote(Note note)
        {
            //Delete blob file using the id of the note
            DeleteBlobFile(note.Id);
            //Delete the note passed to the method
            DatabaseHelper.Delete(note);
            //Update notes in the collection
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
