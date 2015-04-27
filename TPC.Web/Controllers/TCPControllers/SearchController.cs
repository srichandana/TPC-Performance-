using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TPC.Core.Models;
using TPC.Web.Filters;
using TPC.Core;
using TPC.Core.Interfaces;
using Microsoft.Practices.Unity;
using TPC.Core.Models.Models;
using TPC.Common.Enumerations;
using TPC.Core.Models.ViewModels;
using System.Web.Script.Serialization;

namespace TPC.Web.Controllers.TCPControllers
{
    [TPCAuthorize]
    [Serializable]
    public class SearchController : BaseController
    {
        private readonly ISearchService _searchsrv;
        public SearchController(SearchService searchSrv)
        {
            _searchsrv = searchSrv;
            _searchsrv.UserVM = UserVM;
        }
    }
}
