using System;
using System.IO.MemoryMappedFiles;
using System.Text;

namespace FanCtrl
{
    class OSDController
    {
		private const string RTSS_STRING_SHARE_MEMORY = "RTSSSharedMemoryV2";
		private const string RTSS_STRING_FANCTRL = "FanCtrl";

		public static bool update(string osdString)
        {
			bool bResult = false;
			var tempArray = Encoding.Default.GetBytes(RTSS_STRING_FANCTRL);
			var fanCtrlArray = new byte[tempArray.Length + 1];
			Array.Copy(tempArray, fanCtrlArray, tempArray.Length);

			tempArray = Encoding.Default.GetBytes(osdString);
			var osdArray = new byte[tempArray.Length + 1];
			Array.Copy(tempArray, osdArray, tempArray.Length);

			try
            {
                using (var mapFile = MemoryMappedFile.OpenExisting(RTSS_STRING_SHARE_MEMORY, MemoryMappedFileRights.FullControl))
                {
                    using (var view = mapFile.CreateViewStream())
                    {
                        unsafe
                        {
							byte* ptr = null;
							view.SafeMemoryMappedViewHandle.AcquirePointer(ref ptr);
							if (ptr != null)
                            {
								var pMem = (RTSS_SHARED_MEMORY*)ptr;

								if ((pMem->dwSignature == 0x52545353) && (pMem->dwVersion >= 0x00020000))
								{
									//1st pass : find previously captured OSD slot
									//2nd pass : otherwise find the first unused OSD slot and capture it
									for (uint dwPass = 0; dwPass < 2; dwPass++)
									{
										//allow primary OSD clients (i.e. EVGA Precision / MSI Afterburner) to use the first slot exclusively, so third party
										//applications start scanning the slots from the second one
										for (uint dwEntry = 1; dwEntry < pMem->dwOSDArrSize; dwEntry++)
										{
											var pEntry = (RTSS_SHARED_MEMORY_OSD_ENTRY*)(ptr + pMem->dwOSDArrOffset + dwEntry * pMem->dwOSDEntrySize);
											if (pEntry == null)
												continue;

											if (dwPass > 0)
											{
												if (pEntry->szOSDOwner[0] == 0)
												{
													for (int i = 0; i < fanCtrlArray.Length; i++)
                                                    {
														pEntry->szOSDOwner[i] = fanCtrlArray[i];
													}
												}
											}

											bool isExist = true;
											for (int i = 0; i < fanCtrlArray.Length; i++)
                                            {
												if (pEntry->szOSDOwner[i] != fanCtrlArray[i])
                                                {
													isExist = false;
													break;
												}
                                            }

											if (isExist == true)
											{
												//use extended text slot for v2.7 and higher shared memory, it allows displaying 4096 symbols
												//instead of 256 for regular text slot
												if (pMem->dwVersion >= 0x00020007)												
												{
													//OSD locking is supported on v2.14 and higher shared memory
													if (pMem->dwVersion >= 0x0002000e)													
													{
														int maxLength = 4096;
														for (int i = 0; i < osdArray.Length; i++)
                                                        {
															if (i >= maxLength)
																break;
															pEntry->szOSDEx[i] = osdArray[i];
														}
														pMem->dwBusy = 0;
													}
													else
													{
														int maxLength = 4096;
														for (int i = 0; i < osdArray.Length; i++)
														{
															if (i >= maxLength)
																break;
															pEntry->szOSDEx[i] = osdArray[i];
														}
													}
												}
												else
												{
													int maxLength = 256;
													for (int i = 0; i < osdArray.Length; i++)
													{
														if (i >= maxLength)
															break;
														pEntry->szOSD[i] = osdArray[i];
													}
												}

												pMem->dwOSDFrame++;

												bResult = true;
												break;
											}
										}

										if (bResult)
											break;
									}
								}
							}
						}
					}
                }
            }
            catch { }
            return bResult;
        }

        public static void release()
        {
			var tempArray = Encoding.Default.GetBytes(RTSS_STRING_FANCTRL);
			var fanCtrlArray = new byte[tempArray.Length + 1];
			Array.Copy(tempArray, fanCtrlArray, tempArray.Length);
			try
			{
				using (var mapFile = MemoryMappedFile.OpenExisting(RTSS_STRING_SHARE_MEMORY, MemoryMappedFileRights.FullControl))
				{
					using (var view = mapFile.CreateViewStream())
					{
						unsafe
						{
							byte* ptr = null;
							view.SafeMemoryMappedViewHandle.AcquirePointer(ref ptr);
							if (ptr != null)
							{
								var pMem = (RTSS_SHARED_MEMORY*)ptr;

								if ((pMem->dwSignature == 0x52545353) && (pMem->dwVersion >= 0x00020000))
								{
									for (uint dwEntry = 1; dwEntry < pMem->dwOSDArrSize; dwEntry++)
									{
										var pEntry = (RTSS_SHARED_MEMORY_OSD_ENTRY*)(ptr + pMem->dwOSDArrOffset + dwEntry * pMem->dwOSDEntrySize);
										if (pEntry == null)
											continue;

										bool isExist = true;
										for (int i = 0; i < fanCtrlArray.Length; i++)
										{
											if (pEntry->szOSDOwner[i] != fanCtrlArray[i])
											{
												isExist = false;
												break;
											}
										}

										if (isExist == true)
                                        {
											var pEntryByteArray = (byte*)(ptr + pMem->dwOSDArrOffset + dwEntry * pMem->dwOSDEntrySize);
											for (int i = 0; i < pMem->dwOSDEntrySize; i++)
                                            {
												pEntryByteArray[i] = 0;
											}
											pMem->dwOSDFrame++;
										}
									}
								}
							}
						}
					}
				}
			}
			catch { }
		}
    }

    internal unsafe struct RTSS_SHARED_MEMORY_OSD_ENTRY
    {
        public fixed byte szOSD[256];
        //OSD slot text
        public fixed byte szOSDOwner[256];
        //OSD slot owner ID

        //next fields are valid for v2.7 and newer shared memory format only

        public fixed byte szOSDEx[4096];
        //extended OSD slot text

        //next fields are valid for v2.12 and newer shared memory format only

        public fixed byte buffer[262144];
    }

	internal unsafe struct RTSS_SHARED_MEMORY
    {
		public uint dwSignature;
		//signature allows applications to verify status of shared memory

		//The signature can be set to:
		//'RTSS'	- statistics server's memory is initialized and contains 
		//			valid data 
		//0xDEAD	- statistics server's memory is marked for deallocation and
		//			no longer contain valid data
		//otherwise	the memory is not initialized
		public uint dwVersion;
		//structure version ((major<<16) + minor)
		//must be set to 0x0002xxxx for v2.x structure 

		public uint dwAppEntrySize;
		//size of RTSS_SHARED_MEMORY_OSD_ENTRY for compatibility with future versions
		public uint dwAppArrOffset;
		//offset of arrOSD array for compatibility with future versions
		public uint dwAppArrSize;
		//size of arrOSD array for compatibility with future versions

		public uint dwOSDEntrySize;
		//size of RTSS_SHARED_MEMORY_APP_ENTRY for compatibility with future versions
		public uint dwOSDArrOffset;
		//offset of arrApp array for compatibility with future versions
		public uint dwOSDArrSize;
		//size of arrOSD array for compatibility with future versions

		public uint dwOSDFrame;
        //Global OSD frame ID. Increment it to force the server to update OSD for all currently active 3D
        //applications.

        //next fields are valid for v2.14 and newer shared memory format only

        public long dwBusy;
		//set bit 0 when you're writing to shared memory and reset it when done

		//WARNING: do not forget to reset it, otherwise you'll completely lock OSD updates for all clients


		//next fields are valid for v2.15 and newer shared memory format only

		public uint dwDesktopVideoCaptureFlags;
        public fixed uint dwDesktopVideoCaptureStat[5];

        //shared copy of desktop video capture flags and performance stats for 64-bit applications

	}

}
