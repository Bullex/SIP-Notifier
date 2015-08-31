using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace SIP_Notifier
{
    class HistoryDatabase
    {
        private static HistoryDatabase dbInstance;
        private static string databaseName = @".\history.db";
        private readonly SQLiteConnection conn = new SQLiteConnection("Data Source=" + databaseName + "; Version=3;");

        private HistoryDatabase() {
            InitializeDB();
        }

        private void InitializeDB()
        {
            if (!File.Exists(databaseName))
            {
                try
                {
                    SQLiteConnection.CreateFile(databaseName);
                    string sql = "create table history (id INTEGER PRIMARY KEY, date timestamp DEFAULT CURRENT_TIMESTAMP, phone VARCHAR(15), text VARCHAR(2000))";
                    conn.Open();
                    SQLiteCommand command = new SQLiteCommand(sql, conn);
                    command.ExecuteNonQuery();
                }
                catch (SQLiteException e)
                {
                    Debug.WriteLine("Not connected : " + e.ToString());
                }
                finally
                {
                    Debug.WriteLine("End..");
                    conn.Close();
                }
            }
            Debug.WriteLine(File.Exists(databaseName) ? "База данных создана" : "Возникла ошиюка при создании базы данных");
        }

        public static HistoryDatabase getInstance() {
            if(dbInstance == null)
            {
                dbInstance = new HistoryDatabase();
            }
            return dbInstance;
        }

        public void Add(string phone, string text)
        {
            Debug.WriteLine("Update history : " + text);
            try
            {
                string sql = "insert into history (phone, text) values ('" + phone + "', '" + text + "')";
                conn.Open();
                SQLiteCommand command = new SQLiteCommand(sql, conn);
                command.ExecuteNonQuery();
            }
            catch (SQLiteException e)
            {
                Debug.WriteLine("Not connected : " + e.ToString());
            }
            finally
            {
                Debug.WriteLine("End..");
                conn.Close();
            }
        }

        public List<HistoryRow> getAll()
        {
            List<HistoryRow> list = new List<HistoryRow>();
            Debug.WriteLine("Read history");
            try
            {
                string sql = "select * from history order by date asc";
                conn.Open();
                SQLiteCommand command = new SQLiteCommand(sql, conn);
                SQLiteDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(new HistoryRow(reader.GetInt16(0), reader.GetString(3), reader.GetString(2), reader.GetString(1)));
                }
            }
            catch (SQLiteException e)
            {
                Debug.WriteLine("Not connected : " + e.ToString());
                return list;
            }
            finally
            {
                Debug.WriteLine("End..");
                conn.Close();
            }
            return list;
        }
    }
}
