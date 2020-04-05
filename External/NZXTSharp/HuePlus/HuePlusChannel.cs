/*
HuePlusChannel.cs
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
using System.Collections;
using System.Collections.Generic;
using System.Text;

using NZXTSharp;

namespace NZXTSharp.HuePlus {

    /// <summary>
    /// Represents a channel on an NZXT device.
    /// </summary>
    public class HuePlusChannel : IChannel
    {

        private readonly int _ChannelByte;
        private IEffect _Effect = new Fixed(new Color(255, 255, 255));
        private bool _State = true;
        private HuePlus _Parent;
        private HuePlusChannelInfo _ChannelInfo;
        #pragma warning disable IDE0044 // Add readonly modifier
        private List<ISubDevice> _SubDevices = new List<ISubDevice>();
        #pragma warning restore IDE0044 // Add readonly modifier

        #region Properties
        /// <summary>
        /// The channelbyte of the <see cref="HuePlusChannel"/>.
        /// </summary>
        public int ChannelByte { get; }

        /// <summary>
        /// The <see cref="IEffect"/> currently applied to the <see cref="HuePlusChannel"/>.
        /// </summary>
        public IEffect Effect { get => _Effect; }

        /// <summary>
        /// Whether or not the current <see cref="HuePlusChannel"/> is active (on).
        /// </summary>
        public bool State { get => _State; }

        /// <summary>
        /// The <see cref="HuePlusChannel"/>'s <see cref="ChannelInfo"/> object.
        /// </summary>
        public HuePlusChannelInfo ChannelInfo { get => _ChannelInfo; }

        /// <summary>
        /// The device that owns the <see cref="HuePlusChannel"/>.
        /// </summary>
        public HuePlus Parent { get => _Parent; }

        /// <summary>
        /// A list of <see cref="ISubDevice"/>s owned by the <see cref="HuePlusChannel"/>.
        /// </summary>
        public List<ISubDevice> SubDevices { get => _SubDevices; }
        #endregion

        /// <summary>
        /// Constructs a <see cref="HuePlusChannel"/> object with a given <paramref name="ChannelByte"/>.
        /// </summary>
        /// <param name="ChannelByte">The ChannelByte to construct the channel from.</param>
        public HuePlusChannel(int ChannelByte) {
            this._ChannelByte = ChannelByte;
        }

        /// <summary>
        /// Constructs a <see cref="HuePlusChannel"/> object with a given <paramref name="ChannelByte"/>,
        /// owned by a given <paramref name="Parent"/> <see cref="HuePlus"/>.
        /// </summary>
        /// <param name="ChannelByte">The ChannelByte to construct the channel from.</param>
        /// <param name="Parent">The <see cref="HuePlus"/> that will own the <see cref="HuePlusChannel"/></param>
        public HuePlusChannel(int ChannelByte, HuePlus Parent) {
            this.ChannelByte = ChannelByte;
            this._Parent = Parent;
        }

        /// <summary>
        /// Constructs a <see cref="HuePlusChannel"/> object with a given <paramref name="ChannelByte"/>,
        /// owned by a given <paramref name="Parent"/> <see cref="HuePlus"/>,
        /// with a given <see cref="ChannelInfo"/>.
        /// </summary>
        /// <param name="ChannelByte">The ChannelByte to construct the channel from.</param>
        /// <param name="Parent">The <see cref="HuePlus"/> that owns the <see cref="HuePlusChannel"/></param>
        /// <param name="Info">The <see cref="ChannelInfo"/> owned by the <see cref="HuePlusChannel"/>.</param>
        public HuePlusChannel(int ChannelByte, HuePlus Parent, HuePlusChannelInfo Info) {
            this.ChannelByte = ChannelByte;
            this._Parent = Parent;
            this._ChannelInfo = Info;
        }

        internal void BuildSubDevices() {
            for (int i = 0; i < _ChannelInfo.NumSubDevices; i++) {
                switch (_ChannelInfo.Type) {
                    case NZXTDeviceType.Fan:
                        this._SubDevices.Add(new Fan());
                        break;
                    case NZXTDeviceType.Strip:
                        this._SubDevices.Add(new Strip());
                        break;
                }
            }
        }

        internal void UpdateEffect(IEffect newOne)
        {
            this._Effect = newOne;
        }

        /// <summary>
        /// Refreshes all <see cref="ISubDevice"/>s in the <see cref="HuePlusChannel"/>'s <see cref="SubDevices"/> list.
        /// </summary>
        public void RefreshSubDevices()
        {
            BuildSubDevices();
        }

        /// <summary>
        /// Turns the <see cref="HuePlusChannel"/> on.
        /// </summary>
        public void On() {
            this._State = true;
            _Parent.ApplyEffect(this, _Effect);
        }

        /// <summary>
        /// Turns the <see cref="HuePlusChannel"/> off.
        /// </summary>
        public void Off() {
            this._State = false;
            _Parent.ApplyEffect(this, new Fixed(this, new Color(0, 0, 0)), false);
        }

        /// <summary>
        /// Build color bytes for an effect based on the given <see cref="Color"/>.
        /// </summary>
        /// <param name="color"></param>
        /// <returns>A byte array with 140 elements; G, R, B color codes.</returns>
        public byte[] BuildColorBytes(Color color) {
            List<byte> outList = new List<byte>();
            foreach (ISubDevice device in _SubDevices)
            {
                if (device.IsActive) // If active, add effect color
                {
                    byte[][] exp = color.ExpandedChunks(device.NumLeds);
                    for (int LED = 0; LED < device.NumLeds; LED++) {
                        if (device.Leds[LED])
                        {
                            outList.Add(exp[LED][0]);
                            outList.Add(exp[LED][1]);
                            outList.Add(exp[LED][2]);
                        }
                        else
                        {
                            outList.Add(0x00);
                            outList.Add(0x00);
                            outList.Add(0x00);
                        }
                    }
                }
                else { // If not active, add padding bytes
                    for (int led = 0; led < device.NumLeds * 3; led++) {
                        outList.Add(0x00);
                    }
                }
            }
            for (int pad = outList.Count; pad < 120; pad++) { // Pad out remainder
                outList.Add(0x00);
            }
            return outList.ToArray();
        }

        /// <summary>
        /// Builds the color bytes for an effect.
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public byte[] BuildColorBytes(byte[] buffer)
        {
            for (int deviceN = 0; deviceN < SubDevices.Count; deviceN++)
            {
                ISubDevice device = SubDevices[deviceN];
                if (!device.IsActive) // If device is not active, account for device
                {
                    for (int LED = 0; LED < device.NumLeds; LED++)
                    {
                        buffer[(deviceN * device.NumLeds * 3) + (LED * 3)] = 0x00;
                        buffer[(deviceN * device.NumLeds * 3) + (LED * 3) + 1] = 0x00;
                        buffer[(deviceN * device.NumLeds * 3) + (LED * 3) + 2] = 0x00;
                    }

                } else // If device IS active, account for device's LEDs
                {
                    for (int LED = 0; LED < device.Leds.Length; LED++)
                    {
                        if (!device.Leds[LED]) // If LED is not on
                        {
                            buffer[(deviceN * device.NumLeds) + (LED * 3)] = 0x00;
                            buffer[(deviceN * device.NumLeds) + (LED * 3) + 1] = 0x00;
                            buffer[(deviceN * device.NumLeds) + (LED * 3) + 2] = 0x00;
                        }
                    }
                }
            }
            return buffer;
        }

        /// <summary>
        /// Updates the <see cref="HuePlusChannel"/>'s <see cref="ChannelInfo"/>.
        /// </summary>
        public void UpdateChannelInfo() {
            Parent.UpdateChannelInfo(this);
        }

        /// <summary>
        /// Sets the <see cref="HuePlusChannel"/>'s <see cref="ChannelInfo"/> to the given <paramref name="info"/>.
        /// </summary>
        /// <param name="info"></param>
        public void SetChannelInfo(HuePlusChannelInfo info) {
            this._ChannelInfo = info;
        }

        /// <summary>
        /// Returns the <see cref="HuePlusChannel"/>'s ChannelByte.
        /// </summary>
        /// <param name="channel"></param>
        public static explicit operator byte(HuePlusChannel channel) {
            return (byte)channel.ChannelByte;
        }
    }
}
