import { Observable } from "rxjs";
import { APIResponse } from "src/app/shared/api-response/api-response";
import { GoalModel } from "src/app/shared/models/goal.model";
import { IGoalService } from "../interfaces/goal.service";
import { HttpClient } from "@angular/common/http";
import { environment } from "src/environments/environment.dev";
import { Injectable } from "@angular/core";

@Injectable()
export class GoalService implements IGoalService{
    private apiControllerUrl: string = environment.apiHttpsUrl + 'Categories/'; 

    constructor(private httpClient: HttpClient){}
    addToCategory(goalId: string, categoryId: string): Observable<APIResponse<GoalModel>> {
        return this.httpClient.patch<APIResponse<GoalModel>>(this.apiControllerUrl + goalId + '/AddToCategory', { params: {categoryId: categoryId}})
    }
    removeFromCategory(goalId: string, categoryId: string): Observable<APIResponse<GoalModel>> {
        return this.httpClient.patch<APIResponse<GoalModel>>(this.apiControllerUrl + goalId + '/RemoveFromCategory', { params: {categoryId: categoryId}})
    }
    getAllFiltered(collectionId: string, searchQuery: string): Observable<APIResponse<GoalModel[]>> {
        return this.httpClient.get<APIResponse<GoalModel[]>>(this.apiControllerUrl, { params: {collectionId: collectionId, searchQuery: searchQuery}});
    }
    create(model: GoalModel): Observable<APIResponse<GoalModel>> {
        return this.httpClient.post<APIResponse<GoalModel>>(this.apiControllerUrl, model);
    }
    update(id: string, model: GoalModel): Observable<APIResponse<GoalModel>> {
        return this.httpClient.put<APIResponse<GoalModel>>(this.apiControllerUrl + id, model);
    }
    delete(id: string): Observable<APIResponse<GoalModel>> {
        return this.httpClient.delete<APIResponse<GoalModel>>(this.apiControllerUrl + id);
    }
    getById(id: string): Observable<APIResponse<GoalModel>> {
        return this.httpClient.get<APIResponse<GoalModel>>(this.apiControllerUrl + id);
    }
    getAll(): Observable<APIResponse<GoalModel[]>> {
        return this.httpClient.get<APIResponse<GoalModel[]>>(this.apiControllerUrl);
    }

}