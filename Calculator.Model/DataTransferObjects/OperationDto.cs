using Calculator.Model.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.Model.DataTransferObjects
{
    public class OperationDto
    {
        public OperationType Type { get; set; }

        public double ArithmeticResult { get; set; }

        public double FirstParameter { get; set; }

        public double SecondParameter { get; set; }

        public long MasterId { get; set; }

        public long Id { get; set; }
    }
}
