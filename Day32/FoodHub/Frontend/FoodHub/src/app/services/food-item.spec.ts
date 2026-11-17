import { TestBed } from '@angular/core/testing';
import { FoodItem } from './food-item';

describe('FoodItem', () => {
  let service: FoodItem;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(FoodItem);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
