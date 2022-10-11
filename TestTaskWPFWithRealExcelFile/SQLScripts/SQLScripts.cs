using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Windows;

///В чем суть?
///в зависимости от наполнения базы менется суть запросов
///мы либо достаем все объекты а разные из разных таблиц одной базы, 
///либо делаем запросы в зависимости от уровня приоритета.

namespace SqlConn
{

    class MyTable
    {
        public MyTable(string id, string name, string value, string time)
        {
            this.Id = id;
            this.Name = name;
            this.Value = value;
            this.Time = time;
        }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string Time { get; set; }
    }


    class SQLScripts
    {
        /// <summary>
        /// Заполняет левую верхнюю ячейку
        /// </summary>
        /// <param name="debit"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public static List<MyTable> SelectMostImportantTask(double debit, double time)
        {
            List<MyTable> resultList = new List<MyTable>();

            MySqlConnection conn = DBUtils.GetDBConnection();
            try
            {
                conn.Open();
                string sql =  $"SELECT * FROM  matrixarray WHERE normOfTime >= '{((float)time).ToString()}' AND debit >= '{((float)debit).ToString()}';"; //удаляет все записи и заодно обнуляет автоинкремент id

                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = conn,
                    CommandText = sql
                };

                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            object id= null; object name =  null; object debitDB =  null; object timeDB = null;

                            id = reader.GetValue(0);
                            name = reader.GetValue(1);
                            debitDB = reader.GetValue(2);
                            timeDB = reader.GetValue(3);
                            resultList.Add(new MyTable(id.ToString(), name.ToString(), debitDB.ToString(), timeDB.ToString()));
                        }
                      
                    }
                }
            }
            catch (Exception)
            {
                //Console.WriteLine("Не удалось загрузить данные данные" + ex);
                string messageBoxText = "Не удалось выполнить запрос к базе данных MySQL";
                string caption = "Ошибка подключения";
                MessageBoxButton button = MessageBoxButton.OK;
                MessageBoxImage icon = MessageBoxImage.Error;
                MessageBoxResult result;
                result = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);
                //MessageBox.Show("Не удалось загрузить данные данные");
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
            return resultList;
        }

        /// <summary>
        /// Заполняет Правый верхний угол
        /// </summary>
        /// <param name="debit"> Дебиты</param>
        /// <param name="time"></param>
        /// <returns></returns>
        public static List<MyTable> SelectLessImportantTask(double debit, double time)
        {
            List<MyTable> resultList = new List<MyTable>();

            MySqlConnection conn = DBUtils.GetDBConnection();
            try
            {
                conn.Open();
                string sql = $"SELECT * FROM matrixarray WHERE normOfTime < '{((float)time).ToString()}' AND debit >= '{((float)debit).ToString()}';"; //удаляет все записи и заодно обнуляет автоинкремент id

                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = conn,
                    CommandText = sql
                };

                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            object id = null; object name = null; object debitDB = null; object timeDB = null;

                            id = reader.GetValue(0);
                            name = reader.GetValue(1);
                            debitDB = reader.GetValue(2);
                            timeDB = reader.GetValue(3);
                            resultList.Add(new MyTable(id.ToString(), name.ToString(), debitDB.ToString(), timeDB.ToString()));
                        }

                    }
                }
            }
            catch (Exception)
            {
                //Console.WriteLine("Не удалось загрузить данные данные" + ex);
                string messageBoxText = "Не удалось выполнить запрос к базе данных MySQL";
                string caption = "Ошибка подключения";
                MessageBoxButton button = MessageBoxButton.OK;
                MessageBoxImage icon = MessageBoxImage.Error;
                MessageBoxResult result;
                result = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);
                //MessageBox.Show("Не удалось загрузить данные данные");
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
            return resultList;
        }

        /// <summary>
        /// Заполняет Левый Нижний угол
        /// </summary>
        /// <param name="debit"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public static List<MyTable> SelectNotLessImportantTask(double debit, double time) //Левая нижняя ячейка
        {
            List<MyTable> resultList = new List<MyTable>();

            MySqlConnection conn = DBUtils.GetDBConnection();
            try
            {
                conn.Open();
                string sql = $"SELECT * FROM  matrixarray WHERE normOfTime >= '{((float)time).ToString()}' AND debit < '{((float)debit).ToString()}';"; //удаляет все записи и заодно обнуляет автоинкремент id

                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = conn,
                    CommandText = sql
                };

                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            object id = null; object name = null; object debitDB = null; object timeDB = null;

                            id = reader.GetValue(0);
                            name = reader.GetValue(1);
                            debitDB = reader.GetValue(2);
                            timeDB = reader.GetValue(3);
                            resultList.Add(new MyTable(id.ToString(), name.ToString(), debitDB.ToString(), timeDB.ToString()));
                        }

                    }
                }
            }
            catch (Exception)
            {
                //Console.WriteLine("Не удалось загрузить данные данные" + ex);
                string messageBoxText = "Не удалось выполнить запрос к базе данных MySQL";
                string caption = "Ошибка подключения";
                MessageBoxButton button = MessageBoxButton.OK;
                MessageBoxImage icon = MessageBoxImage.Error;
                MessageBoxResult result;
                result = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);
                //MessageBox.Show("Не удалось загрузить данные данные");
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
            return resultList;
        }

        /// <summary>
        /// Заполняет правую нижнюю ячейку
        /// </summary>
        /// <param name="debit"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public static List<MyTable> SelectNotImportantTask(double debit, double time)
        {
            List<MyTable> resultList = new List<MyTable>();

            MySqlConnection conn = DBUtils.GetDBConnection();
            try
            {
                conn.Open();
                string sql = $"SELECT * FROM  matrixarray WHERE normOfTime < '{((float)time).ToString()}' AND debit < '{((float)debit).ToString()}';"; //удаляет все записи и заодно обнуляет автоинкремент id

                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = conn,
                    CommandText = sql
                };

                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            object id = null; object name = null; object debitDB = null; object timeDB = null;

                            id = reader.GetValue(0);
                            name = reader.GetValue(1);
                            debitDB = reader.GetValue(2);
                            timeDB = reader.GetValue(3);
                            resultList.Add(new MyTable(id.ToString(), name.ToString(), debitDB.ToString(), timeDB.ToString()));
                        }

                    }
                }
            }
            catch (Exception)
            {
                //Console.WriteLine("Не удалось загрузить данные данные" + ex);
                string messageBoxText = "Не удалось выполнить запрос к базе данных MySQL";
                string caption = "Ошибка подключения";
                MessageBoxButton button = MessageBoxButton.OK;
                MessageBoxImage icon = MessageBoxImage.Error;
                MessageBoxResult result;
                result = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);
                //MessageBox.Show("Не удалось загрузить данные данные");
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
            return resultList;
        }
    }
}