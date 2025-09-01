export interface Order {
  id?: number;
  customerName: string;
  address: string;
  beanId: number;
  quantity: number;
  orderDate?: string;
}