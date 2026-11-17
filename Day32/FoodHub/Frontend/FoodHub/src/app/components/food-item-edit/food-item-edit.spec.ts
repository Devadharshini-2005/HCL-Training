import { ComponentFixture, TestBed } from '@angular/core/testing';
import { FoodItemEdit } from './food-item-edit';

describe('FoodItemEdit', () => {
  let component: FoodItemEdit;
  let fixture: ComponentFixture<FoodItemEdit>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FoodItemEdit],
    }).compileComponents();

    fixture = TestBed.createComponent(FoodItemEdit);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
