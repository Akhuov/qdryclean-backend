using Microsoft.AspNetCore.Mvc;
using QDryClean.Application.Common.Interfaces;

namespace QDryClean.Api.Controllers
{
    [ApiController]
    [Route("api/v1/utility")]
    public class UtilityController : ControllerBase
    {
        private readonly IQrCodeService _qrCodeService;
        private readonly IBarCodeService _barCodeService;

        public UtilityController(IQrCodeService qrCodeService, IBarCodeService barCodeService)
        {
            _qrCodeService = qrCodeService;
            _barCodeService = barCodeService;
        }

        [HttpGet("generate-qr-code")]
        public IActionResult GenerateQRCode([FromQuery] string text)
        {
            var qrBytes = _qrCodeService.GenerateQrCode(text);
            return File(qrBytes, "image/png");
        }

        [HttpGet("generate-bar-code")]
        public IActionResult GenerateBarCode([FromQuery] string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return BadRequest("Text cannot be empty.");

            // 🔹 Объявляем переменную прямо здесь
            byte[] barcodeBytes = _barCodeService.GenerateCode128(text);

            // 🔹 Используем переменную
            return File(barcodeBytes, "image/svg+xml");
        }
    }
}