import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { BeanListComponent } from './components/bean-list/bean-list.component';
import { BeanDetailComponent } from './components/bean-detail/bean-detail.component';
import { BeanOfTheDayComponent } from './components/bean-of-the-day/bean-of-the-day.component';
import { OrderFormComponent } from './components/order-form/order-form.component';

const routes: Routes = [
  { path: '', component: BeanListComponent },
  { path: 'beans/:id', component: BeanDetailComponent },
  { path: 'bean-of-the-day', component: BeanOfTheDayComponent },
  { path: 'order', component: OrderFormComponent },
  { path: '**', redirectTo: '' } // fallback route
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {}
