# FanCtrl

FanCtrl is a software that allows you to automatically control the fan speed on your PC.<br>
Requires .NET framework 4.6 or higher.<br>
Requires [Visual redistributable 2019(x64)][15]<br><br>

Gigabyte motherboard : If [AppCenter][2] is installed, it is controlled by communicating with EasytuneEngineService.<br>
Other motherboard : Use [LibreHardwareMonitorLib][0] or [OpenHardwareMonitorLib][1] to obtain sensor temperature, fan rpm, and to control the fan.<br>
NZXT Kraken x2 and x3 is support (z3 series is not supported)<br>
EVGA CLC is support<br>
NZXT RGB & Fan Controller is support<br>
DIMM thermal sensor is support<br>

![FanCtrl](https://github.com/lich426/FanCtrl/blob/master/img/1.png)<br>
![Auto Fan Control](https://github.com/lich426/FanCtrl/blob/master/img/2.png)<br>
<br>
## On Screen Display (RTSS)
Requires [Rivatuner statistics server][16]<br>
GROUP stands for one line, and ITEM is the data to display.<br>
![OSD](https://github.com/lich426/FanCtrl/blob/master/img/5.png)<br>
![OSD2](https://github.com/lich426/FanCtrl/blob/master/img/6.png)<br>

## Lighting
You can see the lighting packet on the link.<Br>
NZXT Kraken X2 : [X2.txt][11]<br>
NZXT Kraken X3 : [X3.txt][12]<br>
EVGA CLC : [clc.txt][13]<br>
NZXT RGB & Fan Controller : [RGBnFC.txt][14]<br>
<br>
 Example X2<br>
![Lighting](https://github.com/lich426/FanCtrl/blob/master/img/3.png)<br>
Logo : Spectrum wave<br>
Ring : Spectrum wave<br>
<br>
![Lighting2](https://github.com/lich426/FanCtrl/blob/master/img/4.png)<br>
Logo : Fading<br>
Ring : Pulse<br>
<br>
## Using external Libraries
Sensor and Fan Control : [LibreHardwareMonitorLib][0] or [OpenHardwareMonitorLib][1]<br>
Gigabyte board Sensor and Fan Control : [AppCenter][2]<br>
Nvidia Graphic card Sensor and Fan Control : [NvAPIWrapper][3]<br>
NZXT Kraken USB Communication : [HIDSharp][4]<br>
EVGA CLC USB Communication : [SiUSBXp][5] or [libusb-1.0][6]<br>
Json : [Newtonsoft Json][7]<br>
Graph : [ZedGraph][8]<br>

## License
[GNU General Public License v3.0][9]

## Precautions
 - FanCtrl is free software with a GNU GPLv3 license.<br>
 - You can malfunction while other fan control programs are on at the same time.<br>
 - I am not responsible for hardware that has failed or is not working using this software.<br>
 - Not all types of hardware are supported.<br>

## Donate
<a href="https://www.buymeacoffee.com/lich" target="_blank"><img src="https://cdn.buymeacoffee.com/buttons/v2/default-yellow.png" alt="Buy Me A Coffee" style="height: 60px !important;width: 217px !important;"></a>

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
[15]: https://support.microsoft.com/en-us/help/2977003/the-latest-supported-visual-c-downloads
[16]: https://www.guru3d.com/files-details/rtss-rivatuner-statistics-server-download.html
