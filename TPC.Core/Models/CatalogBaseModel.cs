using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPC.Core.Infrastructure;

namespace TPC.Core.Models
{
    public class CatalogBaseModel
    {
        public int ID { get; set; }
        public string Text { get; set; }
        public string Value { get; set; }
        public bool IsSelected { get; set; }
        public int Type { get; set; }
        public List<ComboBase> comboBase { get; set; }
        public string CatalogType { get; set; }
    }
}
