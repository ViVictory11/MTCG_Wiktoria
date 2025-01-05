using System.Net;
using System.Net.Sockets;
using System.Text;

namespace MTCG_Wiktoria.Server;

public class Server
{
    public void Start()
    {
        TcpListener listener = new TcpListener(IPAddress.Any, 8080);
        listener.Start();
        Console.WriteLine("Server started, listening on port 8080...");

        while (true)
        {
            using TcpClient client = listener.AcceptTcpClient();
            using NetworkStream stream = client.GetStream();

            byte[] buffer = new byte[client.ReceiveBufferSize];
            int bytesRead = stream.Read(buffer, 0, buffer.Length);

            string request = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            string[] requestLines = request.Split(new[] { "\r\n" }, StringSplitOptions.None);
            string requestUrl = requestLines[0].Split(' ')[1]; // Extracting the URL
            string method = requestLines[0].Split(' ')[0]; // Extracting the HTTP method

            int statusCode;
            string responseBody;

            if (method == "GET")
            {
                string authorizationHeader = requestLines.FirstOrDefault(line => line.StartsWith("Authorization:"))?.Split(' ')[1];
                (statusCode, responseBody) = RequestHandler.GetRequest(requestUrl, authorizationHeader);
            }
            else if (method == "POST")
            {
                (statusCode, responseBody) = RequestHandler.PostRequest(requestUrl);
            }
            else
            {
                statusCode = 405;
                responseBody = "Method Not Allowed: This HTTP method is not supported.";
            }

            SendResponse(stream, statusCode, responseBody);
        }
    }
    
    

    private void SendResponse(NetworkStream stream, int statusCode, string responseBody)
    {
        string response = $"HTTP/1.1 {statusCode} \r\n" +
                          "Content-Type: text/plain\r\n" +
                          $"Content-Length: {Encoding.UTF8.GetByteCount(responseBody)}\r\n" +
                          "Connection: close\r\n" +
                          "\r\n" +
                          responseBody;
        
        byte[] responseBuffer = Encoding.UTF8.GetBytes(response);
        stream.Write(responseBuffer, 0, responseBuffer.Length);
    }
}
