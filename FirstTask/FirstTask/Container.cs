using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;
using System.Linq;

namespace FirstTask
{
    class Container : INotifyPropertyChanged
    {
        public string[,] collection;
        public string[] headers;


        private int n;
        private int m;

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }
        public int N { get {return n;}
            set
            {
                n = value;
                OnPropertyChanged("N");
            }
        }
        public int M { get { return m; }
            set
            {
                m = value;
                OnPropertyChanged("M");
            }
        }


        public Container(int n, int m)
        {
            this.m = m;
            this.n = n;
            collection = new string[n, m];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    collection[i, j] = "";
                }
            }
            headers = new string[684];
            for (int i = 0; i < 26; i++)
            {
                for (int j = 0; j < 26; j++)
                {
                    headers[26 * i + j] += (char)('A' + i);
                    headers[26 * i + j] += (char)('A' + j);
                }
            }
        }
    }
}
