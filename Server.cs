using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Server
{
    class Program
    {
        const int port = 5000;
        const string server_ip = "127.0.0.1";


        static void Main(string[] args)
        {
            var client = new TcpClient(server_ip, port);
            var keystore = new X509Store(StoreLocation.LocalMachine);
            keystore.Open(OpenFlags.ReadOnly);
            var allcerts = keystore.Certificates;
            var certs = keystore.Certificates.Find(X509FindType.FindBySerialNumber, "7582af584e2c2199485c15f1fd02d184", false);
            using (SslStream sslStream = new SslStream(client.GetStream(), false, new RemoteCertificateValidationCallback(ValidateClientCertificate), null))
            {
                sslStream.AuthenticateAsServer(certs[0]);
                sslStream.Write(Encoding.ASCII.GetBytes("Hello"));
            }
        }

        private static bool ValidateClientCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }
    }
}
