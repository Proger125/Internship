using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Runtime.CompilerServices;
using System.Text;
namespace FirstTask
{
    class ApplicationViewModel : INotifyPropertyChanged, INotifyCollectionChanged
    {
        public DataTable table { get; set; }
        private Container c;
        public event PropertyChangedEventHandler PropertyChanged;
        public event NotifyCollectionChangedEventHandler CollectionChanged;
        public void OnCollectionChanged([CallerMemberName]string prop = "")
        {
            if (CollectionChanged != null)
            {
                CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add));
            }
        }
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        public ApplicationViewModel()
        {
            c = new Container(5, 5);
            table = makedt(c.collection);
        }
        public DataTable makedt(string[,] input)
        {
            DataTable output = new DataTable();
            for (int i = 0; i < input.GetLength(1); i++)
            {
                output.Columns.Add();
                output.Columns[i].ColumnName = c.headers[i];
            }
            for (int i = 0; i < input.GetLength(0); i++)
            {
                DataRow row = output.NewRow();
                for (int j = 0; j < input.GetLength(1); j++)
                {
                    row[j] = input[i, j];
                }
                output.Rows.Add(row);
            }
            return output;
        }
        public void SetContainer(int n, int m)
        {
            Container container = new Container(n, m);
            int temp_n = Math.Min(n, c.N);
            int temp_m = Math.Min(m, c.M);
            for (int i = 0; i < temp_n; i++)
            {
                for (int j = 0; j < temp_m; j++)
                {
                    container.collection[i, j] = c.collection[i, j];
                }
            }
            c = container;
            table = makedt(c.collection);
            OnPropertyChanged("ReDraw Grid");
            OnCollectionChanged("Add elements");
        }
        public void SetCellValue(string text, int row, int column)
        {
            c.collection[row, column] = text;
            table = makedt(c.collection);
            OnPropertyChanged("RedDraw Grid");
            OnCollectionChanged("One element was changed");
        }
        public string CheckQuotes(string text)
        {
            if (text.Length > 0)
            {
                if (text[0] == '\'')
                {
                    text = text.Remove(0, 1);
                }
            }
            return text;
        }
        public string CheckCalc(string text)
        {
            if (text.Length > 0)
            {
                if (text[0] == '=')
                {
                    text = text.Remove(0, 1);
                    char[] delimetrs = new char[6] { '+', '-', '*', '/','(', ')' };
                    string[] operands = text.Split(delimetrs);
                    double[] variables = new double[100];
                    List<double> list = new List<double>();
                    bool result = true;
                    for (int i = 0; i < operands.Length; i++)
                    {
                        if (operands[i].Length > 0)
                        {
                            string column = operands[i].Substring(0, 2);
                            string r = new string("");
                            r += operands[i].Substring(2);
                            if (Array.IndexOf(c.headers, column) < c.M && Int32.Parse(r) <= c.N)
                            {
                                double element = new double();
                                int col = Array.IndexOf(c.headers, column);
                                int row = Int32.Parse(r.ToString()) - 1;
                                result = Double.TryParse(c.collection[row, col], out element);
                                list.Add(element);
                                if (result)
                                {
                                    continue;
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                    }
                    if (!result)
                    {
                        text = "Error!";
                        return text;
                    }
                    else
                    {
                        int j = 0;
                        for (int i = 0; i < operands.Length; i++)
                        {
                            if (operands[i].Length > 0)
                            {
                                text = text.Replace(operands[i], list[j].ToString());
                                j++;
                            }
                        }
                        text = RPN.Calculate(text).ToString();
                        return text;
                    }
                }
                else
                {
                    return text;
                }
            }
            return text;
        }
    }
}
