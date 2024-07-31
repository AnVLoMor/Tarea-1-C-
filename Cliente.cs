using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Tarea1
{
    public class Cliente
    {
        private Socket _clienteSocket;

        public Cliente()
        {
            _clienteSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public void Conectar(string ip, int puerto)
        {
            _clienteSocket.Connect(new IPEndPoint(IPAddress.Parse(ip), puerto));
        }

        public void EnviarMensaje(string mensaje)
        {
            byte[] data = Encoding.UTF8.GetBytes(mensaje);
            _clienteSocket.Send(data);
            _clienteSocket.Close();
        }
    }
}