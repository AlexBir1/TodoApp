import { Observable } from "rxjs";
import { APIResponse } from "src/app/shared/api-response/api-response";
import { GoalModel } from "src/app/shared/models/goal.model";
import { IGoalService } from "../interfaces/goal.service";
import { HttpClient } from "@angular/common/http";
import { environment } from "src/environments/environment.dev";
import { Injectable } from "@angular/core";
import { GoalCategory } from "src/app/shared/models/goal-category.model";

@Injectable()
export class GoalService implements IGoalService{
    private apiControllerUrl: string = environment.apiHttpsUrl + 'Goals/'; 

    constructor(private httpClient: HttpClient){}
    addToCategory(goalId: string, categoryId: string): Observable<APIResponse<GoalCategory>> {
        return this.httpClient.patch<APIResponse<GoalCategory>>(this.apiControllerUrl + goalId + '/AddToCategory/' + categoryId,{})
    }
    removeFromCategory(goalId: string, categoryId: string): Observable<APIResponse<GoalCategory>> {
        return this.httpClient.patch<APIResponse<GoalCategory>>(this.apiControllerUrl + goalId + '/RemoveFromCategory/' + categoryId,{})
    }
    getAllFiltered(collectionId: string = '', searchQuery: string = '', itemsPerPage: number = 1, selectedPage: number = 1): Observable<APIResponse<GoalModel[]>> {
        return this.httpClient.get<APIResponse<GoalModel[]>>(this.apiControllerUrl, { params: {collectionId: collectionId, searchQuery: searchQuery, itemsPerPage: itemsPerPage, selectedPage: selectedPage}});
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