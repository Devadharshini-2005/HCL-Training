
import { Component, inject,  ChangeDetectorRef } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { CreateCategory } from '../../models/create-category.model';

@Component({
  selector: 'app-category-add',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './category-add.html',
  styleUrl: './category-add.css'
})
export class CategoryAdd {

  private http = inject(HttpClient);
  private cdr = inject(ChangeDetectorRef);

  category: CreateCategory = {
    categoryName: '',
    description: ''
  };

  isSubmitting = false;
  successMessage = '';
  errorMessage = '';

  addCategory(): void {

    this.successMessage = '';
    this.errorMessage = '';

    const request: CreateCategory = {
      categoryName: this.category.categoryName.trim(),
      description: this.category.description?.trim() || null
    };

    if (!request.categoryName) {
      this.errorMessage = 'Category name is required.';
      return;
    }

    this.isSubmitting = true;

    this.http.post(
      'https://localhost:7180/api/categories',
      request
    ).subscribe({
      next: () => {
        this.successMessage = 'Category added successfully!';

        this.category = {
          categoryName: '',
          description: ''
        };

        this.isSubmitting = false;
         this.cdr.detectChanges();
      },
      error: (error) => {
        console.error('Add Category API Error:', error);

        this.errorMessage =
          error.error?.title || 'Unable to add category.';

        this.isSubmitting = false;

        this.cdr.detectChanges();
        
      }
    });
  }
}
