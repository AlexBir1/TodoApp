import { Component, OnInit } from '@angular/core';
import { AccountService } from './services/implementations/account.service';
import { AttachmentService } from './services/implementations/attachment.service';
import { CategoryService } from './services/implementations/category.service';
import { CollectionService } from './services/implementations/collection.service';
import { GoalService } from './services/implementations/goal.service';
import { LocalStorageService } from './services/local-storage.service';
import { AuthorizationModel } from './shared/models/authorization.model';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
  providers: [AccountService, AttachmentService, CategoryService, CollectionService, GoalService]
})
export class AppComponent implements OnInit{
  title = 'TodoApp';
  isNavabarShowing: boolean = false;

  constructor(private localStorage: LocalStorageService<AuthorizationModel>, private accountService: AccountService){}
  ngOnInit(): void {
    throw new Error('Method not implemented.');
  }

  refreshAuthToken(){
    var account = this.localStorage.getAccountFromStorage();
    if(account){
      if(account.keepAuthorized){
        if(!this.validateToken(account)){
            this.accountService.refreshAuthToken(account).subscribe({
                next: (r) => {
                    if(r.isSuccess){
                        this.localStorage.addAccountToStorage(r.data);
                    }
                },
                error: (e) =>{
                }
            });
        }
      }
      else{
          if(!this.validateToken(account)){
              this.localStorage.removeAccountFromStorage();
          }
      }
    }
  }

  validateToken(model: AuthorizationModel): boolean{
    var date = Date.now();
    var expirationDate = new Date(model.tokenExpirationDate).getTime();
    if(date > expirationDate) 
        return false;
    return true
  }

  changeIsNavabarShowing(){
    this.isNavabarShowing = !this.isNavabarShowing;
  }
}
