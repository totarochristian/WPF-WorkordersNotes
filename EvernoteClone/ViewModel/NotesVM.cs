using EvernoteClone.Model;
using EvernoteClone.ViewModel.Commands;
using EvernoteClone.ViewModel.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

		public ObservableCollection<Note> Notes { get; set; }

		public NewNotebookCommand NewNotebookCommand { get; set; }
		public NewNoteCommand NewNoteCommand { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public NotesVM()
		{
			//Define the commands related to the notes view model
			NewNotebookCommand = new NewNotebookCommand(this);
			NewNoteCommand = new NewNoteCommand(this);

			//Define initial values inside the collections displayed in the list view
			Notebooks = new ObservableCollection<Notebook>();
			Notes = new ObservableCollection<Note>();

			//Update notebooks in the collection adding the values saved previously in the database
			GetNotebooks();
		}

		public void CreateNotebook()
		{
			Notebook newNotebook = new Notebook()
			{
				Name = "New notebook"
			};
			DatabaseHelper.Insert(newNotebook);

            //Update notebooks in the collection adding the values saved in the database
            GetNotebooks();
		}

		public void CreateNote(int notebookId)
		{
			Note newNote = new Note()
			{
				NotebookId = notebookId,
				CreatedAt = DateTime.Now,
				UpdatedAt = DateTime.Now,
				Title = $"Note for {DateTime.Now.ToString("yyyy/MM/dd")}",
			};
			DatabaseHelper.Insert(newNote);

            //Update notes in the collection adding the values saved in the database
            GetNotes();
        }

		private void GetNotebooks()
		{
			//Read notebooks from the database
			var notebooks = DatabaseHelper.Read<Notebook>();
			//Clear the collection
			Notebooks.Clear();
			//Add the notebooks readed in the collection
			foreach(var notebook in notebooks)
			{
				Notebooks.Add(notebook);
			}
		}

        private void GetNotes()
        {
			//If there is a selected notebook
			if(SelectedNotebook != null)
			{
                //Read notes from the database that are related to the notebook selected
                var notes = DatabaseHelper.Read<Note>().Where(n => n.NotebookId == SelectedNotebook.Id).ToList();
                //Clear the collection
                Notes.Clear();
                //Add the notes readed in the collection
                foreach (var note in notes)
                {
                    Notes.Add(note);
                }
            }
        }

		private void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
    }
}
