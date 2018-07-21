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

public class myServer1 : MonoBehaviour
{

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

    public void Init()
    {
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
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (!serverStarted)
        {
            return;
        }

        foreach (ServerClient sc in clients)
        {
            // client not connected
            if (!isConnected(sc.tcpSocket))
            {
                sc.tcpSocket.Close(); //close the socket 
                disconnectList.Add(sc);
                continue;
            }

            /// client is connected to the server
            //check for messages from the client, check the stream of every client
            if (sc.stream.DataAvailable)
            {

                StreamReader reader = new StreamReader(sc.stream, true); //reading the data
                string data = reader.ReadLine(); //store data
                //Debug.Log(sc.stream);

                if (data != null)
                {
                    OnIncomingData(sc, data); //process the messages the server gets from the client, from the specific client sc
                }
            }
            else
            {
                //Debug.Log("no stream");
            }


            AcceptConnections();
        }

        //disconnection loop
        for (int i = 0; i < disconnectList.Count - 1; i++)
        {
            //tell our player somebody has disconnected

            clients.Remove(disconnectList[i]);
            disconnectList.RemoveAt(i);
        }
    }



    //* Bind the socket to the local endpoint and listen for incoming connections *//
    public void CreateServer()
    {
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


    //************ start async socket to listen for connections ******************//
    public void AcceptConnections()
    {

        serverSocket.BeginAccept(AcceptCallback, serverSocket);
    }


    //************ async call to finish the connection ******************//
    void AcceptCallback(IAsyncResult ar)
    {
        // Get the socket that handles the client request  
        Socket server = (Socket)ar.AsyncState;


        string allUsers = "";

        foreach (ServerClient c in clients)
        {
            allUsers += c.clientName + "|";
        }


        //finish acceptng the connection
        Socket handler = server.EndAccept(ar);

        //add client to the list of clients. |dictionary key: client value: stake|
        clients.Add(new ServerClient(handler));

        //Debug.Log("S. Someone has connected!!!!");

        //if (clients.Count > 0){
        //    Debug.Log("S. client successfully added to the list of clients");
        //}

        //accept incoming connections again
        AcceptConnections();

        //first message to send to a single client
        BroadcastData("SWHO|", clients[clients.Count - 1]);

    }


    ///***************** Start sending and receiving data *****************///


    //************ send data to the client ******************//
    public void BroadcastData(string data, List<ServerClient> clients)
    {

        //data = "hello from server";

        foreach (ServerClient cl in clients)
        {
            try
            {
                //send data to the client (writing data to the client socket stream)
                StreamWriter writer = new StreamWriter(cl.stream);
                writer.WriteLine(data); //write data
                writer.Flush(); //clear the buffer
                //Debug.Log("S. sent a message to the client");
            }
            catch (Exception ex)
            {
                Debug.Log("Server Error writing data: " + ex.Message);
            }
        }
    }


    //**** overload of the broadcast function that accepts a server client ****//
    //************ and save it into a list of server clients ******************//
    public void BroadcastData(string data, ServerClient c)
    {
        List<ServerClient> client = new List<ServerClient> { c };
        BroadcastData(data, client);
    }


    //************ process the data received from the client ******************//
    public void OnIncomingData(ServerClient client, string data)
    {

        Debug.Log("Server: " + data);

        string[] _data = data.Split('|'); //split by the  '|'

        string cmd = _data[0]; //command to excecute action  

        switch (cmd)
        {
            case "CWHO|":
                client.clientName = _data[1];
                BroadcastData("SCON|" + client.clientName, clients);
                break;

                //default:
                //Debug.Log("S. nothing received");
                //break;
        }
    }


    //************ check if te client is connected to the server *************//
    bool isConnected(Socket c)
    {
        try
        {
            if (c != null && c.Connected)
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

}


//************ definition of the client *************//
public class ServerClient
{

    public Socket tcpSocket; //socket

    public string clientName; //name

    public NetworkStream stream; //stream of the socket


    public ServerClient(Socket clientSocket)
    {
        tcpSocket = clientSocket;

        stream = new NetworkStream(clientSocket); //get the stream of the socket
    }
}





