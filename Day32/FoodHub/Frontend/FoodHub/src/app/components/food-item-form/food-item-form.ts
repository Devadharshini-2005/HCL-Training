
import { Component, inject, OnInit, ChangeDetectorRef } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';

import { FoodItem } from '../../models/food-item.model';
import { Category } from '../../models/category.model';

@Component({
  selector: 'app-food-item-form',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './food-item-form.html',
  styleUrl: './food-item-form.css'
})
export class FoodItemForm implements OnInit {

  private http = inject(HttpClient);
  private cdr = inject(ChangeDetectorRef);

  categories: Category[] = [];

  categoryLoading = false;
  categoryError = '';

  foodItem = {
    foodName: '',
    description: '',
    price: 0,
    imageUrl: '',
    categoryId: 0
  };

  successMessage = '';
  errorMessage = '';

  ngOnInit(): void {
    this.loadCategories();
  }

  // Load categories from backend
  loadCategories(): void {

  console.log('FoodItemForm: Loading categories started');

  this.categoryLoading = true;
  this.categoryError = '';

  this.http.get<Category[]>(
    'https://localhost:7180/api/categories'
  ).subscribe({

    next: (response) => {

      console.log('FoodItemForm: Categories received:', response);

      this.categories = response;
      this.categoryLoading = false;
      this.cdr.detectChanges();

    },

    error: (error) => {

      console.error('FoodItemForm: Category API Error:', error);

      this.categoryError = 'Unable to load categories.';
      this.categoryLoading = false;
      this.cdr.detectChanges();
      
    }

  });
}

  // Add food item
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
        console.error('Validation Errors:', error.error?.errors);

        this.errorMessage =
          error.error?.message ||
          'Please check the submitted food item details.';

      }

    });
  }
}
