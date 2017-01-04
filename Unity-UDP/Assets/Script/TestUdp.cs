using UnityEngine;
using UniRx;
using UdpReceiverUniRx;

public class TestUdp : MonoBehaviour
{
    public UdpReceiverRx udpReceiverRx;
    private IObservable<UdpState> udpStream;

    void Awake()
    {
        // Assume the gameobject also has 'UdpReceiverRx' component.
        udpReceiverRx = GetComponent<UdpReceiverRx>();
    }

    void Start()
    {
        udpStream = udpReceiverRx.udpStream;
        udpStream.ObserveOnMainThread()
            .Subscribe(x =>
            {
                print(x.UdpMsg);
            })
            .AddTo(this);
    }
}