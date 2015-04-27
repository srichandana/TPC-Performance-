using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPC.Core.Models;
using TPC.Core.Models.Models;
using TPC.Context.EntityModel;
using TPC.Core.Models.ViewModels;

namespace TPC.Core.Interfaces
{
    public interface ISearchService : IService<SearchViewModel>
    {
        List<Item> GetDetails();
    }
}
