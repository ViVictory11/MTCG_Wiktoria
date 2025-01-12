namespace Client;

public static class HttpClient
{
    public static string SendRequest(string method, string endpoint, string host = "localhost", int port = 8080, string body = "")
    {
        using (TcpClient client = new TcpClient(host, port))
        {
            NetworkStream stream = client.GetStream();
            string request = $"{method} {endpoint} HTTP/1.1\r\n" +
                             $"Host: {host}:{port}\r\n" +
                             "Connection: close\r\n" +
                             (!string.IsNullOrEmpty(body) ? $"Content-Length: {body.Length}\r\n\r\n{body}" : "\r\n");
            byte[] requestBytes = Encoding.UTF8.GetBytes(request);
            stream.Write(requestBytes, 0, requestBytes.Length);

            byte[] buffer = new byte[client.ReceiveBufferSize];
            int bytesRead = stream.Read(buffer, 0, buffer.Length);
            return Encoding.UTF8.GetString(buffer, 0, bytesRead);
        }
    }
}
