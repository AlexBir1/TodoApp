import { Observable } from "rxjs";
import { APIResponse } from "src/app/shared/api-response/api-response";
import { GoalModel } from "src/app/shared/models/goal.model";
import { IService } from "./service";
import { GoalCategory } from "src/app/shared/models/goal-category.model";

export interface IGoalService extends IService<GoalModel>{
    getAllFiltered(collectionId: string, searchQuery: string, itemsPerPage: number, selectedPage: number): Observable<APIResponse<GoalModel[]>>;
    addToCategory(goalId: string, categoryId: string): Observable<APIResponse<GoalCategory>>;
    removeFromCategory(goalId: string, categoryId: string): Observable<APIResponse<GoalCategory>>;
}