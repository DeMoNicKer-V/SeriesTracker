using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeriesTracker.Services.SyncJournal
{
    public class DeleteItem
    {
        public string Id { get; set; }

        public DeleteItem(string _id)
        {
            Id = _id;
        }
    }
}
