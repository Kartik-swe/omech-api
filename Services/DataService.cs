using Microsoft.AspNetCore.Mvc;
using omech.Models;
using omech.Helpers;
using System.Data.SqlClient;
using Newtonsoft.Json;

namespace omech.Services
{
    public interface IDataService
    {
        /// <summary>
        /// Gets the response for the dataset request.      
        /// </summary>
        /// <returns>A standardized API response object.</returns>
        object Pl_Common(ComParaModel comPara, String TBL_SRNO);
        object DtRawMaterial(ComParaModel comPara);
        
        object IuRawMaterial(IuRawMaterialModel rawMaterialModel);
        object IuRawSlit(IuRawSlitModel rawSlitModel);
        object IuRawSlitArr(Sp_Iu_Raw_Slit_Model rawSlitModel);
        object UpdateIsSlitted(ComParaModel comPara, int SLITTING_SRNO);
        object DtSlitted(ComParaModel comPara);
        object DtRawMaterialShift(ComParaModel comPara, char MATERIAL_FLAG);
        object IuRawMaterialShift(IuShiftRawMaterial rawCom);
        


    }
    public class DataService : IDataService
    {

        private readonly DatabaseHelper _databaseHelper;

        // Injecting the DatabaseHelper instance through the constructor
        public DataService(DatabaseHelper databaseHelper)
        {
            _databaseHelper = databaseHelper;
        }

        public object Pl_Common(ComParaModel comPara, string TBL_SRNO)
        {
            try
            {
                // Validate input parameters to ensure they are not null or invalid
                if (string.IsNullOrEmpty(TBL_SRNO))
                {
                    return CommonHelper.CreateApiResponse(400, "Invalid input parameters.", null); // 400 Bad Request
                }

                // Prepare the parameters for the stored procedure
                var parameters = new Dictionary<string, object>
            {
                { "@USER_SRNO", comPara.USER_SRNO},
                { "@TBL_SRNO", TBL_SRNO.Trim() }
            };

                // Execute the stored procedure
                var dataSet = _databaseHelper.ExecuteStoredProcedureAsDataSet("PLD_COMMON", parameters);

                // Check if the dataset is empty or contains no data
                if (dataSet == null || dataSet.Tables.Count == 0)
                {
                    return CommonHelper.CreateApiResponse(204, "No data found.", null); // 204 No Content
                }

                // Serialize dataset to the desired format
                var result = CommonHelper.SerializeDataSet(dataSet);

                // Return success response with the dataset
                return CommonHelper.CreateApiResponse(200, "Success", result); // 200 OK
            }
            catch (Exception ex)
            {
                // Log the unexpected error
                return CommonHelper.CreateApiResponse(500, $"An unexpected error occurred: {ex.Message}", null); // 500 Internal Server Error
            }
        }
          public object DtRawMaterial(ComParaModel comPara)
        {
            try
            {
                // Prepare the parameters for the stored procedure
                var parameters = new Dictionary<string, object>
            {
                { "@USER_SRNO", comPara.USER_SRNO},
                //{ "@TBL_SRNO", TBL_SRNO.Trim() }
            };

                // Execute the stored procedure
                var dataSet = _databaseHelper.ExecuteStoredProcedureAsDataSet("DT_RAW_MATERIALs", parameters);

                // Check if the dataset is empty or contains no data
                if (dataSet == null || dataSet.Tables.Count == 0)
                {
                    return CommonHelper.CreateApiResponse(204, "No data found.", null); // 204 No Content
                }

                // Serialize dataset to the desired format
                //var result = JsonConvert.ser(dataSet);
                var result = CommonHelper.SerializeDataSet(dataSet);

                // Return success response with the dataset
                return CommonHelper.CreateApiResponse(200, "Success", result); // 200 OK
            }
            catch (Exception ex)
            {
                // Log the unexpected error
                return CommonHelper.CreateApiResponse(500, $"An unexpected error occurred: {ex.Message}", null); // 500 Internal Server Error
            }
        }

        public object IuRawMaterial([FromBody] IuRawMaterialModel rawMaterialModel)  // TWO C_LOCATION
        {
            try
            {
                // Prepare the parameters for the stored procedure
                var parameters = new Dictionary<string, object>

                 {
                    { "@IU_FLAG", rawMaterialModel.IU_FLAG },
                    { "@MATERIAL_C_LOCATION", rawMaterialModel.MATERIAL_C_LOCATION },
                    { "@CHALLAN_NO", rawMaterialModel.CHALLAN_NO },
                    { "@MATERIAL_GRADE_SRNO", rawMaterialModel.MATERIAL_GRADE },
                    { "@MATERIAL_THICKNESS_SRNO", rawMaterialModel.MATERIAL_THICKNESS },
                    { "@MATERIAL_WIDTH", rawMaterialModel.MATERIAL_WIDTH },
                    { "@MATERIAL_WEIGHT", rawMaterialModel.MATERIAL_WEIGHT},
                    { "@RECEIVED_DATE", rawMaterialModel.RECEIVED_DATE},
                    { "@MATERIAL_STATUS_SRNO", rawMaterialModel.MATERIAL_STATUS_SRNO},
                    { "@USER_SRNO", rawMaterialModel.USER_SRNO },
                    { "@MATERIAL_SRNO", rawMaterialModel.MATERIAL_SRNO },
                };


                // Use the singleton DatabaseHelper to execute the stored procedure
                var dataSet = _databaseHelper.ExecuteStoredProcedureAsDataSet("IU_RAW_MATERIALS", parameters);

                if (dataSet.Tables.Count == 0 || dataSet.Tables[0].Rows.Count == 0)
                {
                    return CommonHelper.CreateApiResponse(204, "No data found.", null);
                }

                var result = CommonHelper.SerializeDataSet(dataSet);

                // Return success response with the dataset
                return CommonHelper.CreateApiResponse(200, "Success", result);
            }
            catch (SqlException sqlEx)
            {
                return CommonHelper.CreateApiResponse(500, $"SQL Error: {sqlEx.Message}", null);
            }
            catch (Exception ex)
            {
                return CommonHelper.CreateApiResponse(500, $"Error: {ex.Message}", null);
            }
        }

        public object IuRawSlit([FromBody] IuRawSlitModel model)
        {
            try
            {
                // Prepare the parameters for the stored procedure
                var parameters = new Dictionary<string, object>

                 {
                    { "@IU_FLAG", model.IU_FLAG },
                    { "@MATERIAL_SRNO", model.MATERIAL_SRNO },
                    { "@SLITTING_SRNO_FK", model.SLITTING_SRNO_FK },
                    { "@C_LOCATION", model.C_LOCATION },
                    { "@SLITTING_LEVEL", model.SLITTING_LEVEL },
                    { "@SLITTING_DATE", model.SLITTING_DATE},
                    { "@SLITTING_GRADE_SRNO", model.SLITTING_GRADE_SRNO },
                    { "@SLITTING_THICKNESS_SRNO", model.SLITTING_GRADE_SRNO },
                    { "@SLITTING_WIDTH", model.SLITTING_WIDTH },
                    { "@SLITTING_WEIGHT", model.SLITTING_WEIGHT},
                    { "@DC_NO", model.DC_NO},
                    { "@STATUS_SRNO", model.STATUS_SRNO},
                    { "@IS_SLITTED", model.IS_SLITTED},
                    { "@USER_SRNO", model.USER_SRNO },
                    { "@SLITTING_SCRAP", model.SLITTING_SCRAP },
                    { "@SLITTING_SRNO", model.SLITTING_SRNO },
                };


                // Use the singleton DatabaseHelper to execute the stored procedure
                var dataSet = _databaseHelper.ExecuteStoredProcedureAsDataSet("IU_SLITTING_PROCESSES", parameters);

                if (dataSet.Tables.Count == 0 || dataSet.Tables[0].Rows.Count == 0)
                {
                    return CommonHelper.CreateApiResponse(204, "No data found.", null);
                }

                var result = CommonHelper.SerializeDataSet(dataSet);

                // Return success response with the dataset
                return CommonHelper.CreateApiResponse(200, "Success", result);
            }
            catch (SqlException sqlEx)
            {
                return CommonHelper.CreateApiResponse(500, $"SQL Error: {sqlEx.Message}", null);
            }
            catch (Exception ex)
            {
                return CommonHelper.CreateApiResponse(500, $"Error: {ex.Message}", null);
            }
        }

        public object IuRawSlitArr([FromBody] Sp_Iu_Raw_Slit_Model model)
        {
            try
            {
                // Prepare the parameters for the stored procedure
                var slitDetailsJson = JsonConvert.SerializeObject(model.SlitDetails);

                var parameters = new Dictionary<string, object>

                 {
                    { "@MATERIAL_SRNO", model.MATERIAL_SRNO },
                    //{ "@C_LOCATION", model.C_LOCATION },
                    { "@SLITTING_SRNO_FK", model.SLITTING_SRNO_FK },
                    { "@SLITTING_LEVEL", model.SLITTING_LEVEL },
                    { "@SLITTING_DATE", model.SLITTING_DATE},
                    { "@DC_NO", model.DC_NO},
                    { "@C_LOCATION", model.C_LOCATION},
                    { "@SCRAP", model.SCRAP},
                    { "@CREATED_BY", model.USER_SRNO },
                    { "@slitDetails", slitDetailsJson },
                };


                // Use the singleton DatabaseHelper to execute the stored procedure
                var dataSet = _databaseHelper.ExecuteStoredProcedureAsDataSet("SP_IU_SLITTING_PROCESSES", parameters);

                if (dataSet.Tables.Count == 0 || dataSet.Tables[0].Rows.Count == 0)
                {
                    return CommonHelper.CreateApiResponse(204, "No data found.", null);
                }

                var result = CommonHelper.SerializeDataSet(dataSet);

                // Return success response with the dataset
                return CommonHelper.CreateApiResponse(200, "Success", result);
            }
            catch (SqlException sqlEx)
            {
                return CommonHelper.CreateApiResponse(500, $"SQL Error: {sqlEx.Message}", null);
            }
            catch (Exception ex)
            {
                return CommonHelper.CreateApiResponse(500, $"Error: {ex.Message}", null);
            }

        }
        public object UpdateIsSlitted([FromBody] ComParaModel comPara, int SLITTING_SRNO)
        {
            try
            {
                // Prepare the parameters for the stored procedure
                var parameters = new Dictionary<string, object>

                 {
                    { "@IU_FLAG", 'U' },
                    { "@USER_SRNO", comPara.USER_SRNO },
                    { "@SLITTING_SRNO", SLITTING_SRNO},
                };


                // Use the singleton DatabaseHelper to execute the stored procedure
                var dataSet = _databaseHelper.ExecuteStoredProcedureAsDataSet("UPDATE_IS_SLITTED", parameters);

                if (dataSet.Tables.Count == 0 || dataSet.Tables[0].Rows.Count == 0)
                {
                    return CommonHelper.CreateApiResponse(204, "No data found.", null);
                }

                var result = CommonHelper.SerializeDataSet(dataSet);

                // Return success response with the dataset
                return CommonHelper.CreateApiResponse(200, "Success", result);
            }
            catch (SqlException sqlEx)
            {
                return CommonHelper.CreateApiResponse(500, $"SQL Error: {sqlEx.Message}", null);
            }
            catch (Exception ex)
            {
                return CommonHelper.CreateApiResponse(500, $"Error: {ex.Message}", null);
            }

        } 
        public object DtSlitted([FromBody] ComParaModel comPara)
        {
            try
            {
                // Prepare the parameters for the stored procedure
                var parameters = new Dictionary<string, object>

                 {
                    { "@USER_SRNO", comPara.USER_SRNO },
                };


                // Use the singleton DatabaseHelper to execute the stored procedure
                var dataSet = _databaseHelper.ExecuteStoredProcedureAsDataSet("DT_SLITTED", parameters);

                if (dataSet.Tables.Count == 0 || dataSet.Tables[0].Rows.Count == 0)
                {
                    return CommonHelper.CreateApiResponse(204, "No data found.", null);
                }

                var result = CommonHelper.SerializeDataSet(dataSet);

                // Return success response with the dataset
                return CommonHelper.CreateApiResponse(200, "Success", result);
            }
            catch (SqlException sqlEx)
            {
                return CommonHelper.CreateApiResponse(500, $"SQL Error: {sqlEx.Message}", null);
            }
            catch (Exception ex)
            {
                return CommonHelper.CreateApiResponse(500, $"Error: {ex.Message}", null);
            }

        }
      public object DtRawMaterialShift([FromQuery] ComParaModel comPara, char MATERIAL_FLAG)
        {
            try
            {
                // Prepare the parameters for the stored procedure
                var parameters = new Dictionary<string, object>

                 {
                    { "@USER_SRNO", comPara.USER_SRNO },
                    { "@MATERIAL_FLAG", MATERIAL_FLAG},
                };


                // Use the singleton DatabaseHelper to execute the stored procedure
                var dataSet = _databaseHelper.ExecuteStoredProcedureAsDataSet("DT_RAW_MATERIAL_SHIFT", parameters);

                if (dataSet.Tables.Count == 0 )
                {
                    return CommonHelper.CreateApiResponse(204, "No data found.", null);
                }

                var result = CommonHelper.SerializeDataSet(dataSet);

                // Return success response with the dataset
                return CommonHelper.CreateApiResponse(200, "Success", result);
            }
            catch (SqlException sqlEx)
            {
                return CommonHelper.CreateApiResponse(500, $"SQL Error: {sqlEx.Message}", null);
            }
            catch (Exception ex)
            {
                return CommonHelper.CreateApiResponse(500, $"Error: {ex.Message}", null);
            }

        }
        public object IuRawMaterialShift([FromBody] IuShiftRawMaterial rawCom)
        {
            try
            {
                // Prepare the parameters for the stored procedure
                var parameters = new Dictionary<string, object>

                 {
                    { "@IU_FLAG", rawCom.IU_FLAG },
                    { "@MATERIAL_SRNO", rawCom.MATERIAL_SRNO },
                    { "@SLITTING_SRNO", rawCom.SLITTING_SRNO },
                    { "@FROM_LOCATION", rawCom.FROM_LOCATION },
                    { "@TO_LOCATION", rawCom.TO_LOCATION },
                    { "@SHIFT_DATE", rawCom.SHIFT_DATE },
                    { "@SHIFTING_SRNO", rawCom.SHIFTING_SRNO },
                    { "@USER_SRNO", rawCom.USER_SRNO },
                };


                // Use the singleton DatabaseHelper to execute the stored procedure
                var dataSet = _databaseHelper.ExecuteStoredProcedureAsDataSet("IU_MATERIAL_SHIFT_HISTORY", parameters);

                if (dataSet.Tables.Count == 0 || dataSet.Tables[0].Rows.Count == 0)
                {
                    return CommonHelper.CreateApiResponse(204, "No data found.", null);
                }

                var result = CommonHelper.SerializeDataSet(dataSet);

                // Return success response with the dataset
                return CommonHelper.CreateApiResponse(200, "Success", result);
            }
            catch (SqlException sqlEx)
            {
                return CommonHelper.CreateApiResponse(500, $"SQL Error: {sqlEx.Message}", null);
            }
            catch (Exception ex)
            {
                return CommonHelper.CreateApiResponse(500, $"Error: {ex.Message}", null);
            }

        }
    }
}
