import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Route, Router } from '@angular/router';
import { AccountService } from 'src/app/services/implementations/account.service';
import { AuthorizedAccountService } from 'src/app/services/implementations/authorized-account.service';
import { LocalStorageService } from 'src/app/services/local-storage.service';
import { AuthorizationModel } from 'src/app/shared/models/authorization.model';
import { SignInModel } from 'src/app/shared/models/sign-in.model';
import { SignUpModel } from 'src/app/shared/models/sign-up.model';

@Component({
  selector: 'app-auth',
  templateUrl: './auth.component.html',
  styleUrls: ['./auth.component.css']
})
export class AuthComponent implements OnInit{
  isSignIn: boolean = true;
  signInForm!: FormGroup;
  signUpForm!: FormGroup;
  isLoadingState: boolean = false;

  responseErrors: string[] | null = null;

  constructor(private accountService: AccountService, private authorizedAccountService: AuthorizedAccountService, 
    private localStorageService: LocalStorageService, private router: Router){}

  ngOnInit(): void {
    this.initializeSignInForm();
  }

  initializeSignUpForm(){
    this.signUpForm = new FormGroup({
      username: new FormControl('', Validators.required),
      email: new FormControl('', Validators.required),
      phoneNumber: new FormControl('', Validators.required),
      password: new FormControl('', Validators.required),
      passwordConfirm: new FormControl('', Validators.required),
      keepAuthorized: new FormControl(false, Validators.required),
    });
  }

  initializeSignInForm(){
    this.signInForm = new FormGroup({
      userIdentifier: new FormControl('', Validators.required),
      password: new FormControl('', Validators.required),
      keepAuthorized: new FormControl(false, Validators.required),
    });
  }

  signUp(){
    let signUpModel: SignUpModel = this.signUpForm.value;
    this.isLoadingState = true;
    this.accountService.signUp(signUpModel).subscribe({
      next: (result) => {
        this.isLoadingState = false;
        if(result.isSuccess){
          this.authorizedAccountService.addAccount(result.data);
          this.localStorageService.addAccountToStorage(result.data);
          this.router.navigateByUrl('/Dashboard');
        }
        else 
          this.responseErrors = result.messages;
      },
      error: (error) => {
        this.isLoadingState = false;
      }
    });
  }

  signIn(){
    let signInModel: SignInModel = this.signInForm.value;
    this.isLoadingState = true;
    this.accountService.signIn(signInModel).subscribe({
      next: (result) => {
        this.isLoadingState = false;
        if(result.isSuccess){
          this.authorizedAccountService.addAccount(result.data);
          this.localStorageService.addAccountToStorage(result.data);
          this.router.navigateByUrl('/Dashboard');
        }
        else 
          this.responseErrors = result.messages;
      },
      error: (error) => {
        this.isLoadingState = false;
      }
    });
  }
  
  changeIsSignIn(){
    this.isSignIn = !this.isSignIn;
    if(this.isSignIn)
      this.initializeSignInForm();
    else
    this.initializeSignUpForm();
  }

  onClosedError(){
    this.responseErrors = null;
  }
}
