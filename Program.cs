using System;
using System.Threading;

namespace Tarea1
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 2 || args[0] != "-port")
            {
                Console.WriteLine("Uso: dotnet run -- -port <puerto-escucha>");
                return;
            }

            if (!int.TryParse(args[1], out int puertoEscucha))
            {
                Console.WriteLine("El puerto debe ser un número válido.");
                return;
            }

            Console.WriteLine($"Iniciando chat en el puerto {puertoEscucha}");

            var cliente = new Cliente();
            var servidor = new Servidor();

            servidor.MensajeRecibido += (mensaje) =>
            {
                Console.WriteLine($"Mensaje recibido: {mensaje}");
            };

            servidor.Iniciar(puertoEscucha);

            while (true)
            {
                Console.Write("Ingrese el puerto de destino (o 'salir' para terminar): ");
                string input = Console.ReadLine();

                if (input.ToLower() == "salir")
                    break;

                if (!int.TryParse(input, out int puertoDestino))
                {
                    Console.WriteLine("Puerto inválido. Intente de nuevo.");
                    continue;
                }

                Console.Write("Ingrese el mensaje: ");
                string mensaje = Console.ReadLine();

                try
                {
                    cliente.Conectar("127.0.0.1", puertoDestino);
                    cliente.EnviarMensaje(mensaje);
                    Console.WriteLine("Mensaje enviado.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al enviar el mensaje: {ex.Message}");
                }
            }

            servidor.Detener();
        }
    }
}