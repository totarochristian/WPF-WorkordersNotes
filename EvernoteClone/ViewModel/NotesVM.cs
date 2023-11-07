using EvernoteClone.Model;
using EvernoteClone.ViewModel.Commands;
using EvernoteClone.ViewModel.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvernoteClone.ViewModel
{
    public class NotesVM
    {
        public ObservableCollection<Notebook> Notebooks { get; set; }

		private Notebook selectedNotebook;

		public Notebook SelectedNotebook
		{
			get { return selectedNotebook; }
			set { 
				selectedNotebook = value;

				//TODO: get notes
			}
		}

		public ObservableCollection<Note> Notes { get; set; }

		public NewNotebookCommand NewNotebookCommand { get; set; }
		public NewNoteCommand NewNoteCommand { get; set; }

		public NotesVM()
		{
			NewNotebookCommand = new NewNotebookCommand(this);
			NewNoteCommand = new NewNoteCommand(this);
		}

		public void CreateNote(int notebookId)
		{
			Note newNote = new Note()
			{
				Id = notebookId,
				CreatedAt = DateTime.Now,
				UpdatedAt = DateTime.Now,
				Title = "New note"
			};
			DatabaseHelper.Insert(newNote);
		}
	}
}
