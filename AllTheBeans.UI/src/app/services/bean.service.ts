import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from '../../environments/environment';

export interface Bean {
  id: number;
  name: string;
  description?: string;
  country?: string;
  colour?: string;
  cost: number;
  image?: string;
}

export interface Order {
  id?: number;  // DB Generate this automatically
  customerName: string;
  address: string;
  beanId: number;
  quantity: number;
  orderDate?: string;
}

@Injectable({ providedIn: 'root' })
export class BeanService {
  private api = environment.apiUrl;

  constructor(private http: HttpClient) {}

  private mapToBean(item: any): Bean {
    return {
      id: item._id || item.id,
      name: item.Name || item.name || '',
      description: item.Description || item.description || '',
      country: item.Country || item.country || '',
      colour: item.colour || '',
      cost: parseFloat(String(item.Cost || item.cost || 0).toString().replace('£', '')),
      image: item.Image || item.image || ''
    };
  }

  getBeans(filters?: {
    q?: string;
    country?: string;
    colour?: string;
    minCost?: number;
    maxCost?: number;
    page?: number;
    pageSize?: number;
    sort?: string;
  }): Observable<Bean[]> {
    let params = new HttpParams();
    for (const [k, v] of Object.entries(filters ?? {})) {
      if (v !== undefined && v !== null && v !== '') {
        params = params.set(k, String(v));
      }
    }
    return this.http.get<any[]>(`${this.api}/beans`, { params }).pipe(
      map(list => list.map(item => this.mapToBean(item)))
    );
  }

  getAll(): Observable<Bean[]> {
    return this.http.get<any[]>(`${this.api}/beans`).pipe(
      map(list => list.map(item => this.mapToBean(item)))
    );
  }

  getBeanOfTheDay(): Observable<Bean> {
    return this.http.get<any>(`${this.api}/beans/bean-of-the-day`).pipe(
      map(item => this.mapToBean(item))
    );
  }

  placeOrder(order: Order): Observable<Order> {
    return this.http.post<Order>(`${this.api}/orders`, order);
  }

  getOrders(): Observable<Order[]> {
    return this.http.get<Order[]>(`${this.api}/orders`);
  }

  getOrder(id: number): Observable<Order> {
    return this.http.get<Order>(`${this.api}/orders/${id}`);
  }

  updateOrder(order: Order): Observable<void> {
    return this.http.put<void>(`${this.api}/orders/${order.id}`, order);
  }

  deleteOrder(id: number): Observable<void> {
    return this.http.delete<void>(`${this.api}/orders/${id}`);
  }
}