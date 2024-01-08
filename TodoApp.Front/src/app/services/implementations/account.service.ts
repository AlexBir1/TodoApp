import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { APIResponse } from "src/app/shared/api-response/api-response";
import { AccountModel } from "src/app/shared/models/account.model";
import { AuthorizationModel } from "src/app/shared/models/authorization.model";
import { SignInModel } from "src/app/shared/models/sign-in.model";
import { SignUpModel } from "src/app/shared/models/sign-up.model";
import { environment } from "src/environments/environment.dev";
import { IAccountService } from "../interfaces/account.service";

@Injectable()
export class AccountService implements IAccountService{
    private apiControllerUrl: string = environment.apiHttpsUrl + 'Account/'; 

    constructor(private httpClient: HttpClient){}

    signUp(model: SignUpModel): Observable<APIResponse<AuthorizationModel>> {
        return this.httpClient.put<APIResponse<AuthorizationModel>>(this.apiControllerUrl + 'SignUp', model);
    }
    signIn(model: SignInModel): Observable<APIResponse<AuthorizationModel>> {
        return this.httpClient.put<APIResponse<AuthorizationModel>>(this.apiControllerUrl + 'SignIn', model);
    }
    getById(id: string): Observable<APIResponse<AccountModel>> {
        return this.httpClient.get<APIResponse<AccountModel>>(this.apiControllerUrl + id);
    }
    refreshAuthToken(model: AuthorizationModel): Observable<APIResponse<AuthorizationModel>> {
        return this.httpClient.put<APIResponse<AuthorizationModel>>(this.apiControllerUrl + 'RefreshAuthToken', model);
    }

}