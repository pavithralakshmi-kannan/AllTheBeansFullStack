import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { Observable } from 'rxjs';
import { Bean } from '../models/bean';

export interface DailySelection {
  id: number;
  date: string;
  beanId: number;
  bean?: Bean; 
}

@Injectable({ providedIn: 'root' })
export class DailySelectionsService {
  private readonly base = `${environment.apiUrl}/dailyselections`;

  constructor(private http: HttpClient) {}

  getToday(): Observable<DailySelection | null> {
    return this.http.get<DailySelection | null>(`${this.base}/today`);
  }
}