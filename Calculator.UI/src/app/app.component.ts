import { Component, OnInit, inject } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { OperationDto, OperationType } from './models/calculator.models';
import { CalculatorService } from './services/calculator.service';
import { forkJoin } from 'rxjs';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
  title = 'Calculator.UI';
  calcButtonDigits: number[] = Array.from({length: 10}, (_, index)=> index+1 );
  screenOutput: FormGroup;
  private _formBuilder = inject(FormBuilder);
  private _calcService = inject(CalculatorService);
  operationInputFieldName = "operationText";
  performedOperation = new OperationDto();
  constructor(){
    this.screenOutput = this._formBuilder.group({
      [this.operationInputFieldName]: new FormControl<string>("")
    });
  }

  ngOnInit(){
    this._calcService.getOperationHistory()
    .subscribe(history =>{
      this.performedOperation.masterId = history?.[0]?.[0]?.masterId ?? 0;
    });
  }

  operationLabel(opType: OperationType){
    if(OperationType.Add === opType){
      return '+'
    }
    else if(OperationType.Divide == opType){
      return '/';
    }
    else if(OperationType.Multiply == opType){
      return '*';
    }
    else if(OperationType.Subtract == opType){
      return '-';
    }
    else if(OperationType.MemoryMinus == opType){
       return 'M-';
    }
    else if(OperationType.MemoryPlus == opType){
      return 'M+';
    }
    else if(OperationType.MemoryRecall == opType){
      return "Memory Recall";
    }
    else{
      return '';
    }
  }

  onShowHistoryClick(){
    forkJoin({
      historyById: this._calcService.getOperationHistoryById(this.performedOperation.masterId),
      allHistory: this._calcService.getOperationHistory()
     })
     .subscribe(response =>{
       let displayedHistory: Array<OperationDto> = []; 
       if(response?.historyById?.length > 0){
         displayedHistory = response.historyById
       }
       else{
         displayedHistory = response?.allHistory?.[0] ?? [];
       }
      let historyString =  displayedHistory.map(next => {
        let operationStr = '';
         if(!(next.type === OperationType.MemoryPlus || next.type === OperationType.MemoryMinus)){
            operationStr = `${next.firstParameter}${this.operationLabel(next.type)}${next.secondParameter ?? ''} = ${next.arithmeticResult}`;
        
         }
         else{
           operationStr  = `${next.secondParameter}${this.operationLabel(next.type)}=${next.arithmeticResult}`;
         }
          return operationStr;
       });
       alert(displayedHistory?.length ? historyString.join('\n'): 'No History data');
     })
  }

  onClearAllClick(){
    this._calcService.clearHistory()
    .subscribe(() =>{
      this.screenOutput.get(this.operationInputFieldName)?.setValue('');
      this.performedOperation = new OperationDto();
      alert('History cleared');
    });
  }

  onCalcButtonClick(buttonText: string){
   
     const currentValue =   this.screenOutput.get(this.operationInputFieldName)?.value ?? "";
     const newValue = currentValue+buttonText;
     const validationPattern = /(^([*+\-\/]?)\d+[*+\-\/]?\d+$)|(^([*+\-\/]?)\d+$)|(^([*+\-\/]?)\d+[+*\-\/]?$)/i;
     var paramPattern = /^([*+\-\/]?\d+)[+*\-\/]{1}(\d+)$/;
     var operationPattern = /(?<=\d+)[+\-*\/]/;
     var twoParamsFound = paramPattern.test(currentValue);
     const oneParamFound = (/^\d+$/).test(currentValue);
     if(buttonText == '=' && twoParamsFound){
       var operationString = operationPattern.exec(currentValue)?.[0] ?? '';
       var paramMatch = paramPattern.exec(currentValue);
       var param1 = Number(paramMatch?.[1]);
       var param2 = Number(paramMatch?.[2]);
       this.performOpertaion(operationString, param1, param2)
     }
     else if((buttonText === "M+" || buttonText === "M-") && oneParamFound){
       this.performOpertaion(buttonText, Number(currentValue), 0);
     }
     else if(buttonText === 'MR'){
       this.performOpertaion(buttonText, 0, 0)
     }
     else if(validationPattern.test(newValue)){
        this.screenOutput.get(this.operationInputFieldName)?.setValue(!twoParamsFound ? newValue: currentValue);
     }
  }

  performOpertaion(operation: string, param1: number, param2: number){
   
    this.performedOperation.firstParameter = param1;
    this.performedOperation.secondParameter = param2;
    switch(operation){
      case '+':
         this.performedOperation.type = OperationType.Add;
        break;
      case '-':
         this.performedOperation.type = OperationType.Subtract;
        break;
      case '*':
         this.performedOperation.type = OperationType.Multiply;
        break;
      case '/':
         this.performedOperation.type = OperationType.Divide;
        break;
      case 'M+':
        this.performedOperation.type = OperationType.MemoryPlus;
        break;
      case 'M-':
        this.performedOperation.type = OperationType.MemoryMinus;
        break;
      case 'MR':
        this.performedOperation.type = OperationType.MemoryRecall;
        break;
      default:
    }

    this._calcService.performOperation(this.performedOperation)
    .subscribe(result => {
      this.performedOperation = {...result};
      this.screenOutput.get(this.operationInputFieldName)?.setValue(this.performedOperation.arithmeticResult);
    });
  }

  getControl(controlName: string){
    return this.screenOutput.get(controlName) as FormControl<string | null>;
  }

}

