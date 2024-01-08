import { Observable } from "rxjs";
import { APIResponse } from "src/app/shared/api-response/api-response";
import { AccountModel } from "src/app/shared/models/account.model";
import { AuthorizationModel } from "src/app/shared/models/authorization.model";
import { SignInModel } from "src/app/shared/models/sign-in.model";
import { SignUpModel } from "src/app/shared/models/sign-up.model";

export interface IAccountService{
    signUp(model: SignUpModel): Observable<APIResponse<AuthorizationModel>>;
    signIn(model: SignInModel): Observable<APIResponse<AuthorizationModel>>;
    getById(id: string): Observable<APIResponse<AccountModel>>;
    refreshAuthToken(model: AuthorizationModel): Observable<APIResponse<AuthorizationModel>>;
}