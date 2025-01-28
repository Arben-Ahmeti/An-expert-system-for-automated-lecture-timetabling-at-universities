using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Curriculum_Based_Course_timetabling.Models;

namespace Curriculum_Based_Course_timetabling
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] files = Directory.GetFiles(@"C:\Users\Admin\Desktop\Lecture timetabling\curriculum-based-course-timetabling-master\Curriculum-Based_Course-timetabling\Input"); //C:\Users\Admin\Desktop\Lecture timetabling\Andrea Schaerf\Instances Solutions\cb-ctt-ud2-instances\cb-ctt-ud2");
            foreach (string file in files)
            {
                string[] lines = File.ReadAllLines(file);

                string fileEmri = Path.GetFileNameWithoutExtension(file);


                IO.Read(fileEmri);//"FIM A");//fileEmri);//DDS1");// 
                string text = "";
                for (int i = 0; i < 10; i++)
                {
                    Console.WriteLine("Filloi " + fileEmri);
                    Solution solution = new Solution();
                    ILS ils = new ILS(601);// 306);
                    solution = ils.FindSolution(fileEmri);
                    foreach (var item in solution.assignments)
                    {
                        text += item.course.Id + " " + item.room.Id.Trim() + " " + item.timeslot.Day + " " + item.timeslot.Period + "\n";
                    }

                    using (StreamWriter writer = new StreamWriter(@"C:\Users\Admin\Desktop\Lecture timetabling\curriculum-based-course-timetabling-master\Testet\" + fileEmri + "_" + i + "_" + solution.GetScore() + ".txt"))
                    {
                        writer.WriteLine(text);
                        writer.WriteLine("Score: {0} Courses {1}", solution.GetScore(), solution.assignments.Count);
                    }

                    if (!File.Exists(@"C:\Users\Admin\Desktop\Lecture timetabling\curriculum-based-course-timetabling-master\Testet\WriteLines.txt"))
                    {
                        using (StreamWriter outputFile = new StreamWriter(Path.Combine(@"C:\Users\Admin\Desktop\Lecture timetabling\curriculum-based-course-timetabling-master\Testet\WriteLines.txt")))
                        {
                            outputFile.WriteLine(fileEmri + "  " + solution.GetScore());
                        }
                    }
                    else
                        using (StreamWriter outputFile = new StreamWriter(Path.Combine(@"C:\Users\Admin\Desktop\Lecture timetabling\curriculum-based-course-timetabling-master\Testet", "WriteLines.txt"), true))
                        {
                            outputFile.WriteLine(fileEmri + "  " + solution.GetScore());
                        }
                                       
                }
                Instance.Rooms.Clear();
                Instance.Courses.Clear();
                Instance.FixedAssignments.Clear();
                Instance.Curricula.Clear();
            }
            Console.ReadKey();
        }

    }
}
