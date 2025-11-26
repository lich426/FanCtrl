using System;
using System.Collections.Generic;
using System.Text;

namespace FanCtrl
{
    public class PluginPacket
    {
        public const byte STX = 0xFA;
        public const int HEADER_SIZE = 5;
        public const int HEADER_DATA_COUNT_SIZE = HEADER_SIZE - 1;

        public string IPString { get; set; }
        public int Port { get; set; }

        private List<byte> mRecvList = new List<byte>();

        public PluginPacket(string ipString, int port)
        {
            IPString = ipString;
            Port = port; 
        }

        public void addRecvData(byte[] recvBuffer, int recvDataCount)
        {
            for (int i = 0; i < recvDataCount; i++)
            {
                mRecvList.Add(recvBuffer[i]);
            }
        }

        public bool processData(ref string jsonString)
        {
            while (true)
            {
                int recvCount = parseRecvData(ref jsonString);

                // invalid stx
                if (recvCount < 0)
                {
                    mRecvList.RemoveAt(0);
                    continue;
                }

                if (recvCount == 0)
                {
                    return false;
                }

                mRecvList.RemoveRange(0, recvCount);
                return true;
            }
        }

        // return : -1(invalid stx), parse byte array size
        private int parseRecvData(ref string jsonString)
        {
            // check count size >= HEADER_SIZE
            if (mRecvList.Count < HEADER_SIZE)
            {
                return 0;
            }

            // check STX
            if (mRecvList[0] != STX)
            {
                return -1;
            }

            // data length        
            var temp = new byte[HEADER_DATA_COUNT_SIZE];
            for (int i = 0; i < HEADER_DATA_COUNT_SIZE; i++)
            {
                temp[i] = mRecvList[i + 1];
            }
            int dataLength = BitConverter.ToInt32(temp, 0);

            // check data size
            // not enough data
            if (dataLength < mRecvList.Count - HEADER_SIZE)
            {
                return 0;
            }

            // json data
            var tempBuffer = new byte[dataLength];
            Array.Copy(mRecvList.ToArray(), HEADER_SIZE, tempBuffer, 0, tempBuffer.Length);
            jsonString = Encoding.UTF8.GetString(tempBuffer);

            return dataLength + HEADER_SIZE;
        }

        public static byte[] getSendPacket(string jsonString)
        {
            var list = new List<byte>();

            // STX
            list.Add(STX);

            var jsonArray = Encoding.UTF8.GetBytes(jsonString);

            // data length
            var dataLengthBytes = BitConverter.GetBytes(jsonArray.Length);
            for (int i = 0; i < dataLengthBytes.Length; i++)
            {
                list.Add(dataLengthBytes[i]);
            }

            // data
            for (int i = 0; i < jsonArray.Length; i++)
            {
                list.Add(jsonArray[i]);
            }

            return list.ToArray();
        }
    }
}
