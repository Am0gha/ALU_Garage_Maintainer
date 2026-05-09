import { NgModule, provideBrowserGlobalErrorListeners } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { AppRoutingModule } from './app-routing-module';
import { App } from './app';
import { AddCar } from './Components/add-car/add-car';
import { CommonLayout } from './Layouts/common-layout/common-layout';
import { CarList } from './Components/car-list/car-list';

@NgModule({
  declarations: [
    App,
    AddCar,
    CommonLayout,
    CarList
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule
  ],
  providers: [
    provideBrowserGlobalErrorListeners(),
  ],
  bootstrap: [App]
})
export class AppModule { }
