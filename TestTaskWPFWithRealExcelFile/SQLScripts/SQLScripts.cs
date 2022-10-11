using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Windows;

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
        public static void Conn()
        {
            MySqlConnection conn = DBUtils.GetDBConnection();
            conn.Open();
            string sql = $"SET SESSION transaction ISOLATION LEVEL repeatable read; START TRANSACTION;"; //удаляет все записи и заодно обнуляет автоинкремент id

            MySqlCommand cmd = new MySqlCommand
            {
                Connection = conn,
                CommandText = sql
            };
            cmd.ExecuteReader();
        }


        public static void ConnСheck()
        {
            MySqlConnection conn = DBUtils.GetDBConnection();
            try
            {
                conn.Open();
                //string sql = $"SHOW TABLES LIKE 'matrixarray';"; //удаляет все записи и заодно обнуляет автоинкремент id
                string sql = "select * from `matrixarray` limit 1"; //проверка существует ли база данных
                MySqlCommand cmd = new MySqlCommand
                {
                    Connection = conn,
                    CommandText = sql
                };
                cmd.ExecuteReader();
            }
            catch (Exception)
            {
                string messageBoxText = "База данных не найдена";
                string caption = "Ошибка подключения";
                MessageBoxButton button = MessageBoxButton.OK;
                MessageBoxImage icon = MessageBoxImage.Error;
                MessageBoxResult result;
                result = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public static void ConnClose()
        {
            MySqlConnection conn = DBUtils.GetDBConnection();
            string sql = $"Commit;";

            MySqlCommand cmd = new MySqlCommand
            {
                Connection = conn,
                CommandText = sql
            };
            cmd.ExecuteReader();
            conn.Close();
            conn.Dispose();
        }

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
                string sql =  $"SET SESSION transaction ISOLATION LEVEL REPEATABLE READ; START TRANSACTION;" +
                    $"SELECT * FROM  matrixarray " +
                    $"WHERE normOfTime >= '{((float)time).ToString().Replace(",",".")}' " +
                            $"AND debit >= '{((float)debit).ToString().Replace(",",".")}' " +
                    $"ORDER BY normOfTime DESC, debit DESC;"; 

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
                 sql = $"Commit;"; //завершаем транзакцию

                cmd = new MySqlCommand
                {
                    Connection = conn,
                    CommandText = sql
                };
                cmd.ExecuteReader();
            }
            catch (Exception)
            {
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
                //можно было бы сделать одну функцию, в которую передавали бы и знак неравенства в cmd
                string sql = $"SET SESSION transaction ISOLATION LEVEL REPEATABLE READ; START TRANSACTION;" + 
                    $"SELECT * FROM matrixarray" +
                    $" WHERE normOfTime < '{((float)time).ToString().Replace(",", ".")}' AND debit >= '{((float)debit).ToString().Replace(",", ".")}' " +
                    $"ORDER BY normOfTime DESC, debit DESC;"; 

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

                sql = $"Commit;"; 

                cmd = new MySqlCommand
                {
                    Connection = conn,
                    CommandText = sql
                };
                cmd.ExecuteReader();
            }
            catch (Exception)
            {
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
                string sql = $"SET SESSION transaction ISOLATION LEVEL REPEATABLE READ; START TRANSACTION;" + 
                    $"SELECT * FROM  matrixarray " +
                    $"WHERE normOfTime >= '{((float)time).ToString().Replace(",", ".")}' AND debit < '{((float)debit).ToString().Replace(",", ".")}' " +
                    $"ORDER BY normOfTime DESC, debit DESC;"; //удаляет все записи и заодно обнуляет автоинкремент id
                //ORDER BY normOfTime DESC, debit DESC
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

                sql = $"Commit;"; //удаляет все записи и заодно обнуляет автоинкремент id

                cmd = new MySqlCommand
                {
                    Connection = conn,
                    CommandText = sql
                };
                cmd.ExecuteReader();
            }
            catch (Exception)
            {
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
                string sql = $"SET SESSION transaction ISOLATION LEVEL REPEATABLE READ; START TRANSACTION;" + 
                    $"SELECT * FROM  matrixarray " +
                    $"WHERE normOfTime < '{((float)time).ToString().Replace(",", ".")}' AND debit < '{((float)debit).ToString().Replace(",", ".")}' " +
                    $"ORDER BY normOfTime DESC, debit DESC;"; //удаляет все записи и заодно обнуляет автоинкремент id

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

                sql = $"Commit;"; //удаляет все записи и заодно обнуляет автоинкремент id

                cmd = new MySqlCommand
                {
                    Connection = conn,
                    CommandText = sql
                };
                cmd.ExecuteReader();
            }
            catch (Exception)
            {
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