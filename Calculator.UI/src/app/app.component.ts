import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'Calculator.UI';
  calcButtonDigits: number[] = Array.from({length: 10}, (_, index)=> index+1 );
  constructor(){
    console.log(this.calcButtonDigits);
  }
}
