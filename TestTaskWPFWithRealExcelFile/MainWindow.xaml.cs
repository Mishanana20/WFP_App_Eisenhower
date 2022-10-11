using System;
using System.Collections.Generic;
using System.Configuration;
using System.Windows;
using SqlConn;

namespace TestTaskWPF
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        double debit = 0;
        double time = 0;
        public MainWindow()
        {
            debit = Double.Parse(ConfigurationManager.AppSettings.Get("debit"));
            time = Convert.ToDouble(ConfigurationManager.AppSettings.Get("time"));

            InitializeComponent();
            SQLScripts.ConnСheck(); //для выброса мсжБокса о доступности БД

            Loaded += MainWindow_Loaded;
            System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 17);
            dispatcherTimer.Start();
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            LoadToWPFLists();
        }

        void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            LoadToWPFLists();
        }

        private void LoadToWPFLists()
        {
            List<MyTable> result = new List<MyTable>();
            List<lbMainList> resultList = new List<lbMainList>(); //иначе бы wpf ссылался бы на одни список
            List<lbMainList> resultList2 = new List<lbMainList>();
            List<lbMainList> resultList3 = new List<lbMainList>();
            List<lbMainList> resultList4 = new List<lbMainList>();

            resultList.Clear();
            result.Clear();
            result.AddRange(SQLScripts.SelectMostImportantTask(debit, time));
            for (int i = 0; i < result.Count; i++)
            {
                resultList.Add(new lbMainList(result[i].Id, result[i].Name));
            }
            lbLeftUp.ItemsSource = resultList;

            resultList2.Clear();
            result.Clear();
            result.AddRange(SQLScripts.SelectNotImportantTask(debit, time));
            for (int i = 0; i < result.Count; i++)
            {
                resultList2.Add(new lbMainList(result[i].Id, result[i].Name));
            }
            lbRightDown.ItemsSource = resultList2;

            resultList3.Clear();
            result.Clear();
            result.AddRange(SQLScripts.SelectLessImportantTask(debit, time));
            for (int i = 0; i < result.Count; i++)
            {
                resultList3.Add(new lbMainList(result[i].Id, result[i].Name));
            }
            lbRightUp.ItemsSource = resultList3;

            resultList4.Clear();
            result.Clear();
            result.AddRange(SQLScripts.SelectNotLessImportantTask(debit, time));
            for (int i = 0; i < result.Count; i++)
            {
                resultList4.Add(new lbMainList(result[i].Id, result[i].Name));
            }
            lbLeftDown.ItemsSource = resultList4;
        }

        class lbMainList
        {
            public lbMainList(string id, string name)
            {
                this.Name = name;
                this.Id = id;
            }
            public string Name { get; set; }
            public string Id { get; set; }
        }

    }
}
