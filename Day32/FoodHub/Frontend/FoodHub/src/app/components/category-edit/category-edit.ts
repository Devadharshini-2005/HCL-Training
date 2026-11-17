
import {
  Component,
  OnInit,
  inject,
  ChangeDetectorRef
} from '@angular/core';

import { HttpClient } from '@angular/common/http';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { Category } from '../../models/category.model';

@Component({
  selector: 'app-category-edit',
  standalone: true,
  imports: [FormsModule, RouterLink],
  templateUrl: './category-edit.html',
  styleUrl: './category-edit.css'
})
export class CategoryEdit implements OnInit {

  private http = inject(HttpClient);
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private cdr = inject(ChangeDetectorRef);

  category: Category = {
    categoryId: 0,
    categoryName: '',
    description: ''
  };

  isLoading = true;
  isSubmitting = false;
  successMessage = '';
  errorMessage = '';

  ngOnInit(): void {
    this.loadCategory();
  }

  loadCategory(): void {

    const id = Number(this.route.snapshot.paramMap.get('id'));

    if (!Number.isInteger(id) || id <= 0) {
      this.errorMessage = 'Invalid category ID.';
      this.isLoading = false;
      return;
    }

    this.http.get<Category>(
      `https://localhost:7180/api/categories/${id}`
    ).subscribe({

      next: (data) => {
        this.category = data;
        this.isLoading = false;
        this.cdr.detectChanges();
      },

      error: (error) => {
        console.error('Load Category Error:', error);

        this.errorMessage =
          error.status === 404
            ? 'Category not found.'
            : 'Unable to load category.';

        this.isLoading = false;
        this.cdr.detectChanges();
      }

    });
  }

  updateCategory(): void {

    this.successMessage = '';
    this.errorMessage = '';

    const id = this.category.categoryId;

    const request = {
      categoryName: this.category.categoryName.trim(),
      description: this.category.description?.trim() || null
    };

    if (!request.categoryName) {
      this.errorMessage = 'Category name is required.';
      return;
    }

    this.isSubmitting = true;

    this.http.put<Category>(
      `https://localhost:7180/api/categories/${id}`,
      request
    ).subscribe({

      next: (data) => {
        this.category = data;
        this.successMessage = 'Category updated successfully!';
        this.isSubmitting = false;
        this.cdr.detectChanges();
      },

      error: (error) => {
        console.error('Update Category Error:', error);

        this.errorMessage =
          error.error?.message || 'Unable to update category.';

        this.isSubmitting = false;
        this.cdr.detectChanges();
      }

    });
  }
}
