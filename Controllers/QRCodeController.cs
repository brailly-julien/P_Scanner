using System;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using P_Scanner.Models;
using QRCoder;
using System.Drawing;
using MongoDB.Bson;
using MongoDB.Driver;

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

        [HttpPost]
        public IActionResult UpdateSeat(string qrCodeData) 
        {
            // Extraire les variables du QR code
            var variables = qrCodeData.Split(",");
            var variable1 = variables[0]; // Seat id
            var variable2 = variables[1]; // id_user
            var variable3 = variables[2]; // id_movie

            Console.WriteLine($"variable1: {variable1}, variable2: {variable2}, variable3: {variable3}");

            // Créer une connexion à la base de données MongoDB
            var client = new MongoClient("mongodb+srv://dbUser:dbPassword@serverlessinstanceiottp.ygpm5nj.mongodb.net");
            var database = client.GetDatabase("IotProject");
            var collection = database.GetCollection<BsonDocument>("seats");

            try {
                // Tenter de faire une opération sur la base de données pour établir la connexion
                collection.Find(_ => true).Limit(1).ToList();

                // Si aucune exception n'est levée, la connexion a été établie avec succès
                Console.WriteLine("Connected to the database successfully");
            } catch (Exception e) {
                // Si une exception est levée, la connexion a échoué
                Console.WriteLine($"Failed to connect to the database: {e.Message}");
            }

            // Créer un filtre pour trouver le document avec le bon id
            var filter = Builders<BsonDocument>.Filter.Eq("id", variable1);

            var document = collection.Find(filter).FirstOrDefault();
            if (document == null) 
            {
                Console.WriteLine($"No document found with id {variable1}");
            } 
            else 
            {
                Console.WriteLine($"Document found: {document}");
            }

            // Créer une mise à jour pour définir les nouvelles valeurs pour id_user et id_movie
            var update = Builders<BsonDocument>.Update
                .Set("id_user", variable2)
                .Set("id_movie", variable3);

            // Appliquer la mise à jour
            var result = collection.UpdateOne(filter, update);

            // Vérifier si l'opération a réussi
            if (result.ModifiedCount != 1)
            {
                // Si le nombre de documents modifiés n'est pas 1, une erreur s'est produite
                return Json(new { success = false });
            }

            return Json(new { success = true });
        }

    }
}