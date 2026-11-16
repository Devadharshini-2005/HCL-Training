
import { Component, signal, computed, effect } from '@angular/core';

@Component({
  selector: 'app-student-marks',
  standalone: true,
  templateUrl: './student-marks.html',
  styleUrl: './student-marks.css'
})
export class StudentMarks {

  // SIGNAL: Store marks
  maths = signal(80);
  science = signal(75);
  computer = signal(90);

  // COMPUTED: Calculate total
  total = computed(() =>
    this.maths() + this.science() + this.computer()
  );

  // COMPUTED: Calculate average
  average = computed(() => this.total() / 3);

  // COMPUTED: Calculate result
  result = computed(() =>
    this.average() >= 40 ? 'Pass' : 'Fail'
  );

  // EFFECT: Monitor changes
  constructor() {
    effect(() => {
      console.log('Marks Updated!');
      console.log('Total:', this.total());
      console.log('Average:', this.average());
    });
  }

  updateMaths(event: Event) {
    this.maths.set(Number((event.target as HTMLInputElement).value));
  }

  updateScience(event: Event) {
    this.science.set(Number((event.target as HTMLInputElement).value));
  }

  updateComputer(event: Event) {
    this.computer.set(Number((event.target as HTMLInputElement).value));
  }

  resetMarks() {
    this.maths.set(80);
    this.science.set(75);
    this.computer.set(90);
  }
}
