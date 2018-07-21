using UnityEngine;
using System.Net.Sockets;
using System.IO;
using System;
using System.Collections.Generic;

public class Client : MonoBehaviour {

    public string clientName;

    private bool socketReady;
    private TcpClient socket;
    private NetworkStream stream;
    private StreamWriter writer;
    private StreamReader reader;

    List<GameClient> players = new List<GameClient>();


    public bool ConnectToServer(string host, int port)
    {
        //if already connected ignore this fucntion 
        if(socketReady){
            return false;
        }

        Debug.Log("client connected");

        //create the socket to connect to the server
        try
        {
            socket = new TcpClient(host, port);
            stream = socket.GetStream();
            writer = new StreamWriter(stream);
            reader = new StreamReader(stream);
            socketReady = true;

            Debug.Log("client " + host + " connected to port: " + port.ToString());
        }
        catch (Exception ex)
        {
            Debug.Log("socket error: " + ex.Message);
        }

        return socketReady;
    }

	// Update is called once per frame
	void Update () 
    {
        if (socketReady)
        {
            if (stream.DataAvailable)
            {
                string data = reader.ReadLine();

                if (data != null)
                    OnIncomingData(data);
            }
        }
	}

    //read messages from the server
    private void OnIncomingData(string data)
    {
        Debug.Log("Client: " + data);

        string[] data_received = data.Split('|');

        string command = data_received[0];

        switch (command)
        {
            case "SWHO":
                for (int i = 1; i < data_received.Length; i++)
                {
                    UserConnected(data_received[i], false); //not a host received from server
                }
                Send("CWHO|" + clientName);
                break;
            case "SCNN":
                UserConnected(data_received[1], false);
                break;


            default:
                break;
        }

    }

    void UserConnected(string name, bool host)
    {
        GameClient c = new GameClient();
        c.name = name;

        players.Add(c);
    }

    //send messages to the server
    public void Send(string data)
    {
        if (!socketReady)
            return;

        writer.WriteLine(data);
        writer.Flush();
    }


    private void OnApplicationQuit()
    {
        CloseSocket();
    }

    private void OnDisable()
    {
        CloseSocket();
    }


    void CloseSocket(){

        if (!socketReady)
            return;

        writer.Close();
        reader.Close();
        socket.Close();
        socketReady = false;
    }



}

public class GameClient
{
    public string name;
    public bool isHost;
}
