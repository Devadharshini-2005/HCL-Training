
  let object=[];

function addStudentDetails(){

  let registerNumber = document.getElementById("student-reg").value;
  let studentName = document.getElementById("student-name").value;
  let department = document.getElementById("student-dept").value;
  let emailId = document.getElementById("student-email").value;
  let phoneNumber = document.getElementById("student-phoneno").value;
  let address = document.getElementById("student-address").value;


  object.push(
    {
     "registerNumber" : registerNumber,
     "studentName" : studentName,
     "department" : department,
     "emailId" : emailId,
     "phoneNumber" : phoneNumber,
     "address" : address
    }
  );
  console.log(object);
  alert(studentName + " details stored successfully!");
}

let isPresent=false;

function removeStudentDetails(){

  let registerNumber = document.getElementById("student-reg").value;
  let studentName = document.getElementById("student-name").value;

  for(let i=0;i<object.length;i++){
    if(object[i].registerNumber == registerNumber){
      object.splice(i,1);
      isPresent=true;
      alert(studentName+ " details are removed successfully!")
    console.log(object);
      break;
    }
  }
  if(isPresent == false){
    alert(studentName+ " details not present. Given is wrong!")
    console.log("Details not present");
  }

}

function viewStudentDetails(){

  let content = "<table><tr><th>Register Number</th><th>Student Name</th><th>Department</th><th>Email ID</th><th>Phone Number</th><th>Address</th></tr>";

  for(let i=0;i<object.length;i++){
    content +=`<tr>
    <td>${object[i].registerNumber}</td>
    <td>${object[i].studentName}</td>
    <td>${object[i].department}</td>
    <td>${object[i].emailId}</td>
    <td>${object[i].phoneNumber}</td>
    <td>${object[i].address}</td>
    </tr>`;
  }

  content+=`</table>`;

  document.getElementsById("studentsDetails").innerHTML=content;
  
  console.log("view called");

}



