using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;  
using System.Net;  
using System.Net.Sockets;  
using System.Text;  
using System.Threading; 

public class myServer : MonoBehaviour {
    
    public int port = 8000;
    string host = "";

    private List<ServerClient> clients;
    private List<ServerClient> disconnectList;

    //creating the socket TCP
    public Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

    //create the buffer size, how uch info we can send and receive 
    private byte[] serverBuffer = new byte[1024];

    public bool serverStarted;

	
    // Use this for initialization
	void Start () {

        try
        {
            //start socket
            CreateSocket();
        }
        catch (Exception ex)
        {
            Debug.Log("error when creating the socket " + ex.Message);
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
                    //byte[] buffer_r = new byte[255];

                    //int rec = serverSocket.Receive(buffer_r, 0, buffer_r.Length, 0);

                    //Array.Resize(ref buffer_r, rec);


                //process data

                //send data back to the client
                    //byte[] buffer = Encoding.Default.GetBytes("hello from the server");

                    //serverSocket.Send(buffer, 0, buffer.Length, 0);


                Debug.Log("Client has connected from " + clients[clients.Count - 1].clientName);
            }
        }


    }

    public void CreateSocket(){

        //bind socket
        serverSocket.Bind(new IPEndPoint(IPAddress.Any, port));
        Debug.Log("socket bound");

        //start listening
        serverSocket.Listen(5);
        Debug.Log("socket listening on port: " + port);

    }


    public void AcceptConnections(){

        serverSocket.BeginAccept(AcceptCallback, serverSocket);
    }

    private void AcceptCallback(IAsyncResult ar)
    {

        // Get the socket that handles the client request.  
        Socket server = (Socket)ar.AsyncState;
        Socket handler = server.EndAccept(ar);  

        // Create the state object.  
        StateObject state = new StateObject();  
        state.workSocket = handler;  
        handler.BeginReceive( state.buffer, 0, StateObject.BufferSize, 0,  
            new AsyncCallback(ReadCallback), state);  


        //ad client to dictionary key: client value: stak
        clients.Add(new ServerClient(handler, "guest"));

        AcceptConnections();

        ////send a message to everyone say someone has connected
        //BroadCastMessage(clients[clients.Count - 1].clientName + " has connected", clients);

    }

    private void ReadCallback(IAsyncResult ar){

        // Retrieve the state object and the handler socket  
        // from the asynchronous state object.  
        StateObject state = (StateObject) ar.AsyncState;  
        Socket handler = state.workSocket;  

        // Read data from the client socket.   
        int bytesRead = handler.EndReceive(ar);  

        //store the data received
        state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, state.buffer.Length));  

        string content = state.sb.ToString();

        Debug.Log("data received in the server: " + content);

        //send data back to client
        Send(handler, content);
    }

    private static void Send(Socket handler, String data) {  
        // Convert the string data to byte data using ASCII encoding.  
        byte[] byteData = Encoding.ASCII.GetBytes(data);  

        // Begin sending the data to the remote device.  
        handler.BeginSend(byteData, 0, byteData.Length, 0,  
            new AsyncCallback(SendCallback), handler);  
    }  

    private static void SendCallback(IAsyncResult ar) {  
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


    // State object for reading client data asynchronously  
    public class StateObject {  
        
        // Client  socket.  
        public Socket workSocket = null;  

        // Size of receive buffer.  
        public const int BufferSize = 1024; 

        // Receive buffer.  
        public byte[] buffer = new byte[BufferSize];

        // Received data string.  
        public StringBuilder sb = new StringBuilder();

    }  
}


