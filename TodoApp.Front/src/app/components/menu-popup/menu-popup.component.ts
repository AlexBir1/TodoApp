import { Component, EventEmitter, Output } from '@angular/core';

@Component({
  selector: 'app-menu-popup',
  templateUrl: './menu-popup.component.html',
  styleUrls: ['./menu-popup.component.css']
})
export class MenuPopupComponent {
  @Output() changeMenuOpened: EventEmitter<any> = new EventEmitter<any>();
  @Output() logOutActiveEvent: EventEmitter<any> = new EventEmitter<any>();

  onChangeMenuOpened(){
    this.changeMenuOpened.emit();
  }

  onLogOut(){
    this.logOutActiveEvent.emit();
    this.changeMenuOpened.emit();
  }

}
