using UnityEngine;
using System.Net;
using System.Net.Sockets;
using UniRx;

namespace UdpReceiverUniRx
{

    public class UdpState : System.IEquatable<UdpState>
    {
        // UDP 통신 정보를 거둔다.  송수신 모두 사용할
        public IPEndPoint EndPoint { get; set; }
        public string UdpMsg { get; set; }

        public UdpState(IPEndPoint ep, string udpMsg)
        {
            this.EndPoint = ep;
            this.UdpMsg = udpMsg;
        }
        public override int GetHashCode()
        {
            return EndPoint.Address.GetHashCode();
        }

        public bool Equals(UdpState s)
        {
            if (s == null)
            {
                return false;
            }
            return EndPoint.Address.Equals(s.EndPoint.Address);
        }
    }

    public class UdpReceiverRx : MonoBehaviour
    {
        public int listenPort = 3333;
        public IObservable<UdpState> udpStream;

        private static UdpClient myClient;
        private bool isAppQuitting;

        void Awake()
        {
            udpStream = Observable.Create <UdpState> (observer =>
            {
                Debug.Log (string.Format ( "udpStream thread : {0}", System.Threading.Thread.CurrentThread.ManagedThreadId));

                try
                {
                    myClient = new UdpClient (listenPort);
                }
                catch (SocketException ex)
                {
                    observer. OnError (ex);
                }

                IPEndPoint remoteEP = null;
                myClient.EnableBroadcast = true;
                myClient.Client.ReceiveTimeout = 5000;
                while (!isAppQuitting)
                {
                    try
                    {
                        remoteEP = null;
                        var receivedMsg = System.Text.Encoding.ASCII.GetString (myClient.Receive (ref remoteEP));
                        observer.OnNext (new UdpState (remoteEP, receivedMsg));
                    }
                    catch (SocketException)
                    {
                        Debug.Log ( "UDP :: Receive timeout");
                    }
                }
                observer.OnCompleted ();
                return null;
            })
            .SubscribeOn(Scheduler.ThreadPool)
            .Publish()
            .RefCount();
        }

        void OnApplicationQuit()
        {
            isAppQuitting = true;
            myClient.Client.Blocking = false;
        }
    }
}