
using System.Data.SqlClient;
using System.Data;
public class DatabaseHelper 
{
    private readonly string _connectionString;

    public DatabaseHelper(string connectionString)
    {
        _connectionString = connectionString;
    }

    public DataSet ExecuteStoredProcedureAsDataSet(string storedProcedureName, Dictionary<string, object> parameters = null)
    {
        if (parameters != null && parameters.Count == 0)
        {
            parameters = null; // Set parameters to null if an empty dictionary is passed
        }

        try
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(storedProcedureName, connection))
            using (var adapter = new SqlDataAdapter(command))
            {
                command.CommandType = CommandType.StoredProcedure;

                // If parameters are provided, add them to the command
                if (parameters != null)
                {
                    foreach (var param in parameters)
                    {
                        var parameter = new SqlParameter(param.Key, param.Value ?? DBNull.Value);
                        command.Parameters.Add(parameter);
                    }
                }

                //await connection.OpenAsync();

                var dataSet = new DataSet();
                adapter.Fill(dataSet);

                return dataSet;
            }
        }
        catch (SqlException ex)
        {
            throw new ApplicationException("Database error occurred." + ex.Message);
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Unexpected error occurred." + ex.Message);
        }
    }
    /// <summary>
    /// Executes a stored procedure and returns the result as a DataSet.
    /// </summary>
    /// <param name="storedProcedureName">The name of the stored procedure.</param>
    /// <param name="parameters">The SQL parameters for the stored procedure.</param>
    /// <returns>A DataSet containing the result of the stored procedure.</returns>
    public DataSet ExecuteStoredProcedureAsDataSetOld(string storedProcedureName, SqlParameter[] parameters = null)
    {
        try
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(storedProcedureName, connection))
            using (var adapter = new SqlDataAdapter(command))
            {
                command.CommandType = CommandType.StoredProcedure;

                if (parameters != null)
                {
                    command.Parameters.AddRange(parameters);
                }

                var dataSet = new DataSet();
                adapter.Fill(dataSet);

                return dataSet;
            }
        }
        catch (SqlException ex)
        {
            throw new ApplicationException("Database error occurred." +  ex.Message);
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Unexpected error occurred." +  ex.Message);
        }
    }
}
