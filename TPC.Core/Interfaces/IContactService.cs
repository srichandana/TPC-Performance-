using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPC.Core.Models.ViewModels;

namespace TPC.Core.Interfaces
{
    public interface IContactService : IService<ContactViewModel>
    {
        ContactViewModel getPenworthyContactDetails(int userID);
        void EditProfile(ContactViewModel contactModel);
    }
}
