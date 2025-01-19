using Dapper;
using System.Data;
using System.Data.SqlClient;


namespace omech.Helpers
{
    public class CommonHelper
    {

        /// <summary>
        /// Serializes a DataSet to a dictionary for JSON compatibility.
        /// </summary>
        /// <param name="dataSet">The DataSet to serialize.</param>
        /// <returns>A dictionary representation of the DataSet.</returns>
        public static Dictionary<string, List<Dictionary<string, object>>> SerializeDataSet(DataSet dataSet)
        {
            var result = new Dictionary<string, List<Dictionary<string, object>>>();

            foreach (DataTable table in dataSet.Tables)
            {
                var tableName = string.IsNullOrWhiteSpace(table.TableName) ? "Table" : table.TableName;
                var rows = new List<Dictionary<string, object>>();

                foreach (DataRow row in table.Rows)
                {
                    var rowDict = new Dictionary<string, object>();
                    foreach (DataColumn col in table.Columns)
                    {
                        // Convert DBNull to null
                        rowDict[col.ColumnName] = row[col] == DBNull.Value ? null : row[col];
                        //rowDict[col.ColumnName] = row[col];
                    }
                    rows.Add(rowDict);
                }

                result[tableName] = rows;
            }

            return result;
        }

        /// <summary>
        /// Creates a standardized API response object.
        /// </summary>
        /// <param name="msgId">The status message ID (1 for success, 0 for failure).</param>
        /// <param name="msg">The status message.</param>
        /// <param name="data">The data to include in the response.</param>
        /// <returns>A standardized API response object.</returns>
        public static object CreateApiResponse(int msgId, string msg, object data = null)
        {
            return new
            {
                msgId,
                msg,
                data
            };
        }

    }
}
