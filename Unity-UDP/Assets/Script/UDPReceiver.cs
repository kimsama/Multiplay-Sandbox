using UnityEngine;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class UDPReceiver : MonoBehaviour 
{ 
    public int LOCA_LPORT = 3333; 
    static UdpClient udp; 
    Thread thread; 

    void Start() 
    { 
        udp = new UdpClient(LOCA_LPORT); 
        udp.Client.ReceiveTimeout = 50000; 
        thread = new Thread(new ThreadStart(ThreadMethod)); 
        thread.Start(); 
    } 

    void OnApplicationQuit() 
    { 
        thread.Abort(); 
    }
 
    private static void ThreadMethod() 
    { 
        while (true) 
        { 
            IPEndPoint remoteEP = null; 
            byte[] data = udp.Receive(ref remoteEP); 
            string text = Encoding.ASCII.GetString(data);

            if (!string.IsNullOrEmpty(text))
                Debug.Log(text); 
        } 
    } 
}
