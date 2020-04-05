/*
KrakenXChannel.cs
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
using System.Text;

using NZXTSharp;
using NZXTSharp.Exceptions;

namespace NZXTSharp.KrakenX
{
    /// <summary>
    /// Represents an RGB channel on a <see cref="KrakenX"/> device.
    /// </summary>
    public class KrakenXChannel : IChannel
    {
        private int _ChannelByte;
        private bool[] _Leds;
        private bool _IsActive = true;
        private IEffect _Effect = new Fixed(new Color(255, 255, 255));
        private KrakenX _Parent;

        /// <summary>
        /// The ChannelByte of the <see cref="KrakenXChannel"/>.
        /// </summary>
        public int ChannelByte => _ChannelByte;

        /// <summary>
        /// Whether or not the channel is on.
        /// </summary>
        public bool State => _IsActive;

        /// <summary>
        /// All LEDs owned by the <see cref="KrakenXChannel"/>.
        /// </summary>
        public bool[] Leds { get => _Leds; }
        
        /// <summary>
        /// The number of LEDs owned by the <see cref="KrakenXChannel"/>.
        /// </summary>
        public int NumLeds { get => _Leds.Length; }

        /// <summary>
        /// The effect last applied to the <see cref="KrakenXChannel"/>.
        /// </summary>
        public IEffect Effect { get => _Effect; }


        /// <summary>
        /// Constructs a <see cref="KrakenXChannel"/> instance.
        /// </summary>
        /// <param name="ChannelByte">The channel's ChannelByte.</param>
        /// <param name="Parent">The <see cref="KrakenX"/> device that owns the channel.</param>
        public KrakenXChannel(int ChannelByte, KrakenX Parent)
        {
            this._ChannelByte = ChannelByte;
            this._Parent = Parent;
            BuildLEDs();
        }

        /// <summary>
        /// Builds the LEDs owned by the <see cref="KrakenXChannel"/>.
        /// </summary>
        public void BuildLEDs()
        {
            switch(_ChannelByte)
            {
                case 0x00:
                    this._Leds = new bool[]
                    {
                        true, true, true, true,
                        true, true, true, true
                    };
                    break;
                case 0x01:
                    this._Leds = new bool[] { true };
                    break;
                case 0x02:
                    this._Leds = new bool[]
                    {
                        true, true, true, true,
                        true, true, true, true
                    };
                    break;
                default:
                    throw new InvalidParamException("Invalid ChannelByte given to KrakenXChannel constructor.");
            }
        }

        internal void UpdateEffect(IEffect newOne)
        {
            this._Effect = newOne;
        }
        
        /// <summary>
        /// Turns the <see cref="KrakenXChannel"/> on and re-applies 
        /// the last applied <see cref="KrakenXChannel.Effect"/>
        /// </summary>
        public void On()
        {
            this._IsActive = true;
            _Parent.ApplyEffect(this, _Effect);
        }

        /// <summary>
        /// Turns the <see cref="KrakenXChannel"/> off.
        /// </summary>
        public void Off()
        {
            this._IsActive = false;
            _Parent.ApplyEffect(this, new Fixed(this, new Color(0, 0, 0)), false);
        }

        /// <inheritdoc/>
        public byte[] BuildColorBytes(byte[] _Buffer)
        {
            for (int LedN = 0; LedN < this._Leds.Length; LedN++)
            {
                if (!_Leds[LedN]) // If LED IS NOT active
                {
                    _Buffer[LedN * 3] = 0x00;
                    _Buffer[LedN * 3 + 1] = 0x01;
                    _Buffer[LedN * 3 + 2] = 0x01;
                }
            }
            return _Buffer;
        }

        /// <inheritdoc/>
        public byte[] BuildColorBytes(Color Color)
        {
            List<byte> outBytes = new List<byte>();
            if (_IsActive)
            {
                outBytes.Add(Convert.ToByte(Color.G));
                outBytes.Add(Convert.ToByte(Color.R));
                outBytes.Add(Convert.ToByte(Color.B));

                for (int i = 0; i < _Leds.Length; i++)
                {
                    if (!_Leds[i])
                    {
                        outBytes.Add(0x00);
                        outBytes.Add(0x00);
                        outBytes.Add(0x00);
                    }
                    else
                    {
                        outBytes.Add(Convert.ToByte(Color.R));
                        outBytes.Add(Convert.ToByte(Color.G));
                        outBytes.Add(Convert.ToByte(Color.B));
                    }
                }

                int numToPad = 0x41 - 5 - (9 * 3);

                outBytes = outBytes.PadList(numToPad);

                return outBytes.ToArray();
            } else
            {
                int numToPad = 0x41 - 5;
                outBytes = outBytes.PadList(numToPad);
                return outBytes.ToArray();
            }
        }

        /// <summary>
        /// Toggles an LED's state.
        /// </summary>
        /// <param name="Index">The index of the LED to toggle.</param>
        public void ToggleLed(int Index)
        {
            this._Leds[Index] = !this._Leds[Index];
        }
        
        /// <summary>
        /// Sets the state of an LED.
        /// </summary>
        /// <param name="State">The state to set.</param>
        /// <param name="Index">The index of the LED to set.</param>
        public void SetLed(bool State, int Index)
        {
            this._Leds[Index] = State;
        }

        /// <summary>
        /// Toggles all LEDs in an index range.
        /// </summary>
        /// <param name="Start">The start index.</param>
        /// <param name="End">The end index.</param>
        public void ToggleLedRange(int Start, int End)
        {
            for (int Index = Start; Index <= End; Index++)
            {
                _Leds[Index] = !_Leds[Index];
            }
        }

        /// <summary>
        /// Sets all LEDs in an index range to a given state.
        /// </summary>
        /// <param name="Start">The start index.</param>
        /// <param name="End">The end index.</param>
        /// <param name="State">The state to set.</param>
        public void SetLedRange(int Start, int End, bool State)
        {
            for (int Index = Start; Index <= End; Index++)
            {
                _Leds[Index] = State;
            }
        }

        /// <summary>
        /// Toggles the state of the <see cref="KrakenXChannel"/>.
        /// </summary>
        public void ToggleState()
        {
            this._IsActive = !_IsActive;
        }

        /// <summary>
        /// Sets the state of the <see cref="KrakenXChannel"/>.
        /// </summary>
        /// <param name="State">The state to set.</param>
        public void SetState(bool State)
        {
            this._IsActive = State;
        }

        /// <summary>
        /// Turns all LEDs on.
        /// </summary>
        public void AllLedOn()
        {
            for (int index = 0; index < _Leds.Length; index++)
            {
                _Leds[index] = true;
            }
        }

        /// <summary>
        /// Turns all LEDs off.
        /// </summary>
        public void AllLedOff()
        {
            for (int index = 0; index < _Leds.Length; index++)
            {
                _Leds[index] = false;
            }
        }
    }
}
