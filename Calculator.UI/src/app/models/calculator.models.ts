export const enum OperationType{
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

export class OperationDto{
  type: OperationType = OperationType.Add
  firstParameter: number = 0;
  secondParameter: number = 0;
  masterId: number = 0;
  arithmeticResult: number = 0;
}

export class Operation{
  Data = new OperationDto();
  Label: "-"|"+"|"*"|"/"|"M+"|"M-"|"Recall" = "+"
}
