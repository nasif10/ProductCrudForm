using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ProductCrudForm.DbService
{
    public class DbAccess
    {
        private readonly string _connectionString;

        public DbAccess()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        }

        public DataSet GetResultSet(string commandText, params object[] parameters)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = commandText;
                    cmd.CommandType = CommandType.StoredProcedure;

                    for (int i = 0; i < parameters.Length; i++)
                    {
                        cmd.Parameters.AddWithValue($"@param{i}", parameters[i] ?? DBNull.Value);
                    }

                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    sda.Fill(ds);
                    return ds;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool ExecuteCommand(string commandText, params object[] parameters)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;
                    cmd.CommandText = commandText;
                    cmd.CommandType = CommandType.StoredProcedure;

                    for (int i = 0; i < parameters.Length; i++)
                    {
                        cmd.Parameters.AddWithValue($"@param{i}", parameters[i] ?? DBNull.Value);
                    }

                    int affectedRows = cmd.ExecuteNonQuery();
                    return affectedRows > 0;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}