
import {
  Component,
  OnInit,
  inject,
  ChangeDetectorRef
} from '@angular/core';

import { HttpClient } from '@angular/common/http';
import { FoodItem } from '../../models/food-item.model';
import { RouterLink, Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../services/auth';

@Component({
  selector: 'app-food-item-list',
  standalone: true,
  imports: [RouterLink, FormsModule],
  templateUrl: './food-item-list.html',
  styleUrl: './food-item-list.css'
})
export class FoodItemList implements OnInit {

  private router = inject(Router);
  private http = inject(HttpClient);
  private cdr = inject(ChangeDetectorRef);
  private authService = inject(AuthService);

  foodItems: FoodItem[] = [];
  selectedFoodItem: FoodItem | null = null;
  isLoading = true;
  errorMessage = '';
  searchTerm = '';
  isSearching = false;

  get isAdmin(): boolean {
    return this.authService.getUserRole() === 'Admin';
  }

  ngOnInit(): void {
    this.loadFoodItems();
  }

  loadFoodItems(): void {

    this.isLoading = true;
    this.errorMessage = '';

    this.http.get<FoodItem[]>('https://localhost:7180/api/fooditems')
      .subscribe({

        next: (data) => {
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

  searchFoodItems(): void {

    if (!this.searchTerm.trim()) {
      this.loadFoodItems();
      return;
    }

    this.isSearching = true;
    this.errorMessage = '';

    this.http.get<FoodItem[]>(
      `https://localhost:7180/api/fooditems/search?searchTerm=${encodeURIComponent(this.searchTerm.trim())}`
    ).subscribe({

      next: (data) => {
        this.foodItems = data;
        this.isSearching = false;
        this.cdr.detectChanges();
      },

      error: (error) => {
        console.error('Search API Error:', error);
        this.errorMessage = 'Unable to search food items.';
        this.isSearching = false;
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
