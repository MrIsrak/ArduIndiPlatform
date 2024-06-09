//using UnityEngine;
//using System.Collections; // ����������� ������������ ���� ��� ������ � ����������� (��������, �������� � ���������).
//using System.Collections.Generic; // ����������� ������������ ���� ��� ������ � ����������� �����������.
//using System.ComponentModel; // ����������� ������������ ���� ��� ������ � ������������ � ����������.
//using System.Threading; // ����������� ������������ ���� ��� ������ � ��������.
//using System.IO.Ports;

//public class ArduinoControl : MonoBehaviour
//{
//    public string comPort = "COM3"; // ������� COM-����, � �������� ���������� Arduino
//    private SerialPort serialPort;

//    void Start()
//    {
//        serialPort = new SerialPort(comPort, 9600);
//        serialPort.Open();
//        serialPort.ReadTimeout = 100;
//        InvokeRepeating("ToggleLight", 0f, 1f); // ������ ������ �������
//    }

//    void ToggleLight()
//    {
//        if (serialPort.IsOpen)
//        {
//            serialPort.Write("H"); // ��������� ������� "H" ���������
//            string response = serialPort.ReadLine(); // ��������� ����� �� Arduino (�������������)
//            Debug.Log(response); // ������� ����� � ������� Unity (��� �������)
//        }
//    }

//    void OnApplicationQuit()
//    {
//        if (serialPort != null && serialPort.IsOpen)
//        {
//            serialPort.Write("L"); // ��������� ������� "L" ���������� ����� ������� �� ����������
//            serialPort.Close();
//        }
//    }
//}
