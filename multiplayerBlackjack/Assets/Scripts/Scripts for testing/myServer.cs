using System;
using System.Net;
using System.Text;
using System.Net.Sockets;
using System.Collections.Generic;
using UnityEngine;

public class myServer : MonoBehaviour {

    //useful sockets list
    private static List<ServerClient> clients;
    private static List<ServerClient> disconnectList;

    //connection info
    public static int port = 8000;
    private static string host = "";
    private bool serverStarted;

    //create the buffer size, how much info we can send and receive 
    private static byte[] serverBuffer = new byte[BUFFER_SIZE];
    private const int BUFFER_SIZE = 1024;

    
    //creating the socket TCP
    public static Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

	
    // Use this for initialization
	void Start () {

        try
        {
            //start server
            CreateServer();
        }
        catch (Exception ex)
        {
            Debug.Log("Error when creating the server " + ex.Message);
        }

		
	}
	
	// Update is called once per frame
	void Update () {

        if (!serverStarted)
        {
            return;
        }

        foreach (ServerClient sc in clients)
        {

            Debug.Log(isConnected(serverSocket));
            
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
                //receive data from client
                
                //process data

                //send data back to the client

                Debug.Log("Client has connected from " + clients[clients.Count - 1].clientName);
            }
        }


    }
    
    // Bind the socket to the local endpoint and listen for incoming connections.  
    public static void CreateServer(){
        try
        {
            Debug.Log("Setting up the server...");
            
            //bind socket
            serverSocket.Bind(new IPEndPoint(IPAddress.Any, port));
            Debug.Log("socket bound");

            //start listening
            serverSocket.Listen(5);
            Debug.Log("socket listening on port: " + port);

            //accept connections
            AcceptConnections();
        }
        catch (Exception e)
        {
            Debug.Log("Error when binding to port and listening: " + e.Message);
        }
    }

    //async socket start listening for connections
    public static void AcceptConnections(){

        serverSocket.BeginAccept(AcceptCallback, serverSocket);
    }

    //async socket accept 
    private static void AcceptCallback(IAsyncResult ar)
    {
        // Get the socket that handles the client request  
        Socket socket;
        socket = serverSocket.EndAccept(ar);
        
        //add client to dictionary key: client value: stake
        clients.Add(new ServerClient(socket, "guest"));

        //begin receive data from client
        socket.BeginReceive(serverBuffer, 0, serverBuffer.Length, 0,  
            ReceiveCallback, socket);

        Debug.Log("Client: " + clients[clients.Count - 1].clientName + " has connected");

        AcceptConnections(); //to be able to accept multiple connections

        ////send a message to everyone say someone has connected
        //BroadCastMessage(clients[clients.Count - 1].clientName + " has connected", clients);
    }

    private static void ReceiveCallback(IAsyncResult ar){

        //retrives the socket to handle the received data
        Socket handler = (Socket) ar.AsyncState;  

        // Read data from the client socket.   
        int bytesReceived = handler.EndReceive(ar);
        bytesReceived = handler.EndReceive(ar);

        //resize the amount of bythes received
        Array.Resize(ref serverBuffer, bytesReceived);

        //store the data received
        string content = Encoding.ASCII.GetString(serverBuffer);

        Debug.Log("data received in the server: " + content);

        //send data back to client
        SendToClient(handler, content);

    }

    private static void SendToClient(Socket handler, String data) {  
        // Convert the string data to byte data using ASCII encoding.  
        byte[] byteData = Encoding.ASCII.GetBytes(data);

        // Begin sending the data to the remote device.  
        handler.Send(byteData);
        
        //handler.BeginSend(byteData, 0, byteData.Length, 0,  
        //    SendCallBack, handler);

        handler.BeginReceive(serverBuffer, 0, BUFFER_SIZE, 0, ReceiveCallback, handler);

        Debug.Log("sent to client: " + Encoding.ASCII.GetString(byteData));
    }  

    private static void SendCallBack(IAsyncResult ar) {  
        try {  
            // Retrieve the socket from the state object.  
            Socket handler = (Socket) ar.AsyncState;  

            // Complete sending the data to the remote device.  
            int bytesSent = handler.EndSend(ar);

            Debug.Log("bytes sent to the client: " + bytesSent);

        } catch (Exception e) {  
            Console.WriteLine(e.ToString());  
        }  
    }  

    private bool isConnected(Socket c)
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

    //definition of the client
    public class ServerClient
    {

        public Socket tcp;

        public string clientName;


        public ServerClient(Socket clientSocket, string name)
        {
            clientName = name;
            tcp = clientSocket;
        }
    }

}


