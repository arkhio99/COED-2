using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
namespace COED_2
{
    class Program
    {
        static string pathToTheOutput=@"F:\Git\COED-2\out.txt";
        static string pathToTheInput = @"F:\Git\COED-2\values.csv";
        static double[] ExcelToAr()
        {
            string[] strings = File.ReadAllLines(pathToTheInput);
            double[] ar = new double[strings.Length];
            for(int i=0;i<strings.Length;i++)
            {
                string[] temp = strings[i].Split('\t',';');
                ar[i]=Convert.ToDouble(temp[0]);
            }
            return ar;
        }
        static string print<T>(IEnumerable<T> col)
        {
            string s="";
            foreach(T val in col)
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
            return(xMax-xMin)/(1+3.31*Math.Log10(len));
        }

        static T[] GetSubArray<T>(T[] ar, int left, int n)
        {
            T[] res=new T[n];
            for(int i=0;i<n;i++)
            {
                res[i]=ar[i+left];
            }
            return res;
        }
        static double[] GetArOfni(List<double[]> l)
        {
            var res=new double[l.Count];
            int i=0;
            foreach(var val in l)
            {
                res[i]=val.Length;
                i++;
            }
            return res;
        }
        
        static double[] GetArOfmi(double[] arOfni,int n)
        {
            double[] res=new double[arOfni.Length];
            for(int i=0;i<arOfni.Length;i++)
                res[i]=arOfni[i]/n;
            return res;
        }
        
        static double GetDelta(double[] ar,double chosenSqrAvg)
        {
            double t;
            int v=ar.Length-1;
            if(v<=5)
                t=2.02;
            else if(v<=10)
                t=1.81;
            else if(v<=15)
                t=1.75;
            else if(v<=20)
                t=1.73;
            else if(v<=25)
                t=1.71;
            else if(v<=30)
                t=1.70;
            else if(v<=36)
                t=1.69;
            else if(v<=40)
                t=1.68;
            else if(v<=50)
                t=1.67;
            else if(v<=70)
                t=1.66;
            else if(v<=100)
                t=1.65;
            else
                t=1.64;
            return t*chosenSqrAvg/Math.Sqrt(ar.Length<=30?ar.Length:ar.Length-1);            
        }

        static double[] GetAccumFreq(double[] ni)
        {
            int n=ni.Length;
            double[] res=new double[n];
            res[0]=ni[0];
            for(int i=1;i<n;i++)
            {
                res[i]=res[i-1]+ni[i];
            }
            return res;
        }

        static float[] GetMarkOfDifFunc(double[] mi, double deltX)
        {
            float[] res=new float[mi.Length];
            for(int i=0;i<mi.Length;i++)
                res[i]=(float)(mi[i]/deltX);
            return res;
        }

        static double GetChoseAvg(double[] middles, double[] mi)
        {
            double res=0;
            for(int i=0;i<middles.Length;i++)
            {
                res+=middles[i]*mi[i];
            }
            return res;
        } 

        static double[] GetMiddleOfInterval(List<double[]> list)
        {
            double[] res=new double[list.Count];
            int i=0;
            foreach(double[] val in list)
            {
                res[i]=(val[0]+val[val.Length-1])/2;
                i++;
            }
            return res;
        }

        static double GetDisp(int n, double[] middles,double avg,double[] ni)
        {
            double res=0;
            for(int i=0;i<middles.Length;i++)
            {
                res+=(middles[i]-avg)*(middles[i]-avg)*ni[i];
            }
            res/=(n<=30?n-1:n);
            return res;
        }

        static List<double[]> GetListOfIntervals(double[] ar, double[] rights)
        {
            var list=new List<double[]>();
            int j=0;
            for(int i=0;i<rights.Length;i++)
            {
                List<double> temp=new List<double>();
                while(ar[j]<rights[i])
                {
                    temp.Add(ar[j]);
                    if(j==ar.Length-1)
                        break;
                    j++;
                }
                list.Add(temp.ToArray());
            }
            return list;
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


            output.Write("1. Приближённая ширина интервала=");
            double deltaX=GetDeltaX(ar[0],ar[ar.Length-1],ar.Length);
            output.Write(deltaX.ToString());
            output.Write("\n\n\n");
            deltaX=Math.Round(deltaX);
            double xMinShtrih=ar[0]-0.15*deltaX;
            output.Write("2. xMinShtrih="+xMinShtrih.ToString()+"\n");

            double xMaxShtrih=ar[ar.Length-1]+0.15*deltaX;
            output.Write("3. xMaxShtrih="+xMaxShtrih.ToString()+"\n");

            double k=Math.Round((xMaxShtrih-xMinShtrih)/deltaX);
            output.Write("4. Число интервалов="+k.ToString()+"\n");

            //int lOfLastInterval;
            //int l=0;
            var listOfCi=new List<double>();
            //var listOfIntervals=new List<double[]>();
            bool flag=true;
            int count=0;
            while(flag)
            {
            listOfCi.Clear();    
            double temp=ar[0];
            for(int i=0;i<(int)k;i++)
            {
                temp+=deltaX;
                listOfCi.Add(temp);
            }
            if(ar[ar.Length-1]<listOfCi[listOfCi.Count()-1])
            {
                flag= false;
            }
            else{
                if(count==0) 
                    output.WriteLine($"Правая граница последнего интервала \nравна {listOfCi[listOfCi.Count()-1]}, что меньше xMax. Увеличим deltaX.");
                deltaX+=0.1;
            }
            count++;
            }
            output.WriteLine("Новый deltaX="+deltaX.ToString());

            /*while(true)
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
                r--;
                listOfCi.Add(r);
                l=r+1;
            }*/
            /*
            bool exit=false;
            while(true)
            {   System.Console.WriteLine(lOfLastInterval);
                var d=new List<double>();
                for(int i=0;i<ar.Length/(int)k;i++)
                {
                    d.Add(ar[lOfLastInterval+i]);
                    if(lOfLastInterval+i>=ar.Length-1)
                    {
                        exit=true;
                        break;
                    }
                }
                listOfIntervals.Add(d.ToArray());
                if(exit)
                    break;
                lOfLastInterval+=ar.Length/(int)k;
                
            }*/
            /*if(xMaxShtrih-ar[lOfLastInterval]>deltaX)
            {
                output.Write("xMaxShtrih не входит в последний интервал. Приравняем его к границе\n");
                xMaxShtrih=ar[lOfLastInterval];
                output.Write("xMaxShtrih="+xMaxShtrih.ToString()+"\n");
                k=Math.Round((xMaxShtrih-xMinShtrih)/deltaX);
                output.Write("Число интервалов="+k.ToString()+"\n\n\n");
            }*/

            output.Write("Правые границы интервалов:\n");
            double[] arOfCi=listOfCi.ToArray();
            output.Write(print(arOfCi));

            output.Write("\n\n\nИнтервалы:\n");
            var listOfIntervals=GetListOfIntervals(ar,arOfCi);
            /*listOfIntervals.Add(GetSubArray<double>(ar,0,arOfCi[0]+1));
            for(int i=1;i<arOfCi.Length;i++)
            {
                listOfIntervals.Add(GetSubArray<double>(ar,arOfCi[i-1]+1,arOfCi[i]-arOfCi[i-1]));
            }*/
            var arOfIntervals=listOfIntervals.ToArray();
            for(int i=0;i<arOfIntervals.Length;i++)
            {
                output.Write(i.ToString()+")\n"+print(arOfIntervals[i]));
            }     

            output.WriteLine("\n\n\nГраницы интервалов:");
            output.WriteLine("0) ["+$"{ar[0]:f1}"+";"+$"{arOfCi[0]:f1}"+"]");
            for(int i=1;i<k;i++)
                output.WriteLine(i+") ["+$"{arOfCi[i-1]:f1}"+";"+$"{arOfCi[i]:f1}"+"]");
            output.Write("\n\n\nЧастоты интервалов:\n");
            double[] arOfni=GetArOfni(listOfIntervals);
            output.Write(print(arOfni));

            output.Write("\n\n\nЧастости интервалов:\n");
            double[] arOfmi=GetArOfmi(arOfni,ar.Length);
            output.Write(print(arOfmi));

            

            output.Write("\n\n\nНакопленная частота:");
            double[] AccumFreq=GetAccumFreq(arOfni);
            output.Write(print(AccumFreq));

            output.Write("\n\n\n Интегральная оценка:\n");
            double[] markOfIntFunc=GetAccumFreq(arOfmi);
            output.Write(print(markOfIntFunc));
            
            output.Write("\n\n\nДифференциальная оценка:\n");
            float[] markOfDifFunc=GetMarkOfDifFunc(arOfmi, deltaX);
            output.Write(print(markOfDifFunc));

            output.Write("\n\n\nСередины интервалов:\n");
            double[] middlesOfInts=GetMiddleOfInterval(listOfIntervals);
            output.Write(print(middlesOfInts));

            output.Write("\n\n\n5. Выборочное среднее: \n");
            double chosenAvg=GetChoseAvg(middlesOfInts,arOfmi);
            output.Write(chosenAvg.ToString());

            output.Write("\n\n\n6. Выборочная дисперсия=");
            double disp=GetDisp(ar.Length,middlesOfInts,chosenAvg,arOfni);
            output.Write(disp.ToString());

            output.Write("\n\n\nВыборочное среднее квадратическое отклонение=");
            double chosenSqrAvg=Math.Sqrt(disp);
            output.Write(chosenSqrAvg.ToString());

            output.Write("\n\n\n7. Предельная абсолютная ошибка дельта\\*треугольник*\\=");
            double delta=GetDelta(ar,chosenSqrAvg);
            output.Write(delta.ToString());

            output.Write($"\n\n\n8. Интегральная оценка для математического ожидания: \n<х с чертой сверху> - <треугольник> < M(x) < <х с чертой сверху> + <треугольник> \n ");
            double minMarkOfMatOj=chosenAvg-delta;
            double maxMarkOfMatOj=chosenAvg+delta;
            output.Write($"{minMarkOfMatOj}<M(x)<{maxMarkOfMatOj}");


            output.Write("\n\n\n9. Относительная точность мат.ожидания=");
            double myu=delta/chosenAvg;
            output.Write(myu.ToString());
            
            output.Write("\n\n\n10. Размах вариации=");
            double razmah=ar[ar.Length-1]-ar[0];
            output.Write(razmah.ToString());

            output.Write("\n\n\n11. Коэфициент вариации=");
            double coeff=chosenSqrAvg/chosenAvg;
            output.Write(coeff.ToString());

            /*dislin.scrmod ("revers");
            dislin.setpag("da4p");
            dislin.metafl("cons");
            dislin.disini();
            dislin.pagera();
            dislin.complx ();
            dislin.ticks (1, "x");
            dislin.axslen (1600, 700);
            dislin.name("Численность","x");
            dislin.name("f(x)","y");
            dislin.shdpat(1);
            dislin.axspos(300,800);
            dislin.graf(0.0f, (float)ar[ar.Length-1], 0.0f, (float)ar[ar.Length-1]/(float)k, 0.0f, 0.001f, 0.0001f, 0.0001f);
            dislin.labels ("second", "bars");
            dislin.labpos ("outside", "bars");
            dislin.color  ("red");
            float[] y=new float[listOfCi.Count];
            for(int i=0;i<y.Length;i++)
            {
                y[i]=0;
            }
            float[] arOfCiFloat=new float[listOfCi.Count];
            for(int i=0;i<listOfCi.Count;i++)
                arOfCiFloat[i]=(float)arOfCi[i];
            dislin.bars   (arOfCiFloat, y, markOfDifFunc.ToArray(), arOfCiFloat.Length);
            dislin.color  ("fore");
            dislin.endgrf();
            System.Console.ReadLine();*/
            
            }
        }
    }
}
