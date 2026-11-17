import { ComponentFixture, TestBed } from '@angular/core/testing';
import { FoodItemForm } from './food-item-form';

describe('FoodItemForm', () => {
  let component: FoodItemForm;
  let fixture: ComponentFixture<FoodItemForm>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FoodItemForm],
    }).compileComponents();

    fixture = TestBed.createComponent(FoodItemForm);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
