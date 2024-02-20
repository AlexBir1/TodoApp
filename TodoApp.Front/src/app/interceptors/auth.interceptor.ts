import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { AccountService } from "../services/implementations/account.service";
import { AuthorizedAccountService } from "../services/implementations/authorized-account.service";
import { LocalStorageService } from "../services/local-storage.service";
import { AuthorizationModel } from "../shared/models/authorization.model";
import { makeHeaderWithAuthorization } from "../utilities/make-jwt-header";

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
    constructor(private localStorage: LocalStorageService, private accountService: AccountService, private authAccountService: AuthorizedAccountService) { }

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        let account = this.localStorage.getAccountFromStorage();
        if (!account)
            return next.handle(req);

        if (!this.validateToken(account)) {
            if (account.keepAuthorized) {
                this.accountService.refreshAuthToken(account).subscribe({
                    next: (r) => {
                        if (r.isSuccess) {
                            this.localStorage.addAccountToStorage(r.data);
                            this.authAccountService.addAccount(r.data);
                        }
                    }
                });
            }
            else {
                this.localStorage.removeAccountFromStorage();
                this.authAccountService.removeAccount();
            }
        }

        return next.handle(req.clone({ headers: makeHeaderWithAuthorization(account.token) }));
    }

    validateToken(model: AuthorizationModel): boolean {
        let date = Date.now();
        let expirationDate = new Date(model.tokenExpirationDate).getTime();

        if (date > expirationDate) {
            return false;
        }

        return true;
    }
}