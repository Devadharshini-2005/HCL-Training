"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
let students = [
    {
        id: "101",
        name: "Devadharshini",
        age: 21,
        department: "CSE",
        email: "devadharshini@gmail.com",
        address: "Dindigul",
        phoneno: 6789012345
    },
    {
        id: "102",
        name: "Divya",
        age: 21,
        department: "CSE",
        email: "divya@gmail.com",
        address: "Dindigul",
        phoneno: 6789012345
    },
    {
        id: "103",
        name: "Bavani",
        age: 21,
        department: "CSE",
        email: "bavani@gmail.com",
        address: "Dindigul",
        phoneno: 6789012345
    },
    {
        id: "101",
        name: "Geetha",
        age: 21,
        department: "CSE",
        email: "geetha@gmail.com",
        address: "Dindigul",
        phoneno: 6789012345
    },
    {
        id: "101",
        name: "Abi",
        age: 21,
        department: "CSE",
        email: "abi@gmail.com",
        address: "Dindigul",
        phoneno: 6789012345
    }
];
function result(students) {
    console.log("---Students---");
    students.forEach(student => {
        console.log("Id : ", student.id);
        console.log("Name : ", student.name);
        console.log("Age : ", student.age);
        console.log("Department : ", student.department);
        console.log("Email : ", student.email);
        console.log("Address : ", student.address);
        console.log("Phone No : ", student.phoneno);
    });
}
result(students);
