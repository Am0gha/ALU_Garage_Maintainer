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
  carName: string = '';
  errorMsg: string = "";
  searchCarName: string = "";
  showDivMsg: boolean = false;
  carList = signal<ICar[]>([]);
  visibleCarList = this.carList();
  carClass = signal<string[]>([]);
  selectCarClass: string = "";
  constructor(private readonly aluService:AluApi, private readonly router:Router){
    
  }

  fetchCars(){
    this.aluService.getCarList().subscribe(
      {
        next:(response:ICar[])=>{
          this.carList.set(response);
          this.visibleCarList = this.carList();
          this.showDivMsg=false;
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
    try{
      // console.log(this.carList());
      this.visibleCarList = this.carList().filter(car=>car.class===this.selectCarClass);
    }
    catch(error)
    {
      this.errorMsg="Check the class of the car and try again!";
      this.showDivMsg=true;
    }
  }

  fetchCarOfName()
  {
    try{
      this.searchCarName = this.carName.trim().toUpperCase();
      if(this.viewType === "All")
      {
        if (this.searchCarName === "" || this.searchCarName === null || this.visibleCarList === ([]))
          this.visibleCarList = this.carList();
        else
          this.visibleCarList = this.carList().filter(car => car.name.includes(this.searchCarName));  
      }
      else
      {
        this.fetchCarsOfClass();
        this.visibleCarList = this.visibleCarList.filter(car => car.name.includes(this.searchCarName));
      }
    }
    catch(error){
      this.errorMsg="Check the name of the car and try again!";
      this.showDivMsg=true;
    }
    
  }
  onTypeCarName(value:string)
  {
    this.carName=value;
    this.fetchCarOfName();
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
          this.showDivMsg=false;
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
      this.visibleCarList=this.carList();
    } else {
      this.visibleCarList=([]);
    }
  }
}
