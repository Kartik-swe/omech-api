using Microsoft.AspNetCore.Mvc;
using omech.Models;
using omech.Helpers;
using System.Data.SqlClient;
using Newtonsoft.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Data;
using System.Reflection;
using System.Net.NetworkInformation;

namespace omech.Services
{
    public interface IDataService
    {
        /// <summary>
        /// Gets the response for the dataset request.      
        /// </summary>
        /// <returns>A standardized API response object.</returns>
        object Pl_Common(ComParaModel comPara, string TBL_SRNO);
        object DtRawMaterial(ComParaModel comPara, string? CHALLAN_NO,DateTime? DT_REG_FROM, DateTime? DT_REG_TO, string? SUPPLIER, int? GRADE_SRNO, int? THICKNESS_SRNO, char? IS_SHOW_ALL);
        
        object IuRawMaterial(IuRawMaterialModel rawMaterialModel);
        object IuRawSlit(IuRawSlitModel rawSlitModel);
        object IuRawSlitArr(Sp_Iu_Raw_Slit_Model rawSlitModel);
        object UpdateIsSlitted(ComParaModel comPara, int SRNO, char STATUS_FLAG, char COIL_FLAG);
        object DtSlitted(ComParaModel comPara);
        object DtRawMaterialShift(ComParaModel comPara, char MATERIAL_FLAG, string? CHALLAN_NO, string? REG_DATE_FROM, string? REG_DATE_TO, int? GRADE_SRNO, int? THICNESS_SRNO, int? C_LOCATION, int? TUBE_MILL_SRNO, string? SLITTED_WIDTH, int? WORKING_USER);
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
        object DtMGradeDtl([FromQuery] ComParaModel comPara, int GRADE_SRNO);
        object IuMThickness(IuMasterPara masterPara);
        object IuMOD(IuMasterPara masterPara);
        object IuMLocation(IuMasterPara masterPara);
        object IuMTubeMill(IuMasterPara masterPara);
        object IuMParty(IuMasterPara masterPara);

        object DelMGrade(DelMasterPara masterPara);
        object DelMThickness(DelMasterPara masterPara);
        object DelMOD(DelMasterPara masterPara);
        object DelMLocation(DelMasterPara masterPara);
        object DelMParty(DelMasterPara masterPara);

        object IuStatusLog(IuStatusLogModel statusLogModel);
        object IuStatusProdLog(IuStatusLogProdModel IuStatusLogProd);
        object IuPipes(IuPipesModel iuPipesModel);
        object DtPipes(ComParaModel comPara, int? PR_SRNO, int? GRADE_SRNO, int? THICKNESS_SRNO, int? OD_SRNO, int? C_LOCATION, int? PR_LENGTH);
        object DtPipesLogs(ComParaModel comPara, int? PR_SRNO, int? GRADE_SRNO, int? THICKNESS_SRNO, int? OD_SRNO, int? C_LOCATION, int? PR_LENGTH, int? INV_TYPE, DateTime? DTP_FROM, DateTime? DTP_TO);
        object IuPipeShiftLocation([FromBody] IuPipeShiftLocation_Model iuPipeShiftLocation);
        object IuPipeShiftAction([FromBody] IuPipeShiftLocation_Model iuPipeShiftLocation);
        object IuPipesInvPr([FromBody] IuPipesInvPrModel IuPipesInvPr);
        object GetInvStatusScheduleWise(ComParaModel comPara, int? GRADE_SRNO, int? THICKNESS_SRNO, int? OD_SRNO, int? PR_LENGTH, decimal? PR_PRICE, int? PR_QUANTITY);
        object IuSchedule([FromBody] IuScheduleMaster IuSchedule);
        object DtSchedule([FromQuery] ComParaModel comPara, string? ITEM_TYPE, string? PARTY_NAME, int? PARTY_SRNO, int? STATUS_SRNO, DateTime? ENTRY_DATE_FROM, DateTime? ENTRY_DATE_TO, DateTime? DELIVERY_DATE_FROM, DateTime? DELIVERY_DATE_TO);
        object DispSchedule(ComParaModel comPara, int SCHEDULE_SRNO);
        object DelSchedule(ComParaModel comPara, int SCHEDULE_SRNO);
        object DispDispatchSchedule(ComParaModel comPara, int SCHEDULE_SRNO);
        object IuDispatch([FromBody] IuDispatchModal iuDispatch);
        object IuPoStatus([FromBody] IuPoStatusModel iuPoStatus);
        object IuPoItemsReject([FromBody] IuPoItemsRejectModal IuPoItemsReject);
        object DsPo([FromQuery] ComParaModel comPara, string? ITEM_TYPE, string? TXT_SEARCH, DateTime? FROM_DATE, DateTime? TO_DATE);
        object DispPoAutoMap([FromQuery] ComParaModel comPara, string? SCHEDULE_SRNOS, string? SCHEDULE_DT_SRNOS, int? GRADE_SRNO, int? THICKNESS_SRNO, int? OD_SRNO, int? PARTY_SRNO, string? PO_NUMBER, DateTime? ENTRY_DATE_FROM, DateTime? ENTRY_DATE_TO, DateTime? DELIVERY_DATE_FROM, DateTime? DELIVERY_DATE_TO, int IS_GROUPBY_LENGTH);
        object DtDispatchAnalysis([FromQuery] ComParaModel comPara, int? PARTY_SRNO, int? GRADE_SRNO, int? THICKNESS_SRNO, int? OD_SRNO, DateTime? DISPATCH_DATE_FROM, DateTime? DISPATCH_DATE_TO, string? PO_NUMBER);
        object DtPoAnalysis([FromQuery] ComParaModel comPara, int? PARTY_SRNO, int? GRADE_SRNO, int? THICKNESS_SRNO, int? OD_SRNO, DateTime? DATE_FROM, DateTime? DATE_TO, string? PO_NUMBER);
        object DtScheduleAnalysis([FromQuery] ComParaModel comPara, int? GRADE_SRNO, int? THICKNESS_SRNO, int? OD_SRNO, string? PARTY_NAME, string? PO_NUMBER, DateTime? ENTRY_DATE_FROM, DateTime? ENTRY_DATE_TO, DateTime? DELIVERY_DATE_FROM, DateTime? DELIVERY_DATE_TO);
        object IuThresholdMaster([FromBody] IuThresholdMasterModel IuThresholdMaster);
        object DtThresholdMaster([FromQuery] ComParaModel comPara, int? GRADE_SRNO, int? THICKNESS_SRNO, int? OD_SRNO, decimal? REQUIRED_WIDTH);
        object DelThresholdMaster([FromQuery] ComParaModel comPara, int THRESHOLD_SRNO);
        object DtThresholdDisplay([FromQuery] ComParaModel comPara, int? GRADE_SRNO, int? THICKNESS_SRNO, int? OD_SRNO, decimal? REQUIRED_WIDTH);
        object DtDashPoAnalysis(ComParaModel comPara, DateTime? F_DATE, DateTime? TO_DATE, int? GRADE_SRNO, int? THICNESS_SRNO, decimal? WIDTH, int? STATUS_SRNO, string? COM_SEARCH,int? C_LOCATION);

        // Quotation start
        object DtMPartyDtl([FromQuery] ComParaModel comPara, int PARTY_SRNO);
        object DtCompanyMaster([FromQuery] ComParaModel comPara);
        object IuCompanyMaster(IuCompanyMasterModel companyMasterModel);
        object IuQuotation(IuQuotationMaster quotationMaster);
        object CheckMaterialAvailabilityBatch(CheckMaterialAvailabilityBatchRequest request);
        object DtQuotation([FromQuery] ComParaModel comPara, string? QUOTATION_NO, int? PARTY_SRNO, string? STATUS, DateTime? DATE_FROM, DateTime? DATE_TO);
        object DtQuotationDtl([FromQuery] ComParaModel comPara, int QUOTATION_SRNO);
        object DelQuotation([FromQuery] ComParaModel comPara, int QUOTATION_SRNO);
        // Quotation end


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
        public object DtRawMaterial(ComParaModel comPara, string? CHALLAN_NO, DateTime? DT_REG_FROM, DateTime? DT_REG_TO, string? SUPPLIER, int? GRADE_SRNO, int? THICKNESS_SRNO, char? IS_SHOW_ALL)
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
                    {"@IS_SHOW_ALL", IS_SHOW_ALL },
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
                    { "@RATE_PER_KG", rawMaterialModel.RATE_PER_KG},
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
        public object DtRawMaterialShift([FromQuery] ComParaModel comPara, char MATERIAL_FLAG, string? CHALLAN_NO, string? REG_DATE_FROM, string? REG_DATE_TO, int? GRADE_SRNO, int? THICNESS_SRNO, int? C_LOCATION, int? TUBE_MILL_SRNO, string? SLITTED_WIDTH , int? WORKING_USER)
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
                    { "@SLITTED_WIDTH", SLITTED_WIDTH},
                    { "@WORKING_USER", WORKING_USER},
                };


                // Use the singleton DatabaseHelper to execute the stored procedure
                var dataSet = _databaseHelper.ExecuteStoredProcedureAsDataSet("DT_RAW_MATERIAL_SHIFT", parameters);

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

        public object IuUserType([FromQuery] ComParaModel comPara, char IU_FLAG, int? USER_TYPE_SRNO, string USER_TYPE_NAME, string? USER_TYPE_DESC)
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
            try
            {
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
        public object IuShiftStock([FromQuery] ComParaModel comPara, char IU_FLAG, char COIL_FLAG, char STATUS_FLAG, int SRNO)
        {
            try
            {
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
                    { "@DENSITY", masterPara.DENSITY},
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

        // One grade's full record (including Density) - used by the Grade edit form,
        // since Pl_Common's Table1 grade dropdown only ever returns {value, label, DENSITY}
        // as a flat list and editing needs UOM too.
        public object DtMGradeDtl([FromQuery] ComParaModel comPara, int GRADE_SRNO)
        {
            try
            {
                var parameters = new Dictionary<string, object>
                {
                    { "@GRADE_SRNO", GRADE_SRNO },
                };

                var dataSet = _databaseHelper.ExecuteStoredProcedureAsDataSet("DT_M_GRADE_DTL", parameters);

                if (dataSet.Tables.Count == 0 || dataSet.Tables[0].Rows.Count == 0)
                {
                    return CommonHelper.CreateApiResponse(204, "No data found.", null);
                }

                var result = CommonHelper.SerializeDataSet(dataSet);
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

        public object IuMParty([FromBody] IuMasterPara masterPara)
        {
            try
            {
                // Prepare the parameters for the stored procedure
                var parameters = new Dictionary<string, object>

                 {
                    { "@IU_FLAG", masterPara.IU_FLAG},
                    { "@PARTY_NAME", masterPara.M_NAME},
                    { "@USER_SRNO", masterPara.USER_SRNO},
                    { "@UT_SRNO", masterPara.UT_SRNO},
                    { "@PARTY_SRNO", masterPara.PK_SRNO}
                };


                // Use the singleton DatabaseHelper to execute the stored procedure
                var dataSet = _databaseHelper.ExecuteStoredProcedureAsDataSet("IU_M_PARTY", parameters);

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
            try
            {
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
        public object DelMParty([FromQuery] DelMasterPara masterPara)
        {
            try
            {
                // Prepare the parameters for the stored procedure
                var parameters = new Dictionary<string, object>

                 {
                    { "@PARTY_SRNO", masterPara.PK_SRNO},
                    { "@USER_SRNO", masterPara.USER_SRNO},
                    { "@UT_SRNO", masterPara.UT_SRNO},
                };


                // Use the singleton DatabaseHelper to execute the stored procedure
                var dataSet = _databaseHelper.ExecuteStoredProcedureAsDataSet("DEL_M_PARTY", parameters);

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
                    { "@OD_SRNO", statusLogModel.OD_SRNO},
                    { "@WORKING_USER_SRNO", statusLogModel.WORKING_USER_SRNO},
                    { "@WORKING_SHIFT", statusLogModel.WORKING_SHIFT},
                    { "@RESPONSIBLE_SUPERVISOR_SRNO", statusLogModel.RESPONSIBLE_SUPERVISOR_SRNO},
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

        public object IuStatusProdLog([FromQuery] IuStatusLogProdModel IuStatusLogProd)
        {

            try
            {
                // Prepare the parameters for the stored procedure
                var parameters = new Dictionary<string, object>

                 {
                    { "@MATERIAL_SRNO", IuStatusLogProd.MATERIAL_SRNO},
                    { "@SLITTING_SRNO", IuStatusLogProd.SLITTING_SRNO},
                    { "@PRE_LOG_STATUS_SRNO", IuStatusLogProd.PRE_LOG_STATUS_SRNO},
                    { "@DESCRIPTION", IuStatusLogProd.DESCRIPTION},
                    { "@REMARKS", IuStatusLogProd.REMARKS},
                    { "@STATUS_CHANGE_DATE", IuStatusLogProd.STATUS_CHANGE_DATE},
                    { "@USER_SRNO", IuStatusLogProd.USER_SRNO},
                    { "@OD_SRNO", IuStatusLogProd.OD_SRNO},
                    { "@WORKING_USER_SRNO", IuStatusLogProd.WORKING_USER_SRNO},
                    { "@WORKING_SHIFT", IuStatusLogProd.WORKING_SHIFT},
                    { "@UT_SRNO", IuStatusLogProd.UT_SRNO},
                    { "@LOG_STATUS_SRNO", IuStatusLogProd.LOG_STATUS_SRNO},
                };


                // Use the singleton DatabaseHelper to execute the stored procedure
                var dataSet = _databaseHelper.ExecuteStoredProcedureAsDataSet("IU_STATUS_LOG_PROD", parameters);

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

        public object GetInvStatusScheduleWise([FromQuery] ComParaModel comPara, int? GRADE_SRNO, int? THICKNESS_SRNO, int? OD_SRNO, int? PR_LENGTH, decimal? PR_PRICE, int? PR_QUANTITY)
        {
            try
            {
                // Prepare the parameters for the stored procedure
                var parameters = new Dictionary<string, object>

                 {
                    { "@GRADE_SRNO", GRADE_SRNO},
                    { "@THICKNESS_SRNO", THICKNESS_SRNO},
                    { "@OD_SRNO", OD_SRNO},
                    { "@PR_LENGTH", PR_LENGTH },
                    { "@PR_PRICE", PR_PRICE },
                    { "@PR_QUANTITY", PR_QUANTITY },
                    { "@USER_SRNO", comPara.USER_SRNO },
                };


                // Use the singleton DatabaseHelper to execute the stored procedure
                var dataSet = _databaseHelper.ExecuteStoredProcedureAsDataSet("GET_INV_STATUS_SCHEDULE_WISE", parameters);

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


        // Schedule
        public object IuSchedule([FromBody] IuScheduleMaster IuSchedule)
        {
            try
            {
                string jsonDetail = JsonConvert.SerializeObject(IuSchedule.DETAIL_JSON);

                // Prepare the parameters for the stored procedure
                var parameters = new Dictionary<string, object>
                {
                    { "@IU_FLAG", IuSchedule.IU_FLAG },
                    { "@PARTY_NAME", IuSchedule.PARTY_NAME },
                    { "@PO_NUMBER", IuSchedule.PO_NUMBER },
                    { "@SCHEDULE_DATE", IuSchedule.SCHEDULE_DATE },
                    { "@ESTIMATED_DELIVERY_DATE", IuSchedule.ESTIMATED_DELIVERY_DATE },
                    { "@STATUS_SRNO", IuSchedule.STATUS_SRNO },
                    { "@REMARKS", IuSchedule.REMARKS },
                    { "@ITEM_TYPE", IuSchedule.ITEM_TYPE }, // 
                    { "@PARTY_SRNO", IuSchedule.PARTY_SRNO}, // 
                    { "@DETAIL_JSON", jsonDetail },
                    { "@UT_SRNO", IuSchedule.UT_SRNO },
                    { "@USER_SRNO", IuSchedule.USER_SRNO },
                    { "@SCHEDULE_SRNO", IuSchedule.SCHEDULE_SRNO },
                };




                // Use the singleton DatabaseHelper to execute the stored procedure
                DataSet dataSet = _databaseHelper.ExecuteStoredProcedureAsDataSet("IU_SCHEDULE", parameters);

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

        public object DtSchedule([FromQuery] ComParaModel comPara, string? ITEM_TYPE, string? PARTY_NAME,int? PARTY_SRNO, int? STATUS_SRNO, DateTime? ENTRY_DATE_FROM, DateTime? ENTRY_DATE_TO, DateTime? DELIVERY_DATE_FROM, DateTime? DELIVERY_DATE_TO)
        {
            try
            {
                // Prepare the parameters for the stored procedure
                var parameters = new Dictionary<string, object>

                 {
                    { "@ITEM_TYPE", ITEM_TYPE },
                    { "@PARTY_NAME", PARTY_NAME},
                    { "@PARTY_SRNO", PARTY_SRNO},
                    { "@STATUS_SRNO", STATUS_SRNO},
                    { "@ENTRY_DATE_FROM", ENTRY_DATE_FROM },
                    { "@ENTRY_DATE_TO", ENTRY_DATE_TO },
                    { "@DELIVERY_DATE_FROM", DELIVERY_DATE_FROM },
                    { "@DELIVERY_DATE_TO", DELIVERY_DATE_TO },
                    { "@USER_SRNO", comPara.USER_SRNO },

                };


                // Use the singleton DatabaseHelper to execute the stored procedure
                var dataSet = _databaseHelper.ExecuteStoredProcedureAsDataSet("DT_SCHEDULE", parameters);

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

        public object DispSchedule([FromQuery] ComParaModel comPara, int SCHEDULE_SRNO)
        {
            try
            {
                // Prepare the parameters for the stored procedure
                var parameters = new Dictionary<string, object>

                 {
                    { "@SCHEDULE_SRNO", SCHEDULE_SRNO},
                    { "@USER_SRNO", comPara.USER_SRNO },
                };


                // Use the singleton DatabaseHelper to execute the stored procedure
                var dataSet = _databaseHelper.ExecuteStoredProcedureAsDataSet("DISP_SCHEDULE", parameters);

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

        public object DelSchedule([FromQuery] ComParaModel comPara, int SCHEDULE_SRNO)
        {
            try
            {
                // Prepare the parameters for the stored procedure
                var parameters = new Dictionary<string, object>

                 {
                    { "@SCHEDULE_SRNO", SCHEDULE_SRNO},
                    { "@USER_SRNO", comPara.USER_SRNO },
                };


                // Use the singleton DatabaseHelper to execute the stored procedure
                var dataSet = _databaseHelper.ExecuteStoredProcedureAsDataSet("DEL_SCHEDULE", parameters);

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

        public object DispDispatchSchedule([FromQuery] ComParaModel comPara, int SCHEDULE_SRNO)
        {
            try
            {
                // Prepare the parameters for the stored procedure
                var parameters = new Dictionary<string, object>

                 {
                    { "@SCHEDULE_SRNO", SCHEDULE_SRNO},
                    { "@USER_SRNO", comPara.USER_SRNO },
                };


                // Use the singleton DatabaseHelper to execute the stored procedure
                var dataSet = _databaseHelper.ExecuteStoredProcedureAsDataSet("DISP_DISPATCH_SCHEDULE", parameters);

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

        public object IuDispatch([FromBody] IuDispatchModal iuDispatch)
        {
            try
            {


                // Prepare the parameters for the stored procedure
                var parameters = new Dictionary<string, object>

                    {
                    { "@IU_FLAG", iuDispatch.IU_FLAG},
                    { "@SCHEDULE_DT_SRNO", iuDispatch.SCHEDULE_DT_SRNO},
                    { "@DISPATCH_QTY", iuDispatch.DISPATCH_QTY},
                    { "@DISPATCH_DATE", iuDispatch.DISPATCH_DATE},
                    { "@DC_NO", iuDispatch.DC_NO},
                    { "@UT_SRNO", iuDispatch.UT_SRNO},
                    { "@USER_SRNO", iuDispatch.USER_SRNO},
                    { "@SCHEDULE_SRNO", iuDispatch.SCHEDULE_SRNO},
                    { "@DISPATCH_SRNO", iuDispatch.DISPATCH_SRNO},
                };




                // Use the singleton DatabaseHelper to execute the stored procedure
                DataSet dataSet = _databaseHelper.ExecuteStoredProcedureAsDataSet("IU_DISPATCH", parameters);

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
        public object IuPoStatus([FromBody] IuPoStatusModel iuPoStatus)
        {
            try
            {


                // Prepare the parameters for the stored procedure
                var parameters = new Dictionary<string, object>

                    {
                    { "@IU_FLAG", iuPoStatus.IU_FLAG},
                    { "@STATUS_FLAG", iuPoStatus.STATUS_FLAG},
                    { "@UT_SRNO", iuPoStatus.UT_SRNO},
                    { "@USER_SRNO", iuPoStatus.USER_SRNO},
                    { "@SCHEDULE_SRNO", iuPoStatus.SCHEDULE_SRNO},
                    { "@SCHEDULE_DT_SRNO", iuPoStatus.SCHEDULE_DT_SRNO},
                };




                // Use the singleton DatabaseHelper to execute the stored procedure
                DataSet dataSet = _databaseHelper.ExecuteStoredProcedureAsDataSet("IU_PO_STATUS", parameters);

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

        public object IuPoItemsReject([FromBody] IuPoItemsRejectModal IuPoItemsReject)
        {
            try
            {


                // Prepare the parameters for the stored procedure
                var parameters = new Dictionary<string, object>

                    {
                    { "@IU_FLAG", IuPoItemsReject.IU_FLAG},
                    { "@SCHEDULE_SRNO", IuPoItemsReject.SCHEDULE_SRNO},
                    { "@SCHEDULE_DT_SRNO", IuPoItemsReject.SCHEDULE_DT_SRNO},
                    { "@REJECTED_QTY", IuPoItemsReject.REJECTED_QTY},
                    { "@REjECTED_REMARK", IuPoItemsReject.REjECTED_REMARK},
                    { "@UT_SRNO", IuPoItemsReject.UT_SRNO},
                    { "@USER_SRNO", IuPoItemsReject.USER_SRNO},
                    { "@REJECT_SRNO", IuPoItemsReject.REJECT_SRNO},
                };




                // Use the singleton DatabaseHelper to execute the stored procedure
                DataSet dataSet = _databaseHelper.ExecuteStoredProcedureAsDataSet("IU_PO_ITEMS_REJECT", parameters);

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


        public object DsPo([FromQuery] ComParaModel comPara, string? ITEM_TYPE, string? TXT_SEARCH, DateTime? FROM_DATE, DateTime? TO_DATE)
        {
            try
            {
                // Prepare the parameters for the stored procedure
                var parameters = new Dictionary<string, object>

                 {
                    { "@ITEM_TYPE", ITEM_TYPE },
                    { "@TXT_SEARCH", TXT_SEARCH},
                    { "@FROM_DATE", FROM_DATE},
                    { "@TO_DATE", TO_DATE },
                    { "@USER_SRNO", comPara.USER_SRNO },

                };


                // Use the singleton DatabaseHelper to execute the stored procedure
                var dataSet = _databaseHelper.ExecuteStoredProcedureAsDataSet("DS_PO", parameters);

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
        public object DispPoAutoMap([FromQuery] ComParaModel comPara, string? SCHEDULE_SRNOS, string? SCHEDULE_DT_SRNOS, int? GRADE_SRNO, int? THICKNESS_SRNO, int? OD_SRNO, int? PARTY_SRNO, string? PO_NUMBER, DateTime? ENTRY_DATE_FROM, DateTime? ENTRY_DATE_TO, DateTime? DELIVERY_DATE_FROM, DateTime? DELIVERY_DATE_TO, int IS_GROUPBY_LENGTH)
        {
            try
            {
                // Prepare the parameters for the stored procedure
                var parameters = new Dictionary<string, object>

                 {
                    { "@SCHEDULE_SRNOS", SCHEDULE_SRNOS},
                    { "@SCHEDULE_DT_SRNOS", SCHEDULE_DT_SRNOS},
                     { "@GRADE_SRNO", GRADE_SRNO},
                    { "@THICKNESS_SRNO", THICKNESS_SRNO },
                    { "@OD_SRNO", OD_SRNO },
                    { "@PARTY_SRNO", PARTY_SRNO},
                    { "@PO_NUMBER", PO_NUMBER},
                    { "@ENTRY_DATE_FROM", ENTRY_DATE_FROM },
                    { "@ENTRY_DATE_TO", ENTRY_DATE_TO },
                    { "@DELIVERY_DATE_FROM", DELIVERY_DATE_FROM },
                    { "@DELIVERY_DATE_TO", DELIVERY_DATE_TO },
                    { "@IS_GROUPBY_LENGTH", IS_GROUPBY_LENGTH},
                    { "@USER_SRNO", comPara.USER_SRNO },

                };


                // Use the singleton DatabaseHelper to execute the stored procedure
                var dataSet = _databaseHelper.ExecuteStoredProcedureAsDataSet("DISP_PO_AUTO_MAP", parameters);

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
        public object DtDispatchAnalysis([FromQuery] ComParaModel comPara, int? PARTY_SRNO, int? GRADE_SRNO, int? THICKNESS_SRNO, int? OD_SRNO, DateTime? DISPATCH_DATE_FROM, DateTime? DISPATCH_DATE_TO, string? PO_NUMBER)
        {
            try
            {
                // Prepare the parameters for the stored procedure
                var parameters = new Dictionary<string, object>
                {
                    { "@PARTY_SRNO", PARTY_SRNO },
                    { "@GRADE_SRNO", GRADE_SRNO },
                    { "@THICKNESS_SRNO", THICKNESS_SRNO },
                    { "@OD_SRNO", OD_SRNO },
                    { "@DISPATCH_DATE_FROM", DISPATCH_DATE_FROM },
                    { "@DISPATCH_DATE_TO", DISPATCH_DATE_TO },
                    { "@PO_NUMBER", PO_NUMBER },
                    { "@USER_SRNO", comPara.USER_SRNO },
                };

                // Use the singleton DatabaseHelper to execute the stored procedure
                var dataSet = _databaseHelper.ExecuteStoredProcedureAsDataSet("DT_DISPATCH_ANALYSIS", parameters);

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

        // Consolidated PO Analysis: every result set the /PO/analysis page
        // needs, computed server-side in one round trip (see
        // sql/03_create_dt_po_analysis.sql for the full breakdown of what
        // each returned table contains and how it's scoped).
        public object DtPoAnalysis([FromQuery] ComParaModel comPara, int? PARTY_SRNO, int? GRADE_SRNO, int? THICKNESS_SRNO, int? OD_SRNO, DateTime? DATE_FROM, DateTime? DATE_TO, string? PO_NUMBER)
        {
            try
            {
                var parameters = new Dictionary<string, object>
                {
                    { "@PARTY_SRNO", PARTY_SRNO },
                    { "@GRADE_SRNO", GRADE_SRNO },
                    { "@THICKNESS_SRNO", THICKNESS_SRNO },
                    { "@OD_SRNO", OD_SRNO },
                    { "@DATE_FROM", DATE_FROM },
                    { "@DATE_TO", DATE_TO },
                    { "@PO_NUMBER", PO_NUMBER },
                    { "@USER_SRNO", comPara.USER_SRNO },
                };

                var dataSet = _databaseHelper.ExecuteStoredProcedureAsDataSet("DT_PO_ANALYSIS", parameters);

                if (dataSet.Tables.Count == 0)
                {
                    return CommonHelper.CreateApiResponse(204, "No data found.", null);
                }

                var result = CommonHelper.SerializeDataSet(dataSet);

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
        public object DtScheduleAnalysis([FromQuery] ComParaModel comPara, int? GRADE_SRNO,int? THICKNESS_SRNO, int? OD_SRNO, string? PARTY_NAME,string? PO_NUMBER, DateTime? ENTRY_DATE_FROM, DateTime? ENTRY_DATE_TO, DateTime? DELIVERY_DATE_FROM, DateTime? DELIVERY_DATE_TO)
        {
            try
            {
                // Prepare the parameters for the stored procedure
                var parameters = new Dictionary<string, object>

                 {
                    { "@GRADE_SRNO", GRADE_SRNO},
                    { "@THICKNESS_SRNO", THICKNESS_SRNO },
                    { "@OD_SRNO", OD_SRNO },
                    { "@PARTY_NAME", PARTY_NAME},
                    { "@PO_NUMBER", PO_NUMBER},
                    { "@ENTRY_DATE_FROM", ENTRY_DATE_FROM },
                    { "@ENTRY_DATE_TO", ENTRY_DATE_TO },
                    { "@DELIVERY_DATE_FROM", DELIVERY_DATE_FROM },
                    { "@DELIVERY_DATE_TO", DELIVERY_DATE_TO },
                    { "@USER_SRNO", comPara.USER_SRNO },

                };


                // Use the singleton DatabaseHelper to execute the stored procedure
                var dataSet = _databaseHelper.ExecuteStoredProcedureAsDataSet("DT_SCHEDULE_ANALYSIS", parameters);

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

        // THreshod Start
        public object IuThresholdMaster([FromBody] IuThresholdMasterModel IuThresholdMaster)
        {
            try
            {


                // Prepare the parameters for the stored procedure
                var parameters = new Dictionary<string, object>

                    {
                    { "@GRADE_SRNO", IuThresholdMaster.GRADE_SRNO},
                    { "@THICKNESS_SRNO", IuThresholdMaster.THICKNESS_SRNO},
                    { "@OD_SRNO", IuThresholdMaster.OD_SRNO},
                    { "@MIN_THRESHOLD", IuThresholdMaster.MIN_THRESHOLD},
                    { "@USER_SRNO", IuThresholdMaster.USER_SRNO},
                    { "@THRESHOLD_SRNO", IuThresholdMaster.THRESHOLD_SRNO}  
                };




                // Use the singleton DatabaseHelper to execute the stored procedure
                DataSet dataSet = _databaseHelper.ExecuteStoredProcedureAsDataSet("IU_THRESHOLD_MASTER", parameters);

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

        public object DtThresholdMaster([FromBody] ComParaModel comPara, int? GRADE_SRNO , int? THICKNESS_SRNO, int? OD_SRNO, decimal? REQUIRED_WIDTH )
        {
            try
            {


                // Prepare the parameters for the stored procedure
                var parameters = new Dictionary<string, object>

                    {
                    { "@GRADE_SRNO", GRADE_SRNO},
                    { "@THICKNESS_SRNO", THICKNESS_SRNO},
                    { "@OD_SRNO", OD_SRNO},
                    { "@USER_SRNO", comPara.USER_SRNO},
                };




                // Use the singleton DatabaseHelper to execute the stored procedure
                DataSet dataSet = _databaseHelper.ExecuteStoredProcedureAsDataSet("DT_THRESHOLD_MASTER", parameters);

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

        public object DelThresholdMaster([FromBody] ComParaModel comPara, int THRESHOLD_SRNO)
        {
            try
            {


                // Prepare the parameters for the stored procedure
                var parameters = new Dictionary<string, object>

                    {
                    { "@THRESHOLD_SRNO", THRESHOLD_SRNO},
                    { "@USER_SRNO", comPara.USER_SRNO},
                };




                // Use the singleton DatabaseHelper to execute the stored procedure
                DataSet dataSet = _databaseHelper.ExecuteStoredProcedureAsDataSet("DEL_THRESHOLD_MASTER", parameters);

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

        public object DtThresholdDisplay([FromBody] ComParaModel comPara, int? GRADE_SRNO, int? THICKNESS_SRNO, int? OD_SRNO, decimal? REQUIRED_WIDTH)
        {
            try
            {


                // Prepare the parameters for the stored procedure
                var parameters = new Dictionary<string, object>

                    {
                    { "@GRADE_SRNO", GRADE_SRNO},
                    { "@THICKNESS_SRNO", THICKNESS_SRNO},
                    { "@OD_SRNO", OD_SRNO},
                    { "@USER_SRNO", comPara.USER_SRNO},
                };




                // Use the singleton DatabaseHelper to execute the stored procedure
                DataSet dataSet = _databaseHelper.ExecuteStoredProcedureAsDataSet("DT_THRESHOLD_DISPLAY", parameters);

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
        // THreshod End

        public object DtDashPoAnalysis(ComParaModel comPara, DateTime? F_DATE, DateTime? TO_DATE, int? GRADE_SRNO, int? THICNESS_SRNO, decimal? WIDTH, int? STATUS_SRNO, string? COM_SEARCH,int? C_LOCATION)
        {
            try
            {
                // Prepare the parameters for the stored procedure
                var parameters = new Dictionary<string, object>

                 {
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

        #region "Quatation"
        public object DtMPartyDtl([FromQuery] ComParaModel comPara, int PARTY_SRNO)
        {
            try
            {
                var parameters = new Dictionary<string, object>
                {
                    { "@PARTY_SRNO", PARTY_SRNO },
                };

                var dataSet = _databaseHelper.ExecuteStoredProcedureAsDataSet("DT_M_PARTY_DTL", parameters);

                if (dataSet.Tables.Count == 0 || dataSet.Tables[0].Rows.Count == 0)
                {
                    return CommonHelper.CreateApiResponse(204, "No data found.", null);
                }

                var result = CommonHelper.SerializeDataSet(dataSet);
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

        // Omech's own company profile - always a single row (see IU_COMPANY_MASTER)
        public object DtCompanyMaster([FromQuery] ComParaModel comPara)
        {
            try
            {
                var dataSet = _databaseHelper.ExecuteStoredProcedureAsDataSet("DT_COMPANY_MASTER", new Dictionary<string, object>());

                if (dataSet.Tables.Count == 0)
                {
                    return CommonHelper.CreateApiResponse(204, "No data found.", null);
                }

                var result = CommonHelper.SerializeDataSet(dataSet);
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

        public object IuCompanyMaster(IuCompanyMasterModel companyMasterModel)
        {
            try
            {
                var parameters = new Dictionary<string, object>
                {
                    { "@COMPANY_NAME", companyMasterModel.COMPANY_NAME },
                    { "@ADDRESS_LINE1", companyMasterModel.ADDRESS_LINE1 },
                    { "@ADDRESS_LINE2", companyMasterModel.ADDRESS_LINE2 },
                    { "@CITY", companyMasterModel.CITY },
                    { "@STATE", companyMasterModel.STATE },
                    { "@PINCODE", companyMasterModel.PINCODE },
                    { "@PHONE", companyMasterModel.PHONE },
                    { "@MOBILE", companyMasterModel.MOBILE },
                    { "@EMAIL", companyMasterModel.EMAIL },
                    { "@GST_NO", companyMasterModel.GST_NO },
                    { "@LOGO_PATH", companyMasterModel.LOGO_PATH },
                    { "@BANK_NAME", companyMasterModel.BANK_NAME },
                    { "@BANK_ACCOUNT_NO", companyMasterModel.BANK_ACCOUNT_NO },
                    { "@BANK_IFSC", companyMasterModel.BANK_IFSC },
                    { "@BANK_BRANCH", companyMasterModel.BANK_BRANCH },
                    { "@USER_SRNO", companyMasterModel.USER_SRNO },
                };

                var dataSet = _databaseHelper.ExecuteStoredProcedureAsDataSet("IU_COMPANY_MASTER", parameters);

                if (dataSet.Tables.Count == 0)
                {
                    return CommonHelper.CreateApiResponse(204, "No data found.", null);
                }

                var result = CommonHelper.SerializeDataSet(dataSet);
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

        // ---------------------------------------------------------------
        // Quotation
        // ---------------------------------------------------------------

        public object IuQuotation(IuQuotationMaster quotationMaster)
        {
            try
            {
                string jsonDetail = JsonConvert.SerializeObject(quotationMaster.DETAIL_JSON);

                var parameters = new Dictionary<string, object>
                {
                    { "@IU_FLAG", quotationMaster.IU_FLAG },
                    { "@QUOTATION_NO", quotationMaster.QUOTATION_NO },
                    { "@QUOTATION_DATE", quotationMaster.QUOTATION_DATE },
                    { "@PARTY_SRNO", quotationMaster.PARTY_SRNO },
                    { "@PARTY_NAME", quotationMaster.PARTY_NAME },
                    { "@PARTY_ADDRESS", quotationMaster.PARTY_ADDRESS },
                    { "@PARTY_GST_NO", quotationMaster.PARTY_GST_NO },
                    { "@ENQUIRY_NO", quotationMaster.ENQUIRY_NO },
                    { "@ENQUIRY_DATE", quotationMaster.ENQUIRY_DATE },
                    { "@RATE_BASIS_MODE", quotationMaster.RATE_BASIS_MODE },
                    { "@GST_PERCENT", quotationMaster.GST_PERCENT },
                    { "@PAYMENT_TERMS", quotationMaster.PAYMENT_TERMS },
                    { "@RATE_TERMS", quotationMaster.RATE_TERMS },
                    { "@DELIVERY_TERMS", quotationMaster.DELIVERY_TERMS },
                    { "@VALIDITY", quotationMaster.VALIDITY },
                    { "@STATUS", quotationMaster.STATUS },
                    { "@DETAIL_JSON", jsonDetail },
                    { "@UT_SRNO", quotationMaster.UT_SRNO },
                    { "@USER_SRNO", quotationMaster.USER_SRNO },
                    { "@QUOTATION_SRNO", quotationMaster.QUOTATION_SRNO },
                };

                var dataSet = _databaseHelper.ExecuteStoredProcedureAsDataSet("IU_QUOTATION", parameters);

                if (dataSet.Tables.Count == 0)
                {
                    return CommonHelper.CreateApiResponse(204, "No data found.", null);
                }

                var result = CommonHelper.SerializeDataSet(dataSet);
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

        // Batch material-availability check (auto-highlight for PO Material Mapping) -
        // reuses the exact matching rules GET_INV_STATUS_SCHEDULE_WISE already uses,
        // as a lightweight EXISTS-per-combo check rather than full row detail.
        public object CheckMaterialAvailabilityBatch(CheckMaterialAvailabilityBatchRequest request)
        {
            try
            {
                string combosJson = JsonConvert.SerializeObject(request.COMBOS);

                var parameters = new Dictionary<string, object>
                {
                    { "@COMBOS_JSON", combosJson },
                };

                var dataSet = _databaseHelper.ExecuteStoredProcedureAsDataSet("CHECK_MATERIAL_AVAILABILITY_BATCH", parameters);

                if (dataSet.Tables.Count == 0)
                {
                    return CommonHelper.CreateApiResponse(204, "No data found.", null);
                }

                var result = CommonHelper.SerializeDataSet(dataSet);
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

        public object DtQuotation([FromQuery] ComParaModel comPara, string? QUOTATION_NO, int? PARTY_SRNO, string? STATUS, DateTime? DATE_FROM, DateTime? DATE_TO)
        {
            try
            {
                var parameters = new Dictionary<string, object>
                {
                    { "@QUOTATION_NO", QUOTATION_NO },
                    { "@PARTY_SRNO", PARTY_SRNO },
                    { "@STATUS", STATUS },
                    { "@DATE_FROM", DATE_FROM },
                    { "@DATE_TO", DATE_TO },
                    { "@USER_SRNO", comPara.USER_SRNO },
                };

                var dataSet = _databaseHelper.ExecuteStoredProcedureAsDataSet("DT_QUOTATION", parameters);

                if (dataSet.Tables.Count == 0)
                {
                    return CommonHelper.CreateApiResponse(204, "No data found.", null);
                }

                var result = CommonHelper.SerializeDataSet(dataSet);
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

        public object DtQuotationDtl([FromQuery] ComParaModel comPara, int QUOTATION_SRNO)
        {
            try
            {
                var parameters = new Dictionary<string, object>
                {
                    { "@QUOTATION_SRNO", QUOTATION_SRNO },
                };

                var dataSet = _databaseHelper.ExecuteStoredProcedureAsDataSet("DT_QUOTATION_DTL", parameters);

                if (dataSet.Tables.Count == 0)
                {
                    return CommonHelper.CreateApiResponse(204, "No data found.", null);
                }

                var result = CommonHelper.SerializeDataSet(dataSet);
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

        public object DelQuotation([FromQuery] ComParaModel comPara, int QUOTATION_SRNO)
        {
            try
            {
                var parameters = new Dictionary<string, object>
                {
                    { "@QUOTATION_SRNO", QUOTATION_SRNO },
                    { "@USER_SRNO", comPara.USER_SRNO },
                };

                var dataSet = _databaseHelper.ExecuteStoredProcedureAsDataSet("DEL_QUOTATION", parameters);
                return CommonHelper.CreateApiResponse(200, "Success", null);
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

        #endregion

    }
}
