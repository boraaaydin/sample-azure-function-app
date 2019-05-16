using System;
using System.IO;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace SampleFuncApp
{
    public static class GenerateLicenceFile
    {
        [FunctionName("GenerateLicenceFile")]
        public static void Run(
            [QueueTrigger("orders", Connection = "AzureWebJobsStorage")]Order order,
            [Blob("licences/{rand-guid}.lic")]TextWriter outputBlob,
            ILogger log)
        {
            //log.LogInformation($"C# Queue trigger function processed: {order}");
            outputBlob.WriteLine($"OrderId: {order.OrderId}");
            outputBlob.WriteLine($"Email: {order.Email}");
            outputBlob.WriteLine($"ProductId: {order.ProductId}");
            outputBlob.WriteLine($"Date: {DateTime.UtcNow}");
            var md5 = System.Security.Cryptography.MD5.Create();
            var hash = md5.ComputeHash(
                System.Text.Encoding.UTF8.GetBytes(order.Email + "secret"));
            outputBlob.WriteLine($"SecretCode: {BitConverter.ToString(hash).Replace("-", "")}");

        }
    }
}
