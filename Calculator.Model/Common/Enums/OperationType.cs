using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator.Model.Common.Enums
{
    public enum OperationType
    {
        Add = 1,
        Subtract,
        Multiply,
        Divide,
        MemoryPlus,
        MemoryMinus,
        MemoryRecall,
        ClearEntry,
        ClearAll,
        ShowHistory
    }
}
