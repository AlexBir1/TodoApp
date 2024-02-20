import { Component, EventEmitter, Input, Output } from '@angular/core';
import { UserNotificationModel } from 'src/app/shared/models/user-notification.model';

@Component({
  selector: 'app-goal-notification',
  templateUrl: './goal-notification.component.html',
  styleUrls: ['./goal-notification.component.css']
})
export class GoalNotificationComponent {
  @Input() notification: UserNotificationModel | null = null;
  @Output() closedEvent: EventEmitter<any> = new EventEmitter<any>();
  @Output() showGoalByIdEvent: EventEmitter<string> = new EventEmitter<string>();

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

  closeByButton(event: MouseEvent){
    event.stopPropagation();
    this.closedEvent.emit();
  }

  showGoalById(){
    this.showGoalByIdEvent.emit(this.notification?.goalId);
  }
}
