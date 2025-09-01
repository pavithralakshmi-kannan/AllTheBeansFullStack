import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { BeanService } from '../../services/bean.service';

@Component({
  selector: 'app-bean-detail',
  templateUrl: './bean-detail.component.html',
  styleUrls: ['./bean-detail.component.css']
})
export class BeanDetailComponent implements OnInit {
  bean: any;

  constructor(
    private route: ActivatedRoute,
    private beanService: BeanService
  ) {}

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.beanService.getBeans({ q: id }).subscribe(data => {
        this.bean = data;
      });
    }
  }
}
