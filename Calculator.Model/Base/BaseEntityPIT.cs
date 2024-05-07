using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.Model.Base
{
 
    public class BaseEntityPIT
    {
        public long Id { get; set; }
        public long MasterId { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public bool Inactive { get; set; }
    }
}
