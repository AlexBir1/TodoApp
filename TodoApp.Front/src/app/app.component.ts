import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { Router } from '@angular/router';
import { AuthInterceptor } from './interceptors/auth.interceptor';
import { AccountService } from './services/implementations/account.service';
import { AttachmentService } from './services/implementations/attachment.service';
import { AuthorizedAccountService } from './services/implementations/authorized-account.service';
import { CategoryService } from './services/implementations/category.service';
import { CollectionService } from './services/implementations/collection.service';
import { GoalService } from './services/implementations/goal.service';
import { LocalStorageService } from './services/local-storage.service';
import { AuthorizationModel } from './shared/models/authorization.model';
import { CollectionModel } from './shared/models/collection.model';
import { GoalModel } from './shared/models/goal.model';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
  providers: []
})
export class AppComponent implements OnInit{
  constructor(private localStorageService: LocalStorageService, private authAccountService: AuthorizedAccountService, private router: Router){}
  ngOnInit(): void {
    let account = this.localStorageService.getAccountFromStorage();
    if(account){
      this.authAccountService.addAccount(account);
      this.router.navigateByUrl('/Dashboard');
    }
    else{
      this.authAccountService.removeAccount();
      this.router.navigateByUrl('/Auth');
    }
  }
  title = 'TodoApp';

}
