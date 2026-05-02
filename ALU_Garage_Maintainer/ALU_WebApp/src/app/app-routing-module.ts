import { Component, NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AddCar } from './Components/add-car/add-car';
import { CommonLayout } from './Layouts/common-layout/common-layout'
const routes: Routes = [
  {path:'home', component: CommonLayout, children:[
    {path: 'AddCar', component: AddCar}
  ]
},
  {path: '**', redirectTo: '/home'}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
