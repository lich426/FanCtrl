using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace SIUSBXP_DLL
{
    public class SIUSBXP
    {
        // Return codes
        public const byte SI_SUCCESS = 0x00;
        public const byte SI_DEVICE_NOT_FOUND = 0xFF;
        public const byte SI_INVALID_HANDLE = 0x01;
        public const byte SI_READ_ERROR = 0x02;
        public const byte SI_RX_QUEUE_NOT_READY = 0x03;
        public const byte SI_WRITE_ERROR = 0x04;
        public const byte SI_RESET_ERROR = 0x05;
        public const byte SI_INVALID_PARAMETER = 0x06;
        public const byte SI_INVALID_REQUEST_LENGTH = 0x07;
        public const byte SI_DEVICE_IO_FAILED = 0x08;
        public const byte SI_INVALID_BAUDRATE = 0x09;
        public const byte SI_FUNCTION_NOT_SUPPORTED = 0x0a;
        public const byte SI_GLOBAL_DATA_ERROR = 0x0b;
        public const byte SI_SYSTEM_ERROR_CODE = 0x0c;
        public const byte SI_READ_TIMED_OUT = 0x0d;
        public const byte SI_WRITE_TIMED_OUT = 0x0e;
        public const byte SI_IO_PENDING = 0x0f;

        // GetProductString() function flags
        public const byte SI_RETURN_SERIAL_NUMBER = 0x00;
        public const byte SI_RETURN_DESCRIPTION = 0x01;
        public const byte SI_RETURN_LINK_NAME = 0x02;
        public const byte SI_RETURN_VID = 0x03;
        public const byte SI_RETURN_PID = 0x04;

        // RX Queue status flags
        public const byte SI_RX_NO_OVERRUN = 0x00;
        public const byte SI_RX_EMPTY = 0x00;
        public const byte SI_RX_OVERRUN = 0x01;
        public const byte SI_RX_READY = 0x02;

        // Buffer size limits
        public const int SI_MAX_DEVICE_STRLEN = 256;
        public const int SI_MAX_READ_SIZE = 4096 * 16;
        public const int SI_MAX_WRITE_SIZE = 4096;

        // Input and Output pin Characteristics
        public const byte SI_HELD_INACTIVE = 0x00;
        public const byte SI_HELD_ACTIVE = 0x01;
        public const byte SI_FIRMWARE_CONTROLLED = 0x02;
        public const byte SI_RECEIVE_FLOW_CONTROL = 0x02;
        public const byte SI_TRANSMIT_ACTIVE_SIGNAL = 0x03;
        public const byte SI_STATUS_INPUT = 0x00;
        public const byte SI_HANDSHAKE_LINE = 0x01;

        // Mask and Latch value bit definitions
        public const byte SI_GPIO_0 = 0x01;
        public const byte SI_GPIO_1 = 0x02;
        public const byte SI_GPIO_2 = 0x04;
        public const byte SI_GPIO_3 = 0x08;

        // GetDeviceVersion() return codes
        public const byte SI_CP2101_VERSION = 0x01;
        public const byte SI_CP2102_VERSION = 0x02;
        public const byte SI_CP2103_VERSION = 0x03;
        public const byte SI_CP2104_VERSION = 0x04;

        [DllImport("SiUSBXp.dll")]
        public static extern int SI_GetNumDevices(
        ref uint lpdwNumDevices
        );

		// Caller must set StringBuilder capacity to max possible
		// returned string size before calling this function
        [DllImport("SiUSBXp.dll")]
        public static extern int SI_GetProductString(
        uint dwDeviceNum,
        StringBuilder lpvDeviceString,
        uint dwFlags
        );

        [DllImport("SiUSBXp.dll")]
        public static extern int SI_Open(
        uint dwDevice,
        ref IntPtr cyHandle
        );

        [DllImport("SiUSBXp.dll")]
        public static extern int SI_Close(
        IntPtr cyHandle
        );

        [DllImport("SiUSBXp.dll")]
        public static extern int SI_Read(
        IntPtr cyHandle,
        byte[] lpBuffer,
        uint dwBytesToRead,
        ref uint lpdwBytesReturned,
        IntPtr o
        );

        [DllImport("SiUSBXp.dll")]
        public static extern int SI_Write(
        IntPtr cyHandle,
        byte[] lpBuffer,
        uint dwBytesToWrite,
        ref uint lpdwBytesWritten,
        IntPtr o
        );

        [DllImport("SiUSBXp.dll")]
        public static extern int SI_DeviceIOControl(
        IntPtr cyHandle,
        uint dwIoControlCode,
        byte[] lpInBuffer,
        uint dwBytesToRead,
        byte[] lpOutBuffer,
        uint dwBytesToWrite,
        ref uint lpdwBytesSucceeded
        );

        [DllImport("SiUSBXp.dll")]
        public static extern int SI_FlushBuffers(
        IntPtr cyHandle,
        byte FlushTransmit,
        byte FlushReceive
        );

        [DllImport("SiUSBXp.dll")]
        public static extern int SI_SetTimeouts(
        uint dwReadTimeout,
        uint dwWriteTimeout
        );

        [DllImport("SiUSBXp.dll")]
        public static extern int SI_GetTimeouts(
        ref uint lpdwReadTimeout,
        ref uint lpdwWriteTimeout
        );

        [DllImport("SiUSBXp.dll")]
        public static extern int SI_CheckRXQueue(
        IntPtr cyHandle,
        ref uint lpdwNumBytesInQueue,
        ref uint lpdwQueueStatus
        );

        [DllImport("SiUSBXp.dll")]
        public static extern int SI_SetBaudRate(
        IntPtr cyHandle,
        uint dwBaudRate
        );

        [DllImport("SiUSBXp.dll")]
        public static extern int SI_SetBaudDivisor(
        IntPtr cyHandle,
        ushort wBaudDivisor
        );

        [DllImport("SiUSBXp.dll")]
        public static extern int SI_SetLineControl(
        IntPtr cyHandle,
        ushort wLineControl
        );

        [DllImport("SiUSBXp.dll")]
        public static extern int SI_SetFlowControl(
        IntPtr cyHandle,
        byte bCTS_MaskCode,
        byte bRTS_MaskCode,
        byte bDTR_MaskCode,
        byte bDSR_MaskCode,
        byte bDCD_MaskCode,
        bool bFlowXonXoff
        );

        [DllImport("SiUSBXp.dll")]
        public static extern int SI_GetModemStatus(
        IntPtr cyHandle,
        ref byte ModemStatus
        );

        [DllImport("SiUSBXp.dll")]
        public static extern int SI_SetBreak(
        IntPtr cyHandle,
        ushort wBreakState
        );

        [DllImport("SiUSBXp.dll")]
        public static extern int SI_ReadLatch(
        IntPtr cyHandle,
        ref byte lpbLatch
        );

        [DllImport("SiUSBXp.dll")]
        public static extern int SI_WriteLatch(
        IntPtr cyHandle,
        byte bMask,
        byte bLatch
        );

        [DllImport("SiUSBXp.dll")]
        public static extern int SI_GetPartNumber(
        IntPtr cyHandle,
        ref byte lpbPartNum
        );

        [DllImport("SiUSBXp.dll")]
        public static extern int SI_GetDeviceProductString(
        IntPtr cyHandle,
        byte[] lpProduct,
        ref byte lpbLength,
        bool bConvertToASCII
        );

        [DllImport("SiUSBXp.dll")]
        public static extern int SI_GetDLLVersion(
        ref uint HighVersion,
        ref uint LowVersion
        );

        [DllImport("SiUSBXp.dll")]
        public static extern int SI_GetDriverVersion(
        ref uint HighVersion,
        ref uint LowVersion
        );
    }
}
