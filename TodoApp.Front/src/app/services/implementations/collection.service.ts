import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { APIResponse } from "src/app/shared/api-response/api-response";
import { CollectionModel } from "src/app/shared/models/collection.model";
import { environment } from "src/environments/environment.dev";
import { ICollectionService } from "../interfaces/collection.service";

@Injectable()
export class CollectionService implements ICollectionService{
    private apiControllerUrl: string = environment.apiHttpsUrl + 'Collections/'; 

    constructor(private httpClient: HttpClient){}

    create(model: CollectionModel): Observable<APIResponse<CollectionModel>> {
        return this.httpClient.post<APIResponse<CollectionModel>>(this.apiControllerUrl, model)
    }
    update(id: string, model: CollectionModel): Observable<APIResponse<CollectionModel>> {
        return this.httpClient.put<APIResponse<CollectionModel>>(this.apiControllerUrl + id, model)
    }
    delete(id: string): Observable<APIResponse<CollectionModel>> {
        return this.httpClient.delete<APIResponse<CollectionModel>>(this.apiControllerUrl + id)
    }
    getById(id: string): Observable<APIResponse<CollectionModel>> {
        return this.httpClient.get<APIResponse<CollectionModel>>(this.apiControllerUrl + id)
    }
    getAll(): Observable<APIResponse<CollectionModel[]>> {
        return this.httpClient.get<APIResponse<CollectionModel[]>>(this.apiControllerUrl)
    }
    getAllFiltered(accountId: string): Observable<APIResponse<CollectionModel[]>> {
        return this.httpClient.get<APIResponse<CollectionModel[]>>(this.apiControllerUrl, {params:{accountId: accountId}})
    }
}