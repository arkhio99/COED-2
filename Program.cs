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
        static string print(double[] col)
        {
            string s="";
            foreach(var val in col)
            {
                s+=val.ToString()+"\n";
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
            return (xMax-xMin)/(1+3.31*Math.Log10(len));
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


            output.Write($"Отсортируем его:\n");
            var list=ar.ToList<double>();
            list.Sort();
            ar=list.ToArray<double>();
            output.Write(print(ar));
            output.Write($"Длина массива={ar.Length}\n\n\n");


            output.Write($"Удалим спорные члены:\n");
            ar=DeleteСontroversialMembers(ar);
            output.Write(print(ar));
            output.Write($"Длина массива={ar.Length}\n\n\n");


            output.Write($"Приближённая ширина интервала:\n");
            double deltaX=Math.Round(GetDeltaX(ar[0],ar[ar.Length-1],ar.Length));
            output.Write(deltaX.ToString());

            
            }
        }
    }
}
