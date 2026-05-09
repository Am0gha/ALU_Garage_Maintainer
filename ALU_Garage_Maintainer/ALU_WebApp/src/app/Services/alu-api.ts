import { Injectable } from '@angular/core';
import { Observable,throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { IFuelEip } from '../Interfaces/FuelEip';
import { ICar } from '../Interfaces/Car';
// API returns a plain array of class names (string[])
import { HttpClient,HttpErrorResponse,HttpParams } from '@angular/common/http';
import { observableToBeFn } from 'rxjs/internal/testing/TestScheduler';
// FetchClass interface is not required because the API returns a plain array of strings
@Injectable({
  providedIn: 'root',
})
export class AluApi {
  
  constructor(private readonly http: HttpClient){}

  getAllCarClasses():Observable<string[]>{
    return this.http.get<string[]>('https://localhost:7213/api/Class/GetAllClasses')
    .pipe(catchError(this.errorHandler));
    }

  getAllRarityForClass(cls:string):Observable<string[]>{
    return this.http.get<string[]>('https://localhost:7213/api/Class/GetValidRarityForClass?carClass='+cls)
    .pipe(catchError(this.errorHandler));
  }

  getValidFuelRange(cls:string, rarity:string):Observable<Number[]>{
    return this.http.get<Number[]>('https://localhost:7213/api/Class/GetClassFuelRange?cls=' + cls)
      .pipe(catchError(this.errorHandler));
  }

  getValidStarForClassRarity(cls: string, rarity: string) {
    return this.http.get<Number[]>('https://localhost:7213/api/Class/GetStarsForClassRarity?cls=' + cls + '&rarity=' + rarity)
      .pipe(catchError(this.errorHandler));
  }

  getValidFuelForClassRarityStars(cls: string, rarity: string, stars: Number) {
    return this.http.get<number>('https://localhost:7213/api/Class/GetFuelForClassRarityStars?cls='+cls+'&rarity='+rarity+'&star='+stars)
      .pipe(catchError(this.errorHandler));
  }

  getValidFuelEipForCar(cls: string, rarity: string, stars: Number): Observable<IFuelEip>{
    return this.http.get<IFuelEip>('https://localhost:7213/api/Class/GetFuelEip?cls=' + cls + '&rarity=' + rarity + '&star=' + stars)
    .pipe(catchError(this.errorHandler));
  }

  addCarToDB(car: ICar): Observable<number> {
    return this.http.post<number>('https://localhost:7213/api/Cars/AddCar',car).pipe(catchError(this.errorHandler))
  }

  checkCarExists(carName: string): Observable<boolean> {
    return this.http.get<boolean>('https://localhost:7213/api/Cars/checkCarExists?name='+ carName).pipe(catchError(this.errorHandler))
  }

  getCarList():Observable<ICar[]>
  {
    return this.http.get<ICar[]>('https://localhost:7213/api/Cars/GetAllCars')
    .pipe(catchError(this.errorHandler));
  }

  getCarListOfClass(cls:string):Observable<ICar[]>
  {
    return this.http.get<ICar[]>('https://localhost:7213/api/Cars/GetCarOfClass?carClass='+cls)
    .pipe(catchError(this.errorHandler));
  }

  errorHandler(error:HttpErrorResponse){
    console.error(error);
    return throwError(()=>error.message || "Server Error");
  }
}
