import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { APIResponse } from "src/app/shared/api-response/api-response";
import { CategoryModel } from "src/app/shared/models/category.model";
import { environment } from "src/environments/environment.dev";
import { ICategoryService } from "../interfaces/category.service";

@Injectable()
export class CategoryService implements ICategoryService{
    private apiControllerUrl: string = environment.apiHttpsUrl + 'Categories/'; 

    constructor(private httpClient: HttpClient){}

    create(model: CategoryModel): Observable<APIResponse<CategoryModel>> {
        return this.httpClient.post<APIResponse<CategoryModel>>(this.apiControllerUrl, model);
    }
    update(id: string, model: CategoryModel): Observable<APIResponse<CategoryModel>> {
        return this.httpClient.put<APIResponse<CategoryModel>>(this.apiControllerUrl + id, model);
    }
    delete(id: string): Observable<APIResponse<CategoryModel>> {
        return this.httpClient.delete<APIResponse<CategoryModel>>(this.apiControllerUrl + id);
    }
    getById(id: string): Observable<APIResponse<CategoryModel>> {
        return this.httpClient.get<APIResponse<CategoryModel>>(this.apiControllerUrl + id);
    }
    getAll(): Observable<APIResponse<CategoryModel[]>> {
        return this.httpClient.get<APIResponse<CategoryModel[]>>(this.apiControllerUrl);
    }

}