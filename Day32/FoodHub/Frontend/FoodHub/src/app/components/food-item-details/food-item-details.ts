
import { Component, OnInit, inject, ChangeDetectorRef } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { FoodItem } from '../../models/food-item.model';

@Component({
  selector: 'app-food-item-details',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './food-item-details.html',
  styleUrl: './food-item-details.css'
})
export class FoodItemDetails implements OnInit {

  private route = inject(ActivatedRoute);
  private http = inject(HttpClient);
  private cdr = inject(ChangeDetectorRef);

  foodItem: FoodItem | null = null;
  isLoading = true;
  errorMessage = '';

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));

    this.http.get<FoodItem>(
      `https://localhost:7180/api/fooditems/${id}`
    ).subscribe({
      next: (data) => {
        this.foodItem = data;
        this.isLoading = false;
        this.cdr.detectChanges();
      },
      error: (error) => {
        console.error('Food details API error:', error);
        this.errorMessage = 'Unable to load food item details.';
        this.isLoading = false;
        this.cdr.detectChanges();
      }
    });
  }
}
