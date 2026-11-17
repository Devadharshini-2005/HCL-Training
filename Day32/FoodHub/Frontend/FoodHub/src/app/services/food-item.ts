
import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { FoodItem } from '../models/food-item.model';

@Injectable({
  providedIn: 'root'
})
export class FoodItemService {

  private http = inject(HttpClient);

  private apiUrl = 'https://localhost:7180/api/fooditems';

  getAllFoodItems(): Observable<FoodItem[]> {
    console.log('Calling GET FoodItems API');

    return this.http.get<FoodItem[]>(this.apiUrl);
  }

  addFoodItem(
    foodItem: Omit<FoodItem, 'foodItemId' | 'categoryName'>
  ): Observable<FoodItem> {
    return this.http.post<FoodItem>(this.apiUrl, foodItem);
  }
}
