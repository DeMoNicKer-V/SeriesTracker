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
        public string PrevId { get; set; }

        public AddUpdateItem(string _id, string _prevId) 
        {
            Id = _id;
            PrevId = _prevId;
        }
    }
}
