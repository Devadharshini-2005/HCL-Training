
import { Routes } from '@angular/router';

import { Home } from './components/home/home';
import { About } from './components/about/about';
import { Skills } from './components/skills/skills';
import { Education } from './components/education/education';
import { Projects } from './components/projects/projects';
import { Contact } from './components/contact/contact';

export const routes: Routes = [

  {
    path: '',
    component: Home,
    title: 'Home | Devadharshini'
  },

  {
    path: 'about',
    component: About,
    title: 'About | Devadharshini'
  },

  {
    path: 'skills',
    component: Skills,
    title: 'Skills | Devadharshini'
  },

  {
    path: 'education',
    component: Education,
    title: 'Education | Devadharshini'
  },

  {
    path: 'projects',
    component: Projects,
    title: 'Projects | Devadharshini'
  },

  {
    path: 'contact',
    component: Contact,
    title: 'Contact | Devadharshini'
  },

  {
    path: '**',
    redirectTo: ''
  }

];
