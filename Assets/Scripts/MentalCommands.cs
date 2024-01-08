using System.Collections.Generic;
using UnityEngine;
using EmotivUnityPlugin;
using UnityEngine.UI;
using System;






/*
 
 Smenqi HEADSET IDTO
 */








public class MentalCommands : MonoBehaviour
{
    // Please fill clientId and clientSecret of your application before starting
    private readonly string clientId = "oXwnHROVKZ1Lp7gxWpiY6RBm6sMBqzc6t1GndjZ0";
    private readonly string clientSecret = "025Uv4QWYHVaj69NNhx3udmU6f1r7wXG5H7WOiDwBNvSnSem8M5xyHmnSN37DhzwvgeGaRsWxvbQtwVhI7G3m1kf6XGB5knCWvEh0j4L5P8jBIIe19hWY8yxjGlRaOlc";
    private readonly string appName = "F1BalkanEdition";
    private readonly string appVersion = "3.7.5";
    private readonly string headSetIdSchool = "INSIGHT2-A3D2048A";
    private readonly string headSetId = "INSIGHT2-F144C750";


    EmotivUnityItf _eItf = EmotivUnityItf.Instance;
    float _timerDataUpdate = 0;
    const float TIME_UPDATE_DATA = 1f;
    bool _isDataBufferUsing = false; // default subscribed data will not saved to Data buffer


    private readonly string recordTitle = "record1";     // record Title
    private readonly string recordDescription = "First Try";     // record description
    private readonly string ProfileName = "speev";   // headsetId

    //[SerializeField] public Dropdown ActionNameList;

    //[SerializeField] public InputField  MarkerValue;     // marker value
    //[SerializeField] public InputField  MarkerLabel;     // marker Label
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

    void Start()
    {
        // init EmotivUnityItf without data buffer using
        _eItf.Init(clientId, clientSecret, appName, appVersion);

        // Start
        _eItf.Start();

    }

    // Update is called once per frame
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

        onCreateSessionBtnClick();
        onSubscribeBtnClick();

        int counter = 0;
        string motionHeaderStr = "Motion Header: ";
        string motionDataStr = "Motion Data: ";
        var input = new InputDataController();
        foreach (var ele in _eItf.GetMotionChannels())
        {
            if (counter < 3)
            {
                counter++;
                continue;
            }
            string chanStr = ChannelStringList.ChannelToString(ele);
            double[] data = _eItf.GetMotionData(ele);
            motionHeaderStr += chanStr + ", ";
            if (data != null && data.Length > 0)
            {
                motionDataStr += data[0].ToString() + ", ";
                input.MoveInput = (int)data[0];
            }
                
            else
                motionDataStr += "null, "; // for null value
        }
        string msgLog = motionHeaderStr + "\n" + motionDataStr;
        MessageLog.text = msgLog;
    }

    /// <summary>
    /// create session 
    /// </summary>
    public void onCreateSessionBtnClick()
    {
        Debug.Log("onCreateSessionBtnClick");
        if (!_eItf.IsSessionCreated)
        {
            _eItf.CreateSessionWithHeadset(headSetId);
        }
        else
        {
            UnityEngine.Debug.LogError("There is a session created.");
        }
    }

    /// <summary>
    /// start a record 
    /// </summary>
    public void onStartRecordBtnClick()
    {
        Debug.Log("onStartRecordBtnClick " + recordTitle + ":" + recordDescription);
        if (_eItf.IsSessionCreated && !string.IsNullOrEmpty(recordTitle))
        {
            _eItf.StartRecord(recordTitle, recordDescription);
        }
        else
        {
            UnityEngine.Debug.LogError("Can not start a record because there is no active session or record title is empty.");
        }
    }

    /// <summary>
    /// start a record 
    /// </summary>
    public void onStopRecordBtnClick()
    {
        Debug.Log("onStopRecordBtnClick");
        _eItf.StopRecord();
    }

    /// <summary>
    /// inject marker
    /// </summary>
    //public void onInjectMarkerBtnClick()
    //{
    //    Debug.Log("onInjectMarkerBtnClick " + MarkerValue.text + ":" + MarkerLabel.text);
    //    _eItf.InjectMarker(MarkerLabel.text, MarkerLabel);
    //}

    /// <summary>
    /// subscribe data stream
    /// </summary>
    public void onSubscribeBtnClick()
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

    /// <summary>
    /// un-subscribe data
    /// </summary>
    public void onUnsubscribeBtnClick()
    {
        Debug.Log("onUnsubscribeBtnClick");
        if (GetStreamsList().Count == 0)
        {
            UnityEngine.Debug.LogError("The stream name is empty. Please set a valid stream name before unsubscribing.");
        }
        else
        {
            _eItf.DataSubLog = ""; // clear data subscribing log
            _eItf.UnSubscribeData(GetStreamsList());
        }
    }

    /// <summary>
    /// load an exited profile or create a new profile then load the profile
    /// </summary>
    public void onLoadProfileBtnClick()
    {
        Debug.Log("onLoadProfileBtnClick " + ProfileName);
        _eItf.LoadProfile(ProfileName);
    }

    /// <summary>
    /// unload a profile
    /// </summary>
    public void onUnLoadProfileBtnClick()
    {
        Debug.Log("onUnLoadProfileBtnClick " + ProfileName);
        _eItf.UnLoadProfile(ProfileName);
    }

    /// <summary>
    /// save a profile
    /// </summary>
    public void onSaveProfileBtnClick()
    {
        Debug.Log("onSaveProfileBtnClick " + ProfileName);
        _eItf.SaveProfile(ProfileName);
    }

    /// <summary>
    /// start a mental command training action
    /// </summary>
    //public void onStartMCTrainingBtnClick()
    //{
    //    if (_eItf.IsProfileLoaded)
    //        _eItf.StartMCTraining(ActionNameList.captionText.text);
    //    else
    //        UnityEngine.Debug.LogError("onStartMCTrainingBtnClick: Please load a profile before starting training.");
    //}

    /// <summary>
    /// accept a mental command training
    /// </summary>
    public void onAcceptMCTrainingBtnClick()
    {
        if (_eItf.IsProfileLoaded)
            _eItf.AcceptMCTraining();
        else
            UnityEngine.Debug.LogError("onAcceptMCTrainingBtnClick: Please load a profile before start training.");
    }

    /// <summary>
    /// reject a mental command training
    /// </summary>
    public void onRejectMCTrainingBtnClick()
    {
        if (_eItf.IsProfileLoaded)
            _eItf.RejectMCTraining();
        else
            UnityEngine.Debug.LogError("onRejectMCTrainingBtnClick: Please load a profile before start training.");
    }

    /// <summary>
    /// erase a mental command training
    /// </summary>
    //public void onEraseMCTrainingBtnClick()
    //{
    //    Debug.Log("onEraseMCTrainingBtnClick " + ActionNameList.captionText.text);
    //    if (_eItf.IsProfileLoaded)
    //        _eItf.EraseMCTraining(ActionNameList.captionText.text);
    //    else
    //        UnityEngine.Debug.LogError("onAcceptMCTrainingBtnClick: Please load a profile before start training.");
    //}


    void OnApplicationQuit()
    {
        Debug.Log("Application ending after " + Time.time + " seconds");
        _eItf.Stop();
    }

    private void CheckButtonsInteractable()
    {
        if (!_eItf.IsAuthorizedOK)
            return;

        Button createSessionBtn = GameObject.Find("SessionPart").transform.Find("createSessionBtn").GetComponent<Button>();
        if (!createSessionBtn.interactable)
        {
            createSessionBtn.interactable = true;
            return;
        }

        // make startRecordBtn interactable
        Button startRecordBtn = GameObject.Find("RecordPart").transform.Find("startRecordBtn").GetComponent<Button>();
        Button subscribeBtn = GameObject.Find("SubscribeDataPart").transform.Find("subscribeBtn").GetComponent<Button>();
        Button unsubscribeBtn = GameObject.Find("SubscribeDataPart").transform.Find("unsubscribeBtn").GetComponent<Button>();
        Button loadProfileBtn = GameObject.Find("TrainingPart").transform.Find("loadProfileBtn").GetComponent<Button>();
        Button unloadProfileBtn = GameObject.Find("TrainingPart").transform.Find("unloadProfileBtn").GetComponent<Button>();
        Button saveProfileBtn = GameObject.Find("TrainingPart").transform.Find("saveProfileBtn").GetComponent<Button>();
        Button rejectTrainingBtn = GameObject.Find("TrainingPart").transform.Find("rejectTrainingBtn").GetComponent<Button>();
        Button eraseTrainingBtn = GameObject.Find("TrainingPart").transform.Find("eraseTrainingBtn").GetComponent<Button>();
        Button acceptTrainingBtn = GameObject.Find("TrainingPart").transform.Find("acceptTrainingBtn").GetComponent<Button>();
        Button startTrainingBtn = GameObject.Find("TrainingPart").transform.Find("startTrainingBtn").GetComponent<Button>();
        Button stopRecordBtn = GameObject.Find("RecordPart").transform.Find("stopRecordBtn").GetComponent<Button>();
        Button injectMarkerBtn = GameObject.Find("RecordPart").transform.Find("injectMarkerBtn").GetComponent<Button>();

        startRecordBtn.interactable = _eItf.IsSessionCreated;
        subscribeBtn.interactable = _eItf.IsSessionCreated;
        unsubscribeBtn.interactable = _eItf.IsSessionCreated;
        loadProfileBtn.interactable = _eItf.IsSessionCreated;

        saveProfileBtn.interactable = _eItf.IsProfileLoaded;
        startTrainingBtn.interactable = _eItf.IsProfileLoaded;
        rejectTrainingBtn.interactable = _eItf.IsProfileLoaded;
        eraseTrainingBtn.interactable = _eItf.IsProfileLoaded;
        acceptTrainingBtn.interactable = _eItf.IsProfileLoaded;
        unloadProfileBtn.interactable = _eItf.IsProfileLoaded;

        stopRecordBtn.interactable = _eItf.IsRecording;
        injectMarkerBtn.interactable = _eItf.IsRecording;
    }

    private List<string> GetStreamsList()
    {
        List<string> _streams = new()
        {
            //"eeg",
            "mot"
        };
        //if (EEGToggle)
        //{
        //    _streams.Add("eeg");
        //}
        //if (MOTToggle)
        //{
        //    _streams.Add("mot");
        //}
        //if (PMToggle)
        //{
        //    _streams.Add("met");
        //}
        //if (CQToggle)
        //{
        //    _streams.Add("dev");
        //}
        //if (SYSToggle)
        //{
        //    _streams.Add("sys");
        //}
        //if (EQToggle)
        //{
        //    _streams.Add("eq");
        //}
        //if (POWToggle)
        //{
        //    _streams.Add("pow");
        //}
        //if (FEToggle)
        //{
        //    _streams.Add("fac");
        //}
        //if (COMToggle)
        //{
        //    _streams.Add("com");
        //}
        return _streams;
    }
}