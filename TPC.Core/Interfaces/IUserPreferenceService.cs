using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPC.Core.Models.ViewModels;

namespace TPC.Core.Interfaces
{
    public interface IUserPreferenceService
    {
         bool SaveUserPreferences( string Type, string preferred);
         UserViewModel UserVM { get; set; }
       
    }
}
