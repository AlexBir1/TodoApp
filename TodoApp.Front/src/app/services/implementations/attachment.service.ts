import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { APIResponse } from "src/app/shared/api-response/api-response";
import { AttachmentModel } from "src/app/shared/models/attachment.model";
import { environment } from "src/environments/environment.dev";
import { IAttachmentService } from "../interfaces/attachment.service";

@Injectable()
export class AttachmentService implements IAttachmentService{
    private apiControllerUrl: string = environment.apiHttpsUrl + 'Attachments/'; 

    constructor(private httpClient: HttpClient){}
    
    getAllByGoalId(goalId: string): Observable<APIResponse<AttachmentModel[]>> {
        return this.httpClient.get<APIResponse<AttachmentModel[]>>(this.apiControllerUrl, { params: {goalId: goalId}});
    }
    delete(id: string): Observable<APIResponse<AttachmentModel>> {
        return this.httpClient.delete<APIResponse<AttachmentModel>>(this.apiControllerUrl + id);
    }
    save(id: string): Observable<Blob> {
        return this.httpClient.get<Blob>(this.apiControllerUrl + id);
    }
    create(goalId: string, file: File): Observable<APIResponse<AttachmentModel>> {
        return this.httpClient.post<APIResponse<AttachmentModel>>(this.apiControllerUrl + goalId, file);
    }

}