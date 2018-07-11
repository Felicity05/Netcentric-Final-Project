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

    private NetworkStream stream;
    private StreamWriter streamWriter;
    private StreamReader streamReader;
    public IAsyncResult result;

    //creating the socket TCP
    public Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

    public IPEndPoint conn;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (socketReady)
        {
            Debug.Log("socket ready");

            //if (stream.DataAvailable)
            //{
                //string data = streamReader.ReadLine();
                //if (data != null)
                //{
                    //OnIncomingData(data);
            //    }
            //}
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
            conn = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port);
            //connect to server
            clientSocket.BeginConnect(conn, ConnectCallback, clientSocket);
            //send data to server
            socketReady = true;
            Debug.Log("Client socket ready: "+ socketReady);
            //Debug.Log("client " + host + " connected to port: " + port.ToString());

            //send message to server
            //SendMessage("helooooooooo I'm the client");

            //ReceiveMessage();

        }
        catch (Exception ex)
        {
            Debug.Log("socket error: " + ex.Message);
        }
    }

    private void OnIncomingData(string data)
    {
        Debug.Log("server answer: " + data);
    }

    private static void ConnectCallback(IAsyncResult ar) {  
        try {  
            // Retrieve the socket from the state object.  
            Socket client = (Socket) ar.AsyncState;  

            // Complete the connection.  
            client.EndConnect(ar);  

            Debug.Log("Socket connected to: " + client.RemoteEndPoint.ToString());
            
        } catch (Exception e) {  
            Debug.Log("Error connecting: "+ e);  
        }  
    } 

    public void SendMessages(string msg)
    {
        byte[] msgBuffer = Encoding.Default.GetBytes(msg);

        clientSocket.Send(msgBuffer, 0, msgBuffer.Length, 0);
    }

    public void ReceiveMessage(){

        byte[] buffer = new byte[255];

        int rec = clientSocket.Receive(buffer, 0, buffer.Length, 0);

        Array.Resize(ref buffer, rec);

        Debug.Log("server answer: " + Encoding.Default.GetString(buffer));
    }
}
