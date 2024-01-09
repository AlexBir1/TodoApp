import { Injectable } from "@angular/core";
import { environment } from "src/environments/environment.dev";
import { AuthorizationModel } from "../shared/models/authorization.model";

@Injectable()
export class LocalStorageService{
    addAccountToStorage(model: AuthorizationModel){
        localStorage.setItem(environment.accountKey, JSON.stringify(model));
        return model;
    }
    getAccountFromStorage(){
        let account = JSON.parse(localStorage.getItem(environment.accountKey) as string)
        return account as AuthorizationModel;
    }

    removeAccountFromStorage(){
        localStorage.removeItem(environment.accountKey);
    }

    setItemInStorage(item: any, key: string){
        localStorage.setItem(key, JSON.stringify(item));
        return item;
    }

    getItemFromStorage(key: string){
        return JSON.parse(localStorage.getItem(key) as string);
    }
}