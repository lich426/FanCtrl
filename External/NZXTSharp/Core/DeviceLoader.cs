/*
DeviceLoader.cs
Copyright (C) 2019  Ari Madian

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <https://www.gnu.org/licenses/>.
*/

﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Ports;
using System.Linq;
using System.Threading;

using HidLibrary;

using NZXTSharp.COM;
using NZXTSharp.KrakenX;
using NZXTSharp.HuePlus;
using NZXTSharp.Exceptions;

namespace NZXTSharp
{
    /// <summary>
    /// A convenient interface for loading and interacting with <see cref="INZXTDevice"/>s.
    /// </summary>
    public class DeviceLoader
    {
        #region Fields and Properties
        #region Device Specific
        private static readonly SerialCOMData HuePlusCOMData = new SerialCOMData(Parity.None, StopBits.One, 1000, 1000, 256000, 8);
        #endregion

        private List<INZXTDevice> _Devices;

        private DeviceLoadFilter _Filter;

        private bool _Initialized = false;

        private bool _ThrowExceptions = true;

        public event LogHandler OnLogMessage;

        /// <summary>
        /// Returns the first <see cref="KrakenX.KrakenX"/> instance owned 
        /// by the <see cref="DeviceLoader"/> if one exists.
        /// </summary>
        public KrakenX.KrakenX KrakenX { get => (KrakenX.KrakenX)FindDevice(NZXTDeviceType.KrakenX); }

        /// <summary>
        /// Returns the first <see cref="HuePlus.HuePlus"/> instance owned 
        /// by the <see cref="DeviceLoader"/> if one exists.
        /// </summary>
        public HuePlus.HuePlus HuePlus { get => (HuePlus.HuePlus)FindDevice(NZXTDeviceType.HuePlus); }

        /// <summary>
        /// All <see cref="INZXTDevice"/>s owned by the <see cref="DeviceLoader"/> object.
        /// </summary>
        public IEnumerable<INZXTDevice> Devices { get => new ReadOnlyCollection<INZXTDevice>(_Devices); }

        /// <summary>
        /// Whether or not the <see cref="DeviceLoader"/> is fully initialized.
        /// </summary>
        public bool IsInitialized { get => _Initialized; }

        /// <summary>
        /// Gets the number of <see cref="INZXTDevice"/>s owned by the <see cref="DeviceLoader"/>.
        /// </summary>
        public int NumDevices { get => _Devices.Count; }
        
        /// <summary>
        /// The <see cref="DeviceLoader"/> instance's <see cref="DeviceLoadFilter"/>.
        /// </summary>
        public DeviceLoadFilter Filter 
        {
            get => _Filter;
            set => _Filter = value;
        }

        /// <summary>
        /// Whether or not the <see cref="DeviceLoader"/> will throw exceptions.
        /// </summary>
        public bool ThrowExceptions 
        {
            get => _ThrowExceptions;
            set => _ThrowExceptions = value;
        }
        #endregion

        #region Non Static
        #region Constructors
        /// <summary>
        /// Creates a <see cref="DeviceLoader"/> instance with a given <see cref="DeviceLoadFilter"/>.
        /// </summary>
        /// <param name="Filter">A <see cref="DeviceLoadFilter"/>. Defaults to <see cref="DeviceLoadFilter.All"/>.</param>
        public DeviceLoader(DeviceLoadFilter Filter = DeviceLoadFilter.All)
        {
            this._Filter = Filter;
            Initialize();
        }

        /// <summary>
        /// Creates a <see cref="DeviceLoader"/> instance with a given <see cref="DeviceLoadFilter"/>.
        /// </summary>
        /// <param name="InitializeDevices">Whether or not to automatically initialize and load devices. 
        /// Defaults to true.</param>
        /// <param name="Filter">A <see cref="DeviceLoadFilter"/>. Defaults to <see cref="DeviceLoadFilter.All"/></param>
        public DeviceLoader(bool InitializeDevices, DeviceLoadFilter Filter = DeviceLoadFilter.All)
        {
            this._Filter = Filter;

            if (InitializeDevices)
            {
                Initialize();
            }
        }

        /// <summary>
        /// Creates a <see cref="DeviceLoader"/> instance from a given array of <see cref="INZXTDevice"/>s.
        /// </summary>
        /// <param name="Devices">An array of <see cref="INZXTDevice"/>s.</param>
        public DeviceLoader(INZXTDevice[] Devices)
        {
            this._Devices = new List<INZXTDevice>(Devices);
        }

        /// <summary>
        /// Creates a <see cref="DeviceLoader"/> instance from a given list of <see cref="INZXTDevice"/>s.
        /// </summary>
        /// <param name="Devices">A <see cref="List{T}"/> of <see cref="INZXTDevice"/>s.</param>
        public DeviceLoader(List<INZXTDevice> Devices)
        {
            this._Devices = Devices;
        }

        /// <summary>
        /// Creates a <see cref="DeviceLoader"/> instance from a given list of <see cref="INZXTDevice"/>s.
        /// </summary>
        /// <param name="Devices">A <see cref="ReadOnlyCollection{T}"/> of <see cref="INZXTDevice"/>s.</param>
        public DeviceLoader(ReadOnlyCollection<INZXTDevice> Devices)
        {
            this._Devices = new List<INZXTDevice>(Devices);
        }

        #endregion
        #region Methods

        /// <summary>
        /// Initializes and loads all NZXT devices found on the system.
        /// </summary>
        public void Initialize()
        {
            if (_Initialized)
            {
                if (_ThrowExceptions)
                    throw new InvalidOperationException("DeviceLoader already initialized.");
                else
                    return;
            }

            this._Initialized = false;
            _Devices = GetDevices(_Filter, _ThrowExceptions);
            this._Initialized = true;
        }

        /// <summary>
        /// Applies a given <see cref="IEffect"/> to all devices owned by the 
        /// <see cref="DeviceLoader"/> instance which have RGB capabilites.
        /// </summary>
        /// <param name="Effect">An <see cref="IEffect"/> to apply.</param>
        public void ApplyEffectToDevices(IEffect Effect)
        {
            foreach (INZXTDevice Device in this._Devices)
            {
                try
                {
                    Device.ApplyEffect(Effect);
                }
                catch (InvalidOperationException) {}
                catch (IncompatibleEffectException e)
                {
                    if (_ThrowExceptions)
                    {
                        throw new IncompatibleEffectException(
                            "DeviceLoader.ApplyEffectToDevices; Given effect incompatible with an owned device",
                            e
                        );
                    }
                }
            }
        }

        /// <summary>
        /// Adds an <see cref="INZXTDevice"/> instance to the <see cref="DeviceLoader"/>'s devices array.
        /// </summary>
        /// <param name="Device">The device to add.</param>
        public void AddDevice(INZXTDevice Device)
        {
            List<INZXTDevice> Devices;

            if (_Devices == null) Devices = new List<INZXTDevice>();
            else Devices = new List<INZXTDevice>(_Devices);

            Devices.Add(Device);

            _Devices = Devices;
        }

        /// <summary>
        /// Removes the first occurrance of an <see cref="INZXTDevice"/> with the given <paramref name="Type"/>.
        /// </summary>
        /// <param name="Type">The <see cref="NZXTDeviceType"/> to remove.</param>
        public void RemoveDevice(NZXTDeviceType Type)
        {
            if (_Devices.Count == 0) return;
            List<INZXTDevice> Devices = new List<INZXTDevice>(_Devices);

            foreach (INZXTDevice Device in Devices)
            {
                if (Device.Type == Type)
                {
                    Devices.Remove(Device);
                    break;
                }
            }

            _Devices = Devices;
        }

        /// <summary>
        /// Removes a given <see cref="INZXTDevice"/> from the <see cref="DeviceLoader"/> array.
        /// </summary>
        /// <param name="Device">The <see cref="INZXTDevice"/> to remove.</param>
        public void RemoveDevice(INZXTDevice Device)
        {
            if (_Devices.Count == 0) return;
            List<INZXTDevice> Devices = new List<INZXTDevice>(_Devices);

            foreach (INZXTDevice _Device in Devices)
            {
                if (_Device == Device)
                {
                    Devices.Remove(Device);
                    break;
                }
            }

            _Devices = Devices;
        }

        /// <summary>
        /// Disposes of all <see cref="INZXTDevice"/> instances owned by the <see cref="DeviceLoader"/>.
        /// </summary>
        public void Dispose()
        {
            foreach (INZXTDevice Device in _Devices)
            {
                Device.Dispose();
            }

            _Devices = new List<INZXTDevice>();
        }

        /// <summary>
        /// Reconnects to all <see cref="INZXTDevice"/> instances owned by the <see cref="DeviceLoader"/>.
        /// </summary>
        public void Reconnect()
        {
            foreach (INZXTDevice Device in _Devices)
            {
                Device.Reconnect();
            }
        }

        /// <summary>
        /// Disposes of all <see cref="INZXTDevice"/> instances owned by the <see cref="DeviceLoader"/>,
        /// and re-initializes the <see cref="DeviceLoader"/>.
        /// </summary>
        public void ReInitialize()
        {
            Dispose();
            Initialize();
        }

        /// <summary>
        /// Changes the <see cref="DeviceLoader"/> instance's filter to a new 
        /// <see cref="DeviceLoadFilter"/> <paramref name="Filter"/>
        /// </summary>
        /// <param name="Filter">The new <see cref="DeviceLoadFilter"/></param>
        public void ModifyFilter(DeviceLoadFilter Filter)
        {
            this._Filter = Filter;
        }

        /// <summary>
        /// Filters the existing devices in <see cref="DeviceLoader.Devices"/>
        /// based on the given <see cref="DeviceLoadFilter"/>.
        /// </summary>
        /// <param name="Filter">What kinds of devices to keep.</param>
        public void FilterDevices(DeviceLoadFilter Filter)
        {
            int[] allowedIDs = MapFilterToSupportedIDs.Map(Filter);
            List<INZXTDevice> nDevices = new List<INZXTDevice>();

            foreach (INZXTDevice Device in _Devices)
            {
                if (allowedIDs.Contains(Device.ID))
                {
                    nDevices.Add(Device);
                }
            }

            _Devices = nDevices;
        }

        /// <summary>
        /// Sets all fans owned by all <see cref="INZXTDevice"/>s owned by the
        /// <see cref="DeviceLoader"/> to a given <paramref name="Speed"/>.
        /// </summary>
        /// <param name="Speed">The speed to set (percentage).</param>
        public void SetFanSpeed(int Speed)
        {
            KrakenX.SetFanSpeed(Speed);
        }

        /// <summary>
        /// Sets pumps owned by <see cref="INZXTDevice"/>s owned by the
        /// <see cref="DeviceLoader"/> to a given <paramref name="Speed"/>.
        /// </summary>
        /// <param name="Speed">The speed to set (percentage).</param>
        public void SetPumpSpeed(int Speed)
        {
            KrakenX.SetPumpSpeed(Speed);
        }

        /// <summary>
        /// Turns all RGB lighting channels of all <see cref="INZXTDevice"/>s 
        /// owned by the <see cref="DeviceLoader"/> instance on.
        /// </summary>
        public void LightingOn()
        {
            HuePlus.Channel1.On();
            HuePlus.Channel2.On();

            KrakenX.Ring.On();
            KrakenX.Logo.On();
        }

        /// <summary>
        /// Turns all RGB lighting channels of all <see cref="INZXTDevice"/>s 
        /// owned by the <see cref="DeviceLoader"/> instance off.
        /// </summary>
        public void LightingOff()
        {
            HuePlus.Channel1.Off();
            HuePlus.Channel2.Off();

            KrakenX.Ring.Off();
            KrakenX.Logo.Off();
        }

        /// <summary>
        /// Toggles whether or not the <see cref="DeviceLoader"/> throws exceptions.
        /// </summary>
        public void ToggleThrowExceptions()
        {
            this._ThrowExceptions = this._ThrowExceptions ? false : true;
        }

        #endregion
        #endregion

        #region Static

        /// <summary>
        /// Implicitly converts the <see cref="DeviceLoader"/> to an array of <see cref="INZXTDevice"/>s.
        /// </summary>
        /// <param name="Loader"></param>
        public static implicit operator INZXTDevice[] (DeviceLoader Loader) => Loader.Devices.ToArray();

        /// <summary>
        /// Implicitly converts the <see cref="DeviceLoader"/> to a <see cref="List{T}"/>
        /// of <see cref="INZXTDevice"/>s.
        /// </summary>
        /// <param name="Loader"></param>
        public static implicit operator List<INZXTDevice>(DeviceLoader Loader) => Loader.Devices.ToList();

        /// <summary>
        /// Gets and returns all connected devices.
        /// </summary>
        /// <param name="Filter">A <see cref="DeviceLoadFilter"/>, returned devices will only include
        /// devices that fit into categories as defined by the filter.</param>
        /// <returns>An array of all NZXT devices connected to the system.</returns>
        public static List<INZXTDevice> GetDevices(DeviceLoadFilter Filter = DeviceLoadFilter.All, bool ThrowExceptions = true)
        {
            int[] SupportedHIDIDs = new int[] { 0x170e };
            List<INZXTDevice> devices = new List<INZXTDevice>();

            devices.AddRange(TryGetHIDDevices(Filter));
            devices.AddRange(TryGetSerialDevices(Filter, ThrowExceptions));

            return devices;
        }

        /// <summary>
        /// Tries to get all NZXT HID devices connected to the system.
        /// </summary>
        /// <param name="Filter"></param>
        /// <returns>An array of <see cref="INZXTDevice"/>s.</returns>
        private static INZXTDevice[] TryGetHIDDevices(DeviceLoadFilter Filter)
        {
            List<HidDevice> found = DeviceEnumerator.EnumNZXTDevices().ToList();

            INZXTDevice[] devices = InstantiateHIDDevices(found, Filter);
            return devices;
        }

        /// <summary>
        /// Tries to get all NZXT Serial devices connected to the system.
        /// </summary>
        /// <param name="Filter"></param>
        /// <param name="ThrowExceptions"></param>
        /// <returns>An array of <see cref="INZXTDevice"/>s.</returns>
        private static INZXTDevice[] TryGetSerialDevices(DeviceLoadFilter Filter, bool ThrowExceptions = true)
        {
            List<NZXTDeviceType> DevicesFound = new List<NZXTDeviceType>();
            
            SerialController HuePlusController = new SerialController(SerialPort.GetPortNames(), HuePlusCOMData);
            try
            {
                if ((bool)HuePlusController?.IsOpen) // Try to open connection to Hue+
                {
                    int Retries = 0;

                    while (true)
                    {
                        if (HuePlusController?.Write(new byte[] { 0xc0 }, 1).FirstOrDefault() == 1)
                        {
                            DevicesFound.Add(NZXTDeviceType.HuePlus);
                            break;
                        }
                        else
                        {
                            Retries++;

                            Thread.Sleep(40);

                            if (Retries >= 5) break;
                        }
                    }
                    HuePlusController?.Dispose();
                }
            } catch (NullReferenceException e)
            {
                if (ThrowExceptions)
                {
                    throw e;
                }
            }

            return InstantiateSerialDevices(DevicesFound, Filter);
        }

        /// <summary>
        /// Creates instances of found <see cref="INZXTDevice"/>s that operate
        /// on a serial protocol.
        /// </summary>
        /// <param name="Devices">A List of <see cref="INZXTDevice"/>s found by 
        /// <see cref="DeviceLoader.TryGetSerialDevices(DeviceLoadFilter)"/></param>
        /// <param name="Filter"></param>
        /// <returns>An array containing instances of found <see cref="INZXTDevice"/>s.</returns>
        private static INZXTDevice[] InstantiateSerialDevices(List<NZXTDeviceType> Devices, DeviceLoadFilter Filter)
        {
            List<INZXTDevice> outDevices = new List<INZXTDevice>();
            int[] filterIDs = MapFilterToSupportedIDs.Map(Filter);

            foreach (NZXTDeviceType Type in Devices)
            {
                switch (Type)
                {
                    case NZXTDeviceType.HuePlus:
                        if (filterIDs.Contains(0x11111111))
                        {
                            outDevices.Add(new HuePlus.HuePlus());
                        }
                        break;
                }
            }

            return outDevices.ToArray();
        }

        /// <summary>
        /// Creates instances of found <see cref="INZXTDevice"/>s that operate
        /// on an HID protocol.
        /// </summary>
        /// <param name="Devices">A list of <see cref="INZXTDevice"/>s found by
        /// <see cref="DeviceLoader.TryGetHIDDevices(DeviceLoadFilter)"/></param>
        /// <param name="Filter"></param>
        /// <returns>An array containing instances of found HID devices.</returns>
        private static INZXTDevice[] InstantiateHIDDevices(List<HidDevice> Devices, DeviceLoadFilter Filter)
        {
            int[] SupportedHIDIDs = new int[] { 0x170e };
            int[] filterIDs = MapFilterToSupportedIDs.Map(Filter);
            List<INZXTDevice> outDevices = new List<INZXTDevice>();
            foreach (HidDevice device in Devices)
            {
                int ID = device.Attributes.ProductId;
                if (SupportedHIDIDs.Contains(ID))
                {
                    if (filterIDs.Contains(ID))
                    {
                        outDevices.Add(MapIDtoInstance.Map(ID));
                    }
                }
            }
            return outDevices.ToArray();
        }

        internal INZXTDevice FindDevice(NZXTDeviceType Type)
        {
            foreach (INZXTDevice Device in _Devices)
                if (Device.Type == Type)
                    return Device;

            return null;
        }
        #endregion
    }

    /// <summary>
    /// Maps a <see cref="DeviceLoadFilter"/> to the HID IDs of devices included in that filter.
    /// </summary>
    internal class MapFilterToSupportedIDs
    {
        internal static int[] Map(DeviceLoadFilter Filter)
        {
            switch (Filter)
            {
                case DeviceLoadFilter.All:
                    return new int[]
                    {
                        0x0715, 0x170e, 0x1712, 0x1711, 0x2002,
                        0x2001, 0x2005, 0x1714, 0x1713, 0x11111111
                    };
                case DeviceLoadFilter.Coolers:
                    return new int[]
                    {
                        0x1715, 0x170e, 0x1712
                    };
                case DeviceLoadFilter.FanControllers:
                    return new int[]
                    {
                        0x1711, 0x1714, 0x1713, 0x2005, 0x170e
                    };
                case DeviceLoadFilter.LightingControllers:
                    return new int[]
                    {
                        0x1715, 0x170e, 0x1712, 0x2002, 0x2001,
                        0x2005, 0x1714, 0x1713, 0x11111111
                    };
                case DeviceLoadFilter.Grid: return new int[] { 0x1711 };
                case DeviceLoadFilter.Gridv3: return new int[] { 0x1711 };
                case DeviceLoadFilter.Hue:
                    return new int[]
                    {
                        0x11111111, // HuePlus ID
                        0x2002,
                        0x2001
                    };
                case DeviceLoadFilter.Hue2: return new int[] { 0x2001 };
                case DeviceLoadFilter.HueAmbient: return new int[] { 0x2002 };
                case DeviceLoadFilter.HuePlus: return new int[] { 0x11111111 };
                case DeviceLoadFilter.Kraken:
                    return new int[]
                    {
                        0x1715,
                        0x170e
                    };
                case DeviceLoadFilter.KrakenM: return new int[] { 0x1715 };
                case DeviceLoadFilter.KrakenX: return new int[] { 0x170e };
                case DeviceLoadFilter.KrakenX3: return new int[] { 0x2007 };
                default:
                    return Array.Empty<int>();
            }
        }
    }

    /// <summary>
    /// Maps an HID device ID to an instance of that device's corresponding <see cref="INZXTDevice"/>.
    /// </summary>
    internal class MapIDtoInstance
    {
        internal static INZXTDevice Map(int ID)
        {
            switch (ID)
            {
                case 0x170e:
                    return new KrakenX.KrakenX(NZXTDeviceType.KrakenX);
                case 0x2007:
                    return new KrakenX.KrakenX(NZXTDeviceType.KrakenX3);
                default:
                    throw new Exception(); // TODO
            }
        }
    }
}
