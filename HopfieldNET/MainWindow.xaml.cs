using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections;

namespace HopfieldNET
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        TPole Pole;

        string FName;

        TBoxes Boxes;

        public MainWindow()
        {
            InitializeComponent();

            FName = "boxes.dat";

            Pole = new TPole(g);

            LoadBox();
        }

        private void cmClear(object sender, RoutedEventArgs e)
        {
            Pole = new TPole(g);
        }

        void LoadBox()
        {
            BinaryFormatter formatter = new BinaryFormatter();

            if (File.Exists(FName))
            {
                using (FileStream fs = new FileStream(FName, FileMode.OpenOrCreate))
                {
                    Boxes = (TBoxes)formatter.Deserialize(fs);
                }

                if (Boxes.NN == Pole.NN)
                {
                    return;
                }
            }

            Boxes = new TBoxes(Pole.NN);

            using (FileStream fs = new FileStream(FName, FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, Boxes);
            }

        }

        void StoreBox()
        {
            BinaryFormatter formatter = new BinaryFormatter();

            using (FileStream fs = new FileStream(FName, FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, Boxes);
            }
        }

        private void cmAddBox(object sender, RoutedEventArgs e)
        {
            LoadBox();

            Boxes.Add(Pole.Box);

            StoreBox();
        }

        private void cmClearBoxes(object sender, RoutedEventArgs e)
        {
            Boxes = new TBoxes(Pole.NN);

            StoreBox();
        }

        int CurentBox = 0;

        private void cmShow(object sender, RoutedEventArgs e)
        {
            if (Boxes.Count == 0)
            {
                return;
            }

            if (CurentBox < Boxes.Count)
            {
            }
            else
            {
                CurentBox = 0;
            }

            Pole.Box = new TBox(Boxes[CurentBox]);
            CurentBox++;

            Pole.Draw();
        }

        private void cmFind(object sender, RoutedEventArgs e)
        {
            if (Boxes.Count < 1)
            {
                return;
            }

            THopfield Hopfield = new THopfield(Boxes);

            TBox Box = Hopfield.Find(Pole.Box, 100);

            if (Box == null)
            {
                MessageBox.Show("Образ не найден!");
            }
            else
            {
                Pole.Box = Box;
                Pole.Draw();
            }

        }

        private void cmClose(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
