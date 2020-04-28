using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KMeansClustering
{
    class clustering
    {
        public static List<List<double>> dataTrain = new List<List<double>>();
        public static List<List<double>> dataTest = new List<List<double>>();

        public List<double> centroil1 = new List<double>();
        public List<double> centroil2 = new List<double>();
        public List<int> kumeler = new List<int>();
        public List<int> kumelerYeni = new List<int>();
        public List<double> hatalar = new List<double>();
        

        Random random = new Random();
        public void kMean()
        {
            //küme belirleme işlemi.rastgele iki küme doldurulur.
            for (int i = 0; i < dataTrain.ElementAt(0).Count; i++)
            {
                if (i % 2 == 0)
                    kumeler.Add(0);
                else
                    kumeler.Add(1);
            }


            int control=0;
            while (control<=5000)
            {
                //merkez noktaların belirlenmesi          
                merkezHesapla();

                //toplam hatanın belirlenmesi
                hataHesapla();
             
                if (yeniKumeBelirlendiMi() == false)
                    break;

                control++;
            }
            
        }


        void merkezHesapla()
        {
            centroil1.Clear();
            centroil2.Clear();
            for (int j = 0; j < dataTrain.Count - 1; j++)
            {
                double kumeToplam1 = 0;
                double kumeToplam2 = 0;
                int count1 = 0;
                int count2 = 0;
                for (int k = 0; k < dataTrain.ElementAt(j).Count; k++)
                {
                    if (kumeler.ElementAt(k) == 0)
                    {
                        kumeToplam1 += dataTrain.ElementAt(j).ElementAt(k);
                        count1++;
                    }
                    else
                    {
                        kumeToplam2 += dataTrain.ElementAt(j).ElementAt(k);
                        count2++;
                    }
                }
                centroil1.Add(kumeToplam1 / count1);
                centroil2.Add(kumeToplam2 / count2);
            }

        }

        void hataHesapla()
        {

            double toplamHata = 0;
            for (int j = 0; j < dataTrain.Count - 1; j++)
            {

                for (int k = 0; k < dataTrain.ElementAt(j).Count; k++)
                {
                    if (kumeler.ElementAt(k) == 0)
                    {
                        toplamHata += Math.Pow((centroil1.ElementAt(j)) - (dataTrain.ElementAt(j).ElementAt(k)), 2);

                    }
                    else
                    {
                        toplamHata += Math.Pow((centroil2.ElementAt(j)) - (dataTrain.ElementAt(j).ElementAt(k)), 2);
                    }
                }
            }

            hatalar.Add(toplamHata);
        }

        bool yeniKumeBelirlendiMi()
        {
            kumelerYeni.Clear();
            kumelerYeni.AddRange(kumeler);
            kumeler.Clear();
            int i = 0;
            while (i < kumelerYeni.Count)
            {
                double uzaklik1 = 0;
                double uzaklik2 = 0;
                for (int j = 0; j < dataTrain.Count - 1; j++)
                {

                    uzaklik1 += Math.Pow((centroil1.ElementAt(j)) - (dataTrain.ElementAt(j).ElementAt(i)), 2);
                    uzaklik2 += Math.Pow((centroil2.ElementAt(j)) - (dataTrain.ElementAt(j).ElementAt(i)), 2);

                }
                uzaklik1 = Math.Sqrt(uzaklik1);
                uzaklik2 = Math.Sqrt(uzaklik2);
                if (uzaklik1 <= uzaklik2)
                {
                    kumeler.Add(0);
                }
                else
                {
                    kumeler.Add(1);
                }
                i++;
            }
            int guncelmi = 0;
            for (int m = 0; m < kumeler.Count; m++)
            {
                if (kumeler.ElementAt(m) != kumelerYeni.ElementAt(m))
                    guncelmi++;
            }
            if (guncelmi > 0)
                return true;
            else
                return false;
        }

        public void model()
        {
            kumelerYeni.Clear();
            int i = 0;
            while (i < dataTest.ElementAt(0).Count)
            {
                double uzaklik1 = 0;
                double uzaklik2 = 0;
                for (int j = 0; j < dataTest.Count - 1; j++)
                {

                    uzaklik1 += Math.Pow((centroil1.ElementAt(j)) - (dataTest.ElementAt(j).ElementAt(i)), 2);
                    uzaklik2 += Math.Pow((centroil2.ElementAt(j)) - (dataTest.ElementAt(j).ElementAt(i)), 2);

                }
                uzaklik1 = Math.Sqrt(uzaklik1);
                uzaklik2 = Math.Sqrt(uzaklik2);
                if (uzaklik1 <= uzaklik2)
                {
                    kumelerYeni.Add(0);
                }
                else
                {
                    kumelerYeni.Add(1);
                }
                i++;
            }
        }

        public double accuracy = 0;
        public double precision = 0;
        public double recall = 0;
        public double specificity = 0;
        public double f1Score = 0;
        public void basariHesap(List<List<double>> test,List<int> kume)
        {
            //true posivite
            double tp = 0;
            //true negative
            double tn = 0;
            //false positive
            double fp = 0;
            //false negative
            double fn = 0;

            int ft = 21;
            for (int i = 0; i <kume.Count; i++)
            {
                if (test.ElementAt(ft).ElementAt(i) == 1 | test.ElementAt(ft).ElementAt(i) == 2 | test.ElementAt(ft).ElementAt(i) == 3 & kume.ElementAt(i) == 0)
                    tp++;
                else if (test.ElementAt(ft).ElementAt(i) == 4 | test.ElementAt(ft).ElementAt(i) == 5 | test.ElementAt(ft).ElementAt(i) == 6 & kume.ElementAt(i) == 1)
                    tn++;
                else if (test.ElementAt(ft).ElementAt(i) == 4 | test.ElementAt(ft).ElementAt(i) == 5 | test.ElementAt(ft).ElementAt(i) == 6 & kume.ElementAt(i) == 0)
                    fp++;
                else
                    fn++;
            }

            accuracy = (tp + tn) / (tp + tn + fp + fn);
            precision = tp / (tp + fp);
            recall = tp / (tp + fn);
            specificity = tn / (tn + fp);

            f1Score = 2 * ((precision * recall) / (precision + recall));
            
        }
    }
}
