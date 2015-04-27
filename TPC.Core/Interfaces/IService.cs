using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using TPC.Core.Models.ViewModels;

namespace TPC.Core.Interfaces
{
    public interface IService<T> 
    {
        UserViewModel UserVM { get; set; }

        void CustomerCatalogBarcodeManipulation(int custAutoID, int quoteid);
    }
}
