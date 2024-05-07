using Calculator.Model.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.Model
{
    [Table("MemoryRecall")]
    public class MREntity : BaseEntityPIT
    {
        public double MRValue {  get; set; }
    }
}
