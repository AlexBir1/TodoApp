import { Component, EventEmitter, Output } from '@angular/core';

@Component({
  selector: 'app-logout-popup',
  templateUrl: './logout-popup.component.html',
  styleUrls: ['./logout-popup.component.css']
})
export class LogoutPopupComponent {
  @Output() closeEvent: EventEmitter<any> = new EventEmitter<any>();
  @Output() logOutEvent: EventEmitter<any> = new EventEmitter<any>();

  onClose(){
    this.closeEvent.emit();
  }

  logOut(){
    this.logOutEvent.emit();
  }
}
