using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TPC.Common.Enumerations
{
    public enum ItemStatusEnum
    {
        OnListButNotPreViewable='D',OnListAndPreViewable='B',NotOnListDoNotDisplay='N',OffListItems='O',ReadyToDisplay ='R'
    }
}
