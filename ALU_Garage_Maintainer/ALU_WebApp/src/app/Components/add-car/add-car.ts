import { Component,OnInit,signal } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { AluApi } from '../../Services/alu-api';
import { IFuelEip } from '../../Interfaces/FuelEip';
import { ICar } from '../../Interfaces/Car';
@Component({
  selector: 'app-add-car',
  standalone: false,
  templateUrl: './add-car.html',
  styleUrls: ['./add-car.css'],
})
export class AddCar implements OnInit {
  errorMsg: string = "";
  carClass = signal<string[]>([]);
  showDivMsg: boolean = false;
  rarity = signal<string[]>([]);
  stars = signal<Number[]>([]);
  // carFuel = signal<Number|null>(null);

  car = signal<ICar>(
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
  });
  
  constructor(private readonly aluService:AluApi, private readonly router: Router)
  {

  }

  ngOnInit() {
    this.aluService.getAllCarClasses().subscribe({
      next: (response: string[])=>{
        this.carClass.set(response);
        console.log(this.carClass());
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
    this.aluService.getAllRarityForClass(this.car().class).subscribe({
      next:(response: string[])=>{
        this.rarity.set(response);
        // console.log(this.rarity());
      },
      error:(error) => {
        this.errorMsg=error;
        this.showDivMsg=true;
      },
      complete:()=>console.log("Fetched all the rarity for the selected class.")
    })
  }

  fetchStars() {
    this.aluService.getValidStarForClassRarity(this.car().class, this.car().rarity).subscribe({
      next: (response: Number[]) => {
        this.stars.set(response);
        // console.log(this.stars());
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
    this.aluService.getValidFuelEipForCar(this.car().class,this.car().rarity,this.car().maxStars).subscribe({
      next:(response:IFuelEip) =>{
        this.car.set({
          ...this.car(),
          fuel: response.fuel,
          hasEips: response.fuel != null,
          noEips: response.eipAmt
        });
        
        // console.log(response);
      },
      
      error:(err) => {
        this.errorMsg = err;
        this.showDivMsg = true;
      },
      
      complete: () => console.log("Fetched the valid fuel and EIP amount for the car().")
    })
    this.normalizeCarStats();
  }

  normalizeCarStats(){
    if(this.car().requiresKey==true)
    {
      this.car.set({
        ...this.car(),
        bp1sCount:null
    });
    }

    //Rarity case handling
    switch(this.car().rarity)
    {
      case "COMM":  this.car.set({
                      ...this.car(),
                      bp4sCount: null,
                      bp5sCount: null,
                      bp6sCount: null
                    });
                    break;
      
      case "RARE":  this.car.set({
                      ...this.car(),
                      bp5sCount: null,
                      bp6sCount: null
                    });
                    break;
      //case "EPIC" would be handled in the fetchFuel function's switch case.
    }

    //Stars case handling
    if (this.car().maxStars==5)
    {
      this.car.set({
        ...this.car(),
        bp6sCount: null
      });
      console.log(this.car);
    }

  }

  checkCarExists(addCarForm: NgForm){
    this.aluService.checkCarExists(this.car().name).subscribe({
      next: (response:boolean) => {
        if(response == true)
        {
          alert("This car already exists in the database, please check and try again.");
          addCarForm.reset();
        }
      },
      
      error: (err) => {
        this.errorMsg = err;
        this.showDivMsg = true;
      },

      complete: ()=>console.log("Checked if the given car exists or not.")
    })
  }
  addCar(addCarForm: NgForm){
    
    this.aluService.addCarToDB(this.car()).subscribe({
      next:(response:number|null) => {
        console.log("Response returned: "+response);
        // console.log(this.car);
        switch(response)
        {
          case -1: alert("Car already exists by the name '"+this.car().name+"'. Please try again.");
                   break;
          case -2: alert("Invalid car details, check the value for car's class.");
                   break;
          case -3: alert("Invalid car details, star count for car is out of valid range.");
                   break;
          case -4: alert("Invalid car details, check the key option.");
                   break;
          case -5: alert("Invalid car details, check the EIP requirements.");
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
  
}
