import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-notification',
  templateUrl: './notification.component.html',
  styleUrls: ['./notification.component.css']
})
export class NotificationComponent {
  @Input() message: string | null = '';
  lifeTimeSecs: number = 4;
  @Output() closedEvent: EventEmitter<any> = new EventEmitter<any>();

  constructor(){
    setInterval(() => 
    {
      if(this.lifeTimeSecs > 0)
        this.lifeTimeSecs = this.lifeTimeSecs - 1;
    }, 1000);
    setTimeout(()=>this.close(), 4000);
  }

  close(){
    this.closedEvent.emit();
  }
}
