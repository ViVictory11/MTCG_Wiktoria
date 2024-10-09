using System;
using System.Net.Sockets;
using System.Text;

namespace MTCG_Wiktoria.Server
{
    public class ServerTest
    {
        private static string SendRequest(string request)
        {
            using (TcpClient client = new TcpClient("localhost", 8080))
            {
                NetworkStream stream = client.GetStream();
                byte[] requestBytes = Encoding.UTF8.GetBytes(request);
                stream.Write(requestBytes, 0, requestBytes.Length);

                byte[] buffer = new byte[1024];
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                return Encoding.UTF8.GetString(buffer, 0, bytesRead);
            }
        }

        public static void TestGetRequest()
        {
            Console.WriteLine("Starting Server for GET Request Test...");
            Server server = new Server();
            server.Start();

            string request = "GET /cards HTTP/1.1\r\nHost: localhost\r\n\r\n";
            string response = SendRequest(request);
            Console.WriteLine("GET /cards Response:\n" + response);

            // Server will stop after test
            server.Stop();
        }

        public static void TestPostSignup()
        {
            Console.WriteLine("Starting Server for POST Signup Test...");
            Server server = new Server();
            server.Start();

            string request = "POST /signup?username=test_user&password=test_pass HTTP/1.1\r\nHost: localhost\r\n\r\n";
            string response = SendRequest(request);
            Console.WriteLine("POST /signup Response:\n" + response);

            // Server will stop after test
            server.Stop();
        }

        public static void TestPostLogin()
        {
            Console.WriteLine("Starting Server for POST Login Test...");
            Server server = new Server();
            server.Start();

            string request = "POST /login?username=john_doe&password=password123 HTTP/1.1\r\nHost: localhost\r\n\r\n";
            string response = SendRequest(request);
            Console.WriteLine("POST /login Response:\n" + response);

            // Server will stop after test
            server.Stop();
        }
    }
}
