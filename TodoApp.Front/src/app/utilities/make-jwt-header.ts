import { HttpHeaders } from "@angular/common/http";

export function makeHeaderWithAuthorization(token: string) {
    let headers = new HttpHeaders();
    headers = headers.set('Authorization', 'Bearer ' + token);
    return headers;
  }