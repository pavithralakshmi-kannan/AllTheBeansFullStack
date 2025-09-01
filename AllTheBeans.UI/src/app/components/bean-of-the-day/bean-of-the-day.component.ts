import { Component, OnInit } from '@angular/core';
import { BeanService, Bean } from '../../services/bean.service';

@Component({
  selector: 'app-bean-of-the-day',
  templateUrl: './bean-of-the-day.component.html',
  styleUrls: ['./bean-of-the-day.component.css']
})
export class BeanOfTheDayComponent implements OnInit {
  beanOfTheDay: Bean | null = null;
  message: string | null = null;

  constructor(private beanService: BeanService) {}

  ngOnInit(): void {
    this.beanService.getBeanOfTheDay().subscribe({
      next: (data) => {
        this.beanOfTheDay = data;
        this.message = null;
      },
      error: (err) => {
        console.error('Failed to load bean of the day', err);
        this.message = 'Could not load Bean of the Day. Please try again later.';
      }
    });
  }
}