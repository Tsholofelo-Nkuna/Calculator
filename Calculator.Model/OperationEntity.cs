using Calculator.Model.Base;
using Calculator.Model.Common.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Calculator.Model
{
    [Table("Operations")]
    public class OperationEntity : BaseEntityPIT
    {
        public OperationType Type { get; set; }
        public int OperationId { get; set; }
        public double ArithmeticResult => Type switch
        {
            OperationType.Add or OperationType.MemoryPlus => this.FirstParameter + this.SecondParameter,
            OperationType.Subtract or OperationType.MemoryMinus => this.FirstParameter - this.SecondParameter,
            OperationType.Divide => this.FirstParameter / this.SecondParameter,
            OperationType.Multiply => this.FirstParameter * this.SecondParameter,
            _ => 0
        };

        public double FirstParameter { get; set; }

        public double SecondParameter { get; set; }


    }
}
