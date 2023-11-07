using EvernoteClone.Model;
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

	}
}
