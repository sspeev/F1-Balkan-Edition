using System.Collections.Generic;
using UnityEngine;
using EmotivUnityPlugin;
using UnityEngine.UI;

/// <summary>
/// Here is implamanted the connection to the Emotiv headset and the recived data
/// </summary>
public class MentalCommandsController : MonoBehaviour
{
    // Please fill clientId and clientSecret of your application before starting
    private readonly string clientId = "oXwnHROVKZ1Lp7gxWpiY6RBm6sMBqzc6t1GndjZ0";
    private readonly string clientSecret = "025Uv4QWYHVaj69NNhx3udmU6f1r7wXG5H7WOiDwBNvSnSem8M5xyHmnSN37DhzwvgeGaRsWxvbQtwVhI7G3m1kf6XGB5knCWvEh0j4L5P8jBIIe19hWY8yxjGlRaOlc";
    private readonly string appName = "F1BalkanEdition";
    private readonly string appVersion = "3.7.5";
    private readonly string headSetIdSchool = "INSIGHT2-A3D2048A";//school
    private readonly string headSetId = "INSIGHT2-F144C750";//home


    EmotivUnityItf _eItf = EmotivUnityItf.Instance;
    float _timerDataUpdate = 0;
    const float TIME_UPDATE_DATA = 1f;


    private bool EEGToggle = true;
    private bool MOTToggle = false;
    private bool PMToggle = false;
    private bool CQToggle = false;
    private bool POWToggle = false;
    private bool EQToggle = false;
    private bool COMToggle = false;
    private bool FEToggle = false;
    private bool SYSToggle = false;

    [SerializeField]
    private Text MessageLog;
    private InputDataController input;

    [SerializeField]
    private Interface toggle;

    void Start()
    {
        input = GetComponent<InputDataController>();
        if (toggle.BrainControls)
        {
            _eItf.Init(clientId, clientSecret, appName, appVersion);
            _eItf.Start();
        }
    }

    void Update()
    {
        _timerDataUpdate += Time.deltaTime;
        if (_timerDataUpdate < TIME_UPDATE_DATA)
            return;
        _timerDataUpdate -= TIME_UPDATE_DATA;

        if (_eItf.MessageLog.Contains("Get Error:"))
        {
            // show error in red color
            MessageLog.color = Color.red;
        }
        else
        {
            // update message log
            MessageLog.color = Color.black;
        }
        MessageLog.text = _eItf.MessageLog;
        if (!_eItf.IsAuthorizedOK)
            return;
        // Check to call scan headset if no session is created and no scanning headset
        if (!_eItf.IsSessionCreated && !DataStreamManager.Instance.IsHeadsetScanning)
        {
            // Start scanning headset at headset list screen
            DataStreamManager.Instance.ScanHeadsets();
        }
        if (DataStreamManager.Instance.DetectedHeadsets.Count == 0)
        {
            return;
        }
        CreateSession();
        Subscribe();

        string motionHeaderStr = "Motion Header: ";
        string motionDataStr = "Motion Data: ";
        float multiplyTheData = 2f;
        input.MoveInput = ExtractData(Channel_t.CHAN_Q0, multiplyTheData);
        input.LeftInput = ExtractData(Channel_t.CHAN_Q1, multiplyTheData);
        input.RightInput = ExtractData(Channel_t.CHAN_Q3, multiplyTheData);
        input.BrakeInput = ExtractData(Channel_t.CHAN_Q2, multiplyTheData);
        string msgLog = motionHeaderStr + "\n" + motionDataStr;
        MessageLog.text = msgLog;

        float ExtractData(Channel_t channel, float multiplyTheData)
        {
            string chanStr = ChannelStringList.ChannelToString(channel);
            double[] data = _eItf.GetMotionData(channel);
            motionHeaderStr += chanStr + ", ";
            if (data != null && data.Length > 0)
            {
                motionDataStr += data[0].ToString() + ", ";
            }
            else motionDataStr += "null, "; // for null value

            return multiplyTheData * (float)data[0];
        }
    }

    /// <summary>
    /// create session 
    /// </summary>
    private void CreateSession()
    {
        if (!_eItf.IsSessionCreated)
        {
            _eItf.CreateSessionWithHeadset(headSetIdSchool);
        }
        else
        {
            Debug.LogError("There is a session created.");
        }
    }

    /// <summary>
    /// subscribe data stream
    /// </summary>
    private void Subscribe()
    {
        Debug.Log("onSubscribeBtnClick: " + _eItf.IsSessionCreated + ": " + GetStreamsList().Count);
        if (_eItf.IsSessionCreated)
        {
            if (GetStreamsList().Count == 0)
            {
                UnityEngine.Debug.LogError("The stream name is empty. Please set a valid stream name before subscribing.");
            }
            else
            {
                _eItf.DataSubLog = ""; // clear data subscribing log
                _eItf.SubscribeData(GetStreamsList());
            }
        }
        else
        {
            UnityEngine.Debug.LogError("Must create a session first before subscribing data.");
        }
    }

    void OnApplicationQuit()
    {
        Debug.Log("Application ending after " + Time.time + " seconds");
        _eItf.Stop();
    }

    private List<string> GetStreamsList()
    {
        List<string> _streams = new()
        {
            //"eeg",
            "mot"
        };
        if (EEGToggle)
        {
            _streams.Add("eeg");
        }
        if (MOTToggle)
        {
            _streams.Add("mot");
        }
        if (PMToggle)
        {
            _streams.Add("met");
        }
        if (CQToggle)
        {
            _streams.Add("dev");
        }
        if (SYSToggle)
        {
            _streams.Add("sys");
        }
        if (EQToggle)
        {
            _streams.Add("eq");
        }
        if (POWToggle)
        {
            _streams.Add("pow");
        }
        if (FEToggle)
        {
            _streams.Add("fac");
        }
        if (COMToggle)
        {
            _streams.Add("com");
        }
        return _streams;
    }
}