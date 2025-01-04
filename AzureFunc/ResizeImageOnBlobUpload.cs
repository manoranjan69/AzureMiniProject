//using System;
//using Microsoft.Azure.WebJobs;
//using Microsoft.Azure.WebJobs.Host;
//using Microsoft.Extensions.Logging;
//using SixLabors.ImageSharp;
//using SixLabors.ImageSharp.Formats;
//using SixLabors.ImageSharp.PixelFormats;
//using System.IO;
//using SixLabors.ImageSharp.Processing;

//namespace AzureFunc
//{
//    public static class ResizeImageOnBlobUpload
//    {
//        [FunctionName("ResizeImageOnBlobUpload")]
//        public static void Run([BlobTrigger("functionsalesapp/{name}", Connection = "AzureWebJobsStorage")] Stream myBlob, string name, ILogger log)
//        {
//            log.LogInformation($"C# Blog trigger function processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");
//            //using Image<Rgba32> input = Image.Load<Rgba32>(myBlob,
//            //                                               out IImageFormat format);

//            using Image<Rgba32> input = Image.Load<Rgba32>(myBlob, out IImageFormat format);
//            input.Mutate(x => x.Resize(300, 200));
//        }
//    }
//}
