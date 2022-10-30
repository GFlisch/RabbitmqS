using System;
using System.IO;
using System.Security.Authentication;
using System.Text;
using Arc4u.Security.Cryptography;
using RabbitMQ.client;
using RabbitMQ.Client;
using RabbitMQ.Util;

namespace RabbitMQ.client.Examples
{
    public class TestSSL
    {
        public static int Main(string[] args)
        {
            var cert = Certificate.FindCertificate("buildkitsandbox", location: System.Security.Cryptography.X509Certificates.StoreLocation.CurrentUser);

            ConnectionFactory cf = new ConnectionFactory();

            
            cf.Ssl.Enabled = true;
            cf.Ssl.ServerName = "buildkitsandbox";
            cf.Ssl.Version = SslProtocols.Tls13;
            cf.Ssl.Certs = new System.Security.Cryptography.X509Certificates.X509CertificateCollection();
            cf.Ssl.Certs.Add(cert);
            
            cf.HostName = "localhost";
            cf.Port = 5671;

            using (IConnection conn = cf.CreateConnection())
            {
                using (IModel ch = conn.CreateModel())
                {
                    Console.WriteLine("Successfully connected and opened a channel");
                    ch.QueueDeclare("rabbitmq-dotnet-test", false, false, false, null);
                    Console.WriteLine("Successfully declared a queue");
                    //ch.QueueDelete("rabbitmq-dotnet-test");
                    Console.WriteLine("Successfully deleted the queue");
                }
            }
            return 0;
        }
    }
}