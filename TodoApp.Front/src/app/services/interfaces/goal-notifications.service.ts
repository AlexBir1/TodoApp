import { Observable } from "rxjs";
import { APIResponse } from "src/app/shared/api-response/api-response";
import { GoalModel } from "src/app/shared/models/goal.model";

export interface IGoalNotificationsService{
    create(goals: GoalModel[]): Observable<void>;
    update(goalId: string, newDateTime: Date): Observable<void>;
    delete(goalId: string): Observable<void>;
}