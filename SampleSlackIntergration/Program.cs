using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace SampleSlackIntergration
{
    public class ChannelMsg
    {

        public ChannelMsg(string ChannelId) {

            this.channel = ChannelId;
        }
        
        public string channel { get; set; }
    }

    public class SendChatMsg
    {

        public SendChatMsg(string channelName, string message )
        {

            this.channel = channelName;
            this.text = message;
        }

        public string channel { get; set; }
        public string text { get; set; }
    }


    class Program
    {

        static HttpClient client = new HttpClient();
        static void Main(string[] args)
        {
            RunAsync().GetAwaiter().GetResult();
            Console.ReadLine();
        }



        static async Task<HttpResponseMessage> JoinChannel() {


            HttpResponseMessage response = await client.PostAsJsonAsync("api/conversations.join", new ChannelMsg("C021QHUETG9"));

            return response;
        }


        static async Task<HttpResponseMessage> SendMessage()
        {


            HttpResponseMessage response = await client.PostAsJsonAsync("api/chat.postMessage", new SendChatMsg("development", "Sample message from Net application with intergrated SlackAPI"));

            return response;
        }

        static async Task RunAsync() {

            try {
                client.BaseAddress = new Uri("https://slack.com/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                   new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "xoxb-1536282398881-2385208040001-AgmUPhkovQvIIzmxwBO60Ord");

                var resp = await JoinChannel();
                var content = await resp.Content.ReadAsStringAsync();
                Console.WriteLine(content);

                if (resp.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    resp = await SendMessage();
                    content = await resp.Content.ReadAsStringAsync();
                    Console.WriteLine(content);
                }

            } catch (Exception ex) { Console.WriteLine(ex.Message); }
        }
    }
}
