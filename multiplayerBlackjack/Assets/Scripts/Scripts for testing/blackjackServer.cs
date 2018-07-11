using System.Collections;
using System.Collections.Generic;
//using UnityEngine;
using System.Net.Sockets;
using System.Net;
using System;
using System.Text;



namespace CSharpServer
{

    public class blackjackServer
    {
        int port = 8080;
        const int MAX_PLAYERS = 5;

        //creating the socket
        private static Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        //create the buffer size, how uch info we can send and receive 
        private static byte[] serverBuffer = new byte[1024];

        //create array of clients 
        public static Client[] clients = new Client[MAX_PLAYERS]; 


        private void SetupServer()
        {

            //bind the socket to an ip addres an a port
            serverSocket.Bind(new IPEndPoint(IPAddress.Any, port));

            //start listening for connections availables
            serverSocket.Listen(10);

            //accept new connections 
            serverSocket.BeginAccept(new AsyncCallback(AcceptCallBack), null);

        }

        //whenever we have a pending connection this fucntion gets called
        private static void AcceptCallBack(IAsyncResult ar)
        {

            Socket socket = serverSocket.EndAccept(ar); // to let the client connect to the server

            //once the client is connected the socket is open for others connections 
            serverSocket.BeginAccept(new AsyncCallback(AcceptCallBack), null);

            //for every client 
            for (int i = 0; i < MAX_PLAYERS; i++)
            {
                //check if the slot is open
                if (clients[i].socket == null)
                {
                    clients[i].socket = socket;
                    clients[i].index = i;
                    clients[i].ip = socket.RemoteEndPoint.ToString();
                    clients[i].StartClient(); //start receiving data from server
                    Console.WriteLine("Connection from '{0}' received.", clients[i].ip);
                    return; //so others clients can connect
                }
            }
        }

        //handles packet loss
        public static void SendDataTo(int index, byte[] data)
        {
            byte[] sizeInfo = new byte[4];
            sizeInfo[0] = (byte)data.Length;
            sizeInfo[1] = (byte)(data.Length >> 8);
            sizeInfo[2] = (byte)(data.Length >> 16);
            sizeInfo[3] = (byte)(data.Length >> 24);

            clients[index].socket.Send(sizeInfo); //first send the size of the packet
            clients[index].socket.Send(data); //then send the packet itself 
        }

        public static void SendConnectionOK()
        {
            byte[] bufferinfo = Encoding.Default.GetBytes("hello client!");
            serverSocket.Send(bufferinfo, 0, bufferinfo.Length, 0);

            bufferinfo = new byte[255];

            int rec = serverSocket.Receive(bufferinfo, 0, bufferinfo.Length, 0);

            Array.Resize(ref bufferinfo, rec);

            Console.WriteLine("received: {0}", Encoding.Default.GetString(bufferinfo));

            Console.Read();
        }

    }

    public class Client{

        public int index;
        public string ip;
        public bool closing = false;
        private byte[] clientBuffer = new byte[1024];

        //create a socket
        public Socket socket;

        public void StartClient()
        {
            socket.BeginReceive(clientBuffer, 0, clientBuffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallBack), socket);
        }


        private void ReceiveCallBack(IAsyncResult ar)
        {
            Socket socket = (Socket)ar.AsyncState;

            try
            {
                //gets the byte lenght of the data tha we are sending
                int received = socket.EndReceive(ar);

                if (received <= 0)
                {
                    CloseClient(index);
                }
                else 
                {
                    byte[] dataBuffer = new byte[received];
                Array.Copy(clientBuffer, dataBuffer, received);

                    //TODO handle network information

                    //start receiving information again
                    socket.BeginReceive(clientBuffer, 0, clientBuffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallBack), socket);
                }
            } 
            catch 
            {
                CloseClient(index);
            }
        }

        private void CloseClient(int index)
        {
            closing = true;
            Console.WriteLine("Connection from {0} has been terminated.", ip);
            //create a fucntion PlayerLeft to let know all the others players that the player have left the game 
            socket.Close();
        }

    }




}
