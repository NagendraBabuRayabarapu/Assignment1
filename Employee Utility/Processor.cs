using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Employee_Utility
{
    internal class Processor
    {
        public static async Task<string> MakeHttpCall(HttpMethod method, string url, string json)
        {
            string resjson = "";
            try
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage(method, url);
                if (method.Equals(HttpMethod.Post) || method.Equals(HttpMethod.Put))
                {
                    var content = new StringContent(json, null, "application/json");
                    request.Content = content;
                }
                var response = await client.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    resjson = await response.Content.ReadAsStringAsync();

                }
                else
                {
                    MessageBox.Show(response.Content.ReadAsStringAsync().Result);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception Occurred When making a http request " + "\n" +ex.Message +"\n" + ex.StackTrace, "ERROR", MessageBoxButtons.OK);
            }
            return resjson;
        }
    }
}
