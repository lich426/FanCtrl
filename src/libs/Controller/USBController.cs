﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;

namespace FanCtrl
{
    public enum USBVendorID : int
    {
        Unknown = -1,

        // NZXT
        NZXT = 0x1e71,

        // Asetek
        ASETEK = 0x2433,
    };

    public enum USBProductID : int
    {
        Unknown = -1,

        // KrakenX2 series
        KrakenX2 = 0x170e,

        // KrakenX3 series
        KrakenX3 = 0x2007,

        // KrakenX3 series
        KrakenX3_NEW = 0x2014,

        // EVGA CLC
        CLC = 0xb200,

        // NZXT RGB & Fan Controller V1
        RGBAndFanController = 0x2009,

        // NZXT RGB & Fan Controller V2 / Smart Device V2
        RGBAndFanControllerV2 = 0x2019,
    }

    public class USBController
    {
        public USBVendorID VendorID { get; set; }

        public USBProductID ProductID { get; set; }

        // onRecv callback
        public delegate void OnRecvHandler(byte[] recvArray, int recvDataSize);
        public event OnRecvHandler onRecvHandler;

        public USBController(USBVendorID vendorID, USBProductID productID)
        {
            VendorID = vendorID;
            ProductID = productID;
        }

        public virtual bool start(uint index)
        {
            return false;
        }

        public virtual void stop()
        {

        }

        public virtual void send(byte[] buffer)
        {

        }

        public virtual void send(List<byte[]> bufferList)
        {

        }

        protected void onRecv(byte[] recvArray, int recvDataSize)
        {
            onRecvHandler(recvArray, recvDataSize);
        }        
    }
}
