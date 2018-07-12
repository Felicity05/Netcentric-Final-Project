using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System;
using System.IO;

public class myClient : MonoBehaviour {

    private bool socketReady;
    public static string response;
    private static byte[] clientBuffer = new byte[1024];

    //creating the socket TCP
    public Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

    //declare end point
    public IPEndPoint conn;


	// Use this for initialization
	void Start () {
       
	}
	
	// Update is called once per frame
	void Update () {

        if (socketReady)
        {
            Debug.Log("socket ready");

        }
		
	}

    public void ConnectToServer()
    {
        //if already connected ignore this fucntion 
        if (socketReady)
        {
            return;
        }

        //defaults host and port
        int port = 8000;
        string host = "";


        //connect the socket to the server
        try
        {
            //create end point to connect
            conn = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
            //connect to server
            clientSocket.BeginConnect(conn, ConnectCallback, clientSocket);
            socketReady = true;
            Debug.Log("Client socket ready: "+ socketReady);

            // Send test data to the remote device.  
            SendData(clientSocket, "This is a test");


            // Receive the response from the remote device.  
            ReceiveData(clientSocket);


            // Write the response to the console

        }
        catch (Exception ex)
        {
            Debug.Log("socket error: " + ex.Message);
        }
    }

    
    //async call to connect
    private static void ConnectCallback(IAsyncResult ar) {  
        try {  
            
            // Retrieve the socket from the state object  
            Socket client = (Socket) ar.AsyncState;  

            // Complete the connection  
            client.EndConnect(ar);  

            Debug.Log("Socket connected to: " + client.RemoteEndPoint.ToString());
            
        } catch (Exception e) {  
            Debug.Log("Error connecting: "+ e);  
        }  
    }


    //enclose these in one function
    //send data to server
    public static void SendData(Socket client, string data)
    {
        //convert the string data to bytes
        byte[] byteData = Encoding.ASCII.GetBytes(data);

        // Begin sending the data to the remote device.  
        client.BeginSend(byteData, 0, byteData.Length, 0,
            new AsyncCallback(SendCallBack), client);
    }

    private static void SendCallBack(IAsyncResult ar)
    {
        try
        {
            Socket client = (Socket)ar.AsyncState;

            //send date to the server
            int bytesSent = client.EndSend(ar);

            Debug.Log("client sent: " + bytesSent);
        }
        catch (Exception e)
        {
            Debug.Log("error sending message: " + e);
        }
    }
    //enclose this in one function 



    //receive data from server
    public static void ReceiveData(Socket client){
        
        try
        {
            // Begin receiving the data from the remote device.  
            client.BeginReceive(clientBuffer, 0, clientBuffer.Length, 0,
                                new AsyncCallback(ReceiveCallback), client);
        }
        catch (Exception e)
        {
            Debug.Log("error receiving the data: " + e.Message);
        }

    }

    private static void ReceiveCallback(IAsyncResult ar)
    {
        try
        {
            // Read data from the remote device.  
            Socket client = (Socket) ar.AsyncState;
            int bytesRead = client.EndReceive(ar);

            //don't know why after receiving my info this gets called. 
            if (bytesRead == 0){
                Debug.Log("no data received");
                return;
            }

            var data = new byte[bytesRead];
            Array.Copy(clientBuffer, data, bytesRead);

           // Get the data  
            client.BeginReceive(clientBuffer, 0, clientBuffer.Length, 0,
                                new AsyncCallback(ReceiveCallback), client);

            response = Encoding.Default.GetString(clientBuffer);

            Debug.Log("data from server received in the client: " + response);
             
        }
        catch (Exception e)
        {
            Debug.Log("error: "+e);
        }
    }


    //process the data received
    private void OnIncomingData(string data)
    {
        Debug.Log("server answer: " + data);
    }


}
