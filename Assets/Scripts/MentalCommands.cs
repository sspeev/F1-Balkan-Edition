using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EmotivUnityPlugin;
using System;

public class MentalCommands : MonoBehaviour
{
    BCITraining _bciTraining = new BCITraining();
    bool mentalCmdRcvd = false;
    string mentalCommand;
    Rigidbody ball;

    [SerializeField]
    float thrust = 20.0f;
    Vector3 forward = new Vector3(0, 0, 1);
    Vector3 pull = new Vector3(0, 0, -1);
    Vector3 right = new Vector3(1, 0, 0);
    Vector3 left = new Vector3(-1, 0, 0);

    List<string> dataStreamList = new List<string>()
    {
        DataStreamName.DevInfos, DataStreamName.MentalCommands, DataStreamName.SysEvents
    };
    string profileName = "Chris";
    string headSetId = "";

    // Start is called before the first frame update
    void Start()
    {
        // start connect and authorize
        DataStreamManager.Instance.StartAuthorize();
        _bciTraining.Init(); //IAW email from Emotiv
        ball = GetComponent<Rigidbody>();
    }
    private void OnGUI()
    {
        if (Event.current.Equals(Event.KeyboardEvent("return")))
        {
            DataStreamManager.Instance.StartDataStream(dataStreamList, "EPOCX-41B0C4C2");
            mentalCmdRcvd = true;
        }
        if (Event.current.Equals(Event.KeyboardEvent("q")))
        {
            _bciTraining.QueryProfile();
        }
        if (Event.current.Equals(Event.KeyboardEvent("w")))
        {
            _bciTraining.LoadProfileWithHeadset(profileName, headSetId);
        }

        if (Event.current.Equals(Event.KeyboardEvent("e")))
        {
            _bciTraining.UnLoadProfile(profileName, headSetId);
        }
        if (Event.current.Equals(Event.KeyboardEvent("escape")))
        {
            DataStreamManager.Instance.UnSubscribeData(dataStreamList);
            mentalCmdRcvd = false;
            DataStreamManager.Instance.Stop();
        }
    }
    private void Update()
    {
        if (mentalCmdRcvd)
        {
            mentalCommand = DataStreamManager.Instance.getMentalCommandAction();
            Debug.Log("Works: " + mentalCommand);

            if (mentalCommand == "push")
            {
                ball.AddForce(forward * thrust);
            }
            if (mentalCommand == "pull")
            {
                ball.AddForce(pull * thrust);
            }
            if (mentalCommand == "right")
            {
                ball.AddForce(right * thrust);
            }
            if (mentalCommand == "left")
            {
                ball.AddForce(left * thrust);
            }
        }
    }
    public string getMentalCommandAction()
    {
        return action;
    }
    public double getMentalCommandPower()
    {
        return power;
    }
    private void onMentalCommandReceived(double time, string act,  double pow)
    {

    }
    private void OnMentalCommandReceived(object sender, ArrayList data)
    {
        if (_mentalCommandLists == null || _mentalCommandLists.Count != data.Count)
        {
            Debug.LogAssertion("OnMentalCommandReceived: Mismatch between data and label");
            return;
        }
        double time = Convert.ToDouble(data[0]);
        string act = Convert.ToString(data[1]);
        double pow = Convert.ToDouble(data[2]);
        MentalCommandEventArgs comEvent = new MentalCommandEventArgs(time, act, pow);
        string comListsStr = "";
        foreach (var ele in _mentalCommandLists)
        {
            comListsStr += ele + " , ";
        }
        Debug.Log("MentalCommand labels: " + comListsStr);
        Debug.Log("Mentalcommand datas: " + comEvent.Time.ToString() + " , " + comEvent.Act + " , " + comEvent.Pow);
        action = comEvent.Act;
        power = comEvent.Pow;
        // TODO: emit event to other modules 
        //MentalCommandReceived(this, comEvent);
    }
    
}
