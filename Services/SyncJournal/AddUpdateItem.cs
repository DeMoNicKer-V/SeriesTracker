using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeriesTracker.Services.SyncJournal
{
    public class AddUpdateItem
    {
        public string Id { get; set; }

        public AddUpdateItem(string _id) 
        {
            Id = _id;
        }
    }
}
