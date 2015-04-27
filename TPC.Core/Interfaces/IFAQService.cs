using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPC.Core.Models;
using TPC.Core.Models.Models;
using TPC.Context.EntityModel;

namespace TPC.Core.Interfaces
{
    public interface IFAQService : IService<FAQViewModel>
    {
        FAQViewModel GetDetails();
    }
}
