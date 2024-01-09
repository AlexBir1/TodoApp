import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';

@Component({
  selector: 'app-navbar-top',
  templateUrl: './navbar-top.component.html',
  styleUrls: ['./navbar-top.component.css']
})
export class NavbarTopComponent implements OnInit{
  @Output() changeIsNavbarShowingEvent: EventEmitter<any> = new EventEmitter<any>();
  @Output() logOutEvent: EventEmitter<any> = new EventEmitter<any>();
  @Output() searchGoalsEvent: EventEmitter<string> = new EventEmitter<string>();

  searchForm!: FormGroup;

  ngOnInit(): void {
    this.initializeSignUpForm();
  }

  initializeSignUpForm(){
    this.searchForm = new FormGroup({
      query: new FormControl(''),
    });
  }

  onlogOut(){
    this.logOutEvent.emit();
  }
  onChangeIsNavbarShowing(){
    this.changeIsNavbarShowingEvent.emit();
  }

  onSearchGoals(){
    this.searchGoalsEvent.emit(this.searchForm.controls['query'].value);
  }
}
