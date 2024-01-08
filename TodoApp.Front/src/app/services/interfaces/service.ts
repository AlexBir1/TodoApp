import { Observable } from "rxjs";
import { APIResponse } from "../../shared/api-response/api-response";

export interface IService<T>{
    create(model: T): Observable<APIResponse<T>>;
    update(id: string, model: T): Observable<APIResponse<T>>;
    delete(id: string): Observable<APIResponse<T>>;
    getById(id: string): Observable<APIResponse<T>>;
    getAll(): Observable<APIResponse<T[]>>;
}