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

namespace FanCtrl
{
    public class ServerController
    {
        private object mLock = new object();
        private int mPort = 0;
        private bool mIsStart = false;
        private Socket mSocket;

        private Dictionary<string, ClientConnectorController> mConnectorDictionary = new Dictionary<string, ClientConnectorController>();

        // onConnect callback
        public delegate void OnConnectHandler(ClientConnectorController controller);
        public event OnConnectHandler onConnectHandler;

        // onDisconnect callback
        public delegate void OnDisconnectHandler(ClientConnectorController controller);
        public event OnDisconnectHandler onDisconnectHandler;

        // onRecv callback
        public delegate void OnRecvHandler(ClientConnectorController controller, byte[] recvArray, int recvDataSize);
        public event OnRecvHandler onRecvHandler;

        public bool IsStart
        {
            get 
            {
                Monitor.Enter(mLock);
                var isStart = mIsStart;
                Monitor.Exit(mLock);
                return isStart;
            }
        }

        public bool start(int port)
        {
            Monitor.Enter(mLock);
            if (mIsStart == true)
            {
                Console.WriteLine("ServerController.start() : Already start");
                Monitor.Exit(mLock);
                return true;
            }

            mIsStart = true;
            mPort = port;

            try
            {
                Console.WriteLine("ServerController.start() : Port({0})", port);
                mSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                var ep = new IPEndPoint(IPAddress.Any, mPort);
                mSocket.Bind(ep);
                mSocket.Listen(100);
                this.accept(false);
            }
            catch (Exception ex)
            {
                Console.WriteLine("ServerController.start() : {0}", ex.Message);
                Monitor.Exit(mLock);
                this.stop();
                return false;
            }

            Monitor.Exit(mLock);
            return true;
        }

        public void stop()
        {
            Monitor.Enter(mLock);
            if (mIsStart == false)
            {
                Console.WriteLine("ServerController.start() : Already stop");
                Monitor.Exit(mLock);
                return;
            }

            Console.WriteLine("ServerController.stop() : Port({0})", mPort);
            mIsStart = false;
            try
            {
                foreach (var value in mConnectorDictionary)
                {
                    value.Value.stop();
                }
                mConnectorDictionary.Clear();

                if (mSocket != null)
                {
                    mSocket.Close();
                    mSocket = null;
                }                
            }
            catch (Exception ex)
            {
                Console.WriteLine("ServerController.stop() : {0}", ex.Message);
            }
            Monitor.Exit(mLock);
        }

        private void accept(bool isLock)
        {
            if (isLock == true)
            {
                Monitor.Enter(mLock);
            }

            if (mIsStart == true)
            {
                try
                {
                    mSocket.BeginAccept(new AsyncCallback(handleAccept), mSocket);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("ServerController.accept() : {0}", ex.Message);
                    Monitor.Exit(mLock);
                    this.stop();
                    return;
                }
            } 

            if (isLock == true)
            {
                Monitor.Exit(mLock);
            }
        }
        
        private void handleAccept(IAsyncResult ar)
        {
            Monitor.Enter(mLock);
            try
            {
                var socket = ar.AsyncState as Socket;
                var clientSock = socket.EndAccept(ar);

                var controller = new ClientConnectorController(clientSock);

                string ipString = controller.IPString;
                int port = controller.Port;
                string keyString = string.Format("{0}:{1}", ipString, port);
                mConnectorDictionary.Add(keyString, controller);

                Console.WriteLine("ServerController.handleAccept() : client connected({0})", keyString);

                controller.onDisconnectHandler += (connector) =>
                {
                    Console.WriteLine("ServerController.handleAccept() : client disconnected({0})", keyString);
                    Monitor.Enter(mLock);
                    mConnectorDictionary.Remove(keyString);
                    Monitor.Exit(mLock);
                    onDisconnectHandler?.Invoke(connector);
                };
                controller.onRecvHandler += (connector, recvArray, recvDataSize) =>
                {
                    onRecvHandler?.Invoke(connector, recvArray, recvDataSize);
                };
                controller.start();

                this.accept(false);
                Monitor.Exit(mLock);

                onConnectHandler?.Invoke(controller);
                return;
            }
            catch (Exception ex)
            {
                Console.WriteLine("ServerController.handleAccept() : {0}", ex.Message);
            }
            Monitor.Exit(mLock);
        }

        public void send(byte[] buffer, int sendBytes)
        {
            Monitor.Enter(mLock);
            foreach (var value in mConnectorDictionary)
            {
                value.Value.send(buffer, sendBytes);
            }
            Monitor.Exit(mLock);
        }     
    }
}
