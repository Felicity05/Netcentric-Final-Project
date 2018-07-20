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
    private bool socketReady;
    public static string response;
    private static byte[] clientBuffer = new byte[1024];

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

        if (socketReady)
        {
            //Debug.Log("C.socket ready");
        }
		
	}

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

            //// Receive data from the remote device  
            //ReceiveData(clientSocket);
            //Debug.Log("C.receiving data from server...");


            //// Send test data to the remote device.  
            //SendData(clientSocket, "This is a test");
            //Debug.Log("C.message sent to server");

            socketReady = true;
           // Debug.Log("Client socket ready: " + socketReady);

        }
        catch (Exception ex)
        {
            Debug.Log("Client Socket error: " + ex.Message);
        }

        return socketReady;
    }


    //async call to connect
    static void ConnectCallback(IAsyncResult ar)
    {
        try
        {
            // Retrieve the socket 
            Socket client = (Socket)ar.AsyncState;

            // Complete the connection  
            client.EndConnect(ar);

            //Debug.Log("Client successfully connected!!!!!");
            Debug.Log("Client Socket connected to: " + client.RemoteEndPoint);

        }
        catch (Exception e)
        {
            Debug.Log("Client Error connecting: " + e);
        }
    }


    /////////SEND DATA TO THE SERVER/////////
    //send data to server
    public static void SendData(Socket client, string data)
    {
        //convert the string data to bytes
        byte[] byteData = Encoding.ASCII.GetBytes(data);

        //send the data
    }




    //process the data received
    //void OnIncomingData(string data)
    //{
    //    Debug.Log("server answer: " + data);
    //}



    /////////RECEIVE DATA FROM THE SERVER/////////
    //receive data from server
    public static void ReceiveData(Socket client){
        
        try
        {
            // Begin receiving the data from the remote device.  
            int bytesRead = 0;

            //don't know why after receiving my info this gets called. 
            if (bytesRead <= 0)
            {
                Debug.Log("C.no more data to receive");
                return;
            }

            var data = new byte[bytesRead];
            Array.Copy(clientBuffer, data, bytesRead);
        }
        catch (Exception e)
        {
            Debug.Log("Client Error receiving the data: " + e.Message);
        }

    }



    /////////CLOSES THE SOCKET/////////
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



    public class GameClient
    {
        public string name;
        public bool isHost;
    }
        
}
