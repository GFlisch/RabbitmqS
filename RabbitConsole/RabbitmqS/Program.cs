using Arc4u.Security.Cryptography;
using RabbitMQ.Client;
using System.Security.Authentication;

var cert = Certificate.FindCertificate("buildkitsandbox", location: System.Security.Cryptography.X509Certificates.StoreLocation.CurrentUser);

ConnectionFactory cf = new ConnectionFactory();


cf.Ssl.Enabled = true;
cf.Ssl.ServerName = "buildkitsandbox";
cf.Ssl.Version = SslProtocols.Tls13;

// External is defined with x509 authentication
cf.Ssl.Certs = new System.Security.Cryptography.X509Certificates.X509CertificateCollection();
cf.Ssl.Certs.Add(cert);
cf.AuthMechanisms.Clear();
cf.AuthMechanisms.Add(new ExternalMechanismFactory());

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