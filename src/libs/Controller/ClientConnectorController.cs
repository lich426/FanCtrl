using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.Security.Cryptography;
using static System.Windows.Forms.AxHost;

namespace FanCtrl
{
    public class ClientConnectorController
    {
        private bool mIsStart = false;
        private Socket mSocket;
        private string mIPString;
        private int mPort;

        private const int MAX_BUFFER_SIZE = 4096;
        private byte[] mRecvBuffer = new byte[MAX_BUFFER_SIZE];

        public delegate void OnRecvHandler(ClientConnectorController connector, byte[] recvArray, int recvDataSize);
        public event OnRecvHandler onRecvHandler;

        public delegate void OnDisconnectHandler(ClientConnectorController connector);
        public event OnDisconnectHandler onDisconnectHandler;

        public string IPString => mIPString;
        public int Port => mPort;

        public ClientConnectorController(Socket sock)
        {
            mSocket = sock;
            var endPoint = mSocket.RemoteEndPoint as IPEndPoint;
            mIPString = endPoint.Address.ToString();
            mPort = endPoint.Port;
        }

        public void start()
        {
            if (mIsStart == true)
            {
                Console.WriteLine("ConnectorController.start() : Already start");
                return;
            }
            mIsStart = true;
            this.recv();
        }

        public void stop()
        {
            try
            {
                if (mIsStart == false)
                {
                    Console.WriteLine("ConnectorController.stop() : Already stop");
                    return;
                }

                mIsStart = false;
                mSocket.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("ConnectorController.stop() : {0}", e.Message);
            }
        }

        private void disconnect()
        {
            try
            {
                if (mIsStart == false)
                {
                    Console.WriteLine("ConnectorController.disconnect() : Already stop");
                    return;
                }

                mIsStart = false;
                mSocket.Close();
                mSocket = null;
                onDisconnectHandler?.Invoke(this);
            }
            catch (Exception e)
            {
                Console.WriteLine("ConnectorController.disconnect() : {0}", e.Message);
            }           
        }

        private void recv()
        {
            try
            {
                mSocket.BeginReceive(mRecvBuffer, 0, MAX_BUFFER_SIZE, SocketFlags.None, new AsyncCallback(handleRecv), mSocket);
            }
            catch(Exception e)
            {
                Console.WriteLine("ConnectorController.recv() : {0}", e.Message);
                this.disconnect();
            }
        }

        private void handleRecv(IAsyncResult ar)
        {
            try
            {
                var socket = ar.AsyncState as Socket;
                int recvBytes = socket.EndReceive(ar);
                if (recvBytes > 0)
                {
                    onRecvHandler?.Invoke(this, mRecvBuffer, recvBytes);
                }
                this.recv();
            }
            catch (Exception e)
            {
                Console.WriteLine("ConnectorController.handleRecv() : {0}", e.Message);
                this.disconnect();
            }
        }

        public void send(byte[] buffer, int sendBytes)
        {
            try
            {
                var tempBuffer = new byte[sendBytes];
                Array.Copy(buffer, tempBuffer, sendBytes);
                mSocket.BeginSend(tempBuffer, 0, sendBytes, SocketFlags.None, new AsyncCallback(handleSend), mSocket);
            }
            catch (Exception e)
            {
                Console.WriteLine("ConnectorController.send() : {0}", e.Message);
            }
        }
        
        private void handleSend(IAsyncResult ar)
        {
            try
            {
                var socket = ar.AsyncState as Socket;
                socket.EndSend(ar);
            }
            catch (Exception e)
            {
                Console.WriteLine("ConnectorController.handleSend() : {0}", e.Message);
            }
        }
    }
}
