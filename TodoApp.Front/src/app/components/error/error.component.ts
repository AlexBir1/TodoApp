import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-error',
  templateUrl: './error.component.html',
  styleUrls: ['./error.component.css']
})
export class ErrorComponent {
  @Input() errors: string[] | null = []; 
  @Output() closedEvent: EventEmitter<any> = new EventEmitter<any>();

  lifeTimeSecs: number = 4;
  timeoutSecs!: any;
  intervalSecs!: any;
  isMouseIn: boolean = false;

  constructor(){
    this.startCountdown();
  }

  onMouseEnter(){
    this.lifeTimeSecs = 0;
    this.isMouseIn = true;
    clearInterval(this.intervalSecs);
    clearTimeout(this.timeoutSecs);
  }

  startCountdown(){
    this.lifeTimeSecs = 4;
    this.isMouseIn = false;
    this.intervalSecs = setInterval(() => 
    {
      if(this.lifeTimeSecs > 0)
        this.lifeTimeSecs -= 1;
    }, 1000);
    this.timeoutSecs = setTimeout(()=>
    {
      this.close()
    }, this.lifeTimeSecs * 1000);
  }

  onMouseLeave(){
    this.startCountdown();
  }

  close(){
    this.closedEvent.emit();
  }

}
