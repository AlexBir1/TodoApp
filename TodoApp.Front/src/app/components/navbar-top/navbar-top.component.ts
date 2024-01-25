import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-navbar-top',
  templateUrl: './navbar-top.component.html',
  styleUrls: ['./navbar-top.component.css']
})
export class NavbarTopComponent implements OnInit{
  @Output() changeIsNavbarShowingEvent: EventEmitter<any> = new EventEmitter<any>();
  @Output() logOutActiveEvent: EventEmitter<any> = new EventEmitter<any>();
  @Output() searchGoalsEvent: EventEmitter<string> = new EventEmitter<string>();
  @Output() changeMenuOpened: EventEmitter<any> = new EventEmitter<any>();

  searchForm!: FormGroup;

  ngOnInit(): void {
    this.initializeSignUpForm();
  }

  initializeSignUpForm(){
    this.searchForm = new FormGroup({
      query: new FormControl(''),
    });
  }

  onChangeMenuOpened(){
    this.changeMenuOpened.emit();
  }

  onlogOut(){
    this.logOutActiveEvent.emit();
  }
  onChangeIsNavbarShowing(){
    this.changeIsNavbarShowingEvent.emit();
  }

  onSearchGoals(){
    let value = this.searchForm.controls['query'].value;
    if(value)
      this.searchGoalsEvent.emit(value);
  }

  onEmptyValue(){
    let value = this.searchForm.controls['query'].value;
    if(!value)
      this.searchGoalsEvent.emit("");
  }
}
