

const API = "https://api.restful-api.dev/objects";


const output = document.getElementById("output");




// ==========================================
// GET ALL
// ==========================================


document
    .getElementById("loadBtn")
    .addEventListener("click", getProducts);




async function getProducts() {


    const response = await fetch(API);


    const data = await response.json();


    console.log(data);


    output.textContent = JSON.stringify(data,null,2);
}






// ==========================================
// GET BY ID
// ==========================================


document
    .getElementById("getForm")
    .addEventListener("submit", getProduct);




async function getProduct(event) {


    event.preventDefault(); // it stop form from reloading


    const id = document.getElementById("getId").value;


    const url = `${API}/${id}`; // URL RECONSTRUCTION


    console.log(url);


    const response = await fetch(url);


    const data = await response.json();


    console.log(data);


    output.textContent = JSON.stringify(data, null, 2);
}






// ==========================================
// POST
// ==========================================


document
    .getElementById("addForm")
    .addEventListener("submit", addProduct);




async function addProduct(event) {
    event.preventDefault();
    const name = document.getElementById("addName").value;
    const price = document.getElementById("addPrice").value;
    const color = document.getElementById("addColor").value;
    const product = {
        name: name,
        data: {
            price: Number(price),
            color: color
        }
    };
   
    const response = await fetch(API, {
        method: "POST",
        headers: {
            "Content-Type": "application/json"
        },


        body: JSON.stringify(product)
    });
    const data = await response.json();
    console.log(data);
    output.textContent = JSON.stringify(data, null, 2);
}






// ==========================================
// PUT
// ==========================================


document
    .getElementById("updateForm")
    .addEventListener("submit", updateProduct);




async function updateProduct(event) {


    event.preventDefault();


    const id = document.getElementById("updateId").value;


    const name = document.getElementById("updateName").value;


    const price = document.getElementById("updatePrice").value;


    const color = document.getElementById("updateColor").value;




    const product = {


        name: name,


        data: {
            price: Number(price),
            color: color
        }


    };




    const url = `${API}/${id}`;


    console.log("PUT URL:", url);




    const response = await fetch(url, {


        method: "PUT",


        headers: {
            "Content-Type": "application/json"
        },


        body: JSON.stringify(product)


    });




    const data = await response.json();


    console.log(data);


    output.textContent = JSON.stringify(data, null, 2);
}






// ==========================================
// DELETE
// ==========================================


document
    .getElementById("deleteForm")
    .addEventListener("submit", deleteProduct);




async function deleteProduct(event) {


    event.preventDefault();


    const id = document.getElementById("deleteId").value;


    const url = `${API}/${id}`;


    console.log("DELETE URL:", url);




    const response = await fetch(url, {


        method: "DELETE"


    });




    const data = await response.json();


    console.log(data);


    output.textContent = JSON.stringify(data, null, 2);
}