using Newtonsoft.Json.Linq;
using ConsumpOtp.Models;

namespace ConsumpOtp.Controllers 
{
    public class ServiceHelper 
    {
        private static Uri DataBaseAddress {get; set;} = new Uri("http://localhost:5148");

        private static HttpClient GetClientData()
        {
            var clientData = new HttpClient();
            clientData.BaseAddress = DataBaseAddress;
            return clientData;
        }

       

        public async static Task<OtpRequest> PostOtp(string phoneNumber)
        {
            // Create an anonymous object for the JSON payload
            var payload = new { PhoneNumber = phoneNumber };

            var clientData = GetClientData();

            // Send the request as JSON
            var response = await clientData.PostAsJsonAsync("api/Otp/send", payload);

            // Ensure the response indicates success
            response.EnsureSuccessStatusCode();

            // Read and parse the response
            var json = await response.Content.ReadAsStringAsync();
            return JObject.Parse(json).ToObject<OtpRequest>();
        }

        public async static Task<OtpRequest> PostVerify(string phoneNumber, string otp)
        {
            // Create an anonymous object for the JSON payload
            var payload = new { PhoneNumber = phoneNumber, Otp = otp };

            var clientData = GetClientData();

            var response = await clientData.PostAsJsonAsync("api/Otp/verify", payload);

                // Check the status code before ensuring success
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                // Return null if the status code is 401
                return null;
            }

            // Ensure the response indicates success
            response.EnsureSuccessStatusCode();

            // Read and parse the response
            var json = await response.Content.ReadAsStringAsync();
            return JObject.Parse(json).ToObject<OtpRequest>();
        }

    }
}