using System;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using P_Scanner.Models;
using QRCoder;
using System.Drawing;

namespace P_Scanner.Controllers
{
    public class QRCodeController : Controller
    {
        private readonly IWebHostEnvironment _hostEnvironment;

        public QRCodeController(IWebHostEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Generate(UserInput input)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode($"{input.Variable1},{input.Variable2},{input.Variable3}", QRCodeGenerator.ECCLevel.Q);
            PngByteQRCode pngByteQRCode = new PngByteQRCode(qrCodeData); 
            byte[] byteImage = pngByteQRCode.GetGraphic(20);

            // Save the image to wwwroot/images/qrcode.png
            var webRoot = _hostEnvironment.WebRootPath;
            var file = System.IO.Path.Combine(webRoot, "images/qrcode.png");
            System.IO.File.WriteAllBytes(file, byteImage);

            // Pass the path of the image to the view
            var model = new QRCodeViewModel { ImagePath = "/images/qrcode.png" };
            return View("Show", model);
        }

        [HttpGet]
        public IActionResult Scan()
        {
            return View();
        }
    }
}
