
import { Component, OnInit, inject, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { CourseService } from '../admin/courses/course.service';
import { CourseResponse } from '../admin/courses/course.models';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [RouterLink],
  templateUrl: './home.html',
  styleUrl: './home.css',
})
export class Home implements OnInit {
  private courseService = inject(CourseService);

  courses = signal<CourseResponse[]>([]);
  isLoadingCourses = signal(true);

  ngOnInit(): void {
    this.loadCourses();
  }

  loadCourses(): void {
    this.courseService.getCourses(1, 3, { isActive: true }).subscribe({
      next: (response) => {
        this.courses.set(response.items ?? []);
        this.isLoadingCourses.set(false);
      },
      error: () => {
        this.courses.set([]);
        this.isLoadingCourses.set(false);
      },
    });
  }

  scrollToCourses(): void {
    document.getElementById('courses')?.scrollIntoView({
      behavior: 'smooth',
    });
  }
}
