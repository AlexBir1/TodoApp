
import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { GoalModel } from "src/app/shared/models/goal.model";
import { environment } from "src/environments/environment.dev";

@Injectable()
export class GoalNotificationsService{
    private apiControllerUrl: string = environment.apiHttpsUrl + 'GoalNotifications/'; 

    constructor(private httpClient: HttpClient){}

    create(goals: GoalModel[]): Observable<void>{
        return this.httpClient.post<void>(this.apiControllerUrl, goals);
    }
    update(goalId: string, goal: GoalModel): Observable<void>{
        return this.httpClient.patch<void>(this.apiControllerUrl + goalId, goal);
    }
    delete(goalId: string): Observable<void>{
        return this.httpClient.delete<void>(this.apiControllerUrl + goalId);
    }
}