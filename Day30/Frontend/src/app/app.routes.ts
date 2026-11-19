import { Routes } from '@angular/router';
import { Student } from './component/student/student';
import { StudentList } from './component/student-list/student-list';

export const routes: Routes = [
    {
        path: 'register',
        component: Student
    },{
        path: 'student-list',
        component: StudentList
    },{
        path:'',
        component: Student,
        pathMatch: "full"

    }
];
