using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.IO;
using System.Net;
using System;


public class Client : MonoBehaviour {

    private bool socketReady;
    private TcpClient socket;
    private NetworkStream stream;
    private StreamWriter streamWriter;
    private StreamReader streamReader;

    private Server server;

    public void ConnectToServer()
    {
        //if already connected ignore this fucntion 
        if(socketReady){
            return;
        }

        //defaults host and port
        int port = 8080;
        string host = "";

        Debug.Log("client connected");

        //create the socket to connect to the server
        try
        {
            socket = new TcpClient(host, port);
            stream = socket.GetStream();
            streamWriter = new StreamWriter(stream);
            streamReader = new StreamReader(stream);
            socketReady = true;
            Debug.Log("client " + host + " connected to port: " + port.ToString());
        }
        catch (Exception ex)
        {
            Debug.Log("socket error: " + ex.Message);
        }
    }

	// Update is called once per frame
	void Update () 
    {
        if (socketReady)
        {
            if (stream.DataAvailable)
            {
                string data = streamReader.ReadLine();
                if (data != null)
                {
                    OnIncomingData(data);
                }
            }
        }
	}

    private void OnIncomingData(string data)
    {
        Debug.Log("server answer: " + data);
    }
}
