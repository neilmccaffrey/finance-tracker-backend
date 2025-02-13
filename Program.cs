using System;
using System.Net.Sockets;

class Program
{
    static void Main()
    {
        string host = "junction.proxy.rlwy.net";
        int port = 59468;

        try
        {
            using (var client = new TcpClient())
            {
                client.Connect(host, port);
                Console.WriteLine("Connection successful!");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Connection failed: {ex.Message}");
        }
    }
}