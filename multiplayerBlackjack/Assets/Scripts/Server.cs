using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System;
using System.Net;
using System.IO;

public class Server : MonoBehaviour
{

    public int port = 8080;


    private List<ServerClient> clients;
    private List<ServerClient> disconnectList;

    private TcpListener server;
    public bool serverStarted;

    private void Start()
    {
        clients = new List<ServerClient>();
        disconnectList = new List<ServerClient>();
       

        try
        {
            server = new TcpListener(IPAddress.Any, port); //stablishing connection
            server.Start(); //start listening

            StartListening();
            serverStarted = true;

            Debug.Log("Server has been started on port: " + port.ToString());
        } 
        catch(Exception e)
        {
            Debug.Log("socket error: " + e.Message);
        }

    }

    private void Update()
    {
        if (!serverStarted){
            return;
        }

        foreach (ServerClient sc in clients)
        {
            // is the client still connected?
            if (!isConnected(sc.tcp))
            {
                sc.tcp.Close();
                disconnectList.Add(sc);
                continue;
            }
            //check for messages from the client
            else // client is connected to the server
            {
                NetworkStream stream = sc.tcp.GetStream();

                if (stream.DataAvailable)
                {
                    StreamReader reader = new StreamReader(stream, true);
                    string data = reader.ReadLine();

                    if (data != null){
                        OnIncomingData(sc, data); //process the messages the server gets from the client, from the specific client sc
                    }
                }
            }
        }
    }

    private bool isConnected(TcpClient c){

        try
        {
            if (c != null && c.Client != null && c.Client.Connected)
            {
                if (c.Client.Poll(0, SelectMode.SelectRead))
                {
                    return !(c.Client.Receive(new byte[1], SocketFlags.Peek) == 0);
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


    private void StartListening(){

        server.BeginAcceptTcpClient(AcceptTCPClient, server);
    }

    private void AcceptTCPClient(IAsyncResult ar){

        TcpListener listener = (TcpListener)ar.AsyncState;

        clients.Add(new ServerClient(listener.EndAcceptTcpClient(ar), "guest"));

        StartListening();

        //send a message to everyone say someone has connected
        BroadCastMessage(clients[clients.Count - 1].clientName + " has connected", clients);

    }

    //send a message to all the connected clients
    private void BroadCastMessage(string data, List<ServerClient> cl)
    {
        foreach (ServerClient c in cl)
        {
            try
            {
                StreamWriter writer = new StreamWriter(c.tcp.GetStream());
                writer.WriteLine(data);
                writer.Flush();
            }
            catch (Exception ex)
            {
                Debug.Log("write error: " + ex.Message + "to client: " + c.clientName);
            }
        }
    }


    //receive the data from the client and process it
    private void OnIncomingData(ServerClient sc, string data)
    {
        Debug.Log(sc.clientName + "has sent the following message: " + data);
    }
}

//definition of who is connected to the server 
public class ServerClient {

    public TcpClient tcp;

    public string clientName;


    public ServerClient(TcpClient clientSocket, string name){

        clientName = name;
        tcp = clientSocket;
    }
}