using System;
using System.Configuration;
using System.Collections.Generic;


namespace AS1GaryYutongBao
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(@"///////////////////////");
            Console.WriteLine(string.Format("Name: {0}", ConfigurationManager.AppSettings["name1"]));
            Console.WriteLine(string.Format("Last 3 Digit of Student ID: {0}", ConfigurationManager.AppSettings["id1"]));
            Console.WriteLine(@"\\\\\\\\\\\\\\\\\\\\\\\");
            Console.WriteLine(@"///////////////////////");
            Console.WriteLine(string.Format("Name: {0}", ConfigurationManager.AppSettings["name2"]));
            Console.WriteLine(string.Format("Last 3 Digit of Student ID: {0}", ConfigurationManager.AppSettings["id2"]));
            Console.WriteLine(@"\\\\\\\\\\\\\\\\\\\\\\\");
            Console.WriteLine();
            Console.WriteLine("Enter the department, course id, and credits " +
                "\ne.g. C1175 3 B2200 5 A110 3 " +
                "\n3 courses to be entered");

            
            string userInput = Console.ReadLine();


            string result = getCourse(userInput);
            string author = "Programmed by Gary Yutong Bao & Daniel Bedoya";
            Console.WriteLine(result);
            Console.WriteLine();
            Console.WriteLine(author);

            Console.Read();

        }

        /// <summary>
        /// 1. processes raw input
        /// 2. feeds data to getCosts()
        /// 3. accumulates all costs to get totalCosts
        /// </summary>
        /// <param name="input"></param>
        /// <returns>result in string to main() </returns>
        public static string getCourse(string input)
        {
            string result = ""; 
            try
            {
                string[] userInput = input.Split(' ');
                //[C1175, 3, B2200, 5, A1110,3]
                List<Course> courses = new List<Course>();

                for (int i = 0; i < userInput.Length;)
                {
                    string dptCode = userInput[i][0].ToString().ToUpper();
                    string courseNumber = userInput[i].Substring(1)[0].ToString();
                    double courseLevel = Int16.Parse(courseNumber) * 1000;
                    string level = courseLevel.ToString();
                    double credits = double.Parse(userInput[i + 1]);

                    Course course = new Course(dptCode, level, credits);
                    courses.Add(course);

                    i += 2;

                }

                Dictionary<string, double> totalCosts = new Dictionary<string, double>();
                totalCosts.Add("tuition", 0);
                totalCosts.Add("labFee", 0);
                totalCosts.Add("activityFee", 0);
                foreach (var course in courses)
                {
                    double[] cost = getCost(course);

                    totalCosts["tution"] = totalCosts["tuition"] += cost[0];
                    totalCosts["labFee"] = totalCosts["labFee"] += cost[1];
                }
                totalCosts["activityFee"] = 0.1 * (totalCosts["tuition"] + totalCosts["labFee"]);

                result = String.Format(
                    "Tution fee:  {0:C}" +
                    "\nLab fee:       {1:C}" +
                    "\nActivity fee:  {2:C}" +
                    "\nTotal amt:   {3:C}",
                    totalCosts["tuition"], totalCosts["labFee"], totalCosts["activityFee"], 
                    totalCosts["tuition"]+totalCosts["labFee"]+totalCosts["activityFee"]);

                return result; 

            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message); ;
            }
           
            return result; 
        }

        /// <summary>
        /// gets costs for different categories for one course
        /// </summary>
        /// <param name="course"></param>
        /// <returns>different cost for one course to getCourse</returns>
        public static double[] getCost(Course course)
        {
            double[] costs = new double[2];

            try
            {
                Dictionary<string, double> labFeeDetail = new Dictionary<string, double>
                {
                    { "A", 35.50 },
                    { "B", 10.00 },
                    { "C", 45.00 }
                };

                Dictionary<string, double> tuitionCreditDetail = new Dictionary<string, double>
                {
                    { "1000", 250.00 },
                    { "2000", 300.00 },
                    { "3000", 500.00 },
                    { "4000", 500.00 }
                };


                costs[0] = tuitionCreditDetail[course.level] * course.credits;
                costs[1] = labFeeDetail[course.department] * course.credits;

                return costs;

                
            }
            catch (Exception e)
            {

                Console.Write(e.Message);
            }


            return costs;
        }
      
    }
}
