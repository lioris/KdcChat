using Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinqToSql;
using System.Threading;

namespace DBservice
{

    public class DBservice
    {
        pokerDataBaseDataContext dbContext = new pokerDataBaseDataContext();

        public bool isUserExsist(string Name)
        {
            try
            {
                User exsistUser = dbContext.Users.Single(user => user.Name == Name);

                Console.WriteLine("user with name " + Name + " is exsists");
                return true;
            }
            catch
            {
                Console.WriteLine("user with name " + Name + " not exsists");
                return false;
            }
        }

        

        public User login(string Name, string password)
        {
            Console.WriteLine("trying to login with name = {0} and password = {1}", Name, password);
            try
            {
                User exsistUser = dbContext.Users.Single(user => user.Name == Name);
                return exsistUser;
            }
            catch
            {
                Console.WriteLine("invaild username or password  " + Name + " " + password);
                return null;
            }
        }

        public User Register(string Name, string Password)
        {
            Console.WriteLine("trying to register with name = {0} and password = {1}", Name, Password);

            if (isUserExsist(Name))
                return null;

            User newUser = new User
            {
                Name = Name,
                PassWord = Password,
                Email = "no mail",
                
            };

            dbContext.Users.InsertOnSubmit(newUser);

            try
            {
                dbContext.SubmitChanges();
                Console.WriteLine("submited");
                return newUser;
            }
            catch (Exception e)
            {
                Console.WriteLine("somthing went wrong with the conecrtion");
                return null;
            }
        }
        
        public User getUserById(int id)
        {
            throw new NotImplementedException();
        }

        public User getUserByName(string name)
        {
            throw new NotImplementedException();
        }
        

        
    }
}
