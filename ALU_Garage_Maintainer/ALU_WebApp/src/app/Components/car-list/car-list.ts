import { Component,OnInit } from '@angular/core';
import { ICar } from '../../Interfaces/Car';
import { AluApi } from '../../Services/alu-api';
import { Router } from '@angular/router';
@Component({
  selector: 'app-car-list',
  standalone: false,
  templateUrl: './car-list.html',
  styleUrl: './car-list.css',
})
export class CarList implements OnInit{
  viewType: string = "";
  errorMsg: string = "";
  showDivMsg: boolean = false;
  carList: ICar[] = [];
  carClass: string[] = [];
  selectCarClass: string = "";
  constructor(private readonly aluService:AluApi, private readonly router:Router){
    
  }

  fetchCars(){
    if(this.viewType == "All")
    {
      this.aluService.getCarList().subscribe(
        {
          next:(response:ICar[])=>{
            this.carList = response;
            console.log(this.carList);
          },

          error:(err)=>{
            this.errorMsg = err;
            this.showDivMsg = true;
          },

          complete:() => console.log("Executed the fetchCars function successfully.")
        }
      )
    }

    else if(this.viewType == "Class")
    {
      this.aluService.getCarListOfClass(this.selectCarClass).subscribe(
        {
          next:(response:ICar[]) => {
            this.carList = response;
            console.log(this.carList);
          },

          error:(err) => {
            this.errorMsg = err;
            this.showDivMsg = true;
          },

          complete:()=>console.log("Executed the fetchCars for a given class successfully.")
        }
      )
    }
  }

  ngOnInit(): void {
    this.aluService.getAllCarClasses().subscribe(
      {
        next:(response:string[]) => {
          this.carClass = response;
        },

        error: (err) => {
          this.errorMsg = err;
          this.showDivMsg = true;
        },

        complete: () => console.log("Fetched the list of car class successfully.")
      }
    )
  }
}
