import { Injectable } from "@angular/core";
import { ReplaySubject } from "rxjs";
import { AuthorizationModel } from "src/app/shared/models/authorization.model";

@Injectable()
export class AuthorizedAccountService{
    private accountSource = new ReplaySubject<AuthorizationModel | null>(1);
    
    currentAccount$ = this.accountSource.asObservable();

    addAccount(account: AuthorizationModel){
        this.accountSource.next(account);
    }
     removeAccount(){
        this.accountSource.next(null);
     }
}