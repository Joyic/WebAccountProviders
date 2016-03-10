using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Security.Authentication.Web.Core;
using Windows.Security.Credentials;

namespace WebAccountProviders
{
    class Program
    {
        static void Main(string[] args)
        {
            Program p = new Program();
            p.GetProvider().Wait();
            Console.ReadKey();
        }

        private async Task GetProvider()
        {
            try
            {
                IAsyncOperation<WebAccountProvider> provider = WebAuthenticationCoreManager.FindAccountProviderAsync("login.windows.local");//login.windows.local
                

                //int count = 5;

                //while(provider.Status != AsyncStatus.Completed && count > 0)
                //{
                //    Thread.Sleep(200);
                //    count--;
                //}

                var result = await provider.AsTask();
                if (result != null)
                {
                    Console.WriteLine(result.DisplayName);
                    Console.WriteLine(result.DisplayPurpose);
                    Console.WriteLine(result.Id);
                }
                else
                {
                    Console.WriteLine("there is no default account is setup for the user");
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

        }
    }
}
