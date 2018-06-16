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

    public class DBservice : IDbService
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

        public bool isTableExsist(string Name)
        {
            try
            {
                Table exsistTable = dbContext.Tables.Single(table => table.Name == Name);

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

        public void updataUserMoney(string Name, int money)
        {
            User user = dbContext.Users.SingleOrDefault(x => x.Name == Name);
            
            dbContext.SubmitChanges();
        }

        public User getUserById(int id)
        {
            throw new NotImplementedException();
        }

        public User getUserByName(string name)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Table> RetriveTables()
        {
            Console.WriteLine("retriving tables");
            return dbContext.Tables;
        }

        public int insertNewTable(string userName, string tableName, int numPlayers, int bigBliend)
        {
            Console.WriteLine("{0} trying to save table to DB {1} {2} {3}", userName, tableName, numPlayers, bigBliend);

            List<Table> all = dbContext.Tables.ToList();
            int id = all.Count+1;

            Table newTable = new Table
            {
                Id = id,
                BigBlind = bigBliend,
                CurrentPlaying = "1",
                MaxPlayers = numPlayers.ToString(),
                Name = tableName
            };

            dbContext.Tables.InsertOnSubmit(newTable);

            try
            {
                dbContext.SubmitChanges();
                Console.WriteLine("submited table " + tableName);
                return getTableByName(tableName);
            }
            catch (Exception e)
            {
                Console.WriteLine("somthing went wrong with the conecrtion");
                return -1;
            }
        }

        private int getTableByName(string tableName)
        {
            try
            {
                Table exsistTable = dbContext.Tables.Single(table => table.Name == tableName);

                Console.WriteLine("got table id {0} - ths is {1} ", tableName, exsistTable.Id);
                return exsistTable.Id;
            }
            catch
            {
                Console.WriteLine("coudnt find the table {0}", tableName);
                return -1;
            }
        }


        public void updateTable(int tableId, int currentPlaying)
        {
            Table table = dbContext.Tables.SingleOrDefault(x => x.Id == tableId);
            table.CurrentPlaying = currentPlaying.ToString();
            dbContext.SubmitChanges();
        }


        
    }
}
