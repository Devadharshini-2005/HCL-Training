// Get the saved student list. localStorage keeps it available
// when we move from index.html to viewStudents.html.
function getStudents() {
  return JSON.parse(localStorage.getItem("students")) || [];
}

function saveStudents(students) {
  localStorage.setItem("students", JSON.stringify(students));
}

function addStudentDetails() {
  const registerNumber = document.getElementById("student-reg").value.trim();
  const studentName = document.getElementById("student-name").value.trim();
  const department = document.getElementById("student-dept").value.trim();
  const emailId = document.getElementById("student-email").value.trim();
  const phoneNumber = document.getElementById("student-phoneno").value.trim();
  const address = document.getElementById("student-address").value.trim();

  if (!registerNumber || !studentName || !department || !emailId || !phoneNumber || !address) {
    alert("Please fill in all student details.");
    return;
  }

  const students = getStudents();

  if (students.some(student => student.registerNumber === registerNumber)) {
    alert("This register number already exists.");
    return;
  }

  students.push({ registerNumber, studentName, department, emailId, phoneNumber, address });
  saveStudents(students);

  alert(studentName + " details stored successfully!");
  document.querySelectorAll("#admission input, #admission textarea").forEach(field => field.value = "");
}

function removeStudentDetails() {
  const registerNumber = document.getElementById("student-reg").value.trim();

  if (!registerNumber) {
    alert("Enter the register number of the student to remove.");
    return;
  }

  const students = getStudents();
  const studentIndex = students.findIndex(student => student.registerNumber === registerNumber);

  if (studentIndex === -1) {
    alert("Student details not found. Please check the register number.");
    return;
  }

  const removedStudent = students.splice(studentIndex, 1)[0];
  saveStudents(students);
  alert(removedStudent.studentName + " details removed successfully!");
}

function viewStudentDetails() {
  const container = document.getElementById("studentsDetails");
  if (!container) return;

  const students = getStudents();

  if (students.length === 0) {
    container.innerHTML = '<p class="empty-message">No student details found. Please add a student first.</p>';
    return;
  }

  let content = `<table>
    <thead>
      <tr>
        <th>Register Number</th>
        <th>Student Name</th>
        <th>Department</th>
        <th>Email ID</th>
        <th>Phone Number</th>
        <th>Address</th>
      </tr>
    </thead>
    <tbody>`;

  students.forEach(student => {
    content += `<tr>
      <td>${escapeHTML(student.registerNumber)}</td>
      <td>${escapeHTML(student.studentName)}</td>
      <td>${escapeHTML(student.department)}</td>
      <td>${escapeHTML(student.emailId)}</td>
      <td>${escapeHTML(student.phoneNumber)}</td>
      <td>${escapeHTML(student.address)}</td>
    </tr>`;
  });

  content += "</tbody></table>";
  container.innerHTML = content;
}

// Escape user-entered values before placing them in the HTML table.
function escapeHTML(value) {
  return String(value).replace(/[&<>"']/g, character => ({
    "&": "&amp;",
    "<": "&lt;",
    ">": "&gt;",
    '"': "&quot;",
    "'": "&#39;"
  })[character]);
}

// Run the table display only on the View Students page.
document.addEventListener("DOMContentLoaded", viewStudentDetails);
