using Microsoft.AspNetCore.Mvc;
using omech.Filters;
using omech.Models;
using omech.Services;

namespace omech.Controllers
{
    // Request models — one per tool, matching what the MCP server sends.
    public class McpComPara
    {
        public int UserSrno { get; set; } = 0;
        public int UtSrno { get; set; } = 0;
    }
    public class RawMaterialReq : McpComPara { public string? ChallanNo { get; set; } public DateTime? DateFrom { get; set; } public DateTime? DateTo { get; set; } public string? Supplier { get; set; } public int? GradeSrno { get; set; } public int? ThicknessSrno { get; set; } }
    public class RawInventoryReq : McpComPara { public DateTime? DateFrom { get; set; } public DateTime? DateTo { get; set; } public int? GradeSrno { get; set; } public int? ThicknessSrno { get; set; } public decimal? Width { get; set; } public int? StatusSrno { get; set; } public int? LocationSrno { get; set; } }
    public class PipeInventoryReq : McpComPara { public int? GradeSrno { get; set; } public int? ThicknessSrno { get; set; } public int? OdSrno { get; set; } public int? LocationSrno { get; set; } public decimal? PrLength { get; set; } }
    public class PipeMovementsReq : McpComPara { public int? GradeSrno { get; set; } public int? ThicknessSrno { get; set; } public int? OdSrno { get; set; } public int? LocationSrno { get; set; } public decimal? PrLength { get; set; } public int? InvType { get; set; } public DateTime? DateFrom { get; set; } public DateTime? DateTo { get; set; } }
    public class OrderScheduleReq : McpComPara { public int? GradeSrno { get; set; } public int? ThicknessSrno { get; set; } public int? OdSrno { get; set; } public string? PartyName { get; set; } public string? PoNumber { get; set; } public DateTime? EntryDateFrom { get; set; } public DateTime? EntryDateTo { get; set; } public DateTime? DeliveryDateFrom { get; set; } public DateTime? DeliveryDateTo { get; set; } }
    public class LowStockReq : McpComPara { public int? GradeSrno { get; set; } public int? ThicknessSrno { get; set; } public int? OdSrno { get; set; } }

    [Route("api/Mcp")]
    [ApiController]
    [McpApiKey]
    public class McpDataController : ControllerBase
    {
        private readonly IDataService _dataService;

        public McpDataController(IDataService dataService)
        {
            _dataService = dataService;
        }

        private static ComParaModel ToComPara(McpComPara r) => new() { USER_SRNO = r.UserSrno, UT_SRNO = r.UtSrno };

        [HttpPost("Lookups")]
        public IActionResult Lookups([FromBody] McpComPara r) =>
            Ok(_dataService.Pl_Common(ToComPara(r), "1,2,3,4"));

        [HttpPost("RawMaterial")]
        public IActionResult RawMaterial([FromBody] RawMaterialReq r) =>
            Ok(_dataService.DtRawMaterial(ToComPara(r), r.ChallanNo, r.DateFrom, r.DateTo, r.Supplier, r.GradeSrno, r.ThicknessSrno, null));

        [HttpPost("RawInventory")]
        public IActionResult RawInventory([FromBody] RawInventoryReq r) =>
            Ok(_dataService.DtDashRawInventory(ToComPara(r), null, r.DateFrom, r.DateTo, r.GradeSrno, r.ThicknessSrno, r.Width, r.StatusSrno, r.LocationSrno));

        [HttpPost("PipeInventory")]
        public IActionResult PipeInventory([FromBody] PipeInventoryReq r) =>
            Ok(_dataService.DtPipes(ToComPara(r), null, r.GradeSrno, r.ThicknessSrno, r.OdSrno, r.LocationSrno, (int?)r.PrLength));

        [HttpPost("PipeMovements")]
        public IActionResult PipeMovements([FromBody] PipeMovementsReq r) =>
            Ok(_dataService.DtPipesLogs(ToComPara(r), null, r.GradeSrno, r.ThicknessSrno, r.OdSrno, r.LocationSrno, (int?)r.PrLength, r.InvType, r.DateFrom, r.DateTo));

        [HttpPost("OrderSchedule")]
        public IActionResult OrderSchedule([FromBody] OrderScheduleReq r) =>
            Ok(_dataService.DtScheduleAnalysis(ToComPara(r), r.GradeSrno, r.ThicknessSrno, r.OdSrno, r.PartyName, r.PoNumber, r.EntryDateFrom, r.EntryDateTo, r.DeliveryDateFrom, r.DeliveryDateTo));

        [HttpPost("LowStock")]
        public IActionResult LowStock([FromBody] LowStockReq r) =>
            Ok(_dataService.DtThresholdDisplay(ToComPara(r), r.GradeSrno, r.ThicknessSrno, r.OdSrno, null));
    }
}
