import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';

import { FoodItem } from '../../models/food-item.model';

@Component({
  selector: 'app-food-item-form',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './food-item-form.html',
  styleUrl: './food-item-form.css'
})
export class FoodItemForm {

  private http = inject(HttpClient);

  foodItem = {
    foodName: '',
    description: '',
    price: 0,
    imageUrl: '',
    categoryId: 0
  };

  successMessage = '';
  errorMessage = '';

  addFoodItem(): void {

    this.successMessage = '';
    this.errorMessage = '';

    const request = {
  ...this.foodItem,
  imageUrl: this.foodItem.imageUrl.trim() || null
};

this.http.post<FoodItem>(
  'https://localhost:7180/api/fooditems',
  request
).subscribe({

      next: (response) => {

        console.log('Food item added:', response);

        this.successMessage = 'Food item added successfully!';

        this.foodItem = {
          foodName: '',
          description: '',
          price: 0,
          imageUrl: '',
          categoryId: 0
        };

      },

      error: (error) => {

  console.error('POST API Error:', error);

  console.error(
    'Validation Errors:',
    error.error?.errors
  );

  this.errorMessage = 'Please check the submitted food item details.';

}

    });
  }
}