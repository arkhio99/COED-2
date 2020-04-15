using System;
using System.IO;

namespace проект
{
    class Program
    {
        static double[,] ExcelToAr()
        {
            string path = @"C:\Users\PussyDominator\Desktop\Моё обучение\проект\values.txt";
            string[] strings = File.ReadAllLines(path);
            double[,] ar = new double[strings.Length,10];
            for(int i=0;i<strings.Length;i++)
            {
                string[] temp = strings[i].Split('\t');
                for (int j = 0; j < 10; j++)
                    ar[i, j] = double.Parse(temp[j]);
            }
            return ar;
        }
        static double GetAvg(double[,] ar, int col)
        {
            int how=ar.GetLength(0);
            double result=0;
            for(int i=0;i<how;i++)
                result+=ar[i,col];
            result/=how;
            return result;
        }
        static double[] GetRowOfAvg(double[,] ar)
        {
            int how=ar.GetLength(1);
            double[] m=new double[how];
            for(int i=0;i<how;i++)
                m[i]=GetAvg(ar,i);
            return m;
        }
        static double GradeOfDispersion(double[,] ar,double[] avgs,int col)
        {
            int how=ar.GetLength(0);
            double result=0;
            for(int i=0;i<how; i++)
                result+=(ar[i,col]-avgs[col])*(ar[i,col]-avgs[col]);
            result/=how;
            return result;
        }
        static double[] GetArOfDisp(double[,] ar, double[] avgs)
        {
            int how=ar.GetLength(1);
            double[] m=new double[how];
            for(int i=0;i<how;i++)
                m[i]=GradeOfDispersion(ar,avgs,i);
            return m;
        }
        static double[,] GetStandMatrix(double[,] z, double[] avgs, double[] s)
        {
            int N=z.GetLength(0);
            int p=z.GetLength(1);
            double[,] res=new double[N,p];
            for(int i=0;i<N;i++)
                for(int j=0;j<p;j++)
                    res[i,j]=(z[i,j]-avgs[j])/Math.Sqrt(s[j]);
            return res;
        }
        static double[,] GetCovMatrix(double[,] z,double[] avgs)
        {
            int p=z.GetLength(1);
            int N=z.GetLength(0);
            double[,] res=new double[p,p];
            for(int i=0;i<p;i++)
                for(int j=0;j<p;j++)
                    {
                        double temp=0;
                        for(int k=0;k<N;k++)
                            temp+=(z[k,i]-avgs[i])/(z[k,j]-avgs[j]);
                        res[i,j]=temp/N;
                    }  
            return res;
        }
        static double[,] GetCorrMatrix(double[,] x)
        {
            int N=x.GetLength(0);
            int p=x.GetLength(1);
            double[,] res=new double[p,p];
            for(int i=0;i<p;i++)
                for(int j=0;j<p;j++)
                {
                    res[i,j]=0;
                    for(int k=0;k<N;k++)
                        res[i,j]+=x[k,i]*x[k,j];
                    res[i,j]/=N;
                }
            return res;
        }
        static void printMas(double[,] mas)
        {
            for(int i=0;i<mas.GetLength(0);i++)
            {
                for(int j=0;j<mas.GetLength(1);j++)
                    System.Console.Write($"\t{mas[i,j]:f4}\t");
                System.Console.WriteLine();
            }
        }
        static double GetChosenCorr(double[,] z,double[] avgs, int xCol, int yCol)
        {
            double first=0;
            double second=0;
            for(int i=0;i<z.GetLength(0);i++)
            {
                double temp=(z[i,xCol]-avgs[xCol])*(z[i,yCol]-avgs[yCol]);
                first+=temp;
                second+=temp*temp;
            }
            second=Math.Sqrt(second);
            return first/second;
        }
        static double GetStat(double [,] z, double[] avgs,int xCol,int yCol)
        {
            double r=GetChosenCorr(z,avgs,xCol,yCol);
            double n=z.GetLength(1);
            return r*Math.Sqrt(n-2)/Math.Sqrt(1-r*r);
        }
        static double[,] GetStatTable(double[,] z,double[] avgs)
        {
            int p=z.GetLength(1);
            double[,] res=new double[p,p];
            for(int i=0;i<p;i++)
                for(int j=0;j<p;j++)
                    res[i,j]=GetStat(z,avgs,i,j);
            return res;
        }
        static void printForStat(double[,] stat, double tableValue)
        {
            for(int i=0;i<stat.GetLength(0);i++)
            {
                for(int j=0;j<stat.GetLength(1);j++)
                    if(Math.Abs(stat[i,j])>tableValue)
                        System.Console.Write("H1 \t");
                    else if(Math.Abs(stat[i,j])<tableValue)
                        System.Console.Write("H0 \t");
                    else 
                        System.Console.Write("r \t");
                System.Console.WriteLine();
            }
        }
        static void Main(string[] args)
        {
            double[,] data = ExcelToAr();
            //1а средние по столбцам и оценки дисперсий
            double[] arAvgs=GetRowOfAvg(data);
            double[] arS2 = GetArOfDisp(data,arAvgs);
            //1б стандартизованная матрица
            double[,] X=GetStandMatrix(data,arAvgs,arS2);
            //1в ковариационная матрица
            double[,] covMatrix=GetCovMatrix(data,arAvgs);
            //1г корреляционная матрица
            double[,] R=GetCorrMatrix(X);
            //2 Проверить гипотезу о значимости коэффициентов корреляции
            // между столбцами матрицы данных.
            double[,] t=GetStatTable(data,arAvgs);
            double tableValue=1.9921022;
            printForStat(t,tableValue);
        }
    }
}
