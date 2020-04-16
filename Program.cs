using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
namespace COED_2
{
    class Program
    {
        static string pathToTheOutput=@"F:\Git\COED-2\out.txt";
        static string pathToTheInput = @"F:\Git\COED-2\values.txt";
        static double[] ExcelToAr()
        {
            
            string[] strings = File.ReadAllLines(pathToTheInput);
            double[] ar = new double[strings.Length];
            for(int i=0;i<strings.Length;i++)
            {
                string[] temp = strings[i].Split('\t');
                ar[i]=Convert.ToDouble(temp[0]);
            }
            return ar;
        }
        static string print<T>(IEnumerable<T> col)
        {
            string s="";
            foreach(var val in col)
            {
                s+=" "+val.ToString()+"\n";
            }
            return s;
        }
        static double GetLinAvg(double[] ar, int left, int right)
        {
            int n=right-left+1;
            double res=0;
            for(int i=left+1;i<=right;i++)
            {
                res+=ar[i];
            }
            return res/(right+1-left-1);
        }
        static double GetLinAvg(double[] ar)
        {
            int n=ar.Length;
            double res=0;
            for(int i=0;i<n;i++)
            {
                res+=ar[i];
            }
            return res/n;
        }

        static double GetGeomAvg(double[] ar, int left, int right)
        {
            int n=right-left+1;
            double res=0;
            double linAvg=GetLinAvg(ar,left,right);

            for(int i=left+1;i<=right;i++)
            {
                res+=(ar[i]-linAvg)*(ar[i]-linAvg);
            }
            if(n<=30)
                res/=right+1-left-2;
            else
                res/=right+1-left-1;
            return Math.Sqrt(res);
        }

        static double GetTTheor(int left, int right)
        {
            double tTheor;
            int l=right-left+1;
            if(l<=5)
                tTheor=3.04;
            else if (l<=10)
                tTheor=2.37;
            else if (l<=15)
                tTheor=2.22;
            else if (l<=20)
                tTheor=2.14;
            else if (l<=24)
                tTheor=2.11;
            else if (l<=28)
                tTheor=2.09;
            else if (l<=30)
                tTheor=2.08;
            else if (l<=40)
                tTheor=2.04;
            else if (l<=60)
                tTheor=2.02;
            else if (l<=120)
                tTheor=1.99;
            else tTheor=1.96;
            return tTheor;
        }
        static double GetTCalc(double[] ar,int left, int right, int lOrR)
        {
            return Math.Abs(GetLinAvg(ar,left,right)-ar[lOrR==0?left:right])/GetGeomAvg(ar,left,right);
        }
        static double[] DeleteСontroversialMembers(double[] ar)
        {
            bool lStop=false;
            bool rStop=false;//l - если слева не удаляются символы - true, r - аналогично
            int leftContr=0;
            int rightContr=ar.Length-1;
            int lOrR=0;
            while(true){
                double tCalc=GetTCalc(ar,leftContr,rightContr, lOrR);
                double tTheor=GetTTheor(leftContr,rightContr);
                if(lOrR==0)
                {
                    if(tCalc>tTheor)
                    {
                        leftContr++;
                    }
                    else{
                        lOrR=1;
                        lStop=true;
                    }
                }
                else{
                    if(tCalc>tTheor)
                    {
                        rightContr--;
                    }
                    else{
                        lOrR=0;
                        rStop=true;
                    }
                }
                if(lStop==rStop==true)
                    break;
            }
            var list=new List<double>();
            for(int i=leftContr;i<=rightContr;i++)
            {
                list.Add(ar[i]);
            }
            return list.ToArray();            
        }
        static double GetDeltaX(double xMin, double xMax, double len)
        {
            return Math.Round((xMax-xMin)/(1+3.31*Math.Log10(len)));
        }

        /*static T[] GetSubArray<T>(T[] ar, int left, int n)
        {
            T[] res=new T[n];
            for(int i=0;i<n;i++)
            {
                res[i]=ar[i+left];
            }
            return res;
        }*/
        static double[] GetArOfni(int[] arOfCi,int n)
        {
            var res=new double[arOfCi.Length];
            var intervals=new double[arOfCi[0]+1];
            res[0]=((double)arOfCi[0]+1)/n;
            for(int i=1;i<arOfCi.Length;i++)
            {
                res[i]=((double)(arOfCi[i]-arOfCi[i-1]))/n;
            }
            return res;
        }
        static void Main(string[] args)
        {
            //File.Create(pathToTheOutput);
            File.Delete(pathToTheOutput);
            using(StreamWriter output=new StreamWriter(pathToTheOutput,true))
            {
            output.Write("Наш массив данных:\n");
            var ar=ExcelToAr();
            output.Write(print(ar));
            output.Write($"Длина массива={ar.Length}\n\n\n");


            output.Write("Отсортируем его:\n");
            var list=ar.ToList<double>();
            list.Sort();
            ar=list.ToArray<double>();
            output.Write(print(ar));
            output.Write($"Длина массива={ar.Length}\n\n\n");


            output.Write("Удалим спорные члены:\n");
            ar=DeleteСontroversialMembers(ar);
            output.Write(print(ar));
            output.Write($"Длина массива={ar.Length}\n\n\n");


            output.Write("Приближённая ширина интервала=");
            double deltaX=GetDeltaX(ar[0],ar[ar.Length-1],ar.Length);
            output.Write(deltaX.ToString());
            output.Write("\n\n\n");


            double xMaxShtrih=ar[ar.Length-1]+0.15*deltaX;
            output.Write("xMaxShtrih="+xMaxShtrih.ToString()+"\n");

            double xMinShtrih=ar[0]-0.15*deltaX;
            output.Write("xMinShtrih="+xMinShtrih.ToString()+"\n");

            double k=Math.Floor((xMaxShtrih-xMinShtrih)/deltaX);
            output.Write("Число интервалов="+k.ToString()+"\n");

            int lOfLastInterval;
            int l=0;
            var listOfCi=new List<int>();
            while(true)
            {          
                lOfLastInterval=l;      
                int r=l;
                while(ar[r]-ar[l]<deltaX)
                {
                    if(r==ar.Length-1)
                        break;
                    r++;
                }
                if(r==ar.Length-1)
                    {
                        listOfCi.Add(r);
                        break;
                    }
                listOfCi.Add(r);
                l=r;
            }
            if(xMaxShtrih-ar[lOfLastInterval]>deltaX)
            {
                output.Write("xMaxShtrih не входит в последний интервал. Приравняем его к границе\n");
                xMaxShtrih=ar[lOfLastInterval];
                output.Write("xMaxShtrih="+xMaxShtrih.ToString()+"\n");
                k=Math.Round((xMaxShtrih-xMinShtrih)/deltaX);
                output.Write("Число интервалов="+k.ToString()+"\n\n\n");
            }

            output.Write("Правые границы интервалов:\n");
            int[] arOfCi=listOfCi.ToArray();
            output.Write(print(arOfCi));


            output.Write("\n\n\nЧастоты интервалов:\n");
            double[] arOfni=GetArOfni(arOfCi,ar.Length);
            for(int i=0;i<arOfni.Length;i++)
                output.Write(" "+arOfni[i].ToString()+"\n");


            }
        }
    }
}
