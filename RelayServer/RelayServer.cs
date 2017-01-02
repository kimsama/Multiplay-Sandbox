using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;

namespace Multiplay.Sandbox
{
    public class GameSession : AppSession<GameSession, BinaryRequestInfo>
    {

    }

    class RelayServer : AppServer<GameSession, BinaryRequestInfo>
    {
        public RelayServer() : base(new DefaultReceiveFilterFactory<, BinaryRequestInfo>())
        {

        }
    }
}
