using Flurl.Http;
using System;
using System.Threading.Tasks;

namespace ConsoleAppTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Task.Run(async () =>
            {
                try
                {
                    var result = await "https://localhost:5001/api/v1/token".PostJsonAsync(
                        new { email = "user@example.com", password = "12345" }).ReceiveJson<Token>(); ;

                    Console.WriteLine($"Post count: {result}");
                }
                //catch (FlurlHttpException fx)
                //{
                //    if(fx.StatusCode == 404)
                //        Console.WriteLine($"Error: accountid não encontrado");

                //    Console.WriteLine($"Error: {fx.Message}");
                //}
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    throw;
                }




            }).Wait();

            Console.ReadLine();
        }
    }
}
