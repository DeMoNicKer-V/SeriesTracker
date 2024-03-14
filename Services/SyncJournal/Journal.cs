using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SeriesTracker.Services.Constant;
using System.Text.Json;
using System.Reflection;

namespace SeriesTracker.Services.SyncJournal
{
    public partial class Journal
    {
        [JsonProperty(nameof(UpdateItems))] public List<AddUpdateItem> UpdateItems { get; set; } = new();
        [JsonProperty(nameof(DeleteItems))] public List<DeleteItem> DeleteItems { get; set; } = new();

        public Journal() { }
        public Journal(AddUpdateItem addUpdateItem) 
        {
            UpdateItems.Add(addUpdateItem);
        }
        public Journal(DeleteItem deleteItem)
        {
            DeleteItems.Add(deleteItem);
        }
        public Journal(AddUpdateItem addUpdateItem, DeleteItem deleteItem)
        {
            UpdateItems.Add(addUpdateItem);
            DeleteItems.Add(deleteItem);
        }
        public Journal GetJournal() 
        {
            if (File.Exists(Path.Combine(Parameters.FilePath + "SyncJournal.json")))
            {
                using (var reader = new StreamReader(Path.Combine(Parameters.FilePath + "SyncJournal.json")))
                {
                    string jsonfileData = reader.ReadToEnd();
                    Journal CurrentJournal = JsonConvert.DeserializeObject<Journal>(jsonfileData);
                    CurrentJournal.UpdateItems.AddRange(this.UpdateItems);
                    CurrentJournal.DeleteItems.AddRange(this.DeleteItems);
                    reader.Close();
                    File.Delete(Path.Combine(Parameters.FilePath + "SyncJournal.json"));
                    return CurrentJournal;
                }
            }
            return null;
        }
        public void JournalToJson() 
        {
            Journal CurrentJournal = this;
            if (File.Exists(Path.Combine(Parameters.FilePath + "SyncJournal.json")))
            {
                using (var reader = new StreamReader(Path.Combine(Parameters.FilePath + "SyncJournal.json")))
                {
                    string jsonfileData = reader.ReadToEnd();
                    CurrentJournal = JsonConvert.DeserializeObject<Journal>(jsonfileData);
                    if (CurrentJournal.UpdateItems.FindIndex(s => s.PrevId == this.UpdateItems[0].PrevId) > -1)
                    {
                        int index = CurrentJournal.UpdateItems.FindIndex(s => s.PrevId == this.UpdateItems[0].PrevId);
                        CurrentJournal.UpdateItems[index] = this.UpdateItems[0];
                        CurrentJournal.DeleteItems.AddRange(this.DeleteItems);
                    }
                    else 
                    {
                        CurrentJournal.UpdateItems.AddRange(this.UpdateItems);
                        CurrentJournal.DeleteItems.AddRange(this.DeleteItems);
                    }
                    reader.Close();
                }
            }
            string json = JsonConvert.SerializeObject(CurrentJournal);
            File.WriteAllText(Path.Combine(Parameters.FilePath + "SyncJournal.json"), json);
        }
    }
}
