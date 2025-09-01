import { Component, OnInit } from '@angular/core';
import { BeanService, Bean } from '../../services/bean.service';

@Component({
  selector: 'app-bean-list',
  templateUrl: './bean-list.component.html',
  styleUrls: ['./bean-list.component.css']
})
export class BeanListComponent implements OnInit {
  beans: Bean[] = [];
  filteredBeans: Bean[] = [];
  searchTerm: string = '';

  expandedBeanId: number | null = null; // for collapsible details

  constructor(private beanService: BeanService) {}

  ngOnInit(): void {
    this.loadBeans();
  }

  loadBeans(): void {
    this.beanService.getAll().subscribe({
      next: (data) => {
        this.beans = data;
        this.filteredBeans = data;
      },
      error: (err) => console.error('Failed to fetch beans', err)
    });
  }

  onSearch(): void {
    const term = this.searchTerm.toLowerCase().trim();
    if (!term) {
      this.filteredBeans = this.beans;
      return;
    }
    this.filteredBeans = this.beans.filter(bean =>
      bean.name.toLowerCase().includes(term) ||
      (bean.country ?? '').toLowerCase().includes(term)
    );
  }

  toggleDetails(beanId: number): void {
    this.expandedBeanId = this.expandedBeanId === beanId ? null : beanId;
  }
}