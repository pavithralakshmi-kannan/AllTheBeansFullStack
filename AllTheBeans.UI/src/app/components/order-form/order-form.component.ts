import { Component, OnInit } from '@angular/core';
import { BeanService, Bean, Order } from '../../services/bean.service';

@Component({
  selector: 'app-order-form',
  templateUrl: './order-form.component.html',
  styleUrls: ['./order-form.component.css']
})
export class OrderFormComponent implements OnInit {
  beans: Bean[] = [];
  selectedBean: Bean | null = null;

  quantity: number = 1;
  customerName: string = '';
  address: string = '';
  message: string | null = null;

  constructor(private beanService: BeanService) {}

  ngOnInit(): void {
    // Fetch all beans for dropdown
    this.beanService.getAll().subscribe({
      next: (data) => this.beans = data,
      error: (err) => {
        console.error('Failed to fetch beans', err);
        this.message = 'Failed to load beans. Please try again later.';
      }
    });
  }

  get total(): number {
    return (this.selectedBean?.cost || 0) * this.quantity;
  }

  placeOrder(): void {
    if (!this.selectedBean || !this.selectedBean.id) {
      this.message = 'Please select a bean first.';
      return;
    }
    if (!this.customerName.trim() || !this.address.trim()) {
      this.message = 'Please provide your name and address.';
      return;
    }
    if (this.quantity <= 0) {
      this.message = 'Quantity must be at least 1.';
      return;
    }

    const order: Order = {
      customerName: this.customerName.trim(),
      address: this.address.trim(),
      beanId: this.selectedBean.id,
      quantity: this.quantity
    };

    console.log('Sending order:', order);

    this.beanService.placeOrder(order).subscribe({
      next: () => {
        this.message = 'Order placed successfully!';
        // Reset form
        this.selectedBean = null;
        this.customerName = '';
        this.address = '';
        this.quantity = 1;
      },
      error: (err) => {
        console.error('Order placement failed:', err);
        this.message = 'Failed to place order. Please try again.';
      }
    });
  }
}