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
        [HttpGet("Pl_Common")]
        public IActionResult Pl_Common([FromQuery] ComParaModel comPara, String TBL_SRNO)
        {
            var response = _dataService.Pl_Common(comPara, TBL_SRNO);
            return Ok(response);
        }

        [HttpGet("DtRawMaterial")]
        public IActionResult DtRawMaterial([FromQuery] ComParaModel comPara)
        {
            var response = _dataService.DtRawMaterial(comPara);
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
    }
}
