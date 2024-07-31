using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Tarea1
{
    public class Servidor
    {
        private Socket _servidorSocket;
        private const int BUFFER_SIZE = 2048;
        private byte[] _buffer = new byte[BUFFER_SIZE];
        private bool _running = false;

        public event Action<string> MensajeRecibido;

        public Servidor()
        {
            _servidorSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public void Iniciar(int puerto)
        {
            _servidorSocket.Bind(new IPEndPoint(IPAddress.Any, puerto));
            _servidorSocket.Listen(10);
            _running = true;

            Thread listenThread = new Thread(() =>
            {
                while (_running)
                {
                    try
                    {
                        Socket clienteSocket = _servidorSocket.Accept();
                        Thread clientThread = new Thread(() => ManejadorCliente(clienteSocket));
                        clientThread.Start();
                    }
                    catch (SocketException)
                    {
                        // El socket se cerró, salimos del bucle
                        break;
                    }
                }
            });
            listenThread.Start();
        }

        private void ManejadorCliente(Socket clienteSocket)
        {
            while (_running)
            {
                try
                {
                    int recibido = clienteSocket.Receive(_buffer);
                    if (recibido == 0) break;

                    string mensaje = Encoding.UTF8.GetString(_buffer, 0, recibido);
                    MensajeRecibido?.Invoke(mensaje);
                }
                catch (SocketException)
                {
                    break;
                }
            }
            clienteSocket.Close();
        }

        public void Detener()
        {
            _running = false;
            _servidorSocket.Close();
        }
    }
}