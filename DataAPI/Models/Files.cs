using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAPI.Models
{
    public class File
    {
        public int fileId { get; set; }
        public String name { get; set; }
        public String fileNumber { get; set; }
        public String status { get; set; }
        public double totalSalesPrice { get; set; }
        public DateTime openDate { get; set; }
        public DateTime closeDate { get; set; }
        public String transactionType { get; set; }
    }
}
