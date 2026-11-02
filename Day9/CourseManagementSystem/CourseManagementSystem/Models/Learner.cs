
using System;
using System.Collections.Generic;
using System.Text;

namespace CourseManagementSystem.Models
{
    internal class Learner
    {

        public string learnerId;

        public string LearnerId
        {
            get
            {
                return learnerId;
            }
            set
            {
                if (value.Trim() != "")
                {
                    learnerId = value.Trim();
                }
            }
        }

        public string name;

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                if (value.Trim() != "")
                {
                    name = value;
                }
            }
        }

        public string contactNumber;

        public string ContactNumber
        {
            get
            {
                return contactNumber;
            }
            set
            {
                contactNumber = value;
            }
        }

        public string email;

        public string Email
        {
            get
            {
                return email;
            }
            set
            {
                email = value;
            }
        }


        public Learner(string learnerId, string name, string contactNumber, string email)
        {
            this.learnerId = learnerId;
            this.name = name;
            this.contactNumber = contactNumber;
            this.email = email;
        }

        public Learner()
        {

        }
        public void AddLearners(Dictionary<string, Learner> learnerList)
        {
            Console.WriteLine("-----Add Learner------");

            string learnerId = "";
            string name = "";
            string contactNumber = "";
            string email = "";

            int count = 1;
            while (learnerId.Trim() == "" || learnerList.ContainsKey(learnerId) == true ||
            name.Trim() == "" || contactNumber.Trim().Length != 10 ||
                !contactNumber.All(char.IsDigit) || !(contactNumber[0] >= '6' && contactNumber[0] <= '9') ||
                email.Trim() == "" || !email.Contains('@') || !email.EndsWith(".com"))
            {
                if (count > 1)
                {
                    if (learnerId.Trim() == "" && name.Trim() == "")
                    {
                        Console.WriteLine("Invaild LearnerId and Name!!");
                    }
                    else if (contactNumber.Trim().Length != 10 ||
                    !contactNumber.All(char.IsDigit) || contactNumber.Length <= 0 ||
                    !(contactNumber[0] >= '6' && contactNumber[0] <= '9'))
                    {
                        Console.WriteLine("Invaild Contact Number!!");

                    }
                    else if (learnerList.ContainsKey(learnerId) == true)
                    {
                        Console.WriteLine("Invaild Learner ID it already exit!!");
                    }
                    else if (email.Trim() == "" || !email.Contains('@') || !email.EndsWith(".com"))
                    {
                        Console.WriteLine("Invaild Email Id!!");
                    }
                }
                Console.WriteLine("Enter the Learner Id : ");
                learnerId = Console.ReadLine();
                Console.WriteLine("Enter the Name : ");
                name = Console.ReadLine();
                Console.WriteLine("Enter the Contact Number : ");
                contactNumber = Console.ReadLine();
                Console.WriteLine("Enter the Email : ");
                email = Console.ReadLine();
                count++;
            }
            Learner learner = new Learner(learnerId, name, contactNumber, email);

            learnerList.Add(learnerId, learner);
            Console.WriteLine($"{name} your details added successfully");

        }

        public void DisplayALlLearners(Dictionary<string, Learner> learnerList)
        {
            Console.WriteLine("---All Learners Details---");

            foreach (string id in learnerList.Keys)
            {
                Console.WriteLine($"Learner Id : {learnerList[id].learnerId}");
                Console.WriteLine($"Name : {learnerList[id].name}");
                Console.WriteLine($"Contact Number : {learnerList[id].contactNumber}");
                Console.WriteLine($"Email Id : {learnerList[id].email}");
            }
        }
        public void SearchLearnerById(Dictionary<string, Learner> learnerList)
        {
            Console.WriteLine("---Search Learners using Learner Id---");
            Console.WriteLine("Enter the Learner Id : ");
            string key = Console.ReadLine();

            if (learnerList.ContainsKey(key))
            {
                Console.WriteLine($"Name : {learnerList[key].name}");
                Console.WriteLine($"Contact Number : {learnerList[key].contactNumber}");
                Console.WriteLine($"Email Id : {learnerList[key].email}");
            }
            else
            {
                Console.WriteLine($"{key} is not in the Learner List!!");
            }
        }
    }
}


