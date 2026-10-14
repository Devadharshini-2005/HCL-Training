function fun()
{

  let height=parseInt(document.getElementById("height").value)/100;
  let weight=parseInt(document.getElementById("weight").value);
  let bmi=weight/(height*height);

  if(bmi<18.5){
    document.getElementById("res").innerHTML="You are in Underweight";
  }else if(bmi<25){
    document.getElementById("res").innerHTML="You are in Normalweight";
  }else if(bmi<30){
  document.getElementById("res").innerHTML="You are in Overweight";
  }

}

fun()