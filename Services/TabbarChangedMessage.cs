using CommunityToolkit.Mvvm.Messaging.Messages;

namespace SeriesTracker.Services
{
    public class TabbarChangedMessage : ValueChangedMessage<bool>
    {
        public TabbarChangedMessage(bool isvisable) : base(isvisable)
        {
        }
    }
}
