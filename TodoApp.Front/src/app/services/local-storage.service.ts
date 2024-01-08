import { Injectable } from "@angular/core";
import { environment } from "src/environments/environment.dev";
import { AuthorizationModel } from "../shared/models/authorization.model";

@Injectable()
export class LocalStorageService<T>{
    addAccountToStorage(model: AuthorizationModel){
        localStorage.setItem(environment.accountKey, JSON.stringify(model));
        return model;
    }
    getAccountFromStorage(){
        return JSON.parse(localStorage.getItem(environment.accountKey) as string) as AuthorizationModel;
    }

    removeAccountFromStorage(){
        localStorage.removeItem(environment.accountKey);
    }

    setItemInStorage(item: T, key: string){
        localStorage.setItem(key, JSON.stringify(item));
        return item;
    }

    getItemFromStorage<T>(key: string){
        return JSON.parse(localStorage.getItem(key) as string) as T;
    }

    getItemJsonFromStorage(key: string): string{
        return localStorage.getItem(key) as string
    }
}