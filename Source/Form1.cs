using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ExcelDataReader;
using System.IO;



/// <summary>
/// standing ->1 |
/// sitting ->2  | --> 0 hareketsiz  küme
/// laying ->3   |
/// 
/// walking ->4          |
/// WALKING DOWNSTAIRS->5| --> 1 harekketli küme
/// WALKING UPSTAIRS->6  |
/// </summary>

namespace KMeansClustering
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
           
            try
            {
                DataSet ds;

                bool control = false;
                using (OpenFileDialog ofd = new OpenFileDialog()
                {
                    Filter = "Excel Dosyası |*.xlsx| Excel Dosyası|*.xls",
                    Title = "Excel Dosyası Seçiniz..",
                    ValidateNames = true,
                    RestoreDirectory = true
                })
                {
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        FileStream fs = File.Open(ofd.FileName, FileMode.Open, FileAccess.Read);
                        IExcelDataReader okuyucu = ExcelReaderFactory.CreateOpenXmlReader(fs);

                        int sinir = 22;

                        for (int i = 0; i < sinir; i++)
                        {
                            List<double> list = new List<double>();
                            clustering.dataTrain.Add(list);
                        }

                        ds = okuyucu.AsDataSet();

                        
                        while (okuyucu.Read())
                        {

                            if (okuyucu.GetDouble(0)!=0)
                            {
                                for (int j = 0; j < sinir; j++)
                                {
                                    clustering.dataTrain.ElementAt(j).Add(okuyucu.GetDouble(j));
                                }

                            }
                            else
                            {
                                break;
                            }
                        }
                        
                        okuyucu.Close();
                        control = true;
                    }
                }
                if (control)
                {
                    label7.Text = clustering.dataTrain.ElementAt(0).Count.ToString();
                    MessageBox.Show("Eğitim verisi yüklendi");
                }
            }
            catch
            {
                clustering.dataTrain.Clear();
                MessageBox.Show("Excel dökümanı şu an başka bir uygulama tarafından kullanılıyor.\nLütfen Excel uygulamasını kapatınız.");
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            try
            {
                DataSet ds;
                bool control = false;
                using (OpenFileDialog ofd = new OpenFileDialog()
                {
                    Filter = "Excel Dosyası |*.xlsx| Excel Dosyası|*.xls",
                    Title = "Excel Dosyası Seçiniz..",
                    ValidateNames = true,
                    RestoreDirectory = true
                })
                {
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {     
                        int sinir = 22;
                        for (int i = 0; i < sinir; i++)
                        {
                            List<double> list = new List<double>();
                            clustering.dataTest.Add(list);
                        }
                        FileStream fs = File.Open(ofd.FileName, FileMode.Open, FileAccess.Read);
                        IExcelDataReader okuyucu = ExcelReaderFactory.CreateOpenXmlReader(fs);

                        ds = okuyucu.AsDataSet();


                        while (okuyucu.Read())
                        {

                            if (okuyucu.GetDouble(0) !=0)
                            {
                                for (int j = 0; j < sinir; j++)
                                {
                                    clustering.dataTest.ElementAt(j).Add(okuyucu.GetDouble(j));
                                }

                            }
                            else
                            {
                                break;
                            }
                        }
                        okuyucu.Close();
                        control = true;
                    }
                }
                if (control)
                {
                    label8.Text = clustering.dataTest.ElementAt(0).Count.ToString();
                    label9.Text = (clustering.dataTest.Count - 1).ToString();
                    MessageBox.Show("Test verisi yüklendi");
                }
            }
            catch
            {
                clustering.dataTest.Clear();
                MessageBox.Show("Excel dökümanı şu an başka bir uygulama tarafından kullanılıyor.\nLütfen Excel uygulamasını kapatınız.");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {

            if (clustering.dataTest.Count > 0 & clustering.dataTrain.Count>0)
            {
                clustering obj = new clustering();
                obj.kMean();
                obj.model();
                obj.basariHesap(clustering.dataTest,obj.kumelerYeni);


                label14.Text = "%" + Math.Round(obj.accuracy,2).ToString();
                label15.Text = "%" + Math.Round(obj.precision, 2).ToString();
                label16.Text = "%" + Math.Round(obj.recall, 2).ToString();
                label17.Text = "%" + Math.Round(obj.specificity, 2).ToString();
                label18.Text = "%" + Math.Round(obj.f1Score, 2).ToString();

            }
            else
                MessageBox.Show("Lütfen eğitim ve test verisini sisteme yükleyiniz!");
        }

    
    }
}
