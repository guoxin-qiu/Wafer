using System;
using System.Security.Cryptography;

namespace Wafer.Apis.Utils
{
    public class Generator
    {
        private static readonly string[] firstNames = {
            //male
            "James", "John", "Robert", "Michael", "William",
            "David", "Richard", "Charles", "Joseph", "Thomas",
            "Christopher", "Daniel", "Paul", "Mark", "Donald",
            "George", "Kenneth", "Steven", "Edward", "Brian",
            "Ronald", "Anthony", "Kevin", "Jason", "Matthew",
            "Gary", "Timothy", "Jose", "Larry", "Jeffrey",
            "Frank", "Scott", "Eric",
            //female
            "Mary", "Patricia", "Linda", "Barbara", "Elizabeth",
            "Jennifer", "Maria", "Susan", "Margaret", "Dorothy",
            "Lisa", "Nancy", "Karen", "Betty", "Helen",
            "Sandra", "Donna", "Carol", "Ruth", "Sharon",
            "Michelle", "Laura", "Sarah", "Kimberly", "Deborah",
            "Jessica", "Shirley", "Cynthia", "Angela", "Melissa",
            "Brenda", "Amy", "Anna"
        };
        private static readonly string[] lastNames = 
        {
            "Smith", "Johnson", "Williams", "Brown", "Jones",
            "Miller", "Davis", "Garcia", "Rodriguez", "Wilson",
            "Martinez", "Anderson", "Taylor", "Thomas", "Hernandez",
            "Moore", "Martin", "Jackson", "Thompson", "White",
            "Lopez", "Lee", "Gonzalez", "Harris", "Clark",
            "Lewis", "Robinson", "Walker", "Perez", "Hall",
            "Young", "Allen"
        };
        private static readonly string[] emailSuffix =
        {
            "gmail.com", "yahoo.com", "msn.com", "hotmail.com", "aol.com",
            "ask.com", "live.com", "yeah.net"
        };

        public static (string name, string fullName, string email) NewUserInfo()
        {
            var rm = new Random();
            var firstName = firstNames[rm.Next(firstNames.Length - 1)];
            var lastName = lastNames[rm.Next(lastNames.Length - 1)];
            var fullName = $"{firstName} {lastName}";
            var name = firstName.ToLower() + rm.Next(99);
            var email = $"{name}@{emailSuffix[rm.Next(emailSuffix.Length - 1)]}";

            return (name, fullName, email);
        }

        private static string EncryptMD5(string str)
        {
            return BitConverter.ToString(MD5.Create().ComputeHash(System.Text.Encoding.Default.GetBytes(str))).Replace("-", "").ToLower();
        }

        public static string EncryptedDefaultPassword()
        {
            return EncryptMD5("admin");
        }

        public static string GetToken(string username, string password)
        {
            return EncryptMD5($"{username}:{password}");
        }
    }
}
