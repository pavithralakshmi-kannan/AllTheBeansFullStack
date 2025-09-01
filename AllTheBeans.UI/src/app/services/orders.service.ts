import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { Observable } from 'rxjs';
import { Order } from '../models/order';

@Injectable({ providedIn: 'root' })
export class OrdersService {
  private readonly base = `${environment.apiUrl}/orders`;

  constructor(private http: HttpClient) {}

  create(order: Omit<Order, 'id' | 'orderDate'>): Observable<Order> {
    return this.http.post<Order>(this.base, order);
  }
}
