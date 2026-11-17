import { ComponentFixture, TestBed } from '@angular/core/testing';
import { FoodItemList } from './food-item-list';

describe('FoodItemList', () => {
  let component: FoodItemList;
  let fixture: ComponentFixture<FoodItemList>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [FoodItemList],
    }).compileComponents();

    fixture = TestBed.createComponent(FoodItemList);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
