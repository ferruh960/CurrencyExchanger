using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml;

namespace SELF
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void NumberValidation(object sender, TextCompositionEventArgs e)
        {
            //regular expression to enter only numbers to textbox.
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        //variable definitation.
        private DataSet dsDovizKur;
        List<doviz> Kurlar = new List<doviz>();

        public void DovizKur()
        {
            //get data into dataset from xml file by ReadXml
            dsDovizKur = new DataSet();
            dsDovizKur.ReadXml(@"http://www.tcmb.gov.tr/kurlar/today.xml");

            //add TL to list, which does not exist in xml.
            doviz tl = new doviz();
            tl.Alis = 1.0; tl.Satis = 1.0; tl.DovizAdi = "TL"; tl.Birim = 1;
            Kurlar.Add(tl);
            cmbFrom.Items.Add(tl.ToString());
            cmbTo.Items.Add(tl.ToString());

            // there is 19 rows which must be pulled.
            for (int i = 0; i < 19; i++)
            {
                //get every row and make class,
                DataRow dr = dsDovizKur.Tables[1].Rows[i];
                doviz dvz = new doviz();
                dvz.Birim = Convert.ToInt32(dr[0].ToString());
                dvz.DovizAdi = dr[1].ToString();
                dvz.Alis = Convert.ToDouble(dr[3].ToString().Replace('.', ','));
                dvz.Satis = Convert.ToDouble(dr[4].ToString().Replace('.', ','));
                
                //add class into list, and comboboxes
                Kurlar.Add(dvz);
                cmbFrom.Items.Add(dvz.ToString());
                cmbTo.Items.Add(dvz.ToString());
            }

            //format of date is customized in frontend side -> (dd.MM.yyyy)
            lblTarih.Content = DateTime.Today;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            double from, To, rate, result;

            //currency changed to each other.
            from = Kurlar.Find(x => x.DovizAdi == cmbFrom.SelectedItem.ToString()).Satis;
            To = Kurlar.Find(x => x.DovizAdi == cmbTo.SelectedItem.ToString()).Satis;
            rate = from / To;
            int sayi = Convert.ToInt32(txtSayi.Text.ToString());
            result = Math.Round(sayi * rate, 4); //only 4 dijit after decimal
            lblResult.Content = result.ToString();
            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //call function.
            DovizKur();
            //fill the datagrid from xml data.
            datagrid1.ItemsSource = dsDovizKur.Tables[1].DefaultView;
            foreach (DataGridColumn col in datagrid1.Columns)
            {
                //hide all columns other then, Isim-ForexBuyin-ForexSelling columns.
                if (col.Header.ToString() != "Isim" && col.Header.ToString() != "ForexBuying" && col.Header.ToString() != "ForexSelling")
                    col.Visibility = Visibility.Collapsed;
            }
        }

        private void TxtSayi_GotFocus(object sender, RoutedEventArgs e)
        {
            //default value for chang is 1, if you click to alter the value it must be automatically empty.
            txtSayi.Text = string.Empty;
        }
    }
}
