using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeriesTracker.Services.SyncJournal
{
    public class AddUpdateItem
    {
        public int Id { get; set; }
        public int PrevId { get; set; }

        public AddUpdateItem(int _id, int _prevId) 
        {
            Id = _id;
            PrevId = _prevId;
        }
    }
}
