using RailStream_Server.Models.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RailStream_Server.Managers
{
    internal class ConnectionsManager
    {
        private void SendResponse(TcpClient client, ServerResponce responce)
        {
            NetworkStream networkStream = client.GetStream();

            try
            {
                if (networkStream.CanRead)
                {
                    Byte[] bytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(responce));
                    networkStream.Write(bytes, 0, bytes.Length);
                }
            }

            catch (Exception e)
            {

            }
        }
    }
}
