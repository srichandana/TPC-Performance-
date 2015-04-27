﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPC.Core.Models;

namespace TPC.Core.Interfaces
{
   public interface IItemDetailedPartialService : IService<ItemDetailedPartialViewModel>
    {
       ItemDetailedPartialViewModel GetItemDetailedPartial();
    }
}
