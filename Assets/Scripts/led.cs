//using UnityEngine;
//using System.Collections; // Импортируем пространство имен для работы с коллекциями (например, списками и массивами).
//using System.Collections.Generic; // Импортируем пространство имен для работы с обобщенными коллекциями.
//using System.ComponentModel; // Импортируем пространство имен для работы с компонентами и свойствами.
//using System.Threading; // Импортируем пространство имен для работы с потоками.
//using System.IO.Ports;

//public class ArduinoControl : MonoBehaviour
//{
//    public string comPort = "COM3"; // Укажите COM-порт, к которому подключена Arduino
//    private SerialPort serialPort;

//    void Start()
//    {
//        serialPort = new SerialPort(comPort, 9600);
//        serialPort.Open();
//        serialPort.ReadTimeout = 100;
//        InvokeRepeating("ToggleLight", 0f, 1f); // Мигать каждую секунду
//    }

//    void ToggleLight()
//    {
//        if (serialPort.IsOpen)
//        {
//            serialPort.Write("H"); // Отправить команду "H" включения
//            string response = serialPort.ReadLine(); // Прочитать ответ от Arduino (необязательно)
//            Debug.Log(response); // Вывести ответ в консоль Unity (для отладки)
//        }
//    }

//    void OnApplicationQuit()
//    {
//        if (serialPort != null && serialPort.IsOpen)
//        {
//            serialPort.Write("L"); // Отправить команду "L" выключения перед выходом из приложения
//            serialPort.Close();
//        }
//    }
//}
