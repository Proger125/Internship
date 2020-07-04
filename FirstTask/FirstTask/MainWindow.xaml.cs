using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace FirstTask
{
    public partial class MainWindow : Window
    {
        private ApplicationViewModel model;
        DispatcherTimer timer;
        public MainWindow()
        {
            InitializeComponent();
            model = new ApplicationViewModel();
            MainTable.ItemsSource = model.table.DefaultView;
            timer = new DispatcherTimer();
            timer.Tick += new EventHandler(timerTick);
            timer.Interval = new TimeSpan(0, 0, 0, 0, 100);
            timer.Start();
        }
        private void timerTick(object sender, EventArgs e)
        {
            MainTable.ItemsSource = model.table.DefaultView;
            timer.Stop();
        }
        private void DrawButton_Click(object sender, RoutedEventArgs e)
        {
            string firstext = FirstTextBox.Text;
            string secondtext = SecondTextBox.Text;
            int temp_n, temp_m;
            bool IsInt = Int32.TryParse(firstext, out temp_n);
            if (IsInt == true)
            {
                IsInt = Int32.TryParse(secondtext, out temp_m);
                if (IsInt == true)
                {
                    model.SetContainer(temp_n, temp_m);
                    MainTable.ItemsSource = model.table.DefaultView;
                }
            }
        }
        private void MainTable_LoadingRow(object sender, DataGridRowEventArgs e)
        {
           e.Row.Header = (e.Row.GetIndex() + 1).ToString();
        }

        private void MainTable_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            string text = ((TextBox)e.EditingElement).Text;
            int row = e.Row.GetIndex();
            int column = e.Column.DisplayIndex;
            text = model.CheckQuotes(text);
            text = model.CheckCalc(text);
            model.SetCellValue(text, row, column);
            timer.Start();
        }
    }
}
