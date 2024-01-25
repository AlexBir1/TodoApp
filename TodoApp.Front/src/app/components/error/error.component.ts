import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-error',
  templateUrl: './error.component.html',
  styleUrls: ['./error.component.css']
})
export class ErrorComponent {
  @Input() errors: string[] | null = []; 
  lifeTimeSecs: number = 4;
  @Output() closedEvent: EventEmitter<any> = new EventEmitter<any>();

  constructor(){
    this.lifeTimeSecs = 4;
    setInterval(() => 
    {
      if(this.lifeTimeSecs > 0)
        this.lifeTimeSecs = this.lifeTimeSecs - 1;
    }, 1000);
    setTimeout(()=>
    {
      this.close()
    }, 4000);
  }

  close(){
    this.closedEvent.emit();
  }

}
