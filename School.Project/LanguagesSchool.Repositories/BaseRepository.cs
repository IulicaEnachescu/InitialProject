using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SchoolDBModel.EntityTypes;
using System.Data.SqlClient;
using System.Data;
using DataAccessConnection;
using System.Reflection;

namespace LanguagesSchool.Repositories
{
    public abstract class BaseRepository<T> : IBaseRepository<T> where T : EntityBase, new()
    {

        protected virtual string TableName { get;  }
        public abstract int Add(T entity);


        public abstract int Update(T entity);
        public abstract int Save(T entity);
        //ok delete item
        public void Delete(T entity)
        {
            try
            {
                if (entity.Id == 0)
                {
                    return;
                }
                string commandText = $"DELETE FROM {TableName} WHERE [Id] = @Id";
                SqlParameter parameterId = new SqlParameter("Id", SqlDbType.Int);
                parameterId.Value = entity.Id;
                SqlParameter[] param = new SqlParameter[1] { parameterId };
                SqlHelper.ExecuteNonQuery(commandText, param);
                //Console.WriteLine($"S-a sters cartea cu id-ul {obj.Id}");

            }
            catch (SqlException e)
            {
                Console.WriteLine(e.StackTrace);          }
            
        }
       //ok getall
        public virtual SqlDataReader GetAll()
        
        {
            string commandText = $"Select * from {TableName}";
                       
           SqlDataReader rows = SqlHelper.ExecuteReader(commandText);

            return rows;
        }
        
        //not ok
        //get all to list
        public virtual List<T> GetA()

        {
            try
            {
             List<T> lista = new List<T>();
            SqlDataReader rows = GetAll();
            lista= ToList(rows);
            Console.WriteLine(lista);
            return lista;
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("SqlException on GetAll in List<T>");
                return null;
            }
           
        }
        //ok select item by Id
        public SqlDataReader GetById(int id)
        {
            try
            {
                string commandText = $"SELECT * FROM {TableName} WHERE [Id] = @Id";
                SqlParameter parameterId = new SqlParameter("Id", SqlDbType.Int);
                parameterId.Value = id;
                SqlParameter[] param = new SqlParameter[1] { parameterId };
                SqlDataReader row = SqlHelper.ExecuteReader(commandText, param);
                return row;
            }
            catch (SqlException e)
            {
                Console.WriteLine("SqlException on GetById: Id not found");
                Console.WriteLine(e.Message);
                return null;
            }

            
        }

        

        //metoda nu este verificata
        //delete list
        public virtual void Delete(List<T> entities)
        {
            try
            {
                var collectionId = IdentityCollectionToSqlIdFormat(entities);
                if (string.IsNullOrEmpty(collectionId))
                {
                    return;
                }
                string commandText = $"DELETE FROM {TableName} WHERE [Id] in @(Id)";
                SqlParameter parameterId = new SqlParameter("Id", SqlDbType.Int);
                parameterId.Value = collectionId;
                SqlParameter[] param = new SqlParameter[1] { parameterId };
                SqlHelper.ExecuteNonQuery(commandText, param);
            }
            catch (SqlException e)
            {

                Console.WriteLine("SqlException on Delete: Id notfound");
                Console.WriteLine(e.Message);
            }
            
        }
        protected virtual string IdentityCollectionToSqlIdFormat(List<T> collection)
        {
            var array = collection.Select(x => x.Id);
            return string.Join(",", array);
        }
        //metoda arunca exceptie
        //procedura sql:
        /*CREATE PROCEDURE Usp_Test 
	-- Add the parameters for the stored procedure here
	
	@tableName Varchar(max),
	@whereCond Varchar(max),
	@tot Int OUTPUT
	
AS
BEGIN
	 Set NoCount ON
    Declare @SQLQuery AS Varchar(max)
    Set @SQLQuery = 'Select count(Id) From '+@tableName+' where ' + @whereCond INTO @tot
    Execute sp_Executesql @SQLQuery
END*/
        public int Count(string conditie)
        {
            
            try
            {
               // var command = $"select count(Id) from @TableName where @Conditie";
                SqlParameter parametertableName = new SqlParameter("tabelName", SqlDbType.VarChar);
                parametertableName.Value = TableName;
                SqlParameter parameterwhereCond = new SqlParameter("whereCond", SqlDbType.VarChar);
                parameterwhereCond.Value = conditie;
                SqlParameter[] param = new SqlParameter[2] { parametertableName, parameterwhereCond};
                //var count = SqlHelper.ExecuteScalar(command, param);
                //return Convert.ToInt32(count);
                var connection = ConnectionManager.GetConnection();
                

                SqlCommand sqlComm = new SqlCommand("Usp_Test", connection);
                sqlComm.CommandType = CommandType.StoredProcedure;


                // System.InvalidCastException: 'The SqlParameterCollection only accepts non-null 
                //SqlParameter type objects, not SqlParameter[] objects.'
                sqlComm.Parameters.Add(param);
                var i=sqlComm.ExecuteNonQuery();
                return (dynamic)i;

            }
            catch (SqlException e)
            {

                Console.WriteLine(e.Message);
                throw;
            }
        }
        

/*using (var con = new SqlConnection("ConnectionString"))
            {
                using (var cmd = new SqlCommand("stp_UpsertMyTable", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@MyTable", SqlDbType.Structured).Value = dt;
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }*/

        public int Count()
        {
                try
                {
                    var command = $"select count(Id) from {TableName}";
                    var count =SqlHelper.ExecuteScalar(command);
                    return Convert.ToInt32(count);

                }
                catch (SqlException e)
                {

                    Console.WriteLine(e.Message);
                    throw;
                }

        }
        public virtual List<T> ToList(SqlDataReader rdr)
        {

            List<T> ret = new List<T>();
            T entity = new T();
            Type typ = typeof(T);
            PropertyInfo col;
            List<PropertyInfo> columns = new List<PropertyInfo>();
            Type TypeT = typeof(T);
            ConstructorInfo ctor = TypeT.GetConstructor(Type.EmptyTypes);
            if (ctor == null)
            {
                throw new InvalidOperationException($"Type {TypeT.Name} does not have a default constructor.");
            }
            while (rdr.Read())
            {
                T newInst = (T)ctor.Invoke(null);
                for (int i = 0; i < rdr.FieldCount; i++)
                {
                    string propName = rdr.GetName(i);
                    PropertyInfo propInfo = TypeT.GetProperty(propName);
                    if (propInfo != null)
                    {
                        object value = rdr.GetValue(i);
                        if (value == DBNull.Value)
                        {
                            propInfo.SetValue(newInst, null);
                        }
                        else
                        {
                            propInfo.SetValue(newInst, value);
                        }
                        ret.Add(entity);
                    }
                }
            }
                // Get all the properties in Entity Class
                PropertyInfo[] props = typ.GetProperties();

                for (int index = 0; index < rdr.FieldCount; index++)
                {
                    // See if column name maps directly to property name
                    // Console.WriteLine(props[index]);  
                    col = props.FirstOrDefault(c => c.Name == rdr.GetName(index));
                    if (col != null)
                    {
                        // Console.WriteLine(col.Name);
                        columns.Add(col);
                    }
                }

                while (rdr.Read())
                {
                    //entity = Activator.CreateInstance(entity);
                    entity = Activator.CreateInstance<T>();
                    for (int i = 0; i < columns.Count; i++)
                    {
                        if (rdr[columns[i].Name].Equals(DBNull.Value))
                        {
                            columns[i].SetValue(entity, null, null);
                        }

                        else
                        {
                            //Convert.ChangeType(value, propertyInfo.PropertyType)
                            // rdr[columns[i].Name] = Convert.ChangeType(rdr[columns[i].Name, props[i]);
                            // dynamic item = rdr[columns[i].Name].GetType();
                            //Console.WriteLine(item);
                            columns[i].SetValue(entity, rdr[columns[i].Name], null);
                            Console.WriteLine(columns[i]);
                        }
                    }

                    ret.Add(entity);
                }
                rdr.Close();
                return ret;
            }
       

            //Nu se face transferul parametrilor
            public SqlDataReader GetByCondition(string condition)
            {
                try
                {
                    string commandText = $"SELECT * FROM {TableName} WHERE {condition}";

                    SqlDataReader row = SqlHelper.ExecuteReader(commandText);
                    return row;
                }
                catch (SqlException e)
                {
                    Console.WriteLine("SqlException on GetByCondition");
                    Console.WriteLine(e.Message);
                    return null;
                }
            //String[] strArr;

            // if (Str.Contains(' ')) // this is to optimize code
            // {
            //   strArr = Str.Split(' ').ToArray();
           

        }
        

           
    }
}
/*public static IEnumerable<T> Query<T>(this SqlConnection cn, string sql) {
           Type TypeT = typeof(T);
           ConstructorInfo ctor = TypeT.GetConstructor(Type.EmptyTypes);
           if (ctor == null) {
               throw new InvalidOperationException($"Type {TypeT.Name} does not have a default constructor.");
           }
           using (SqlCommand cmd = new SqlCommand(sql, cn)) {
               using (SqlDataReader reader = cmd.ExecuteReader()) {
                   while (reader.Read()) {
                       T newInst = (T)ctor.Invoke(null);
                       for (int i = 0; i < reader.FieldCount; i++) {
                           string propName = reader.GetName(i);
                           PropertyInfo propInfo = TypeT.GetProperty(propName);
                           if (propInfo != null) {
                               object value = reader.GetValue(i);
                               if (value == DBNull.Value) {
                                   propInfo.SetValue(newInst, null);
                               } else {
                                   propInfo.SetValue(newInst, value);
                               }
                           }
                       }
                       yield return newInst;
                   }
               }*/
/*public static List<T> ToList (SqlDataReader table) 
{
 List<PropertyInfo> properties = typeof(T).GetProperties().ToList();
 List<T> result = new List<T>();

 while (table.Read())
     {
     var currentRow = table;

     {
     var item = CreateItemFromRow<T>((DataRow)row, properties);
     result.Add(item);
 }

 return result;
}*/
// Get data from SqlReader to Generic List 
