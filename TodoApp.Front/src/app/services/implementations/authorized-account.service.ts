import { Injectable } from "@angular/core";
import { BehaviorSubject, ReplaySubject } from "rxjs";
import { AuthorizationModel } from "src/app/shared/models/authorization.model";

@Injectable()
export class AuthorizedAccountService{
    private accountSource = new BehaviorSubject<AuthorizationModel | null>(null);
    
    currentAccount$ = this.accountSource.asObservable();

    addAccount(account: AuthorizationModel){
        this.accountSource.next(account);
    }
     removeAccount(){
        this.accountSource.next(null);
     }
}