
import {
  Component,
  OnInit,
  inject,
  ChangeDetectorRef
} from '@angular/core';

import { HttpClient } from '@angular/common/http';
import { FoodItem } from '../../models/food-item.model';
import { RouterLink } from '@angular/router';
import { Router } from '@angular/router';

@Component({
  selector: 'app-food-item-list',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './food-item-list.html',
  styleUrl: './food-item-list.css'
})
export class FoodItemList implements OnInit {

  private router = inject(Router);

  private http = inject(HttpClient);
  private cdr = inject(ChangeDetectorRef);

  foodItems: FoodItem[] = [];
  selectedFoodItem: FoodItem | null = null;
  isLoading = true;
  errorMessage = '';

  ngOnInit(): void {
    this.loadFoodItems();
  }

  loadFoodItems(): void {

    this.isLoading = true;
    this.errorMessage = '';

    this.http.get<FoodItem[]>('https://localhost:7180/api/fooditems')
      .subscribe({

        next: (data) => {

          console.log('Received food items:', data);

          this.foodItems = data;
          this.isLoading = false;

          this.cdr.detectChanges();

        },

        error: (error) => {

          console.error('API Error:', error);

          this.errorMessage = 'Unable to load food items.';
          this.isLoading = false;

          this.cdr.detectChanges();

        }

      });
  }

  getFoodItemById(id: number): void {

  this.selectedFoodItem = null;
  this.errorMessage = '';

  this.http.get<FoodItem>(
    `https://localhost:7180/api/fooditems/${id}`
  ).subscribe({

    next: (data) => {

      console.log('Food item details:', data);

      this.selectedFoodItem = data;

      this.cdr.detectChanges();
    },

    error: (error) => {

      console.error('GET by ID Error:', error);

      this.errorMessage = 'Unable to load food item details.';

      this.cdr.detectChanges();
    }

  });
}

deleteFoodItem(id: number): void {

  const confirmDelete = window.confirm(
    'Are you sure you want to delete this food item?'
  );

  if (!confirmDelete) {
    return;
  }

  this.http.delete(
    `https://localhost:7180/api/fooditems/${id}`
  ).subscribe({
    next: () => {
      alert('Food item deleted successfully!');
      this.loadFoodItems();
    },
    error: (error) => {
      console.error('DELETE API Error:', error);
      alert('Unable to delete food item.');
    }
  });
}

}
