using Curriculum_Based_Course_timetabling.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Curriculum_Based_Course_timetabling
{
    class ILS
    {
        readonly int iterations;
        readonly int seconds;

      
        public static int lengthTL;
        internal static string[] TabuListTS;// = new string[ILS.lengthTL];//cr
        internal static string[] TabuListSR;// = new string[ILS.lengthTL];//cr
        internal static int prtIndex = 10;


        public ILS(int seconds = 60 , int iterations = 1000)
        {
            this.iterations = iterations;
            this.seconds = seconds;
        }

        public Solution FindSolution(string fileEmri)
        {
            DateTime startTime = DateTime.Now;
            DateTime startTime2 = startTime.AddSeconds(30);
            Random rnd = new Random();//Guid.NewGuid().GetHashCode()
            int iterations_count = 0;
            List<int> T = new List<int>() { 15, 11, 12, 100, 14, 15 };
            Solution S = new Solution();
            S.assignments = S.GenerateSolution();
            lengthTL = (int)(S.assignments.Count() / 3);
            TabuListTS = new string[ILS.lengthTL];
            TabuListSR = new string[ILS.lengthTL];
            Console.WriteLine("Score ne fillim : {0}", S.GetScore());
            IO.WriteToFile("Fillimi", S.GetScore(), fileEmri);
            Solution H = new Solution();
            H = H.Copy(S.assignments);
            Solution Best = new Solution();
            Best = Best.Copy(S.assignments);
            var R = new Solution();

            Stopwatch s = new Stopwatch();
            s.Start();
            bool KaPermiresim = true;
            Random random = new Random();

            while (s.Elapsed < TimeSpan.FromSeconds(seconds)) //iterations_count < iterations && s.Elapsed < TimeSpan.FromSeconds(seconds))
            {
                int climb_iterations = 0;
                int time = T[rnd.Next(T.Count)];

                while (climb_iterations < time && s.Elapsed < TimeSpan.FromSeconds(seconds))//iterations_count < iterations &&
                {
                    R = R.Copy(S.assignments);

                    if (KaPermiresim)//climb_iterations % 2 == 0) // 
                    {
                        R.Tweak();
                    }
                    else
                    {
                        R.TweakSR();
                    }

                    
                    if (R.GetScore() < S.GetScore())
                    {
                        S = S.Copy(R.assignments);
                    }
                    else
                        KaPermiresim = !KaPermiresim;

                    climb_iterations++;
                }

                if (S.GetScore() < Best.GetScore())
                {
                    Best = Best.Copy(S.assignments);
                }
               
                H = H.Copy(NewHomeBase(H, S).assignments);
                S = S.Copy(H.assignments);

                S.Perturb();
                iterations_count++;

                Console.WriteLine(Best.GetScore());
                if (DateTime.Now > startTime2)
                {
                    IO.WriteToFile(startTime.ToString(), Best.GetScore(), fileEmri);
                    startTime2 = startTime2.AddSeconds(30);
                }
                if (Best.GetScore() == 0)
                {
                    break;
                }
            }

            s.Stop();
            return Best;
        }

        public Solution NewHomeBase(Solution H, Solution S)
        {
            return S.GetScore() < H.GetScore() ? S : H;
        }

        private bool TimeDifferenceReached(DateTime startTime,int total_execution_seconds)
        {
            bool result = (int)DateTime.Now.Subtract(startTime).TotalSeconds > total_execution_seconds ? true : false;
            return result;
        }
    }
}
