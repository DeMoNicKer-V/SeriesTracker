namespace SeriesTracker.Models
{
    public sealed class Sync
    {
        public static readonly Sync Off = new(0, "Отключена");
        public static readonly Sync AfterClose = new(1, "После закрытия");
        public static readonly Sync OneDay = new(2, "Раз в день");
        public static readonly Sync OneWeek = new(3, "Раз в неделю");

        private Sync(int syncId, string displayName)
        {
            SyncId = syncId;
            DisplayName = displayName;
        }

        public static List<Sync> AvailableSyncs
        {
            get;
        } = new()
    {
        Off,
        AfterClose,
        OneDay,
        OneWeek
    };

        public string DisplayName
        {
            get;
        }

        public int SyncId
        {
            get;
        }
    }
}