using EmotivUnityPlugin;
using System.Collections.Generic;
using UnityEngine;

public class EmotivControlTest : MonoBehaviour
{
    EmotivUnityItf eup = new();
    private Interface settings = new();

    [SerializeField]
    private GameObject EmotivGameObj;

    private readonly string clientId = "V0xa11";
    private readonly string clientSecret = "xAwmtNXPNBkczijYGA3clI0jP4JVLhpuvCASolQuVuJrX4E8qmbgpumMUWI7TaJw9Nqi3Zpiz13Wv6WvK2zE2iICuVSjtRGAlcs3bKeBbVgUXHq9XOMLXwYCpIxvGsnn";
    private readonly string appName = "UnityApp";
    private readonly string appVersion = "3.6.9 ";
    private readonly string headSetId = "INSIGHT2-A3D2048A";
    private DataStreamManager dsm = DataStreamManager.Instance;

    private readonly List<string> channels = new()
        {
        "AF4",
        "AF3",
        "T7",
        "T8"
    };

    private void Awake()
    {
        eup.Init(clientId, clientSecret, appName, appVersion, false);
        eup.Start();
    }

    void Update()
    {
        if (settings.BrainControls)
        {
            EmotivGameObj.SetActive(true);
        }
        else EmotivGameObj.SetActive(false);


        // Check to call scan headset if no session is created and no scanning headset
        if (!eup.IsSessionCreated && !dsm.IsHeadsetScanning)
        {
            // Start scanning headset at headset list screen
            dsm.ScanHeadsets();
        }
        if (dsm.DetectedHeadsets.Count == 0)
        {
            return;
        }
        eup.CreateSessionWithHeadset(headSetId);
        if (eup.IsSessionCreated)
        {
            eup.SubscribeData(channels);
            double[] eegData = eup.GetMotionData(Channel_t.CHAN_AF4);
        }
    }
}
