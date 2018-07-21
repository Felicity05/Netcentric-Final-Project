using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.IO;

public class myServer1 : MonoBehaviour
{

    public int port = 8000;

    static List<ServerClient> clients;
    static List<ServerClient> disconnectList;

    //creating the socket TCP
    Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

    bool serverStarted;

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
            if (!isConnected(sc))
            {
                sc.tcpSocket.Close(); //close the socket 
                disconnectList.Add(sc);
                continue;
            }

            /// client is connected to the server
            //check for messages from the client, check the stream of every client
            NetworkStream s = new NetworkStream(sc.tcpSocket);

            if (s.DataAvailable)
            {

                StreamReader reader = new StreamReader(s, true); //reading the data
                string data = reader.ReadLine(); //store data
                //Debug.Log(sc.stream);

                if (data != null)
                {
                    OnIncomingData(sc, data); //process the messages the server gets from the client, from the specific client sc
                }
            }
        }

        //disconnection loop
        for (int i = 0; i < disconnectList.Count; i++)
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
            StartListeing(); //start listening for connections

            serverStarted = true;

        }
        catch (Exception e)
        {

            Debug.Log("Server Error when binding to port and listening: " + e.Message);
        }

    }


    //************ start async socket to listen for connections ******************//
    public void StartListeing()
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

        ServerClient handler = new ServerClient(server.EndAccept(ar));

        //add client to the list of clients. |dictionary key: client value: stake|
        clients.Add(handler);

        //accept incoming connections again
        StartListeing();

        Debug.Log("S. Someone has connected!!!!");

        //first message to send to a single client
        BroadcastData("SWHO|" + allUsers, clients[clients.Count - 1]);

    }


    ///***************** Start sending and receiving data *****************///


    //************ send data to the client ******************//
    void BroadcastData(string data, List<ServerClient> cl)
    {

        //data = "hello from server";

        foreach (ServerClient c in cl)
        {
            try
            {
               //get the stream of the socket
                NetworkStream stream = new NetworkStream(c.tcpSocket);

                //send data to the client (writing data to the client socket stream
                StreamWriter writer = new StreamWriter(stream);
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
    public void BroadcastData(string data, ServerClient cl)
    {
        List<ServerClient> client = new List<ServerClient> { cl };
        BroadcastData(data, client);
    }


    //************ process the data received from the client ******************//
    public void OnIncomingData(ServerClient c, string data)
    {

        Debug.Log("Server: " + data);

        string[] data_received = data.Split('|');

        string command = data_received[0];  //command to execute action

        switch (command)
        {
            case "CWHO":
                c.clientName = data_received[1];
                c.isHost = (data_received[2] != "0");  //is host is true (data_received[2] == "0") ? false : true
                BroadcastData("SCNN|" + c.clientName, clients);
                break;


            default:
                break;
        }
    }


    //************ check if te client is connected to the server *************//
    bool isConnected(ServerClient c) // in any case review this and chnage it to socket
    {
        try
        {
            if (c != null && c.tcpSocket != null && c.tcpSocket.Connected)
            {
                if (c.tcpSocket.Poll(0, SelectMode.SelectRead))
                {
                    return !(c.tcpSocket.Receive(new byte[1], SocketFlags.Peek) == 0);
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

    public bool isHost; //host the game?

    public ServerClient(Socket clientSocket)
    {
        tcpSocket = clientSocket;
    }
}





