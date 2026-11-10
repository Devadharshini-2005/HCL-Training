# JSONDemo – Serialization and Deserialization in C#

## 📌 Overview

This project is a C# console application that demonstrates JSON serialization and deserialization using the `System.Text.Json` namespace.

It focuses on converting a list of student objects, including their associated course details, into JSON format and converting the JSON data back into C# objects.

## 🎯 Objectives

- Understand JSON serialization and deserialization.
- Work with the `System.Text.Json` library.
- Convert C# objects into JSON strings.
- Convert JSON strings back into C# objects.
- Practice working with lists and nested objects.
- Display deserialized student and course information in the console.

## 🛠️ Technologies Used

- **Language:** C#
- **Framework:** .NET
- **Library:** `System.Text.Json`
- **Application Type:** Console Application
- **IDE:** Visual Studio

## 📂 Project Structure

```text
JSONDemo/
├── Student.cs / JSONPractice.cs
├── Program.cs
├── JSONDemo.csproj
└── README.md
```

*Note: The `Student` and `Course` classes, along with `JSONPractice`, are currently defined in the same source file. The structure above is illustrative.*

## 📚 Concepts Practiced

### 1. Student Class

The `Student` class contains the following properties:

- `Id` – Student identification number.
- `Name` – Student name.
- `CourseList` – List of courses associated with the student.

### 2. Course Class

The `Course` class represents course details:

- `Id` – Course identification number.
- `Name` – Course name.
- `Price` – Course price.

Each student can have multiple courses through the `CourseList` property.

### 3. JSON Serialization

Serialization converts C# objects into a JSON string.

The project uses:

```csharp
string json2 = JsonSerializer.Serialize(
    studentList,
    new JsonSerializerOptions { WriteIndented = true }
);
```

The `WriteIndented = true` option formats the JSON output with indentation, making it easier to read.

### 4. JSON Deserialization

Deserialization converts the JSON string back into a list of student objects.

```csharp
List<Student>? jsonList =
    JsonSerializer.Deserialize<List<Student>>(json2);
```

The program checks whether the returned list is not null before displaying its contents.

### 5. Nested Objects and Collections

- Creates a list of students.
- Assigns a list of courses to each student.
- Serializes the complete collection, including nested course details.
- Deserializes the JSON data.
- Uses nested `foreach` loops to display student and course information.

## ▶️ How to Run the Project

1. Open the `JSONDemo` project in Visual Studio.
2. Ensure the project builds successfully.
3. Open `Program.cs`.
4. Run the application using **Ctrl + F5**.
5. View the serialized JSON and deserialized student and course details in the console.

## 🖥️ Expected Output

The program first displays the student list as formatted JSON.

A simplified example of the JSON structure is:

```json
[
  {
    "Id": 1,
    "Name": "Gethz",
    "CourseList": [
      {
        "Id": 101,
        "Name": "python",
        "Price": 23.45
      }
    ]
  }
]
```

The actual output contains three students, each with two course entries.

After serialization, the program prints the deserialized information, including student IDs, student names, course names, course IDs, and prices.

## 💡 Key Learning Outcomes

After completing this practice, you will understand:

- How serialization and deserialization work in C#.
- How to use `JsonSerializer.Serialize()` and `JsonSerializer.Deserialize()`.
- How to serialize lists of objects.
- How nested collections are represented in JSON.
- How to retrieve and display data after deserialization.
- How to use `System.Text.Json` in console applications.

## 🚀 Conclusion

This practice demonstrates how to convert structured C# data into JSON and restore it to objects. These concepts are useful when working with APIs, configuration files, data exchange, and JSON-based storage.
