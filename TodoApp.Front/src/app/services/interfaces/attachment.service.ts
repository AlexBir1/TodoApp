import { Observable } from "rxjs";
import { APIResponse } from "src/app/shared/api-response/api-response";
import { AttachmentModel } from "src/app/shared/models/attachment.model";

export interface IAttachmentService{
   getAllByGoalId(goalId: string): Observable<APIResponse<AttachmentModel[]>>;
   delete(id: string): Observable<APIResponse<AttachmentModel>>;
   save(id: string): Observable<Blob>;
   create(goalId: string, file: File): Observable<APIResponse<AttachmentModel>>;
}