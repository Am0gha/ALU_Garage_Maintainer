import { Component,OnInit,signal } from '@angular/core';
import { ICar } from '../../Interfaces/Car';
import { AluApi } from '../../Services/alu-api';
import { Router } from '@angular/router';
@Component({
  selector: 'app-car-list',
  standalone: false,
  templateUrl: './car-list.html',
  styleUrls: ['./car-list.css'],
})
export class CarList implements OnInit{
  viewType: string = 'All';
  errorMsg: string = "";
  showDivMsg: boolean = false;
  carList = signal<ICar[]>([]);
  carClass = signal<string[]>([]);
  selectCarClass: string = "";
  constructor(private readonly aluService:AluApi, private readonly router:Router){
    
  }

  fetchCars(){
    this.aluService.getCarList().subscribe(
      {
        next:(response:ICar[])=>{
          this.carList.set(response);
          console.log(this.carList());
        },

        error:(err)=>{
          this.errorMsg = err;
          this.showDivMsg = true;
        },

        complete:() => console.log("Executed the fetchCars function successfully.")
      }
    )
  }

  fetchCarsOfClass(){
      this.aluService.getCarListOfClass(this.selectCarClass).subscribe(
        {
          next:(response:ICar[]) => {
            this.carList.set(response);
            console.log(this.carList());
          },

          error:(err) => {
                        
            this.errorMsg = err;
            this.showDivMsg = true;
          },

          complete:()=>
            {
              console.log("Executed the fetchCars for a given class successfully :"+this.selectCarClass);
            }
          }
      )
  }

  onSelectCarClassChange(value: string) {
    this.selectCarClass = value;
    this.fetchCarsOfClass();
  }

  ngOnInit(): void {
    this.fetchCars();
    //We don't need to fetch the classes instead
    //We can add hyperlinks for each class
    this.aluService.getAllCarClasses().subscribe(
      {
        next:(response:string[]) => {
          this.carClass.set(response);
        },

        error: (err) => {
          this.errorMsg = err;
          this.showDivMsg = true;
        },

        complete: () => console.log("Fetched the list of car class successfully.")
      }
    )
  }

  onViewTypeChange(value: string) {
    this.viewType = value;
    if (value === 'All') {
      this.fetchCars();
    } else {
      this.carList.set([]);
    }
  }
}
