
import { Routes } from '@angular/router';

import { FoodItemList } from './components/food-item-list/food-item-list';
import { FoodItemForm } from './components/food-item-form/food-item-form';
import { FoodItemDetails } from './components/food-item-details/food-item-details';
import { FoodItemEdit } from './components/food-item-edit/food-item-edit';
import { CategoryList } from './components/category-list/category-list';
import { CategoryAdd } from './components/category-add/category-add';
import { CategoryDetails } from './components/category-details/category-details';
import { CategoryEdit } from './components/category-edit/category-edit';

export const routes: Routes = [
  {
    path: '',
    component: FoodItemList
  },
  {
  path: 'categories',
  component: CategoryList
},
{
  path: 'categories/add',
  component: CategoryAdd
},
{
  path: 'categories/:id',
  component: CategoryDetails
},
{
  path: 'categories/edit/:id',
  component: CategoryEdit
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
