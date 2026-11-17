import {
  Component,
  OnInit,
  inject,
  ChangeDetectorRef
} from '@angular/core';

import { HttpClient } from '@angular/common/http';
import { Category } from '../../models/category.model';

@Component({
  selector: 'app-category-list',
  standalone: true,
  imports: [],
  templateUrl: './category-list.html',
  styleUrl: './category-list.css'
})
export class CategoryList implements OnInit {

  private http = inject(HttpClient);
  private cdr = inject(ChangeDetectorRef);

  categories: Category[] = [];

  isLoading = true;
  errorMessage = '';

  ngOnInit(): void {
    this.loadCategories();
  }

  loadCategories(): void {

    this.isLoading = true;
    this.errorMessage = '';

    this.http.get<Category[]>(
      'https://localhost:7180/api/categories'
    ).subscribe({

      next: (data) => {

        console.log('Received categories:', data);

        this.categories = data;
        this.isLoading = false;

        this.cdr.detectChanges();
      },

      error: (error) => {

        console.error('Category API Error:', error);

        this.errorMessage = 'Unable to load categories.';
        this.isLoading = false;

        this.cdr.detectChanges();
      }

    });
  }
}