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
        object DtRawMaterial(ComParaModel comPara, string? CHALLAN_NO,DateTime? DT_REG_FROM, DateTime? DT_REG_TO, string? SUPPLIER, int? GRADE_SRNO, int? THICKNESS_SRNO);
        
        object IuRawMaterial(IuRawMaterialModel rawMaterialModel);
        object IuRawSlit(IuRawSlitModel rawSlitModel);
        object IuRawSlitArr(Sp_Iu_Raw_Slit_Model rawSlitModel);
        object UpdateIsSlitted(ComParaModel comPara, int SLITTING_SRNO);
        object DtSlitted(ComParaModel comPara);
        object DtRawMaterialShift(ComParaModel comPara, char MATERIAL_FLAG, string? CHALLAN_NO, string? REG_DATE_FROM, string? REG_DATE_TO);
        object IuRawMaterialShift(IuShiftRawMaterial rawCom);
        object DtDashRawInventory(ComParaModel comPara, char? MATERIAL_FLAG, DateTime? F_DATE, DateTime? TO_DATE, int? GRADE_SRNO, int? THICNESS_SRNO, decimal? WIDTH, int? STATUS_SRNO, int? C_LOCATION);
        object DtDashRawInventoryDtl(ComParaModel comPara, char? MATERIAL_FLAG, string MATERIAL_SRNOS ,string? SLITTING_SRNOS );
        object DtUsers(ComParaModel comPara);
        object DtUserTypes(ComParaModel comPara);
        object IuUser(IuUserModel userModel);

        object IuUserType(ComParaModel comPara,char IU_FLAG, int? USER_TYPE_SRNO, string USER_TYPE_NAME, string? USER_TYPE_DESC);

        object DelRawSlit(ComParaModel comPara, int SRNO, bool IS_MOTHER_COIL);
        public object IuShiftStock([FromQuery] ComParaModel comPara, char IU_FLAG, char COIL_FLAG, char STATUS_FLAG, int SRNO);
        object IuMGrade(IuMasterPara masterPara);
        object IuMThickness(IuMasterPara masterPara);
        object IuMOD(IuMasterPara masterPara);

        object DelMGrade(DelMasterPara masterPara);
        object DelMThickness(DelMasterPara masterPara);
        object DelMOD(DelMasterPara masterPara);






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
          public object DtRawMaterial(ComParaModel comPara, string? CHALLAN_NO, DateTime? DT_REG_FROM, DateTime? DT_REG_TO, string? SUPPLIER, int? GRADE_SRNO, int? THICKNESS_SRNO)
        {
            try
            {
                // Prepare the parameters for the stored procedure
                var parameters = new Dictionary<string, object>
            {
                { "@CHALLAN_NO", CHALLAN_NO },
                { "@DT_REG_FROM", DT_REG_FROM },
                { "@DT_REG_TO", DT_REG_TO },
                { "@SUPPLIER", SUPPLIER },
                { "@GRADE_SRNO", GRADE_SRNO },
                { "@THICKNESS_SRNO", THICKNESS_SRNO },
                { "@USER_SRNO", comPara.USER_SRNO},
                { "@UT_SRNO", comPara.UT_SRNO}
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
                    { "@MATERIAL_SCRAP", rawMaterialModel.MATERIAL_SCRAP},
                    { "@SUPPILERS", rawMaterialModel.SUPPILERS},
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
      public object DtRawMaterialShift([FromQuery] ComParaModel comPara, char MATERIAL_FLAG, string? CHALLAN_NO, string? REG_DATE_FROM, string? REG_DATE_TO)
        {
            try
            {
                // Prepare the parameters for the stored procedure
                var parameters = new Dictionary<string, object>

                 {
                    { "@USER_SRNO", comPara.USER_SRNO },
                    { "@MATERIAL_FLAG", MATERIAL_FLAG},
                    { "@CHALLAN_NO", CHALLAN_NO},
                    { "@REG_DATE_FROM", REG_DATE_FROM},
                    { "@REG_DATE_TO", REG_DATE_TO},
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
        public object DtDashRawInventory(ComParaModel comPara, char? MATERIAL_FLAG, DateTime? F_DATE, DateTime? TO_DATE, int? GRADE_SRNO, int? THICNESS_SRNO, decimal? WIDTH, int? STATUS_SRNO, int? C_LOCATION)
        {
            try
            {
                // Prepare the parameters for the stored procedure
                var parameters = new Dictionary<string, object>

                 {
                    { "@MATERIAL_FLAG", MATERIAL_FLAG},
                    { "@F_DATE", F_DATE},
                    { "@TO_DATE", TO_DATE},
                    { "@GRADE_SRNO", GRADE_SRNO},
                    { "@THICNESS_SRNO", THICNESS_SRNO},
                    { "@WIDTH", WIDTH},
                    { "@STATUS_SRNO", STATUS_SRNO},
                    { "@C_LOCATION", C_LOCATION},
                    { "@USER_SRNO", comPara.USER_SRNO },
                };


                // Use the singleton DatabaseHelper to execute the stored procedure
                var dataSet = _databaseHelper.ExecuteStoredProcedureAsDataSet("DT_DASH_RAW_INVENTORY", parameters);

                if (dataSet.Tables.Count == 0)
                {
                    return CommonHelper.CreateApiResponse(204, "No data found.", null);
                }

                var result = CommonHelper.SerializeDataSet(dataSet);

                // Return success response with the dataset
                return CommonHelper.CreateApiResponse(200, "Success", result);
            }

            catch (Exception ex)
            {
                return CommonHelper.CreateApiResponse(500, $"Error: {ex.Message}", null);
            }
        }  
        public object DtDashRawInventoryDtl(ComParaModel comPara, char? MATERIAL_FLAG, string MATERIAL_SRNOS, string? SLITTING_SRNOS)
        {
            try
            {
                // Prepare the parameters for the stored procedure
                var parameters = new Dictionary<string, object>

                 {
                    { "@MATERIAL_FLAG", MATERIAL_FLAG},
                    { "@MATERIAL_SRNOS", MATERIAL_SRNOS},
                    { "@SLITTING_SRNOS", SLITTING_SRNOS},
                    { "@USER_SRNO", comPara.USER_SRNO },
                };


                // Use the singleton DatabaseHelper to execute the stored procedure
                var dataSet = _databaseHelper.ExecuteStoredProcedureAsDataSet("DT_DASH_RAW_INVENTORY_DTL", parameters);

                if (dataSet.Tables.Count == 0)
                {
                    return CommonHelper.CreateApiResponse(204, "No data found.", null);
                }

                var result = CommonHelper.SerializeDataSet(dataSet);

                // Return success response with the dataset
                return CommonHelper.CreateApiResponse(200, "Success", result);
            }

            catch (Exception ex)
            {
                return CommonHelper.CreateApiResponse(500, $"Error: {ex.Message}", null);
            }
        }
        public object DtUsers(ComParaModel comPara)
        {
            try
            {
                // Prepare the parameters for the stored procedure
                var parameters = new Dictionary<string, object>

                 {
                    { "@USER_SRNO", comPara.USER_SRNO },
                };


                // Use the singleton DatabaseHelper to execute the stored procedure
                var dataSet = _databaseHelper.ExecuteStoredProcedureAsDataSet("DT_USERS", parameters);

                if (dataSet.Tables.Count == 0)
                {
                    return CommonHelper.CreateApiResponse(204, "No data found.", null);
                }

                var result = CommonHelper.SerializeDataSet(dataSet);

                // Return success response with the dataset
                return CommonHelper.CreateApiResponse(200, "Success", result);
            }
           
            catch (Exception ex)
            {
                return CommonHelper.CreateApiResponse(500, $"Error: {ex.Message}", null);
            }

        }
        public object DtUserTypes(ComParaModel comPara)
        {
            try
            {
                // Prepare the parameters for the stored procedure
                var parameters = new Dictionary<string, object>

                 {
                    { "@USER_SRNO", comPara.USER_SRNO },
                };


                // Use the singleton DatabaseHelper to execute the stored procedure
                var dataSet = _databaseHelper.ExecuteStoredProcedureAsDataSet("DT_USER_TYPES", parameters);

                if (dataSet.Tables.Count == 0)
                {
                    return CommonHelper.CreateApiResponse(204, "No data found.", null);
                }

                var result = CommonHelper.SerializeDataSet(dataSet);

                // Return success response with the dataset
                return CommonHelper.CreateApiResponse(200, "Success", result);
            }
           
            catch (Exception ex)
            {
                return CommonHelper.CreateApiResponse(500, $"Error: {ex.Message}", null);
            }

        }

        // IuUser
        public object IuUser([FromBody] IuUserModel userModel)
        {
            try
            {
                // Prepare the parameters for the stored procedure
                var parameters = new Dictionary<string, object>

                 {
                    { "@IU_FLAG", userModel.IU_FLAG},
                    { "@USERNAME", userModel.USERNAME},
                    { "@F_NAME", userModel.F_NAME},
                    { "@L_NAME", userModel.L_NAME},
                    { "@USER_TYPE", userModel.USER_TYPE},
                    { "@PASSWORD", userModel.PASSWORD},
                    { "@EMAIL", userModel.EMAIL},
                    { "@CONTACT_NO", userModel.CONTACT_NO},
                    { "@IS_ACTIVE", userModel.IS_ACTIVE},
                    { "@CREATE_BY", userModel.USER_SRNO},
                    { "@USER_SRNO", userModel.USER_SRNO_PK},
                };


                // Use the singleton DatabaseHelper to execute the stored procedure
                var dataSet = _databaseHelper.ExecuteStoredProcedureAsDataSet("IU_M_USER", parameters);

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

        public object IuUserType([FromQuery] ComParaModel comPara,char IU_FLAG, int? USER_TYPE_SRNO, string USER_TYPE_NAME, string? USER_TYPE_DESC)
        {
            try
            {
                // Prepare the parameters for the stored procedure
                var parameters = new Dictionary<string, object>

                 {
                    { "@IU_FLAG", IU_FLAG},
                    { "@USER_TYPE_NAME", USER_TYPE_NAME},
                    { "@USER_TYPE_DESC", USER_TYPE_DESC},
                    { "@USER_SRNO", comPara.USER_SRNO},
                    { "@USER_TYPE_SRNO", USER_TYPE_SRNO},
                };


                // Use the singleton DatabaseHelper to execute the stored procedure
                var dataSet = _databaseHelper.ExecuteStoredProcedureAsDataSet("IU_M_USERTYPE", parameters);

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

        public object DelRawSlit([FromQuery] ComParaModel comPara, int SRNO, bool IS_MOTHER_COIL)
        {
            try {
                var parameters = new Dictionary<string, object> { };
                string SP_NAME = "";
                if (IS_MOTHER_COIL)
                {
                    SP_NAME = "DEL_RAW_MATERIALS";
                    parameters = new Dictionary<string, object>
                    {
                        { "@MATERIAL_SRNO", SRNO},
                        { "@USER_SRNO", comPara.USER_SRNO},
                    };
                }
                else
                    {
                    SP_NAME = "DEL_SLITTING_PROCESSES";
                    parameters = new Dictionary<string, object>
                    {
                        { "@SLITTING_SRNO", SRNO},
                        { "@USER_SRNO", comPara.USER_SRNO},
                    };
                }


                // Prepare the parameters for the stored procedure
                


                // Use the singleton DatabaseHelper to execute the stored procedure
                var dataSet = _databaseHelper.ExecuteStoredProcedureAsDataSet(SP_NAME, parameters);

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
        public object IuShiftStock([FromQuery] ComParaModel comPara, char IU_FLAG, char COIL_FLAG, char STATUS_FLAG,  int SRNO)
        {
            try {
                // Prepare the parameters for the stored procedure
                var parameters = new Dictionary<string, object>

                 {
                    { "@IU_FLAG", IU_FLAG},
                    { "@COIL_FLAG", COIL_FLAG},
                    { "@STATUS_FLAG", STATUS_FLAG},
                    { "@SRNO", SRNO},
                    { "@USER_SRNO", comPara.USER_SRNO},
                    { "@UT_SRNO", comPara.UT_SRNO},
                };


                // Prepare the parameters for the stored procedure
                


                // Use the singleton DatabaseHelper to execute the stored procedure
                var dataSet = _databaseHelper.ExecuteStoredProcedureAsDataSet("IU_SHIFT_STOCK", parameters);

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


        // MASTER APIS

        public object IuMGrade([FromBody] IuMasterPara masterPara)
        {
            try
            {
                // Prepare the parameters for the stored procedure
                var parameters = new Dictionary<string, object>

                 {
                    { "@IU_FLAG", masterPara.IU_FLAG},
                    { "@GRADE", masterPara.M_NAME},
                    { "@UOM", masterPara.UOM},
                    { "@USER_SRNO", masterPara.USER_SRNO},
                    { "@UT_SRNO", masterPara.UT_SRNO},
                    { "@GRADE_SRNO", masterPara.PK_SRNO}
                };


                // Use the singleton DatabaseHelper to execute the stored procedure
                var dataSet = _databaseHelper.ExecuteStoredProcedureAsDataSet("IU_M_GRADE", parameters);

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

        public object IuMThickness([FromBody] IuMasterPara masterPara)
        {
            try
            {
                // Prepare the parameters for the stored procedure
                var parameters = new Dictionary<string, object>

                 {
                    { "@IU_FLAG", masterPara.IU_FLAG},
                    { "@THICKNESS", masterPara.M_NAME},
                    { "@UOM", masterPara.UOM},
                    { "@USER_SRNO", masterPara.USER_SRNO},
                    { "@UT_SRNO", masterPara.UT_SRNO},
                    { "@THICKNESS_SRNO", masterPara.PK_SRNO}
                };


                // Use the singleton DatabaseHelper to execute the stored procedure
                var dataSet = _databaseHelper.ExecuteStoredProcedureAsDataSet("IU_M_THICKNESS", parameters);

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

        public object IuMOD([FromBody] IuMasterPara masterPara)
        {
            try
            {
                // Prepare the parameters for the stored procedure
                var parameters = new Dictionary<string, object>

                 {
                    { "@IU_FLAG", masterPara.IU_FLAG},
                    { "@OD", masterPara.M_NAME},
                    { "@UOM", masterPara.UOM},
                    { "@USER_SRNO", masterPara.USER_SRNO},
                    { "@UT_SRNO", masterPara.UT_SRNO},
                    { "@OD_SRNO", masterPara.PK_SRNO}
                };


                // Use the singleton DatabaseHelper to execute the stored procedure
                var dataSet = _databaseHelper.ExecuteStoredProcedureAsDataSet("IU_M_OD", parameters);

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

        // MASTER DELLETE Apis
        public object DelMGrade([FromQuery] DelMasterPara masterPara)
        {
            try {
                // Prepare the parameters for the stored procedure
                var parameters = new Dictionary<string, object>

                 {
                    { "@GRADE_SRNO", masterPara.PK_SRNO},
                    { "@USER_SRNO", masterPara.USER_SRNO},
                    { "@UT_SRNO", masterPara.UT_SRNO},
                };


                // Use the singleton DatabaseHelper to execute the stored procedure
                var dataSet = _databaseHelper.ExecuteStoredProcedureAsDataSet("DEL_M_GRADE", parameters);

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

        public object DelMThickness([FromQuery] DelMasterPara masterPara)
        {
            try
            {
                // Prepare the parameters for the stored procedure
                var parameters = new Dictionary<string, object>

                 {
                    { "@THICKNESS_SRNO", masterPara.PK_SRNO},
                    { "@USER_SRNO", masterPara.USER_SRNO},
                    { "@UT_SRNO", masterPara.UT_SRNO},
                };


                // Use the singleton DatabaseHelper to execute the stored procedure
                var dataSet = _databaseHelper.ExecuteStoredProcedureAsDataSet("DEL_M_THICKNESS", parameters);

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

        public object DelMOD([FromQuery] DelMasterPara masterPara)
        {
            try
            {
                // Prepare the parameters for the stored procedure
                var parameters = new Dictionary<string, object>

                 {
                    { "@OD_SRNO", masterPara.PK_SRNO},
                    { "@USER_SRNO", masterPara.USER_SRNO},
                    { "@UT_SRNO", masterPara.UT_SRNO},
                };


                // Use the singleton DatabaseHelper to execute the stored procedure
                var dataSet = _databaseHelper.ExecuteStoredProcedureAsDataSet("DEL_M_OD", parameters);

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
