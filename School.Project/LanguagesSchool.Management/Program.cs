using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LanguagesSchool.Repositories;
using SchoolDBModel.EntityTypes;
using System.Data.SqlClient;

namespace LanguagesSchool.Management
{
    class Program
    
	{	        
		
	
        static void Main(string[] args)
        {
            try
            {
                CourseRepository rep = new CourseRepository();
                Course c = new Course();
                c.NumberOfLessons = 1;
                c.Language = LanguageTypes.French;
                c.Level = LevelTypes.A1Beginer;
                c.StatusActive = false;
                c.Category = CategoryTypes.Adults;
                c.Description = "Curs franceza";
              //Console.WriteLine("Count conditie" + rep.Count("[Id]=1"));
                Console.WriteLine("Add: ", rep.Add(c));
                Course c1 = new Course();
                c1.Id = 3;
                c1.NumberOfLessons = 1;
                c1.Language = LanguageTypes.German;
                c1.Level = LevelTypes.A1Beginer;
                c1.StatusActive = false;
                c1.Category = CategoryTypes.Children;
                c1.Description = "Curs germana";
                Console.WriteLine("update: ", rep.Update(c1));
                // rep.Delete(c);
                IList<Course> rez=rep.GetAll();
              // var rez = rep.GetA();
                foreach (var item in rez)
                {
                    Console.WriteLine(rep.GetString(item));
                }
                Console.WriteLine(rep.ToString(rez));

                var rez2 = rep.GetById(2);
                Console.WriteLine($"Get by Id 2 {rep.GetString(rez2)}");
                Console.ReadLine();
            }
            catch (SqlException e)
            {

                Console.WriteLine(e.StackTrace);
            }
            //catch(System.InvalidCastException e)
            //{

              //  Console.WriteLine(e.StackTrace);
            //}
            catch (ArgumentException e)
            {

                Console.WriteLine(e.StackTrace);
            }
            finally
                {
                Console.ReadLine();
            }
        }
    }
}
