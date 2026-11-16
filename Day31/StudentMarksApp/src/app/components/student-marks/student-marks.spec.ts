import { ComponentFixture, TestBed } from '@angular/core/testing';
import { StudentMarks } from './student-marks';

describe('StudentMarks', () => {
  let component: StudentMarks;
  let fixture: ComponentFixture<StudentMarks>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [StudentMarks],
    }).compileComponents();

    fixture = TestBed.createComponent(StudentMarks);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
