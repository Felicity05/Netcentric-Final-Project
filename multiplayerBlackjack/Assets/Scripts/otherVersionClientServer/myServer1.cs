using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;  
using System.Net;  
using System.Net.Sockets;  
using System.Text;  
using System.Threading;
using System.IO;

public class myServer1 : MonoBehaviour {
    
    public int port = 8000;
    string host = "";

    static List<ServerClient> clients;
    static List<ServerClient> disconnectList;

    //creating the socket TCP
    public Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

    //create the buffer size, how much info we can send and receive 
    byte[] serverBuffer = new byte[1024];

    bool serverStarted;

    public string content;

    /*instead of creating the server in Start I need to created in aother function, 
    because the server gets called when the player cliks the button "host game" 
    and then wait for more players to join, and we need to change scenes so 
    we don't want to destroy the server after loading the new scene
    */

    public void Init(){
        DontDestroyOnLoad(gameObject); //don't destroy the server once the new scene is loaded

        //instaciating the lists
        clients = new List<ServerClient>();
        disconnectList = new List<ServerClient>();

        try
        {
            //call the function create server here
            CreateServer();
        }
        catch (Exception ex)
        {
            Debug.Log("Error when creating the server: " + ex.Message);
            //Show dialog error to the player
        }
    }

	
    // Use this for initialization
	void Start () {
            
	}
	
	// Update is called once per frame
	void Update () {
		
        if (!serverStarted)
        {
            return;
        }

        foreach (ServerClient sc in clients)
        {
            // is the client still connected?
            if (!isConnected(sc.tcpSocket))
            {
                sc.tcpSocket.Close(); //close the socket 
                disconnectList.Add(sc);
                continue;
            }
            //check for messages from the client, check the stream of every client
            else // client is connected to the server
            {
                //NetworkStream stream = new NetworkStream(sc.tcpSocket);

                //if (stream.DataAvailable){

                    //StreamReader reader = new StreamReader(stream, true); //reading the data
                    //string data = reader.ReadLine(); //store data

                //if there is data
               


                //}


                AcceptConnections();


            }
        }

        //disconnection loop
        for (int i = 0; i < disconnectList.Count - 1; i++)
        {
            //tell our player somebody has disconnected

            clients.Remove(disconnectList[i]);
            disconnectList.RemoveAt(i);
        }
    }

    
    // Bind the socket to the local endpoint and listen for incoming connections.  
    public void CreateServer(){
        try
        {
            Debug.Log("Setting up the server...");
            
            //bind socket
            serverSocket.Bind(new IPEndPoint(IPAddress.Any, port));
            Debug.Log("Server socket bound");

            //start listening
            serverSocket.Listen(3); //only 3 connections at a time
            Debug.Log("Server socket listening on port: " + port);

            //accept connections
            AcceptConnections();

            serverStarted = true;
        }
        catch (Exception e)
        {

            Debug.Log("Server Error when binding to port and listening: " + e.Message);
        }
       
    }

    //start async socket to listen for connections
    public void AcceptConnections(){

        serverSocket.BeginAccept(AcceptCallback, serverSocket);
    }

    //async socket/********************************************************
    void AcceptCallback(IAsyncResult ar)
    {
        // Get the socket that handles the client request  
        Socket server = (Socket)ar.AsyncState;
        Socket handler = server.EndAccept(ar);

        //add client to dictionary key: client value: stake
        clients.Add(new ServerClient(handler));

        //accept incoming connections again
        AcceptConnections();

        Debug.Log("S. Someone has connected!!!!");

        if (clients.Count > 0){
            Debug.Log("S. client successfully added to the list of clients");
        }

        //send a message to everyone say someone has connected
        //BroadCastData("hello from server", clients);  
    }


    /////////CHECK IF THERE IS DATA TO BE RECEIVED/////////

    public void receiveData(ServerClient socket){

        //begin receiving data from the client

        
    }

    /////////PROCESS DATA RECEIVED/////////

    //public void OnIncommingData(ServerClient client, string data){

    //    Debug.Log("client has send: " + data);
        
    //}

    /////////SEND DATA PROCESSED BACK TO THE CLIENT/////////

    public void BroadCastData(List<ServerClient> clients){

        string data = "hello from server";

        foreach (var cl in clients)
        {
            try
            {
                //send data back to client
                Send(cl.tcpSocket, data); //"hello from server"
                Debug.Log("S. sent a message to the client");
            }
            catch (Exception ex)
            {
                Debug.Log("Server Error writing data: " + ex.Message);
            }
        }

    }

    static void Send(Socket handler, String data)
    {

        // Convert the string data to byte data using ASCII encoding  
        byte[] byteData = Encoding.ASCII.GetBytes(data);

        // Begin sending the data to the remote device  

    }



    /////////check if te client is connected to the server/////////

    bool isConnected(Socket c)
    {
        try
        {
            if (c != null && c != null && c.Connected)
            {
                if (c.Poll(0, SelectMode.SelectRead))
                {
                    return !(c.Receive(new byte[1], SocketFlags.Peek) == 0);
                }
                return true;
            }
            return false;
        }
        catch
        {
            return false;
        }
    }


    /////////definition of the client/////////

    public class ServerClient
    {

        public Socket tcpSocket;

        public string clientName;


        public ServerClient(Socket clientSocket)
        {
            tcpSocket = clientSocket;
        }
    }


}


