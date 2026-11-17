
import {
  Component,
  OnInit,
  inject,
  ChangeDetectorRef
} from '@angular/core';

import { HttpClient } from '@angular/common/http';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { Category } from '../../models/category.model';

@Component({
  selector: 'app-category-details',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './category-details.html',
  styleUrl: './category-details.css'
})
export class CategoryDetails implements OnInit {

  private http = inject(HttpClient);
  private route = inject(ActivatedRoute);
  private cdr = inject(ChangeDetectorRef);

  category: Category | null = null;

  isLoading = true;
  errorMessage = '';

  ngOnInit(): void {
    this.loadCategoryDetails();
  }

  loadCategoryDetails(): void {

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
        console.log('Category details:', data);

        this.category = data;
        this.isLoading = false;

        this.cdr.detectChanges();
      },

      error: (error) => {
        console.error('Category Details API Error:', error);

        this.errorMessage =
          error.status === 404
            ? 'Category not found.'
            : 'Unable to load category details.';

        this.isLoading = false;

        this.cdr.detectChanges();
      }
    });
  }
}
