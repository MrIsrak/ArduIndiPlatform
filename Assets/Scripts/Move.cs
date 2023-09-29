//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using System.IO.Ports;
//using System.Threading;

//public class Move : MonoBehaviour
//{

//    Thread IOThread = new Thread(DataThread);
//    private static SerialPort sp;
//    private static string incomingMsg = "";
//    private static string outgoungMsg = "";

//    private static void DataThread()
//    {
//        sp = new SerialPort("COM3", 9600);
//        sp.Open();

//        while (true)
//        {
//            if (outgoungMsg != "")
//            {
//                sp.Write(outgoungMsg);
//                outgoungMsg = "";
//            }
//            incomingMsg = sp.ReadExisting();
//            Thread.Sleep(200);

//        }
//    }

//    void OnDestroy()
//    {
//        IOThread.Abort();
//        sp.Close();
//    }

//    void Start()
//    {
//        IOThread.Start();
//    }

//    void Update()
//    {
//        if (incomingMsg != "") 
//        {
//            Debug.Log(incomingMsg);

//        }
//        if (Input.GetKeyDown(KeyCode.Alpha1))
//        {
//            outgoungMsg = "0";
//        }
//        else if (Input.GetKeyDown(KeyCode.Alpha2))
//        {
//            outgoungMsg = "1";
//        }
//    }


//}
