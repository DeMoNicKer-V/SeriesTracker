using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeriesTracker.Services.Exceptions
{
    internal class LimitQuotaCustomSeachException: Exception
    {

        public LimitQuotaCustomSeachException() : base(
            "Вы израсходовали суточный лимит на поиск изображений. \nЛимит обновится на следующий день.") 
        { 

        }
    }
}
