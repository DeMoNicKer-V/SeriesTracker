using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeriesTracker.Services.SyncJournal
{
    public class DeleteItem
    {
        public int Id { get; set; }

        public DeleteItem(int _id)
        {
            Id = _id;
        }
    }
}
