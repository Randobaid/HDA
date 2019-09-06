using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HDA.Core.Models.HDACore
{
    public class DrugClassLookups
    {
        public int DrugClassLookupID { get; set; }
        public string DrugClassLooukItem { get; set; }
        public int DrugId { get; set; }
        public string DrugName { get; set; }
               
    }
}