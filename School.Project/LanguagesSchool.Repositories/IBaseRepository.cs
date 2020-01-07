using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using SchoolDBModel.EntityTypes;

namespace LanguagesSchool.Repositories
{
    interface IBaseRepository<T> where T : EntityBase, new()
    {
        SqlDataReader GetById(int id);
        SqlDataReader GetByCondition(string condition);
        SqlDataReader GetAll();
        int Add(T entity);
        int Save(T entity);
        int Update(T entity);
        void Delete(T entity);
        void Delete(List <T> entities);
        int Count(string conditie);
        int Count();
    }
}