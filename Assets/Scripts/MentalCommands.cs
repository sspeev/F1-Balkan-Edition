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
    public Rigidbody car;

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
    string profileName = "speev";
    string headSetId = "";

    void Start()
    {
        // start connect and authorize
        DataStreamManager.Instance.StartAuthorize();
        _bciTraining.Init(); //IAW email from Emotiv
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
                car.AddForce(forward * thrust);
            }
            if (mentalCommand == "pull")
            {
                car.AddForce(pull * thrust);
            }
            if (mentalCommand == "right")
            {
                car.AddForce(right * thrust);
            }
            if (mentalCommand == "left")
            {
                car.AddForce(left * thrust);
            }
        }
    }
    private void onMentalCommandReceived(double time, string act,  double pow)
    {

    }
}
