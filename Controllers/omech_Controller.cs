using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using omech.Models;
//using omech.Helpers;

using omech.Services;
using System.Numerics;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace omech.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class omechController : ControllerBase
    {
        private readonly IDataService _dataService;

        public omechController(IDataService dataService)
        {
            _dataService = dataService;
        }

        // GET: api/<omech_Controller>
        [Authorize]
        [HttpGet("Pl_Common")]
        public IActionResult Pl_Common([FromQuery] ComParaModel comPara, String TBL_SRNO)
        {
            var response = _dataService.Pl_Common(comPara, TBL_SRNO);
            return Ok(response);
        }

        [Authorize]
        [HttpGet("DtRawMaterial")]
        public IActionResult DtRawMaterial([FromQuery] ComParaModel comPara, string? CHALLAN_NO, DateTime? DT_REG_FROM, DateTime? DT_REG_TO, string? SUPPLIER, int? GRADE_SRNO, int? THICKNESS_SRNO, char? IS_SHOW_ALL)
        {
            var response = _dataService.DtRawMaterial(comPara,CHALLAN_NO,DT_REG_FROM,DT_REG_TO,SUPPLIER,GRADE_SRNO,THICKNESS_SRNO, IS_SHOW_ALL);
            return Ok(response);
        }

        [Authorize]
        [HttpPost("IuRawMaterial")]
        public IActionResult IuRawMaterial([FromBody] IuRawMaterialModel RawMaterialModel)
        {
            var response = _dataService.IuRawMaterial(RawMaterialModel);
            return Ok(response);
        }

        [Authorize]
        [HttpPost("IuRawSlit")] // CHEKC ONCW IS IT USED OR NOT
        public IActionResult IuRawSlit([FromBody] IuRawSlitModel rawSlitModel)
        {
            var response = _dataService.IuRawSlit(rawSlitModel);
            return Ok(response);
        }

        [Authorize]
        [HttpPost("IuRawSlitArr")]
        public IActionResult IuRawSlitArr([FromBody] Sp_Iu_Raw_Slit_Model rawSlitModel)
        {
            var response = _dataService.IuRawSlitArr(rawSlitModel);
            return Ok(response);
        }

        [Authorize]
        [HttpGet("UpdateIsSlitted")]
        public IActionResult UpdateIsSlitted([FromQuery] ComParaModel comPara, int SRNO, char STATUS_FLAG,char COIL_FLAG)
        {
            var response = _dataService.UpdateIsSlitted(comPara, SRNO,STATUS_FLAG,COIL_FLAG);
            return Ok(response);

        }

        [Authorize]
        [HttpGet("DtSlitted")]
        public IActionResult DtSlitted([FromQuery] ComParaModel comPara)
        {
            var response = _dataService.DtSlitted(comPara);
            return Ok(response);

        }

        [Authorize]
        [HttpGet("DtRawMaterialShift")]
        public IActionResult DtRawMaterialShift([FromQuery] ComParaModel comPara, char MATERIAL_FLAG, string? CHALLAN_NO, string? REG_DATE_FROM, string? REG_DATE_TO,int? GRADE_SRNO, int? THICNESS_SRNO, int? C_LOCATION , int? TUBE_MILL_SRNO, string? SLITTED_WIDTH, int? WORKING_USER)
        {
            var response = _dataService.DtRawMaterialShift(comPara, MATERIAL_FLAG, CHALLAN_NO, REG_DATE_FROM, REG_DATE_TO,GRADE_SRNO, THICNESS_SRNO, C_LOCATION, TUBE_MILL_SRNO, SLITTED_WIDTH, WORKING_USER);
            return Ok(response);

        }

        [Authorize]
        [HttpPost("IuRawMaterialShift")]
        public IActionResult IuRawMaterialShift([FromBody] IuShiftRawMaterial iuShiftRawMaterial)
        {
            var response = _dataService.IuRawMaterialShift(iuShiftRawMaterial);
            return Ok(response);

        }

        // Get Methods for tech dsahboard data
        [Authorize]
        [HttpGet("DtDashRawInventory")]
        public IActionResult DtDashRawInventory([FromQuery]  ComParaModel comPara, char? MATERIAL_FLAG, DateTime? F_DATE, DateTime? TO_DATE,int? GRADE_SRNO,int? THICNESS_SRNO, decimal? WIDTH, int? STATUS_SRNO,int? C_LOCATION)
        {
            var response = _dataService.DtDashRawInventory(comPara, MATERIAL_FLAG, F_DATE,TO_DATE, GRADE_SRNO, THICNESS_SRNO, WIDTH, STATUS_SRNO, C_LOCATION);
            return Ok(response);
        }

        // Get Methods for tech dsahboard data DTLS
        [Authorize]
        [HttpGet("DtDashRawInventoryDtl")]
        public IActionResult DtDashRawInventoryDtl([FromQuery] ComParaModel comPara, char? MATERIAL_FLAG, string MATERIAL_SRNOS, string? SLITTING_SRNOS)
        {
            var response = _dataService.DtDashRawInventoryDtl(comPara, MATERIAL_FLAG, MATERIAL_SRNOS,SLITTING_SRNOS);
            return Ok(response);
        }

        // Get Method for get users
        [Authorize]
        [HttpGet("DtUsers")]
        public IActionResult DtUsers([FromQuery] ComParaModel comPara)
        {
            var response = _dataService.DtUsers(comPara);
            return Ok(response);
        }

        // Get Method for get user type
        [Authorize]
        [HttpGet("DtUserTypes")]
        public IActionResult DtUserTypes([FromQuery] ComParaModel comPara)
        {
            var response = _dataService.DtUserTypes(comPara);
            return Ok(response);
        }

        // POST method for Insert the users
        [Authorize]
        [HttpPost("IuUser")]
        public IActionResult IuUser([FromBody] IuUserModel userModel)
        {
            var response = _dataService.IuUser(userModel);
            return Ok(response);
        }

        //POST METHOD FOR USER TYPE
        [Authorize]
        [HttpGet("IuUserType")]
        public IActionResult IuUserType([FromQuery] ComParaModel comPara, char IU_FLAG, int? USER_TYPE_SRNO, string USER_TYPE_NAME, string? USER_TYPE_DESC)
        {
            var response = _dataService.IuUserType(comPara,IU_FLAG,USER_TYPE_SRNO,USER_TYPE_NAME,USER_TYPE_DESC);
            return Ok(response);
        }

        // Delete method for the raw material and slit process
        [Authorize]
        [HttpDelete("DelRawSlit")]
        public IActionResult DelRawSlit([FromQuery] ComParaModel comPara, int SRNO, bool IS_MOTHER_COIL)
        {
            var response = _dataService.DelRawSlit(comPara, SRNO, IS_MOTHER_COIL);
            return Ok(response);
        }

        // Update the status of stock materialsa
        [Authorize]
        [HttpGet("IuShiftStock")]
        public IActionResult IuShiftStock([FromQuery] ComParaModel comPara, char IU_FLAG, char COIL_FLAG, char STATUS_FLAG, int SRNO)
        {
            var response = _dataService.IuShiftStock(comPara,IU_FLAG,COIL_FLAG,STATUS_FLAG,SRNO);
            return Ok(response);
        }


        // Master API
        [Authorize]
        [HttpPost("IuMGrade")]
        public IActionResult IuMGrade([FromBody] IuMasterPara masterPara)
        {
            var response = _dataService.IuMGrade(masterPara);
            return Ok(response);
        }

        [Authorize]
        [HttpGet("DtMGradeDtl")]
        public IActionResult DtMGradeDtl([FromQuery] ComParaModel comPara, int GRADE_SRNO)
        {
            var response = _dataService.DtMGradeDtl(comPara, GRADE_SRNO);
            return Ok(response);
        }

        [Authorize]
        [HttpPost("IuMThickness")]
        public IActionResult IuMThickness([FromBody] IuMasterPara masterPara)
        {
            var response = _dataService.IuMThickness(masterPara);
            return Ok(response);
        }

        [Authorize]
        [HttpPost("IuMOD")]
        public IActionResult IuMOD([FromBody] IuMasterPara masterPara)
        {
            var response = _dataService.IuMOD(masterPara);
            return Ok(response);
        }

        [Authorize]
        [HttpPost("IuMLocation")]
        public IActionResult IuMLocation([FromBody] IuMasterPara masterPara)
        {
            var response = _dataService.IuMLocation(masterPara);
            return Ok(response);
        }

        [Authorize]
        [HttpPost("IuMTubeMill")]
        public IActionResult IuMTubeMill([FromBody] IuMasterPara masterPara)
        {
            var response = _dataService.IuMTubeMill(masterPara);
            return Ok(response);
        }

        [Authorize]
        [HttpPost("IuMParty")]
        public IActionResult IuMParty([FromBody] IuMasterPara masterPara)
        {
            var response = _dataService.IuMParty(masterPara);
            return Ok(response);
        }


        //Master Delete Api
        [Authorize]
        [HttpDelete("DelMGrade")]
        public IActionResult DelMGrade([FromQuery] DelMasterPara masterPara)
        {
            var response = _dataService.DelMGrade(masterPara);
            return Ok(response);
        }

        [Authorize]
        [HttpDelete("DelMThickness")]
        public IActionResult DelMThickness([FromQuery] DelMasterPara masterPara)
        {
            var response = _dataService.DelMThickness(masterPara);
            return Ok(response);
        }

        [Authorize]
        [HttpDelete("DelMOD")]
        public IActionResult DelMOD([FromQuery] DelMasterPara masterPara)
        {
            var response = _dataService.DelMOD(masterPara);
            return Ok(response);
        }

        [Authorize]
        [HttpDelete("DelMLocation")]
        public IActionResult DelMLocation([FromQuery] DelMasterPara masterPara)
        {
            var response = _dataService.DelMLocation(masterPara);
            return Ok(response);
        }

        [Authorize]
        [HttpDelete("DelMParty")]
        public IActionResult DelMParty([FromQuery] DelMasterPara masterPara)
        {
            var response = _dataService.DelMParty(masterPara);
            return Ok(response);
        }

        // 
        [Authorize]
        [HttpPost("IuStatusLog")]
        public IActionResult IuStatusLog([FromBody] IuStatusLogModel iuStatusLog)
        {
            var response = _dataService.IuStatusLog(iuStatusLog);
            return Ok(response);
        }

        [Authorize]
        [HttpPost("IuStatusLogProd")]
        public IActionResult IuStatusProdLog([FromBody] IuStatusLogProdModel IuStatusLogProd)
        {
            var response = _dataService.IuStatusProdLog(IuStatusLogProd);
            return Ok(response);
        }

        [Authorize]
        // 
        [HttpPost("IuPipes")]
        public IActionResult IuPipes([FromBody] IuPipesModel iuPipesModel)
        {
            var response = _dataService.IuPipes(iuPipesModel);
            return Ok(response);
        }

        [Authorize]
        [HttpGet("DtPipes")]
        public IActionResult DtPipes([FromQuery] ComParaModel comPara, int? PR_SRNO,int? GRADE_SRNO, int? THICKNESS_SRNO, int? OD_SRNO, int? C_LOCATION, int? PR_LENGTH)
        {
            var response = _dataService.DtPipes(comPara, PR_SRNO, GRADE_SRNO,THICKNESS_SRNO,OD_SRNO, C_LOCATION,PR_LENGTH);
            return Ok(response);

        }

        [Authorize]
        [HttpGet("DtPipesLogs")]
        public IActionResult DtPipesLogs([FromQuery] ComParaModel comPara, int? PR_SRNO, int? GRADE_SRNO, int? THICKNESS_SRNO, int? OD_SRNO, int? C_LOCATION, int? PR_LENGTH , int? INV_TYPE, DateTime? DTP_FROM, DateTime? DTP_TO)
        {
            var response = _dataService.DtPipesLogs(comPara, PR_SRNO, GRADE_SRNO, THICKNESS_SRNO, OD_SRNO, C_LOCATION, PR_LENGTH, INV_TYPE, DTP_FROM,DTP_TO);
            return Ok(response);

        }

        // Update the lOCATION OF PIPES
        [Authorize]
        [HttpPost("IuPipeShiftLocation")]
        public IActionResult IuPipeShiftLocation([FromBody] IuPipeShiftLocation_Model iuPipeShiftLocation)
        {
            var response = _dataService.IuPipeShiftLocation(iuPipeShiftLocation);
            return Ok(response);
        }

        // Update the lOCATION OF PIPES
        [Authorize]
        [HttpPost("IuPipeShiftAction")]
        public IActionResult IuPipeShiftAction([FromBody] IuPipeShiftLocation_Model iuPipeShiftLocation)
        {
            var response = _dataService.IuPipeShiftAction(iuPipeShiftLocation);
            return Ok(response);
        }


        // Update the lOCATION OF PIPES
        [Authorize]
        [HttpPost("IuPipesInvPr")]
        public IActionResult IuPipesInvPr([FromBody] IuPipesInvPrModel IuPipesInvPr)
        {
            var response = _dataService.IuPipesInvPr(IuPipesInvPr);
            return Ok(response);
        }

        [Authorize]
        [HttpGet("GetInvStatusScheduleWise")]
        public IActionResult GetInvStatusScheduleWise([FromQuery] ComParaModel comPara, int? GRADE_SRNO, int? THICKNESS_SRNO, int? OD_SRNO, int? PR_LENGTH, decimal? PR_PRICE, int PR_QUANTITY)
        {
            var response = _dataService.GetInvStatusScheduleWise(comPara, GRADE_SRNO, THICKNESS_SRNO, OD_SRNO, PR_LENGTH, PR_PRICE, PR_QUANTITY);
            return Ok(response);

        }

        // Schedule
        [Authorize]
        [HttpPost("IuSchedule")]
        public IActionResult IuSchedule([FromBody] IuScheduleMaster IuSchedule)
        {
            var response = _dataService.IuSchedule(IuSchedule);
            return Ok(response);
        }

        [Authorize]
        [HttpGet("DtSchedule")]
        public IActionResult DtSchedule([FromQuery] ComParaModel comPara, string? ITEM_TYPE, string? PARTY_NAME, int? PARTY_SRNO, int? STATUS_SRNO, DateTime? ENTRY_DATE_FROM, DateTime? ENTRY_DATE_TO, DateTime? DELIVERY_DATE_FROM, DateTime? DELIVERY_DATE_TO)
        {
            var response = _dataService.DtSchedule(comPara, ITEM_TYPE,PARTY_NAME,PARTY_SRNO, STATUS_SRNO,ENTRY_DATE_FROM,ENTRY_DATE_TO,DELIVERY_DATE_FROM,DELIVERY_DATE_TO);
            return Ok(response);

        }

        // Delete method for the schedule
        [Authorize]
        [HttpDelete("DelSchedule")]
        public IActionResult DelSchedule([FromQuery] ComParaModel comPara, int SCHEDULE_SRNO)
        {
            var response = _dataService.DelSchedule(comPara, SCHEDULE_SRNO);
            return Ok(response);
        }

        [Authorize]
        [HttpGet("DispSchedule")]
        public IActionResult DispSchedule([FromQuery] ComParaModel comPara, int SCHEDULE_SRNO)
        {
            var response = _dataService.DispSchedule(comPara, SCHEDULE_SRNO);
            return Ok(response);

        }

        [Authorize]
        [HttpGet("DispDispatchSchedule")]
        public IActionResult DispDispatchSchedule([FromQuery] ComParaModel comPara, int SCHEDULE_SRNO)
        {
            var response = _dataService.DispDispatchSchedule(comPara, SCHEDULE_SRNO);
            return Ok(response);

        }

        [Authorize]
        [HttpPost("IuDispatch")]
        public IActionResult IuDispatch([FromBody] IuDispatchModal IuDispatch)
        {
            var response = _dataService.IuDispatch(IuDispatch);
            return Ok(response);
        }

        [Authorize]
        [HttpPost("IuPoStatus")]
        public IActionResult IuPoStatus([FromBody] IuPoStatusModel iuPoStatus)
        {
            var response = _dataService.IuPoStatus(iuPoStatus);
            return Ok(response);
        }

        //Purchase order reject itemes details save
        [Authorize]
        [HttpPost("IuPoItemsReject")]
        public IActionResult IuPoItemsReject([FromBody] IuPoItemsRejectModal IuPoItemsReject)
        {
            var response = _dataService.IuPoItemsReject(IuPoItemsReject);
            return Ok(response);
        }

        [Authorize]
        [HttpGet("DsPo")]
        public IActionResult DsPo([FromQuery] ComParaModel comPara, string? ITEM_TYPE, string? TXT_SEARCH, DateTime? FROM_DATE, DateTime? TO_DATE)
        {
            var response = _dataService.DsPo(comPara, ITEM_TYPE, TXT_SEARCH,FROM_DATE,TO_DATE);
            return Ok(response);

        }

        [Authorize]
        [HttpGet("DispPoAutoMap")]
        public IActionResult DispPoAutoMap([FromQuery] ComParaModel comPara, string? SCHEDULE_SRNOS, string? SCHEDULE_DT_SRNOS, int? GRADE_SRNO, int? THICKNESS_SRNO, int? OD_SRNO, int? PARTY_SRNO, string? PO_NUMBER, DateTime? ENTRY_DATE_FROM, DateTime? ENTRY_DATE_TO, DateTime? DELIVERY_DATE_FROM, DateTime? DELIVERY_DATE_TO, int IS_GROUPBY_LENGTH)
        {
            var response = _dataService.DispPoAutoMap(comPara, SCHEDULE_SRNOS,SCHEDULE_DT_SRNOS, GRADE_SRNO, THICKNESS_SRNO, OD_SRNO, PARTY_SRNO, PO_NUMBER, ENTRY_DATE_FROM, ENTRY_DATE_TO, DELIVERY_DATE_FROM, DELIVERY_DATE_TO, IS_GROUPBY_LENGTH);
            return Ok(response);

        }

        [Authorize]
        [HttpGet("DtDispatchAnalysis")]
        public IActionResult DtDispatchAnalysis([FromQuery] ComParaModel comPara, int? PARTY_SRNO, int? GRADE_SRNO, int? THICKNESS_SRNO, int? OD_SRNO, DateTime? DISPATCH_DATE_FROM, DateTime? DISPATCH_DATE_TO, string? PO_NUMBER)
        {
            var response = _dataService.DtDispatchAnalysis(comPara, PARTY_SRNO, GRADE_SRNO, THICKNESS_SRNO, OD_SRNO, DISPATCH_DATE_FROM, DISPATCH_DATE_TO, PO_NUMBER);
            return Ok(response);
        }

        [Authorize]
        [HttpGet("DtPoAnalysis")]
        public IActionResult DtPoAnalysis([FromQuery] ComParaModel comPara, int? PARTY_SRNO, int? GRADE_SRNO, int? THICKNESS_SRNO, int? OD_SRNO, DateTime? DATE_FROM, DateTime? DATE_TO, string? PO_NUMBER)
        {
            var response = _dataService.DtPoAnalysis(comPara, PARTY_SRNO, GRADE_SRNO, THICKNESS_SRNO, OD_SRNO, DATE_FROM, DATE_TO, PO_NUMBER);
            return Ok(response);
        }

        [Authorize]
        [HttpGet("DtScheduleAnalysis")]
        public object DtScheduleAnalysis([FromQuery] ComParaModel comPara, int? GRADE_SRNO, int? THICKNESS_SRNO, int? OD_SRNO, string? PARTY_NAME, string? PO_NUMBER, DateTime? ENTRY_DATE_FROM, DateTime? ENTRY_DATE_TO, DateTime? DELIVERY_DATE_FROM, DateTime? DELIVERY_DATE_TO)
        {
            var response = _dataService.DtScheduleAnalysis(comPara, GRADE_SRNO,THICKNESS_SRNO,OD_SRNO,PARTY_NAME,PO_NUMBER,ENTRY_DATE_FROM,ENTRY_DATE_TO,DELIVERY_DATE_FROM,DELIVERY_DATE_TO);
            return Ok(response);
        }

        // THreshold Start - IU_THRESHOLD_MASTER
        [Authorize]
        [HttpPost("IuThresholdMaster")]
        public IActionResult IuThresholdMaster([FromBody] IuThresholdMasterModel IuThresholdMaster)
        {
            var response = _dataService.IuThresholdMaster(IuThresholdMaster);
            return Ok(response);
        }

        //[Authorize]
        [HttpGet("DtThresholdMaster")]
        public object DtThresholdMaster([FromQuery] ComParaModel comPara, int? GRADE_SRNO, int? THICKNESS_SRNO, int? OD_SRNO,decimal? REQUIRED_WIDTH)
        {
            var response = _dataService.DtThresholdMaster(comPara, GRADE_SRNO, THICKNESS_SRNO, OD_SRNO, REQUIRED_WIDTH);
            return Ok(response);
        }

        [Authorize]
        [HttpDelete("DelThresholdMaster")]
        public object DelThresholdMaster([FromQuery] ComParaModel comPara, int THRESHOLD_SRNO)
        {
            var response = _dataService.DelThresholdMaster(comPara, THRESHOLD_SRNO);
            return Ok(response);
        }

        [Authorize]
        [HttpGet("DtThresholdDisplay")]
        public object DtThresholdDisplay([FromQuery] ComParaModel comPara, int? GRADE_SRNO, int? THICKNESS_SRNO, int? OD_SRNO, decimal? REQUIRED_WIDTH)
        {
            var response = _dataService.DtThresholdDisplay(comPara, GRADE_SRNO, THICKNESS_SRNO, OD_SRNO, REQUIRED_WIDTH);
            return Ok(response);
        }
        // THreshold End


        // Get Methods for tech dsahboard data - Po Anayalis
        [Authorize]
        [HttpGet("DtDashPoAnalysis")]
        public IActionResult DtDashPoAnalysis([FromQuery] ComParaModel comPara, DateTime? F_DATE, DateTime? TO_DATE, int? GRADE_SRNO, int? THICNESS_SRNO, decimal? WIDTH, int? STATUS_SRNO, string? COM_SEARCH, int? C_LOCATION)
        {
            var response = _dataService.DtDashPoAnalysis(comPara, F_DATE, TO_DATE, GRADE_SRNO, THICNESS_SRNO, WIDTH, STATUS_SRNO, COM_SEARCH, C_LOCATION);
            return Ok(response);
        }

        [HttpGet("GetIps")]
        public IActionResult GetIps()
        {
            // Get client IP
            var clientIp = HttpContext.Connection.RemoteIpAddress?.ToString();

            // Get server IP
            var serverIp = HttpContext.Connection.LocalIpAddress?.ToString();

            return Ok(new
            {
                ClientIp = clientIp,
                ServerIp = serverIp
            });
        }


        [HttpGet("GetIps1")]
        public IActionResult GetIps1()
        {
            // Check for proxy header (real client IP)
            var forwardedIp = HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();

            // Fallback to connection IP
            var clientIp = forwardedIp ?? HttpContext.Connection.RemoteIpAddress?.MapToIPv4().ToString();

            // Server IP
            var serverIp = HttpContext.Connection.LocalIpAddress?.MapToIPv4().ToString();

            return Ok(new
            {
                ClientIp = clientIp,
                ServerIp = serverIp
            });
        }

        #region "Quatotaion"

        [Authorize]
        [HttpGet("DtMPartyDtl")]
        public IActionResult DtMPartyDtl([FromQuery] ComParaModel comPara, int PARTY_SRNO)
        {
            var response = _dataService.DtMPartyDtl(comPara, PARTY_SRNO);
            return Ok(response);
        }

        [Authorize]
        [HttpGet("DtCompanyMaster")]
        public IActionResult DtCompanyMaster([FromQuery] ComParaModel comPara)
        {
            var response = _dataService.DtCompanyMaster(comPara);
            return Ok(response);
        }

        [Authorize]
        [HttpPost("IuCompanyMaster")]
        public IActionResult IuCompanyMaster([FromBody] IuCompanyMasterModel companyMasterModel)
        {
            var response = _dataService.IuCompanyMaster(companyMasterModel);
            return Ok(response);
        }

        [Authorize]
        [HttpPost("IuQuotation")]
        public IActionResult IuQuotation([FromBody] IuQuotationMaster quotationMaster)
        {
            var response = _dataService.IuQuotation(quotationMaster);
            return Ok(response);
        }

        [Authorize]
        [HttpPost("CheckMaterialAvailabilityBatch")]
        public IActionResult CheckMaterialAvailabilityBatch([FromBody] CheckMaterialAvailabilityBatchRequest request)
        {
            var response = _dataService.CheckMaterialAvailabilityBatch(request);
            return Ok(response);
        }

        [Authorize]
        [HttpGet("DtQuotation")]
        public IActionResult DtQuotation([FromQuery] ComParaModel comPara, string? QUOTATION_NO, int? PARTY_SRNO, string? STATUS, DateTime? DATE_FROM, DateTime? DATE_TO)
        {
            var response = _dataService.DtQuotation(comPara, QUOTATION_NO, PARTY_SRNO, STATUS, DATE_FROM, DATE_TO);
            return Ok(response);
        }

        [Authorize]
        [HttpGet("DtQuotationDtl")]
        public IActionResult DtQuotationDtl([FromQuery] ComParaModel comPara, int QUOTATION_SRNO)
        {
            var response = _dataService.DtQuotationDtl(comPara, QUOTATION_SRNO);
            return Ok(response);
        }

        [Authorize]
        [HttpDelete("DelQuotation")]
        public IActionResult DelQuotation([FromQuery] ComParaModel comPara, int QUOTATION_SRNO)
        {
            var response = _dataService.DelQuotation(comPara, QUOTATION_SRNO);
            return Ok(response);
        }



        #endregion


    }
}
