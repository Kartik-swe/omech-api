using Microsoft.AspNetCore.Mvc;
using omech.Models;
using omech.Helpers;
using System.Data.SqlClient;
using Newtonsoft.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Data;

namespace omech.Services
{
    public interface IDataService
    {
        /// <summary>
        /// Gets the response for the dataset request.      
        /// </summary>
        /// <returns>A standardized API response object.</returns>
        object Pl_Common(ComParaModel comPara, string TBL_SRNO);
        object DtRawMaterial(ComParaModel comPara, string? CHALLAN_NO,DateTime? DT_REG_FROM, DateTime? DT_REG_TO, string? SUPPLIER, int? GRADE_SRNO, int? THICKNESS_SRNO);
        
        object IuRawMaterial(IuRawMaterialModel rawMaterialModel);
        object IuRawSlit(IuRawSlitModel rawSlitModel);
        object IuRawSlitArr(Sp_Iu_Raw_Slit_Model rawSlitModel);
        object UpdateIsSlitted(ComParaModel comPara, int SRNO, char STATUS_FLAG, char COIL_FLAG);
        object DtSlitted(ComParaModel comPara);
        object DtRawMaterialShift(ComParaModel comPara, char MATERIAL_FLAG, string? CHALLAN_NO, string? REG_DATE_FROM, string? REG_DATE_TO, int? GRADE_SRNO, int? THICNESS_SRNO, int? C_LOCATION, int? TUBE_MILL_SRNO);
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
        object IuMLocation(IuMasterPara masterPara);
        object IuMTubeMill(IuMasterPara masterPara);

        object DelMGrade(DelMasterPara masterPara);
        object DelMThickness(DelMasterPara masterPara);
        object DelMOD(DelMasterPara masterPara);
        object DelMLocation(DelMasterPara masterPara);

        object IuStatusLog(IuStatusLogModel statusLogModel);
        object IuPipes(IuPipesModel iuPipesModel);
        object DtPipes(ComParaModel comPara, int? PR_SRNO, int? GRADE_SRNO, int? THICKNESS_SRNO, int? OD_SRNO, int? C_LOCATION, int? PR_LENGTH);
        object DtPipesLogs(ComParaModel comPara, int? PR_SRNO, int? GRADE_SRNO, int? THICKNESS_SRNO, int? OD_SRNO, int? C_LOCATION, int? PR_LENGTH, int? INV_TYPE, DateTime? DTP_FROM, DateTime? DTP_TO);
        object IuPipeShiftLocation([FromBody] IuPipeShiftLocation_Model iuPipeShiftLocation);
        object IuPipeShiftAction([FromBody] IuPipeShiftLocation_Model iuPipeShiftLocation);
        object IuPipesInvPr([FromBody] IuPipesInvPrModel IuPipesInvPr);





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
                    { "@MATERIAL_TYPE", rawMaterialModel.MATERIAL_TYPE },
                    { "@MATERIAL_WIDTH", rawMaterialModel.MATERIAL_WIDTH },
                    { "@MATERIAL_WEIGHT", rawMaterialModel.MATERIAL_WEIGHT},
                    { "@RECEIVED_DATE", rawMaterialModel.RECEIVED_DATE},
                    { "@MATERIAL_STATUS_SRNO", rawMaterialModel.MATERIAL_STATUS_SRNO},
                    { "@MATERIAL_SCRAP", rawMaterialModel.MATERIAL_SCRAP},
                    { "@MATERIAL_SCRAP_WEIGHT", rawMaterialModel.MATERIAL_SCRAP_WEIGHT},
                    { "@SUPPLIER", rawMaterialModel.SUPPLIER},
                    { "@NOS", rawMaterialModel.NOS},
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
                    { "@SLITTING_SCRAP_WEIGHT", model.SLITTING_SCRAP_WEIGHT},
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
                    { "@SLITTING_SCRAP_WEIGHT", model.SLITTING_SCRAP_WEIGHT},
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
        public object UpdateIsSlitted([FromBody] ComParaModel comPara, int SRNO, char STATUS_FLAG, char COIL_FLAG)
        {
            try
            {
                // Prepare the parameters for the stored procedure
                var parameters = new Dictionary<string, object>

                 {
                    { "@IU_FLAG", 'U' },
                    { "@USER_SRNO", comPara.USER_SRNO },
                    { "@SRNO", SRNO},
                    { "@STATUS_FLAG", STATUS_FLAG},
                    { "@COIL_FLAG", COIL_FLAG},
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
      public object DtRawMaterialShift([FromQuery] ComParaModel comPara, char MATERIAL_FLAG, string? CHALLAN_NO, string? REG_DATE_FROM, string? REG_DATE_TO, int? GRADE_SRNO, int? THICNESS_SRNO, int? C_LOCATION, int? TUBE_MILL_SRNO)
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
                    { "@GRADE_SRNO", GRADE_SRNO},
                    { "@THICNESS_SRNO", THICNESS_SRNO},
                    { "@C_LOCATION", C_LOCATION},
                    { "@TUBE_MILL_SRNO", TUBE_MILL_SRNO},
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
        public object IuMLocation([FromBody] IuMasterPara masterPara)
        {
            try
            {
                // Prepare the parameters for the stored procedure
                var parameters = new Dictionary<string, object>

                 {
                    { "@IU_FLAG", masterPara.IU_FLAG},
                    { "@LOCATION", masterPara.M_NAME},
                    { "@UOM", masterPara.UOM},
                    { "@USER_SRNO", masterPara.USER_SRNO},
                    { "@UT_SRNO", masterPara.UT_SRNO},
                    { "@VENDOR_SRNO", masterPara.PK_SRNO}
                };


                // Use the singleton DatabaseHelper to execute the stored procedure
                var dataSet = _databaseHelper.ExecuteStoredProcedureAsDataSet("IU_M_LOCATION", parameters);

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

        public object IuMTubeMill([FromBody] IuMasterPara masterPara)
        {
            try
            {
                // Prepare the parameters for the stored procedure
                var parameters = new Dictionary<string, object>

                 {
                    { "@IU_FLAG", masterPara.IU_FLAG},
                    { "@MACHINE_TYPE", 'T'},
                    { "@MACHINE_NAME", masterPara.M_NAME},
                    { "@UOM", masterPara.UOM},
                    { "@USER_SRNO", masterPara.USER_SRNO},
                    { "@UT_SRNO", masterPara.UT_SRNO},
                    { "@MACHINE_SRNO", masterPara.PK_SRNO}
                };


                // Use the singleton DatabaseHelper to execute the stored procedure
                var dataSet = _databaseHelper.ExecuteStoredProcedureAsDataSet("IU_M_MACHINE", parameters);

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

        public object DelMLocation([FromQuery] DelMasterPara masterPara)
        {
            try
            {
                // Prepare the parameters for the stored procedure
                var parameters = new Dictionary<string, object>

                 {
                    { "@VENDOR_SRNO", masterPara.PK_SRNO},
                    { "@USER_SRNO", masterPara.USER_SRNO},
                    { "@UT_SRNO", masterPara.UT_SRNO},
                };


                // Use the singleton DatabaseHelper to execute the stored procedure
                var dataSet = _databaseHelper.ExecuteStoredProcedureAsDataSet("DEL_M_LOCATION", parameters);

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
        public object DelM_MACHINE([FromQuery] DelMasterPara masterPara)
        {
            try
            {
                // Prepare the parameters for the stored procedure
                var parameters = new Dictionary<string, object>

                 {
                    { "@MACHINE_SRNO", masterPara.PK_SRNO},
                    { "@USER_SRNO", masterPara.USER_SRNO},
                    { "@UT_SRNO", masterPara.UT_SRNO},
                };


                // Use the singleton DatabaseHelper to execute the stored procedure
                var dataSet = _databaseHelper.ExecuteStoredProcedureAsDataSet("DEL_M_MACHINE", parameters);

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

        public object IuStatusLog([FromQuery] IuStatusLogModel statusLogModel)
        {
   
            try
            {
                // Prepare the parameters for the stored procedure
                var parameters = new Dictionary<string, object>

                 {
                    { "@MATERIAL_SRNO", statusLogModel.MATERIAL_SRNO},
                    { "@SLITTING_SRNO", statusLogModel.SLITTING_SRNO},
                    { "@PRE_LOG_STATUS_SRNO", statusLogModel.PRE_LOG_STATUS_SRNO},
                    { "@DESCRIPTION", statusLogModel.DESCRIPTION},
                    { "@REMARKS", statusLogModel.REMARKS},
                    { "@STATUS_CHANGE_DATE", statusLogModel.STATUS_CHANGE_DATE},
                    { "@USER_SRNO", statusLogModel.USER_SRNO},
                    { "@UT_SRNO", statusLogModel.UT_SRNO},
                    { "@LOG_STATUS_SRNO", statusLogModel.LOG_STATUS_SRNO},
                };


                // Use the singleton DatabaseHelper to execute the stored procedure
                var dataSet = _databaseHelper.ExecuteStoredProcedureAsDataSet("IU_STATUS_LOG", parameters);

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

        public object IuPipes([FromBody] IuPipesModel iuPipesModel)
        { 
            try
            {
                // Prepare the parameters for the stored procedure
                var parameters = new Dictionary<string, object>

                 {
                    { "@IU_FLAG", iuPipesModel.IU_FLAG},
                    { "@MATERIAL_SRNO", iuPipesModel.MATERIAL_SRNO},
                    { "@SLITTING_SRNO", iuPipesModel.SLITTING_SRNO},
                    { "@MACHINE_SRNO", iuPipesModel.MACHINE_SRNO},
                    { "@OD_SRNO", iuPipesModel.OD_SRNO},
                    { "@GRADE_SRNO", iuPipesModel.GRADE_SRNO},
                    { "@THICKNESS_SRNO", iuPipesModel.THICKNESS_SRNO},
                    { "@WORK_SHIFT_SRNO", iuPipesModel.WORK_SHIFT_SRNO},
                    { "@C_LOCATION", iuPipesModel.C_LOCATION},
                    { "@IS_COIL_COMPLETED", iuPipesModel.IS_COIL_COMPLETED},
                    { "@P_LENGTH", iuPipesModel.P_LENGTH},
                    { "@PIPE_NOS", iuPipesModel.PIPE_NOS},
                    { "@PG_SCRAP_WT", iuPipesModel.PG_SCRAP_WT},
                    { "@P_WEIGHT", iuPipesModel.P_WEIGHT},
                    { "@REMARKS", iuPipesModel.REMARKS},
                    { "@TRN_DATE", iuPipesModel.TRN_DATE},
                    { "@TRN_BY", iuPipesModel.TRN_BY},
                    { "@TRN_REMARK", iuPipesModel.TRN_REMARK},
                    { "@UT_SRNO", iuPipesModel.UT_SRNO},
                    { "@USER_SRNO", iuPipesModel.USER_SRNO},
                    { "@PG_SRNO", iuPipesModel.PG_SRNO}
                };


                // Use the singleton DatabaseHelper to execute the stored procedure
                    var dataSet = _databaseHelper.ExecuteStoredProcedureAsDataSet("IU_PIPES_INV", parameters);

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


        public object DtPipes([FromQuery] ComParaModel comPara, int? PR_SRNO, int? GRADE_SRNO, int? THICKNESS_SRNO, int? OD_SRNO, int? C_LOCATION, int? PR_LENGTH)
        {
            try
            {
                // Prepare the parameters for the stored procedure
                var parameters = new Dictionary<string, object>

                 {
                    { "@PR_SRNO", PR_SRNO},
                    { "@GRADE_SRNO", GRADE_SRNO},
                    { "@THICKNESS_SRNO", THICKNESS_SRNO},
                    { "@OD_SRNO", OD_SRNO},
                    { "@C_LOCATION", C_LOCATION},
                    { "@PR_LENGTH", PR_LENGTH },
                    { "@USER_SRNO", comPara.USER_SRNO },
                };


                // Use the singleton DatabaseHelper to execute the stored procedure
                var dataSet = _databaseHelper.ExecuteStoredProcedureAsDataSet("DT_PIPES", parameters);

                if (dataSet.Tables.Count == 0)
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
        public object DtPipesLogs([FromQuery] ComParaModel comPara, int? PR_SRNO, int? GRADE_SRNO, int? THICKNESS_SRNO, int? OD_SRNO, int? C_LOCATION, int? PR_LENGTH, int? INV_TYPE, DateTime? DTP_FROM, DateTime? DTP_TO)
        {
            try
            {
                // Prepare the parameters for the stored procedure
                var parameters = new Dictionary<string, object>

                 {
                    { "@PR_SRNO", PR_SRNO},
                    { "@GRADE_SRNO", GRADE_SRNO},
                    { "@THICKNESS_SRNO", THICKNESS_SRNO},
                    { "@OD_SRNO", OD_SRNO},
                    { "@C_LOCATION", C_LOCATION},
                    { "@PR_LENGTH", PR_LENGTH },
                    { "@INV_TYPE", INV_TYPE},
                    { "@DTP_FROM", DTP_FROM},
                    { "@DTP_TO", DTP_TO},
                    { "@USER_SRNO", comPara.USER_SRNO },
                };


                // Use the singleton DatabaseHelper to execute the stored procedure
                var dataSet = _databaseHelper.ExecuteStoredProcedureAsDataSet("DT_PIPES_LOGS", parameters);

                if (dataSet.Tables.Count == 0)
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

        public object IuPipeShiftLocation([FromBody] IuPipeShiftLocation_Model iuPipeShiftLocation)
        {
            try
            {
                // Prepare the parameters for the stored procedure
                var parameters = new Dictionary<string, object>

                 {
                    { "@IU_FLAG", iuPipeShiftLocation.USER_SRNO },
                    { "@PR_INV_SRNO", iuPipeShiftLocation.PR_INV_SRNO},
                    { "@PR_SRNO", iuPipeShiftLocation.PR_SRNO },
                    { "@FROM_LOCATION", iuPipeShiftLocation.FROM_LOCATION },
                    { "@TO_LOCATION", iuPipeShiftLocation.TO_LOCATION},
                    { "@PIPE_NOS", iuPipeShiftLocation.PIPE_NOS},
                    { "@TRN_DATE", iuPipeShiftLocation.TRN_DATE},
                    { "@TRN_BY", iuPipeShiftLocation.TRN_BY},
                    { "@TRN_REMARK", iuPipeShiftLocation.TRN_REMARK},
                    { "@USER_SRNO", iuPipeShiftLocation.USER_SRNO },
                };


                // Use the singleton DatabaseHelper to execute the stored procedure
                var dataSet = _databaseHelper.ExecuteStoredProcedureAsDataSet("IU_PIPE_SHIFT_LOCATION", parameters);

                if (dataSet.Tables.Count == 0)
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

        public object IuPipeShiftAction([FromBody] IuPipeShiftLocation_Model IuPipeShiftLocation)
        {
            try
            {
                DataSet dataSet = null;

                if (IuPipeShiftLocation.ACTION_FLAG == "SELL")
                {
                    // Prepare the parameters for the stored procedure
                    var parameters = new Dictionary<string, object>

                     {
                        { "@IU_FLAG", IuPipeShiftLocation.IU_FLAG },
                        { "@PR_INV_SRNO", IuPipeShiftLocation.PR_INV_SRNO},
                        { "@PR_SRNO", IuPipeShiftLocation.PR_SRNO },
                        { "@PIPE_NOS", IuPipeShiftLocation.PIPE_NOS},
                        { "@FROM_LOCATION", IuPipeShiftLocation.FROM_LOCATION},
                        { "@CUSTOMER_NAME", IuPipeShiftLocation.CUSTOMER_NAME},
                        { "@INVOICE_NUMBER", IuPipeShiftLocation.INVOICE_NUMBER },
                        { "@TRN_DATE", IuPipeShiftLocation.TRN_DATE},
                        { "@TRN_BY", IuPipeShiftLocation.TRN_BY},
                        { "@TRN_REMARK", IuPipeShiftLocation.TRN_REMARK},
                        { "@USER_SRNO", IuPipeShiftLocation.USER_SRNO },
                    };


                    // Use the singleton DatabaseHelper to execute the stored procedure
                    dataSet = _databaseHelper.ExecuteStoredProcedureAsDataSet("SP_SELL_PIPES", parameters);
                }

                else if (IuPipeShiftLocation.ACTION_FLAG == "SHIFT")
                {
                    // Prepare the parameters for the stored procedure
                    var parameters = new Dictionary<string, object>

                     {
                        { "@IU_FLAG", IuPipeShiftLocation.IU_FLAG },
                        { "@PR_INV_SRNO", IuPipeShiftLocation.PR_INV_SRNO},
                        { "@PR_SRNO", IuPipeShiftLocation.PR_SRNO },
                        { "@FROM_LOCATION", IuPipeShiftLocation.FROM_LOCATION },
                        { "@TO_LOCATION", IuPipeShiftLocation.TO_LOCATION},
                        { "@PIPE_NOS", IuPipeShiftLocation.PIPE_NOS},
                        { "@TRN_DATE", IuPipeShiftLocation.TRN_DATE},
                        { "@TRN_BY", IuPipeShiftLocation.TRN_BY},
                        { "@TRN_REMARK", IuPipeShiftLocation.TRN_REMARK},
                        { "@USER_SRNO", IuPipeShiftLocation.USER_SRNO },
                    };


                    // Use the singleton DatabaseHelper to execute the stored procedure
                    dataSet = _databaseHelper.ExecuteStoredProcedureAsDataSet("IU_PIPE_SHIFT_LOCATION", parameters);
                }
                else
                {
                    // Prepare the parameters for the stored procedure
                    var parameters = new Dictionary<string, object>

                     {
                         { "@IU_FLAG", IuPipeShiftLocation.IU_FLAG },
                        { "@PR_INV_SRNO", IuPipeShiftLocation.PR_INV_SRNO},
                        { "@PR_SRNO", IuPipeShiftLocation.PR_SRNO },
                        { "@PIPE_NOS", IuPipeShiftLocation.PIPE_NOS},
                        { "@FROM_LOCATION", IuPipeShiftLocation.FROM_LOCATION},
                        { "@LEASRE_MACHINE_NUMBER", IuPipeShiftLocation.LEASRE_MACHINE_NUMBER},
                        { "@TRN_DATE", IuPipeShiftLocation.TRN_DATE},
                        { "@TRN_BY", IuPipeShiftLocation.TRN_BY},
                        { "@TRN_REMARK", IuPipeShiftLocation.TRN_REMARK},
                        { "@USER_SRNO", IuPipeShiftLocation.USER_SRNO },
                    };


                    // Use the singleton DatabaseHelper to execute the stored procedure
                    dataSet = _databaseHelper.ExecuteStoredProcedureAsDataSet("SP_CUT_PIPES", parameters);
                }
                if (dataSet.Tables.Count == 0)
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
        public object IuPipesInvPr([FromBody] IuPipesInvPrModel IuPipesInvPr)
        {
            try
            {

                // Prepare the parameters for the stored procedure
                var parameters = new Dictionary<string, object>

                    {
                    { "@IU_FLAG", IuPipesInvPr.USER_SRNO },
                    { "@FLAG", IuPipesInvPr.FLAG },
                    { "@PR_INV_SRNO", IuPipesInvPr.PR_INV_SRNO},
                    { "@PIPE_NOS", IuPipesInvPr.PIPE_NOS},
                    { "@UT_SRNO", IuPipesInvPr.UT_SRNO },
                    { "@USER_SRNO", IuPipesInvPr.USER_SRNO },
                };




                // Use the singleton DatabaseHelper to execute the stored procedure
                DataSet dataSet = _databaseHelper.ExecuteStoredProcedureAsDataSet("IU_PIPES_INV_PR", parameters);
                
                if (dataSet.Tables.Count == 0)
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
