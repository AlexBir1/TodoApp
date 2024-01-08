import { Observable } from "rxjs";
import { APIResponse } from "src/app/shared/api-response/api-response";
import { GoalModel } from "src/app/shared/models/goal.model";
import { IService } from "./service";

export interface IGoalService extends IService<GoalModel>{
    getAllFiltered(collectionId: string, searchQuery: string): Observable<APIResponse<GoalModel[]>>;
    addToCategory(goalId: string, categoryId: string): Observable<APIResponse<GoalModel>>;
    removeFromCategory(goalId: string, categoryId: string): Observable<APIResponse<GoalModel>>;
}