using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System;
using System.IO;

public class myClient1 : MonoBehaviour {

    public string clientName;
    bool socketReady;

    //list of all the game clients connected
    List<GameClient> players = new List<GameClient>();


    //to have access to the stream of the socket
    static NetworkStream stream;
    static StreamWriter streamWriter;
    static StreamReader streamReader;

    //creating the socket TCP
    public static Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

    //declare end point
    public IPEndPoint conn;


	// Use this for initialization
	void Start () {

        DontDestroyOnLoad(gameObject); //don't destroy the client when moving on to the next scene
	}
	
	// Update is called once per frame
	void Update () {

        if (socketReady) //if the socket is connected
        {
            //Debug.Log("C.socket ready");
            if (stream.DataAvailable) //check if there is a message 
            {
                string data = streamReader.ReadLine(); //if so read it

                //if there is data
                if (data != null) 
                {
                    OnIncomingData(data); //receive the data and prepare it to process it
                }
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
            //create end point to connect
            conn = new IPEndPoint(IPAddress.Parse(hostAdd), port);

            //connect to server
            clientSocket.BeginConnect(conn, ConnectCallback, clientSocket);

            socketReady = true;
           // Debug.Log("Client socket ready: " + socketReady);

        }
        catch (Exception ex)
        {
            Debug.Log("Client Socket error: " + ex.Message);
        }

        return socketReady;
    }


    //************ async call to finish the connection ******************//
    static void ConnectCallback(IAsyncResult ar)
    {
        try
        {
            // Retrieve the socket 
            Socket client = (Socket)ar.AsyncState;

            // Complete the connection  
            client.EndConnect(ar);


            //get the stream of the client socket
            stream = new NetworkStream(clientSocket);
            streamWriter = new StreamWriter(stream);
            streamReader = new StreamReader(stream);


            //Debug.Log("Client successfully connected!!!!!");
            Debug.Log("Client Socket connected to: " + client.RemoteEndPoint);

        }
        catch (Exception e)
        {
            Debug.Log("Client Error connecting: " + e);
        }
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

        string[] _data = data.Split('|'); //split by the  '|'

        string cmd = _data[0]; //command to excecute action  

        switch (cmd)
        {
            case "SWHO":
                for (int i = 1; i < _data.Length - 1; i++)
                {
                    UserConnected(_data[i], false); //since we are receiving from S(server)
                }
                SendData("CWHO|" + clientName);
                break;

            case "SCON":
                UserConnected(_data[1], false); //for now TODO find a way of idetifiying the host
                break;


            default:
                Debug.Log("C. nothing has been received");
                break;
        }

    }



    //************ process data received from the server ******************//
    void UserConnected(string Name, bool host)
    {
        GameClient gameClient = new GameClient();

        gameClient.name = Name;

        players.Add(gameClient);
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

        clientSocket.Close();
        socketReady = false;
    }


    //************ definition of game client ******************//
    public class GameClient
    {
        public string name;
        public bool isHost;
    }
        
}
