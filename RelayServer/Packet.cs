using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;
using SuperSocket.Facility.Protocol;
using SuperSocket.Common;

namespace Multiplay.Sandbox
{


    public class GamePlayRequestInfo : BinaryRequestInfo
    {
        public int nKey
        {
            get; private set;
        }

        public GamePlayRequestInfo(int key, byte[] body) : base(null, body)
        {
            nKey = key;
        }
    }

    class GamePlayReceiveFilter : FixedHeaderReceiveFilter<GamePlayRequestInfo>
    {
        const int HEADERSIZE = 8;
        public GamePlayReceiveFilter()
            : base(HEADERSIZE)
        {
        }

        /// <summary>
        /// overrided to return the length of the body according to the received header.
        /// </summary>
        protected override int GetBodyLengthFromHeader(byte[] header, int offset, int length)
        {
            if (!BitConverter.IsLittleEndian)
                Array.Reverse(header, offset + 4, 4);

            return BitConverter.ToInt32(header, offset + 4);
        }

        /// <summary>
        /// overrided to return the RequestInfo instance according to the received header and body.
        /// </summary>
        protected override GamePlayRequestInfo ResolveRequestInfo(ArraySegment<byte> header, byte[] bodyBuffer, int offset, int length)
        {
            if (!BitConverter.IsLittleEndian)
                Array.Reverse(header.Array, 0, 4);

            return new GamePlayRequestInfo(BitConverter.ToInt32(header.Array, 0), bodyBuffer.CloneRange(offset, length));

        }
    }
}
