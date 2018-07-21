using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System;
using System.IO;

public class myClient1 : MonoBehaviour
{

    public string clientName;
    public bool isHost; //host the game?

    bool socketReady;

    //to have access to the stream of the socket
    static NetworkStream stream;
    static StreamWriter streamWriter;
    static StreamReader streamReader;

    //creating the socket TCP
    public static Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

    //list of all the game clients connected
    List<GameClient> players = new List<GameClient>();


    // Use this for initialization
    void Start()
    {
        DontDestroyOnLoad(gameObject); //don't destroy the client when moving on to the next scene
    }

    // Update is called once per frame
    void Update()
    {
        if (socketReady) //if the socket is connected
        {
            //Debug.Log("C.socket ready");
            if (stream.DataAvailable) //check if there is a message 
            {
                string data = streamReader.ReadLine(); //if so read it

                //if there is data
                if (data != null)
                    OnIncomingData(data); //receive the data and prepare it to process it
                
            }
        }
    }


    //************ function to connect to the server ******************//
    public bool ConnectToServer(string hostAdd, int port)
    {
        //if already connected ignore this fucntion 
        if (socketReady)
        {
            return false;
        }

        //connect the socket to the server
        try
        {
            //connect to server
            clientSocket.Connect(hostAdd, port);

            //get the stream of the client socket
            stream = new NetworkStream(clientSocket);
            streamWriter = new StreamWriter(stream);
            streamReader = new StreamReader(stream);

            socketReady = true;

            Debug.Log("Client Socket connected to: " + clientSocket.RemoteEndPoint);

        }
        catch (Exception ex)
        {
            Debug.Log("Client Socket error: " + ex.Message);
        }

        return socketReady;
    }


    ///***************** Start sending and receiving data *****************///


    //************ send data from the client to the server ******************//
    public void SendData(string data)
    {
        if (!socketReady)
            return;

        //grab the stream and write on it
        streamWriter.WriteLine(data); //the streamWriter is on the stream
        streamWriter.Flush(); //clear the buffer
    }



    //************ read the data received from the server ******************//
    void OnIncomingData(string data)
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
                SendData("CWHO|" + clientName + "|" + ((isHost)?1:0).ToString()); //1 is host, 0 is not host
                break;
            case "SCNN":
                UserConnected(data_received[1], false);
                break;


            default:
                break;
        }
    }


    //************ process data received from the server ******************//
    void UserConnected(string name, bool host)
    {
        GameClient gameClient = new GameClient();

        gameClient.name = name;

        players.Add(gameClient);

        //Debug.Log(players.Count);

        //if there are 2 or 3 players connected
        if (players.Count == 3 || players.Count == 4)
        {
            GameController.Instance.StartGame(); //go to the game scene
        }

    }



    //************ closes the socket ******************//
    void OnApplicationQuit()
    {
        CloseSocket();
    }

    void OnDisable()
    {
        CloseSocket();
    }


    void CloseSocket()
    {

        if (!socketReady)
        {
            return;
        }

        streamWriter.Close();
        streamReader.Close();
        clientSocket.Close();
        socketReady = false;
    }
}


//************ definition of game client ******************//
public class GameClient
{
    public string name;
    public bool isHost;
}
    

