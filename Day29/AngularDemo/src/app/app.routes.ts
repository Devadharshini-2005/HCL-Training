import { Routes } from '@angular/router';
import { Student } from './component/student/student';


export const routes: Routes = [
    {
        path : '',
        component : Student,
        pathMatch : "full"
    },
    {
        path : "register",
        component :Student
    }
];
