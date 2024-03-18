using Newtonsoft.Json;
using SeriesTracker.Services.Constant;
using System.Linq;
using System.Text.Json;

namespace SeriesTracker.Services.SyncJournal
{
    public partial class Journal
    {
        public Journal()
        { }

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

        [JsonProperty(nameof(DeleteItems))] public List<DeleteItem> DeleteItems { get; set; } = new();
        [JsonProperty(nameof(UpdateItems))] public List<AddUpdateItem> UpdateItems { get; set; } = new();

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

        public void UpdateJournal() 
        {
            if (File.Exists(Path.Combine(Parameters.FilePath + "SyncJournal.json")))
            {
                var CurrentJournal = this;
                using (var reader = new StreamReader(Path.Combine(Parameters.FilePath + "SyncJournal.json")))
                {
                    string jsonfileData = reader.ReadToEnd();
                    CurrentJournal = JsonConvert.DeserializeObject<Journal>(jsonfileData);
                    if (CurrentJournal.DeleteItems.Count > 0)
                    {
                        CurrentJournal.DeleteItems.Clear();
                    }
                        reader.Close();
                }
                string json = JsonConvert.SerializeObject(CurrentJournal);
                File.WriteAllText(Path.Combine(Parameters.FilePath + "SyncJournal.json"), json);
            }


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

                    if (this.UpdateItems.Count > 0)
                    {
                        int updateIndex = CurrentJournal.UpdateItems.FindIndex(s => s.PrevId == this.UpdateItems[0].PrevId);
                        if (updateIndex > -1)
                        {
                            CurrentJournal.UpdateItems[updateIndex] = this.UpdateItems[0];
                        }
                        else { CurrentJournal.UpdateItems.AddRange(this.UpdateItems); }
                    }

                    if (this.DeleteItems.Count > 0)
                    {
                        int deleteIndex = CurrentJournal.DeleteItems.FindIndex(s => s.Id == this.DeleteItems[0].Id);
                        if (deleteIndex > -1)
                        {
                            CurrentJournal.DeleteItems[deleteIndex] = this.DeleteItems[0];
                        }
                        else
                        {
                            CurrentJournal.DeleteItems.AddRange(this.DeleteItems);
                        }
                    }
                   
                    reader.Close();
                }
            }
            string json = JsonConvert.SerializeObject(CurrentJournal);
            File.WriteAllText(Path.Combine(Parameters.FilePath + "SyncJournal.json"), json);
        }
    }
}