import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { hrvList, hrvOutput, hrvRecord } from '../Structures/structures';

@Injectable({
  providedIn: 'root'
})
export class HRVRecordService {

  constructor(private http: HttpClient) { 
  }

  public AddHrv(record: hrvRecord) : Observable<hrvOutput> { 
    return this.http.post<hrvOutput>("/api/HRV", record); 
  }

  public GetHrv() : Observable<hrvList[]> { 
    return this.http.get<hrvList[]>("/api/HRV");
  }

  public UpdateHrv(record: hrvRecord, id: number) : Observable<hrvOutput> { 
    return this.http.put<hrvOutput>(`/api/HRV/${id}`, record);
  }

  public GetAHrv(id: number) : Observable<hrvOutput> { 
    return this.http.get<hrvOutput>(`/api/HRV/${id}`); 
  }

  public DeleteHRV(id: number) : Observable<any> { 
    return this.http.delete<any>(`/api/HRV/${id}`);
  }
}
