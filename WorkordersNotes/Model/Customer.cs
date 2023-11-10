using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkordersNotes.Model
{
    public class Customer : HasId
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
    }
}
