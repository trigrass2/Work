using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAllocator
{
    public static class Scope
    {
        private static readonly object locker = new object();
        private static readonly string fName = "errors.log";
        private static readonly string fgName = "global errors.log";

        public static void WriteGlobalError(string message)
        {

            lock (locker)
            {
                try
                {
                    using (StreamWriter sw = new StreamWriter(File.Open(fgName, FileMode.Append, FileAccess.Write), Encoding.UTF8))
                    {
                        sw.WriteLine(String.Format("{0} {1}", DateTime.Now.ToString() + " - ", message));
                    }
                }
                catch (Exception) { }
            }


        }

        public static void WriteError(string message)
        {
            
            lock (locker)
            {
                try
                {
                    using (StreamWriter sw = new StreamWriter(File.Open(fName, FileMode.Append, FileAccess.Write), Encoding.UTF8))
                    {
                        sw.WriteLine(String.Format("{0} {1}", DateTime.Now.ToString() + " - ", message));
                    }
                }
                catch (Exception) { }
            }


        }

        private static DataTable ToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection props =
                TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prop = props[i];
                DataColumn dataColumn = new DataColumn();
                dataColumn.AllowDBNull = true;
                dataColumn.ColumnName = prop.Name;
                dataColumn.DataType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                table.Columns.Add(dataColumn);
            }
            object[] values = new object[props.Count];
            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = props[i].GetValue(item);
                }
                table.Rows.Add(values);
            }
            return table;
        }

        public static void BulkInsert<T>(IList<T> data, SqlConnection connection, string tablename)
        {
            DataTable dataTable = ToDataTable(data);
            SqlTransaction transaction = null;
            connection.Open();
            try
            {
                transaction = connection.BeginTransaction();
                using (var sqlBulkCopy = new SqlBulkCopy(connection, SqlBulkCopyOptions.TableLock, transaction))
                {
                    sqlBulkCopy.DestinationTableName = tablename;
                    foreach (DataColumn dc in dataTable.Columns)
                    {
                        sqlBulkCopy.ColumnMappings.Add(dc.ColumnName, dc.ColumnName);
                    }
                    sqlBulkCopy.WriteToServer(dataTable);
                }
                transaction.Commit();
            }
            catch (Exception e)
            {
                transaction.Rollback();
                throw e;
            }
        }


    }
}
