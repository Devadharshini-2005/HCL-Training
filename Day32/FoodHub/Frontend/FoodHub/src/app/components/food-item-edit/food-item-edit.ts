
import {
  Component,
  OnInit,
  inject,
  ChangeDetectorRef
} from '@angular/core';

import { FormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute, Router } from '@angular/router';

import { FoodItem } from '../../models/food-item.model';
import { Category } from '../../models/category.model';

@Component({
  selector: 'app-food-item-edit',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './food-item-edit.html',
  styleUrl: './food-item-edit.css'
})
export class FoodItemEdit implements OnInit {

  private http = inject(HttpClient);
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private cdr = inject(ChangeDetectorRef);

  foodItemId = 0;

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

  isLoading = true;
  successMessage = '';
  errorMessage = '';

  ngOnInit(): void {

    this.foodItemId = Number(
      this.route.snapshot.paramMap.get('id')
    );

    this.loadCategories();
    this.loadFoodItem();
  }

  // Load categories from backend
  loadCategories(): void {

    this.categoryLoading = true;
    this.categoryError = '';

    this.http.get<Category[]>(
      'https://localhost:7180/api/categories'
    ).subscribe({

      next: (response) => {

        console.log('Edit Form: Categories received:', response);

        this.categories = response;
        this.categoryLoading = false;

        this.cdr.detectChanges();
      },

      error: (error) => {

        console.error('Category Loading Error:', error);

        this.categoryError = 'Unable to load categories.';
        this.categoryLoading = false;

        this.cdr.detectChanges();
      }

    });
  }

  // Load existing food item
  loadFoodItem(): void {

    this.http.get<FoodItem>(
      `https://localhost:7180/api/fooditems/${this.foodItemId}`
    ).subscribe({

      next: (data) => {

        this.foodItem = {
          foodName: data.foodName,
          description: data.description ?? '',
          price: data.price,
          imageUrl: data.imageUrl ?? '',
          categoryId: data.categoryId
        };

        console.log('Edit Form: Food item received:', data);
console.log('Existing Category ID:', data.categoryId);

        this.isLoading = false;

        this.cdr.detectChanges();
      },

      error: (error) => {

        console.error('GET Food Item Error:', error);

        this.errorMessage = 'Unable to load food item.';
        this.isLoading = false;

        this.cdr.detectChanges();
      }

    });
  }

  // Update food item
  updateFoodItem(): void {

    this.successMessage = '';
    this.errorMessage = '';

    const request = {
      ...this.foodItem,
      imageUrl: this.foodItem.imageUrl.trim() || null
    };

    this.http.put(
      `https://localhost:7180/api/fooditems/${this.foodItemId}`,
      request
    ).subscribe({

      next: () => {

        this.successMessage = 'Food item updated successfully!';

        this.cdr.detectChanges();

        this.router.navigate([
          '/food-items',
          this.foodItemId
        ]);
      },

      error: (error) => {

        console.error('PUT Food Item Error:', error);

        this.errorMessage =
          error.error?.message ||
          'Unable to update food item.';

        this.cdr.detectChanges();
      }

    });
  }

  compareCategoryIds(id1: number, id2: number): boolean {
  return id1 === id2;
}

}
