import { Component,OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { AluApi } from '../../Services/alu-api';
import { IFuelEip } from '../../Interfaces/FuelEip';
import { ICar } from '../../Interfaces/Car';
@Component({
  selector: 'app-add-car',
  standalone: false,
  templateUrl: './add-car.html',
  styleUrl: './add-car.css',
})
export class AddCar implements OnInit {
  errorMsg: string = "";
  carClass: string[] = [];
  showDivMsg: boolean = false;
  rarity: string[]=[];
  stars: Number[] = [];
  carFuel: Number|null = null;

  car: ICar=
  {
    class:"",
    maxRank: 0,
    rarity: "",
    maxStars: 0,
    name: "",
    fuel: 0,
    topSpeed: 0,
    acceleration: 0,
    handling:  0,
    nitro: 0,
    hasEips: false,
    noEips: 0,
    requiresKey: false,
    bp1sCount: null,
    bp2sCount: null,
    bp3sCount: null,
    bp4sCount: null,
    bp5sCount: null,
    bp6sCount: null
  }
  


  ngOnInit() {
    this.aluService.getAllCarClasses().subscribe({
      next: (response: string[])=>{
        this.carClass=response;
        console.log(this.carClass);
      },
      error: (error) => {
        this.errorMsg=error;
        this.showDivMsg=true;
      },
      complete:()=>console.log("Fetched all the classes of cars")
    }
  )
  }

  fetchRarity(){
    this.aluService.getAllRarityForClass(this.car.class).subscribe({
      next:(response: string[])=>{
        this.rarity=response;
        // console.log(this.rarity);
      },
      error:(error) => {
        this.errorMsg=error;
        this.showDivMsg=true;
      },
      complete:()=>console.log("Fetched all the rarity for the selected class.")
    })
  }

  fetchStars() {
    this.aluService.getValidStarForClassRarity(this.car.class, this.car.rarity).subscribe({
      next: (response: Number[]) => {
        this.stars = response;
        // console.log(this.stars);
      },
      error: (error) => {
        this.errorMsg = error;
        this.showDivMsg = true;
      },
      complete: () => console.log("Fetched the stars for the given car's class and rarity.")
    })
    this.normalizeCarStats();
  }

  fetchFuel() {
    this.aluService.getValidFuelEipForCar(this.car.class,this.car.rarity,this.car.maxStars).subscribe({
      next:(response:IFuelEip) =>{
        this.car.fuel = response.fuel;
        if(response.fuel != null)
          this.car.hasEips = true;
        this.car.noEips = response.eipAmt;
        
        // console.log(response);
      },
      
      error:(err) => {
        this.errorMsg = err;
        this.showDivMsg = true;
      },
      
      complete: () => console.log("Fetched the valid fuel and EIP amount for the car.")
    })
    this.normalizeCarStats();
  }

  normalizeCarStats(){
    if(this.car.requiresKey==true)
    {
      this.car.bp1sCount=null;
    }

    //Rarity case handling
    switch(this.car.rarity)
    {
      case "COMM": this.car.bp4sCount=null;
                    this.car.bp5sCount=null;
                    this.car.bp6sCount=null;
                    break;
      case "RARE": this.car.bp5sCount=null;
                    this.car.bp6sCount=null;
                    break;
      //case "EPIC" would be handled in the fetchFuel function's switch case.
    }

    //Stars case handling
    if (this.car.maxStars==5)
    {
      this.car.bp6sCount=null;  //-> This case handles the EPIC rarity issue for bp_5s_count and bp_6s_count
      console.log(this.car);
    }

  }


  addCar(addCarForm: NgForm){
    
    this.aluService.addCarToDB(this.car).subscribe({
      next:(response:number|null) => {
        console.log("Response returned: "+response);
        // console.log(this.car);
        switch(response)
        {
          case -1: alert("Invalid car details, check the value for car's class.");
                   break;
          case -2: alert("Invalid car details, star count for car is out of valid range.");
                   break;
          case -3: alert("Invalid car details, check the key option.");
                   break;
          case -4: alert("Invalid car details, check the EIP requirements.");
                   break;
          case null: alert("Something went wrong in the backend. Contact the admin.");
                     break;
          default: alert("Car added successfully to the database.");
                   addCarForm.reset();
                   console.log("returned value: "+typeof(response));
                   break;
        }

      },

      error: (err) => {
        this.errorMsg = err;
        this.showDivMsg = true;
      },

      complete: () => console.log("addCar function executed successfully.")
    })
  }
  constructor(private readonly aluService:AluApi, private readonly router: Router)
  {

  }
}
