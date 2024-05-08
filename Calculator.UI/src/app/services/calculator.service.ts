import { Injectable, inject, isDevMode } from '@angular/core';
import { OperationDto, OperationType } from '../models/calculator.models';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CalculatorService {

  private _httpClient = inject(HttpClient);
  constructor() { }

  get serviceUrl(){
    return isDevMode() ? 'https://localhost:7279/api/Calculator' : '';
  }

  getOperationHistory(): Observable<Array<OperationDto[]>>{
    return this._httpClient.get<Array<OperationDto[]>>(this.serviceUrl);
  }

  getOperationHistoryById(masterId: number): Observable<Array<OperationDto>>{
    return this._httpClient.get<Array<OperationDto>>(`${this.serviceUrl}/${masterId}`);
  }

  performOperation(operation: OperationDto): Observable<OperationDto>{
    let result: Observable<OperationDto>  = of(new OperationDto());
    switch(operation.type){
      case OperationType.Add:
        result = this._httpClient.get<OperationDto>(`${this.serviceUrl}/Add/${operation.firstParameter}/${operation.secondParameter}?masterId=${operation.masterId}`);
      break;
      case OperationType.Subtract:
         result = this._httpClient.get<OperationDto>(`${this.serviceUrl}/Subtract/${operation.firstParameter}/${operation.secondParameter}?masterId=${operation.masterId}`);
        break;
      case OperationType.Divide:
         result = this._httpClient.get<OperationDto>(`${this.serviceUrl}/Divide/${operation.firstParameter}/${operation.secondParameter}?masterId=${operation.masterId}`);
        break;
      case OperationType.Multiply:
         result = this._httpClient.get<OperationDto>(`${this.serviceUrl}/Multiply/${operation.firstParameter}/${operation.secondParameter}?masterId=${operation.masterId}`);
        break;
      case OperationType.MemoryMinus:
         result = this._httpClient.get<OperationDto>(`${this.serviceUrl}/MRMinus/${operation.firstParameter}?masterId=${operation.masterId}`);
       break;
      case OperationType.MemoryPlus:
        result = this._httpClient.get<OperationDto>(`${this.serviceUrl}/MRPlus/${operation.firstParameter}?masterId=${operation.masterId}`);
        break;
      case OperationType.MemoryRecall:
        result = this._httpClient.get<OperationDto>(`${this.serviceUrl}/MR/${operation.masterId}`);
        break;
    }
     return result;
  }

  clearHistory(){
    return this._httpClient.delete(this.serviceUrl);
  }

  clearHistoryById(masterId: number): Observable<OperationDto>{
    return this._httpClient.delete<OperationDto>(`${this.serviceUrl}/${masterId}`);
  }
}
