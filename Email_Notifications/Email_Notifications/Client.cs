using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Email_Notifications
{
    class Client
    {
        private sqlClass mydb = null;
        private string sPath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName), "DataUser.db");
        private string sSql = string.Empty;
        private List<string> users = new List<string>();
        private List<string> activeusers = new List<string>();
        private Dictionary<string, string> account = new Dictionary<string, string>();
        private Dictionary<string, string> activeaccount = new Dictionary<string, string>();
        private Dictionary<string, int> accauntsMessages = new Dictionary<string, int>();


        /// <summary>
        /// не работает
        /// </summary>
        public void CreateDB()
        {
            if (File.Exists(sPath))
                return;
            using (File.Create(sPath)) { }
            mydb = new sqlClass();
            sSql = @"CREATE TABLE [User] (
                [Email] TEXT(50) NOT NULL ON CONFLICT ROLLBACK, 
                [Password] TEXT(50) NOT NULL ON CONFLICT ROLLBACK, 
                [Active] BOOL, 
                [LastCountMessage] INTEGER, 
                CONSTRAINT [sqlite_autoindex_User_1] PRIMARY KEY ([Email]));
            CREATE TABLE [Story] (
                [EmailUser] TEXT(50) NOT NULL ON CONFLICT ROLLBACK CONSTRAINT [UserStory] REFERENCES [User]([Email]), 
                [EmailSender] TEXT(50) NOT NULL ON CONFLICT ROLLBACK, 
                [Theme] TEXT, 
                [TextLetter] TEXT, 
                [Name] TEXT);";
            //Пытаемся создать таблицу
            mydb.iExecuteNonQuery(sPath, sSql, 0);
            mydb = null;
        }

        public Dictionary<string, int> GetAccountMessages()
        {
            accauntsMessages = new Dictionary<string, int>();
            mydb = new sqlClass();
            sSql = "select Email, LastCountMessage from  User;";
            DataRow[] datarows = mydb.drExecute(sPath, sSql);
            if (datarows == null)
            {
                mydb = null;
                return new Dictionary<string, int>();
            }
            else
            {
                foreach (DataRow dr in datarows)
                {
                    accauntsMessages.Add(dr["Email"].ToString(), int.Parse(dr["LastCountMessage"].ToString()));
                }
                return accauntsMessages;
            }
        }

        public void UpdateAccountMessages(string email, int i)
        {
            mydb = new sqlClass();
            sSql = @"Update User set LastCountMessage =" + i + " where Email='" + email + "'";
            DataRow[] datarows = mydb.drExecute(sPath, sSql);
            if (datarows == null)
            {
                mydb = null;
                return;
            }
            else
            {
                mydb = null;
            }
        }


        //Работа с аккаунтами пользователя

        public void DeleteEmail(string email)
        {
            mydb = new sqlClass();
            sSql = "delete from  User where Email='"+email+"'";
            if (mydb.iExecuteNonQuery(sPath, sSql, 1) == 0)
            {
                //MessageBox.Show("Не удалось получить соединение с базой данных");
                mydb = null;
                return;
            }
            else
            {
                //MessageBox.Show("Пользователь-"+email+", успешно удален");
                mydb = null;
            }
            return;
        
        }

        public void InsertUser(string email, string password,bool active)
        {
            mydb = new sqlClass();
            sSql = @"insert into User(Email,Password,Active,LastCountMessage) values('" + email + "','" + password + "','" + active + "',-1);";

            //Проверка работы
            if (mydb.iExecuteNonQuery(sPath, sSql, 1) == 0)
            {
                //MessageBox.Show("Не удалось получить соединение с базой данных");
                mydb = null;
                return;
            }
            else
            {
                //MessageBox.Show("Пользователь-"+email+", успешно добавлен");
                mydb = null;
            }
            return;
        }
        public void UpdateEmail(string oldEmail, string newEmail, string password)
        {
            mydb = new sqlClass();
            sSql = @"Update User set Email='" + newEmail + "', Password='" + password + "',LastCountMessage =-1 where Email='" + oldEmail + "';";
            //Проверка работы
            if (mydb.iExecuteNonQuery(sPath, sSql, 1) == 0)
            {
                mydb = null;
                return;
            }
            else
            {
                //MessageBox.Show("Запись обновлена!");
                mydb = null;
            }
        }
        public void UpdateActiveUser(string email, string active)
        {
            mydb = new sqlClass();
            sSql = @"Update User set Active=" + active + ",LastCountMessage =-1 where Email='" + email + "';";
            //Проверка работы
            if (mydb.iExecuteNonQuery(sPath, sSql, 1) == 0)
            {
                mydb = null;
                return;
            }
            else
            {
                //MessageBox.Show("Запись обновлена!");
                mydb = null;
            }
        }
        public void DeleteAllEmail()
        {
            mydb = new sqlClass();
            sSql = "delete from  User";
            if (mydb.iExecuteNonQuery(sPath, sSql, 1) == 0)
            {
                //MessageBox.Show("Не удалось получить соединение с базой данных");
                mydb = null;
                return;
            }
            else
            {
                //MessageBox.Show("Все учетные записи удалены");
                mydb = null;
            }
            return;
        }
        public List<string> DisplayActiveUsers()
        {
            activeusers = new List<string>();
            mydb = new sqlClass();
            sSql = "select Email from  User where Active<>0";
            DataRow[] datarows = mydb.drExecute(sPath, sSql);
            if (datarows == null)
            {
                mydb = null;
                return new List<string>();
            }
            else
            {
            foreach (DataRow dr in datarows)
            {
                activeusers.Add(dr["Email"].ToString());
            }
            return activeusers;
            }

        }
        public List<string> DisplayAllUsers()
        {
            
            users = new List<string>();
            mydb = new sqlClass();
            sSql = "select Email from  User";
            DataRow[] datarows = mydb.drExecute(sPath, sSql);
            if (datarows == null)
            {
                mydb = null;
                return new List<string>();
            }
            else
            {
                foreach (DataRow dr in datarows)
                {
                    users.Add(dr["Email"].ToString());
                }
                return users;
            }

        }
        public Dictionary<string, string> AllAccount()
        {
            account = new Dictionary<string, string>();
            mydb = new sqlClass();
            sSql = "select Email, Password from  User;";
            DataRow[] datarows = mydb.drExecute(sPath, sSql);
            if (datarows == null)
            {
                mydb = null;
                return new Dictionary<string,string>();
            }
            else
            {
                foreach (DataRow dr in datarows)
                {
                    account.Add(dr["Email"].ToString(), dr["Password"].ToString());
                }
                return account;
            }

        }
        public Dictionary<string, string> ActiveAccount()
        {
            activeaccount = new Dictionary<string, string>();
            mydb = new sqlClass();
            sSql = "select Email, Password from  User where Active<>0";
            DataRow[] datarows = mydb.drExecute(sPath, sSql);
            if (datarows == null)
            {
                mydb = null;
                return new Dictionary<string,string>();
            }
            else
            {
                foreach (DataRow dr in datarows)
                {
                    activeaccount.Add(dr["Email"].ToString(), dr["Password"].ToString());
                }
                return activeaccount;
            }

        }


        //Работа с историей сообщений
        public void InsertMessage(string emailUser, string emailSender, string theme, string textLetter, string name)
        {
            mydb = new sqlClass();
            sSql = @"insert into Story(EmailUser,EmailSender,Theme,TextLetter, Name) values('" + emailUser + "','" + emailSender + "','"+theme+ "','"+textLetter+ "','"+name + "');";

            //Проверка работы
            if (mydb.iExecuteNonQuery(sPath, sSql, 1) == 0)
            {
                MessageBox.Show("Не удалось получить соединение с базой данных");
                mydb = null;
                return;
            }
            else
            {
                mydb = null;
            }
            return;
        }
        public void DeleteMessage(string emailUser, string emailSender, string theme, string textLetter, string name)
        {
            mydb = new sqlClass();
            sSql = @"delete from  Story(EmailUser,EmailSender,Theme,TextLetter, Name) where EmailUser="+emailUser+"and EmailSender="+emailSender+" and Theme="+theme+" and TextLetter="+textLetter+" and Name= "+name;

            //Проверка работы
            if (mydb.iExecuteNonQuery(sPath, sSql, 1) == 0)
            {
                MessageBox.Show("Не удалось получить соединение с базой данных");
                mydb = null;
                return;
            }
            else
            {
                mydb = null;
            }
            return;
        }
    }
}