using System;
using System.IO.Ports;
using System.Text;
using UnityEngine;

public class SerialInput : MonoBehaviour
{
    public string PortName;
    public int BaudRate = 9600;
    private SerialPort m_SerialConnection;

    void Start()
    {
        ConnectToPort();
    }

    public void ConnectToPort()
    {
        try
        {
            m_SerialConnection = new SerialPort(PortName, BaudRate)
            {
                Encoding = System.Text.Encoding.UTF8,
                DtrEnable = true
            };

            m_SerialConnection.Open();
            Debug.Log($"Connected to {PortName}");
        }
        catch (Exception e)
        {
            Debug.LogWarning(e.Message);
        }
    }

    public void Disconnect()
    {
        if (m_SerialConnection != null)
        {
            if (m_SerialConnection.IsOpen)
            {
                m_SerialConnection.Close();
            }

            m_SerialConnection.Dispose();
            m_SerialConnection = null;
            Debug.Log("Disconnected");
        }
    }

    private void OnDestroy()
    {
        Disconnect();
    }

    private void Update()
    {
        try
        {
            if (m_SerialConnection != null && m_SerialConnection.IsOpen)
            {
                int bytesToRead = m_SerialConnection.BytesToRead;
                if (bytesToRead > 0)
                {
                    byte[] buff = new byte[bytesToRead];
                    int read = m_SerialConnection.Read(buff, 0, bytesToRead);
                    if (read > 0)
                    {
                        PrintBytes(ref buff);
                    }
                }
            }
        }
        catch (Exception e)
        {
            Disconnect();
            Debug.LogWarning(e.Message);
        }
    }

    private void PrintBytes(ref byte[] bytes)
    {
        StringBuilder sb = new StringBuilder();
        foreach (byte b in bytes)
        {
            sb.Append($"{b} ");
        }
        Debug.Log(sb.ToString());
        //int result = bytes[4] | (bytes[5] << 8) | (bytes[6] << 16) | (bytes[7] << 24);
        //Debug.Log(result);
    }
}
