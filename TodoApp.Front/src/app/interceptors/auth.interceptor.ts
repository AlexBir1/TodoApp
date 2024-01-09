import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { AccountService } from "../services/implementations/account.service";
import { AuthorizedAccountService } from "../services/implementations/authorized-account.service";
import { LocalStorageService } from "../services/local-storage.service";
import { AuthorizationModel } from "../shared/models/authorization.model";
import { makeHeaderWithAuthorization } from "../utilities/make-jwt-header";

@Injectable()
export class AuthInterceptor implements HttpInterceptor{
    constructor(private localStorage: LocalStorageService, private accountService: AccountService, private authAccountService: AuthorizedAccountService){}

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        var account = this.localStorage.getAccountFromStorage();

        if(req.url.includes("RefreshAuthToken") && req.method === "PUT")
            return next.handle(req);

        if(account){
            if(account.keepAuthorized){
                if(!this.validateToken(account)){
                    this.accountService.refreshAuthToken(account).subscribe({
                        next: (r) => {
                            if(r.isSuccess){
                                this.localStorage.addAccountToStorage(r.data);
                                this.authAccountService.addAccount(r.data);
                            }
                        },
                        error: (e) =>{
                        }
                    });
                }
            }
            else{
                this.localStorage.removeAccountFromStorage();
                this.authAccountService.removeAccount();
            }
            return next.handle(req.clone({headers: makeHeaderWithAuthorization(account.token)}));
        }
        return next.handle(req);
    }

    validateToken(model: AuthorizationModel): boolean{
        var date = Date.now();
        var expirationDate = new Date(model.tokenExpirationDate).getTime();
        if(date > expirationDate) 
            return false;
        return true
    }
}