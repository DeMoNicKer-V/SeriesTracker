using CommunityToolkit.Mvvm.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeriesTracker
{
    public class TabbarChangedMessage : ValueChangedMessage<bool>
    {
        public TabbarChangedMessage(bool isvisable) : base(isvisable)
        {
        }
    }
    }
