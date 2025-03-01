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

        [HttpGet("DtRawMaterial")]
        public IActionResult DtRawMaterial([FromQuery] ComParaModel comPara, string? CHALLAN_NO, DateTime? DT_REG_FROM, DateTime? DT_REG_TO, string? SUPPLIER, int? GRADE_SRNO, int? THICKNESS_SRNO)
        {
            var response = _dataService.DtRawMaterial(comPara,CHALLAN_NO,DT_REG_FROM,DT_REG_TO,SUPPLIER,GRADE_SRNO,THICKNESS_SRNO);
            return Ok(response);
        }

        [HttpPost("IuRawMaterial")]
        public IActionResult IuRawMaterial([FromBody] IuRawMaterialModel RawMaterialModel)
        {
            var response = _dataService.IuRawMaterial(RawMaterialModel);
            return Ok(response);
        }

        [HttpPost("IuRawSlit")] // CHEKC ONCW IS IT USED OR NOT
        public IActionResult IuRawSlit([FromBody] IuRawSlitModel rawSlitModel)
        {
            var response = _dataService.IuRawSlit(rawSlitModel);
            return Ok(response);
        }

        [HttpPost("IuRawSlitArr")]
        public IActionResult IuRawSlitArr([FromBody] Sp_Iu_Raw_Slit_Model rawSlitModel)
        {
            var response = _dataService.IuRawSlitArr(rawSlitModel);
            return Ok(response);
        }
        [HttpGet("UpdateIsSlitted")]
        public IActionResult UpdateIsSlitted([FromQuery] ComParaModel comPara, int SLITTING_SRNO)
        {
            var response = _dataService.UpdateIsSlitted(comPara, SLITTING_SRNO);
            return Ok(response);

        } 
        [HttpGet("DtSlitted")]
        public IActionResult DtSlitted([FromQuery] ComParaModel comPara)
        {
            var response = _dataService.DtSlitted(comPara);
            return Ok(response);

        }
        [HttpGet("DtRawMaterialShift")]
        public IActionResult DtRawMaterialShift([FromQuery] ComParaModel comPara, char MATERIAL_FLAG)
        {
            var response = _dataService.DtRawMaterialShift(comPara, MATERIAL_FLAG);
            return Ok(response);

        } 
        [HttpPost("IuRawMaterialShift")]
        public IActionResult IuRawMaterialShift([FromBody] IuShiftRawMaterial iuShiftRawMaterial)
        {
            var response = _dataService.IuRawMaterialShift(iuShiftRawMaterial);
            return Ok(response);

        }

        // Get Methods for tech dsahboard data
        [HttpGet("DtDashRawInventory")]
        public IActionResult DtDashRawInventory([FromQuery]  ComParaModel comPara, char? MATERIAL_FLAG, DateTime? F_DATE, DateTime? TO_DATE,int? GRADE_SRNO,int? THICNESS_SRNO, decimal? WIDTH, int? STATUS_SRNO,int? C_LOCATION)
        {
            var response = _dataService.DtDashRawInventory(comPara, MATERIAL_FLAG, F_DATE,TO_DATE, GRADE_SRNO, THICNESS_SRNO, WIDTH, STATUS_SRNO, C_LOCATION);
            return Ok(response);
        }
        // Get Methods for tech dsahboard data DTLS
        [HttpGet("DtDashRawInventoryDtl")]
        public IActionResult DtDashRawInventoryDtl([FromQuery] ComParaModel comPara, char? MATERIAL_FLAG, string MATERIAL_SRNOS, string? SLITTING_SRNOS)
        {
            var response = _dataService.DtDashRawInventoryDtl(comPara, MATERIAL_FLAG, MATERIAL_SRNOS,SLITTING_SRNOS);
            return Ok(response);
        }

        // Get Method for get users
        [HttpGet("DtUsers")]
        public IActionResult DtUsers([FromQuery] ComParaModel comPara)
        {
            var response = _dataService.DtUsers(comPara);
            return Ok(response);
        }
        // Get Method for get user type
        [HttpGet("DtUserTypes")]
        public IActionResult DtUserTypes([FromQuery] ComParaModel comPara)
        {
            var response = _dataService.DtUserTypes(comPara);
            return Ok(response);
        }

        // POST method for Insert the users
        [HttpPost("IuUser")]
        public IActionResult IuUser([FromBody] IuUserModel userModel)
        {
            var response = _dataService.IuUser(userModel);
            return Ok(response);
        }

        //POST METHOD FOR USER TYPE
        [HttpGet("IuUserType")]
        public IActionResult IuUserType([FromQuery] ComParaModel comPara, char IU_FLAG, int? USER_TYPE_SRNO, string USER_TYPE_NAME, string? USER_TYPE_DESC)
        {
            var response = _dataService.IuUserType(comPara,IU_FLAG,USER_TYPE_SRNO,USER_TYPE_NAME,USER_TYPE_DESC);
            return Ok(response);
        }

        // Delete method for the raw material and slit process
        [HttpDelete("DelRawSlit")]
        public IActionResult DelRawSlit([FromQuery] ComParaModel comPara, int SRNO, bool IS_MOTHER_COIL)
        {
            var response = _dataService.DelRawSlit(comPara, SRNO, IS_MOTHER_COIL);
            return Ok(response);
        } 
        
        // Update the status of stock materialsa
        [HttpGet("IuShiftStock")]
        public IActionResult IuShiftStock([FromQuery] ComParaModel comPara, char IU_FLAG, char COIL_FLAG, char STATUS_FLAG, int SRNO)
        {
            var response = _dataService.IuShiftStock(comPara,IU_FLAG,COIL_FLAG,STATUS_FLAG,SRNO);
            return Ok(response);
        }


        // Master API
        [HttpPost("IuMGrade")]
        public IActionResult IuMGrade([FromBody] IuMasterPara masterPara)
        {
            var response = _dataService.IuMGrade(masterPara);
            return Ok(response);
        }

        [HttpPost("IuMThickness")]
        public IActionResult IuMThickness([FromBody] IuMasterPara masterPara)
        {
            var response = _dataService.IuMThickness(masterPara);
            return Ok(response);
        }

        [HttpPost("IuMOD")]
        public IActionResult IuMOD([FromBody] IuMasterPara masterPara)
        {
            var response = _dataService.IuMOD(masterPara);
            return Ok(response);
        }

        //Master Delete Api
        [HttpDelete("DelMGrade")]
        public IActionResult DelMGrade([FromQuery] DelMasterPara masterPara)
        {
            var response = _dataService.DelMGrade(masterPara);
            return Ok(response);
        }

        [HttpDelete("DelMThickness")]
        public IActionResult DelMThickness([FromQuery] DelMasterPara masterPara)
        {
            var response = _dataService.DelMThickness(masterPara);
            return Ok(response);
        }

        [HttpDelete("DelMOD")]
        public IActionResult DelMOD([FromQuery] DelMasterPara masterPara)
        {
            var response = _dataService.DelMOD(masterPara);
            return Ok(response);
        }

    }
}
