import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

import { AppComponent } from './app.component';
import { AppRoutingModule } from './app-routing.module';

import { BeanListComponent } from './components/bean-list/bean-list.component';
import { BeanDetailComponent } from './components/bean-detail/bean-detail.component';
import { BeanOfTheDayComponent } from './components/bean-of-the-day/bean-of-the-day.component';
import { OrderFormComponent } from './components/order-form/order-form.component';

@NgModule({
  declarations: [
    AppComponent,
    BeanListComponent,
    BeanDetailComponent,
    BeanOfTheDayComponent,
    OrderFormComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    CommonModule
  ],
  bootstrap: [AppComponent]
})
export class AppModule {}