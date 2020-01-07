using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Threading.Tasks;
using SchoolDBModel.EntityTypes;
using DataAccessConnection;
using System.Data;

namespace LanguagesSchool.Repositories
{
    public class CourseRepository : BaseRepository<Course>
    {
        protected override string TableName
        {
            get
            { return "[dbo].[Course]"; }
        }
        public IList<Course> Courses { get; set; } = new List<Course>();
        public override int Add(Course entity)
        {
            try
            {
                string commandText = $"Insert into {TableName} values (@NumberOfLessons, @Description, " +
          $"@Category, @Language, @Level, @StatusActive); select scope_identity();";
                SqlParameter parameterNumberOfLessons = new SqlParameter("NumberOfLessons", SqlDbType.Int);
                parameterNumberOfLessons.Value = entity.NumberOfLessons;
                SqlParameter parameterDescription = new SqlParameter("Description", SqlDbType.VarChar);
                parameterDescription.Value = entity.Description;
                SqlParameter parameterCategory = new SqlParameter("Category", SqlDbType.VarChar);
                parameterCategory.Value = entity.Category;
                SqlParameter parameterLanguage = new SqlParameter("Language", SqlDbType.VarChar);
                parameterLanguage.Value = entity.Language;
                SqlParameter parameterLevel = new SqlParameter("Level", SqlDbType.VarChar);
                parameterLevel.Value = entity.Level;
                SqlParameter parameterStatusActive = new SqlParameter("StatusActive", SqlDbType.Bit);
                parameterStatusActive.Value = entity.StatusActive;
                SqlParameter[] param = new SqlParameter[6] { parameterNumberOfLessons, parameterDescription, parameterCategory,
            parameterLanguage, parameterLevel,  parameterStatusActive};
                var nr = SqlHelper.ExecuteScalar(commandText, param);
                return Convert.ToInt32(nr);
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message);
                return 0;
            }
          
     }
        public override int Save(Course entity)
        {

            int rez;
            if (entity.Id == 0)
               rez= Add(entity);
            else
               rez=Update(entity);
   
            return rez;
        }

        public override int Update(Course entity)

        {
            try
            {
                if (GetById(entity.Id) == null) return 0;

                SqlDataReader read = base.GetById(entity.Id);
                //complete te object entity from database if has empty fields
                Course copyCourse =ReadRow(read);
                if (string.IsNullOrEmpty(entity.Description)) entity.Description = copyCourse.Description;
                if (object.Equals(entity.Language, null)) entity.Language = copyCourse.Language;
                if (object.Equals(entity.Level, null)) entity.Level = copyCourse.Level;
                if (object.Equals(entity.Category, null)) entity.Category = copyCourse.Category;
                //update command
                string commandText = $"Update {TableName} SET [NumberOfLessons]=@NumberOfLessons, [Description]=@Description, [Category]=@Category,[Language]=@Language, [Level]=@Level,[StatusActive]=@StatusActive where [Id]=@Id";
                SqlParameter parameterNumberOfLessons = new SqlParameter("NumberOfLessons", SqlDbType.Int);
                parameterNumberOfLessons.Value = entity.NumberOfLessons;
                SqlParameter parameterDescription = new SqlParameter("Description", SqlDbType.VarChar);
                parameterDescription.Value = entity.Description;
                SqlParameter parameterCategory = new SqlParameter("Category", SqlDbType.VarChar);
                parameterCategory.Value = entity.Category;
                SqlParameter parameterLanguage = new SqlParameter("Language", SqlDbType.VarChar);
                parameterLanguage.Value = entity.Language;
                SqlParameter parameterLevel = new SqlParameter("Level", SqlDbType.VarChar);
                parameterLevel.Value = entity.Level;
                SqlParameter parameterStatusActive = new SqlParameter("StatusActive", SqlDbType.Bit);
                parameterStatusActive.Value = entity.StatusActive;
                SqlParameter parameterId = new SqlParameter("Id", SqlDbType.Int);
                parameterId.Value = entity.Id;
                SqlParameter[] param = new SqlParameter[7] { parameterNumberOfLessons, parameterDescription, parameterCategory,
            parameterLanguage, parameterLevel,  parameterStatusActive, parameterId};
                SqlHelper.ExecuteNonQuery(commandText, param);
                return entity.Id;
            }
            catch (SqlException e)
            {

                Console.WriteLine(e.Message);
                return 0;
            }
        }
       
        public new IList<Course> GetAll()
        {
            IList<Course> lista = new List<Course>();
            SqlDataReader lista1 = base.GetAll();
            lista = ReadReader(lista1);
            return lista;
        }
        //nu functioneaza pt ca nu este realizat cu sql param
        public new IList<Course> GetByCondition(string condition)
        {
            IList<Course> lista = new List<Course>();
            SqlDataReader lista1 = base.GetByCondition(condition);
            lista = ReadReader(lista1);
            
            return lista;
        }
        public new Course GetById(int id)
        {
            SqlDataReader element= base.GetById(id);
            return ReadRow(element);

        }
       //get data from reader to list
        private static IList<Course> ReadReader(SqlDataReader read)
        {
            IList<Course> courses = new List<Course>();
            while (read.Read())
            {
                                
              var currentRow = read;
                Course course = new Course();
                course.Id = (int)currentRow["Id"];
                course.NumberOfLessons = (int)currentRow["NumberOfLessons"];
                course.Description = currentRow["Description"].ToString();

                course.Category = (CategoryTypes)Enum.Parse(typeof(CategoryTypes), currentRow["Category"].ToString());
                course.Language = (LanguageTypes)Enum.Parse(typeof(LanguageTypes), currentRow["Language"].ToString());
                course.Level = (LevelTypes)Enum.Parse(typeof(LevelTypes), currentRow["Level"].ToString());
                course.StatusActive = (bool)currentRow["StatusActive"];
                courses.Add(course);
            }
            read.Close();
            return courses;
        }
        //get data from reader for a row
        private static Course ReadRow(SqlDataReader read)
        {
            
            Course course = new Course();
            while (read.Read())
            {
                var currentRow = read;
               
                course.Id = (int)currentRow["Id"];
                course.NumberOfLessons = (int)currentRow["NumberOfLessons"];
                course.Description = currentRow["Description"].ToString();

                course.Category = (CategoryTypes)Enum.Parse(typeof(CategoryTypes), currentRow["Category"].ToString());
                course.Language = (LanguageTypes)Enum.Parse(typeof(LanguageTypes), currentRow["Language"].ToString());
                course.Level = (LevelTypes)Enum.Parse(typeof(LevelTypes), currentRow["Level"].ToString());
                course.StatusActive = (bool)currentRow["StatusActive"];
                
            }
            read.Close();
            return course;
        }
        //to string for an object course
        public string GetString(Course course)
        {
            StringBuilder str = new StringBuilder();
            str.Append($"Curs: {course.Id} ");
            str.Append($"Description:  {course.Language}, {course.Level}, {course.Category}, " +
                $"{course.Description}, number of lessons: {course.NumberOfLessons}, Active: {course.StatusActive}");
            return str.ToString();
             }
       //to string for list of course
        public string ToString(IList <Course> lista)
        {
            StringBuilder build = new StringBuilder();
            foreach (var item in lista)
            {
                build.Append($"{GetString(item) }\n");
            }
            return build.ToString();
        }
    }
}
