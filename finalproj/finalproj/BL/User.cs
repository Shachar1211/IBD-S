using System;
using System.Collections.Generic;
using System.Data;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography;
using finalproj.BL;

namespace finalproj.BL
{
    public class User
    {
        private int id;
        private string username;
        private string firstName;
        private string lastName;
        private string email;
        private string password;
        private DateTime dateOfBirth;
        private string gender;
        private string typeOfIBD;
        private string profilePicture;
        //private string passwordResetToken;
        //private DateTime tokenExpiration;

        public static List<User> Users = new List<User>();
  

        public User(string username, string firstName, string lastName, string email, string password, DateTime dateOfBirth, string gender, string typeOfIBD, string profilePicture )
        {
            Username = username;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Password = password;
            DateOfBirth = dateOfBirth;
            Gender = gender;
            TypeOfIBD = typeOfIBD;
            ProfilePicture = profilePicture;
        }

        public User()
        {
        }

        public User(string email)
        {
            Email = email;
        }
        // בנאי רק עם מייל וסיסמה, לשימוש בתהליכי כניסה

        public User(string email, string password)
        {
            Email = email;
            Password = password;
        }

        public string Username { get => username; set => username = value; }
        public string FirstName { get => firstName; set => firstName = value; }
        public string LastName { get => lastName; set => lastName = value; }
        public string Email { get => email; set => email = value; }
        public string Password { get => password; set => password = value; }
        public DateTime DateOfBirth { get => dateOfBirth; set => dateOfBirth = value; }
        public string Gender { get => gender; set => gender = value; }
        public string TypeOfIBD { get => typeOfIBD; set => typeOfIBD = value; }
        public string ProfilePicture { get => profilePicture; set => profilePicture = value; }
        public int Id { get => id; set => id = value; }
        //public string PasswordResetToken { get => passwordResetToken; set => passwordResetToken = value; }
        //public DateTime TokenExpiration { get => tokenExpiration; set => tokenExpiration = value; }

        public bool Insert()
        {
            Users = Read();
            if (!Users.Exists(user => user.Email == this.Email || user.Username == this.Username))
            {
                string path = Path.Combine(Directory.GetCurrentDirectory(), "uploadedFiles");
                //יצירת תיקיה להעלאת התמונות אם לא קיימת
                Directory.CreateDirectory(path);
                if (this.ProfilePicture != "") {      
                    //שמירת התמונה
                    System.IO.File.WriteAllBytes($@"{path}/{this.Username}.jpg", Convert.FromBase64String(this.ProfilePicture));
                    this.ProfilePicture = $@"https://proj.ruppin.ac.il/cgroup57/test2/tar1/Images/{this.Username}.jpg";
                }
                else
                {
                    this.ProfilePicture = $@"https://proj.ruppin.ac.il/cgroup57/test2/tar1/Images/userImage.png";
                }
              
                DBservicesUser dbs = new DBservicesUser();
                dbs.Insert(this);
                return true;
            }
            return false;
        }

        public List<User> Read()
        {
            DBservicesUser dbs = new DBservicesUser();
            return dbs.Read();
        }
        public User ReadOne(string email)
        {
            DBservicesUser dbs = new DBservicesUser();
            return dbs.ReadOne(email);
        }

        public int Update()
        {
            if (this.IsBase64(this.ProfilePicture) == true)
            {
                string path = Path.Combine(Directory.GetCurrentDirectory(), "uploadedFiles");
                //יצירת תיקיה להעלאת התמונות אם לא קיימת
                Directory.CreateDirectory(path);
                System.IO.File.WriteAllBytes($@"{path}/{this.Username}.jpg", Convert.FromBase64String(this.ProfilePicture));
                this.ProfilePicture = $@"https://proj.ruppin.ac.il/cgroup57/test2/tar1/Images/{this.Username}.jpg";

            }
            DBservicesUser dbs = new DBservicesUser();
            return dbs.Update(this);
        }

        public User LogIn()
        {
            DBservicesUser dbs = new DBservicesUser();
            return dbs.LogIn(this);
        }


        public bool Delete()
        {
            DBservicesUser dbs = new DBservicesUser();
            return dbs.Delete(this);
        }


        public bool IsBase64(string value)
        {
            try
            {
                // מנסה להמיר את הערך ל-Base64
                byte[] bytes = Convert.FromBase64String(value);
                // אם ההמרה הצליחה, אז הערך הוא באמת Base64
                return true;
            }
            catch (FormatException)
            {
                // אם ההמרה נכשלה, זאת אומרת שהערך אינו בפורמט Base64 תקין
                return false;
            }
        }


        public bool AddFriend(int friendId)
        {
            DBservicesUser dbs = new DBservicesUser();
            return dbs.AddFriend(this.Id, friendId);
        }
        public List<User> GetFriends()
        {
            DBservicesUser dbs = new DBservicesUser();
            return dbs.GetFriends(this.Id);
        }
    }
}
