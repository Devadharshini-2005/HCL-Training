
import { Routes } from '@angular/router';

import { FoodItemList } from './components/food-item-list/food-item-list';
import { FoodItemForm } from './components/food-item-form/food-item-form';
import { FoodItemDetails } from './components/food-item-details/food-item-details';
import { FoodItemEdit } from './components/food-item-edit/food-item-edit';

export const routes: Routes = [
  {
    path: '',
    component: FoodItemList
  },
  {
    path: 'food-items/add',
    component: FoodItemForm
  },
  {
    path: 'food-items/edit/:id',
    component: FoodItemEdit
  },
  {
    path: 'food-items/:id',
    component: FoodItemDetails
  },
  {
    path: '**',
    redirectTo: ''
  }
];
