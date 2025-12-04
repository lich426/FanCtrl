# FanCtrl

FanCtrl is a software that allows you to automatically control the fan speed on your PC.<br>

## Requires
- .NET framework 4.7.2
- [Visual redistributable 2019(x64)][15]<br>
- The OSD feature must have the [Rivatuner Statistics Server][16] installed and running.<br>

## Support
- Motherboard<br>
- NZXT Kraken is support<br>
- EVGA CLC is support<br>
- NZXT RGB & Fan Controller is support<br>
- DIMM thermal sensor is support<br>
- liquidctl is support<br>

## Main
<img width="1049" height="635" alt="image" src="https://github.com/user-attachments/assets/3ef35376-9118-46f4-a321-d901b511be37" /><br>
- Show temperature, fan speed and fan control.<br>
- The percentage of the fan control can be changed to simply control the pwm.(not saved) <br>
- You can rename each item.<br>

## Option
<img width="211" height="801" alt="image" src="https://github.com/user-attachments/assets/2f4a8024-5e12-4ac6-97f7-2f53035c5894" /><br>
- LibreHardwareMonitor : You can choose whether to use the library or not, and you can choose which devices are required for control.<br>
- NvAPIWrapper : Allows you to add the NVIDIA graphics card control library.<br>
- NZXT Kraken : NZXT Kraken support<br>
- EVGA CLC : EVGA CLC support<br>
- NZXT RGB & Fan Controller : NZXT RGB & Fan controller support<br>
- HWiNFO : Communicated with [HWiNFO][17] to get sensor temperature and fan rpm ([Link][18]) <br>
- liquidctl : [liquidctl][19] support<br>
- Language : English, Korean, Japanese, French, Spanish, Russian, Chinese<br>
- Tray icon animation : tray icon animation starts when checked for automatic fan control activation.<br>
- Fahrenheit : set the temperature to Fahrenheit.<br>
- Start minimized : starts with minimal when the program runs.<br>
- Start with Windows : Auto-Run at windows start.<br>
- Delay(sec) : Delay time before auto-run at windows start.<br>
- Reset : Initialize all settings and libraries.<br>

## Auto Fan Control
![control](https://user-images.githubusercontent.com/26077884/109592420-99097280-7b52-11eb-88d8-55483dd935ad.png)<br>
- Check to enable automatic fan control, select the temperature sensor to target, add the fan to control, and select from the list to display the graph, but adjust the graph accordingly.<br>
- Mode : only four names are divided, but must be set separately.<br>
- Preset : Allows you to save or load the current graph's setup state.<br>
- Unit : You can change the unit of temperature and pwm percentage.(1, 5, 10) <br>
- Hysteresis : If the pwm changes when the temperature drops, the pwm changes only after the hysteresis temperature drops further.<br>
- Step : determines whether the graph setting is step or linear.<br>
- Auto : The fan control operates in the bios default state up to the set temperature. (LHM, OHM, Gigabyte, NvAPIWrapper only, see [link][18])<br>
- When you click Apply and OK, it is saved as a file and works with the settings you set.<br>
- Automatically reads from a file to act on the next program run.<br>

## On Screen Display (RTSS)
![osd](https://user-images.githubusercontent.com/26077884/109592729-1503ba80-7b53-11eb-9db2-6977f613c59d.png)<br>
![osd2](https://user-images.githubusercontent.com/26077884/109592732-16cd7e00-7b53-11eb-92bd-eb6b7321593f.png)<br>
- Requires [Rivatuner statistics server][16]<br>
- GROUP stands for one line, and ITEM is the data to display.<br>

## Lighting
You can see the lighting packet on the link.<Br>
NZXT Kraken X2 : [X2.txt][11]<br>
NZXT Kraken X3 : [X3.txt][12]<br>
EVGA CLC : [clc.txt][13]<br>
NZXT RGB & Fan Controller : [RGBnFC.txt][14]<br>
<br>
 Example X2<br>
 ![lighting1](https://user-images.githubusercontent.com/26077884/109592758-22b94000-7b53-11eb-9036-b0d69db31c51.png)<br>
Logo : Spectrum wave<br>
Ring : Spectrum wave<br>
<br>
![lighting2](https://user-images.githubusercontent.com/26077884/109592769-2947b780-7b53-11eb-868a-d17813774b12.png)<br>
Logo : Fading<br>
Ring : Pulse<br>

## liquidctl
How to use : [Link][20]

## Plugins
How to use : [Link][21]

## Using external Libraries
Sensor and Fan Control : [LibreHardwareMonitorLib][0]<br>
Nvidia Graphic card Sensor and Fan Control : [NvAPIWrapper][3]<br>
NZXT Kraken USB Communication : [HIDSharp][4]<br>
EVGA CLC USB Communication : [SiUSBXp][5] or [libusb-1.0][6]<br>
Json : [Newtonsoft Json][7]<br>
Graph : [ZedGraph][8]<br>
liquidctl plugin : [liquidctl][19]<br>

## License
[GNU General Public License v3.0][9]

## Precautions
 - FanCtrl is free software with a GNU GPLv3 license.<br>
 - You can malfunction while other fan control programs are on at the same time.<br>
 - I am not responsible for hardware that has failed or is not working using this software.<br>
 - Not all types of hardware are supported.<br>

## Donate
<a href="https://paypal.me/lich426" target="_blank"><img src="https://www.paypalobjects.com/webstatic/en_US/i/buttons/pp-acceptance-large.png"/></a></td></tr></table><!-- PayPal Logo --></a><br><br>
 Bitcoin(BTC) : bc1p36n3atpv7d477tpgxcn4rztvx7cst68lqn8qa6uhvxe803fu7akq3c9j0x<br>
 Ethereum(ETH) : 0xE431Af19a04926d461B97cD190e10F817155ABcC<br>

[0]: https://github.com/LibreHardwareMonitor/LibreHardwareMonitor
[1]: https://github.com/openhardwaremonitor/openhardwaremonitor
[2]: https://www.gigabyte.com/Support/Utility/Motherboard
[3]: https://github.com/falahati/NvAPIWrapper
[4]: https://www.zer7.com/software/hidsharp
[5]: https://www.silabs.com/products/development-tools/software/direct-access-drivers
[6]: https://libusb.info
[7]: https://www.newtonsoft.com/json
[8]: http://zedgraph.sourceforge.net/samples.html
[9]: https://github.com/lich426/FanCtrl/blob/master/LICENSE
[11]: https://github.com/lich426/FanCtrl/blob/master/Packet/X2.txt
[12]: https://github.com/lich426/FanCtrl/blob/master/Packet/X3.txt
[13]: https://github.com/lich426/FanCtrl/blob/master/Packet/clc.txt
[14]: https://github.com/lich426/FanCtrl/blob/master/Packet/RGBnFC.txt
[15]: https://support.microsoft.com/ko-kr/help/2977003/the-latest-supported-visual-c-downloads
[16]: https://www.guru3d.com/files-details/rtss-rivatuner-statistics-server-download.html
[17]: https://www.hwinfo.com
[18]: https://github.com/lich426/FanCtrl/releases/tag/v1.3.5
[19]: https://github.com/liquidctl/liquidctl
[20]: https://github.com/lich426/FanCtrl/releases/tag/v1.5.1
[21]: https://github.com/lich426/FanCtrl/blob/master/Plugin.md
