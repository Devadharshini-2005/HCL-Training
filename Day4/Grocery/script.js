let Products=[
  {
    "pro":"Basmati Rice",
    "price":1000
  },
  {
    "pro":"Toor dal",
    "price":150
  },
  {
    "pro":"Rava",
    "price":100
  },
  {
    "pro":"SunFlower Oil",
    "price":40
  },
  {
    "pro":"Salt",
    "price":15
  },
  {
    "pro":"Wheat flour",
    "price":60
  },
  {
    "pro":"Sugar",
    "price":30
  },
  {
    "pro":"Onions",
    "price":50
  },
  {
    "pro":"Tomato",
    "price":30
  },
  {
    "pro":"Potato",
    "price":60
  }
];

let items=[];

let totamt=0;

function addPrice(){
  let product=document.getElementById("product").value;
  let quantity=document.getElementById("quantity").value;
  if(quantity <= 0){
    alert("Invaild quantity!!");
  }
  else{
    for(let i=0;i<Products.length;i++){
      if(Products[i].pro===product){
        totamt+=(quantity*Products[i].price);
        items.push({
          "product":product,
          "quantity":quantity,
          "price":Products[i].price
        });
        alert(`${product} added successfully!`);
        break;
      }
    }
  }
}

function total(){
   let output="";

   for(let i=0;i<items.length;i++){
    output+=`<tr>
    <td>${items[i].product}</td><td>|</td>
    <td>${items[i].quantity}</td><td>|</td>
    <td>${items[i].price}</td>
    </tr>`
   }
   output+=`<tr><td>Total</td></td><td><td></td><td></td><td>${totamt}</td></tr>`;
   document.getElementById("total").innerHTML=output;
}