# FanControl

FanControl is a software that allows you to automatically control the fan speed on your PC.<br>
Requires .NET framework 4.6 or higher.<br>
Gigabyte board : App Center installation is required (https://www.gigabyte.com/Support/Utility/Motherboard)<br>
NZXT Kraken : x2, x3 support (z3 series is not supported)<br>

![FanControl](https://github.com/lich426/FanControl/blob/master/img/1.png)<br>
![Auto Fan Control](https://github.com/lich426/FanControl/blob/master/img/2.png)

## Used Library
Sensor and Fan Control : [LibreHardwareMonitorLib][0], [OpenHardwareMonitorLib][1]<br>
Nvidia Graphic card Sensor and Fan Control : [NvAPIWrapper][2]<br>
NZXT Kraken Control : [NZXTSharp][3]<br>
USB Communication : [HidLibrary][4]<br>
Json : [Newtonsoft Json][5]<br>
Graph : [ZedGraph][6]<br>

## License
[GNU General Public License v3.0][7]

## Precautions
 - FanControl is free software with a GNU GPLv3 license.<br>
 - You can malfunction while other fan control programs are on at the same time.<br>
 - I am not responsible for hardware that has failed or is not working using this software.<br>
 - Not all types of hardware are supported.<br>
 
## Portable
Download : [FanControl_v1.0.9.zip][8]

[0]: https://github.com/LibreHardwareMonitor/LibreHardwareMonitor
[1]: https://github.com/openhardwaremonitor/openhardwaremonitor
[2]: https://github.com/falahati/NvAPIWrapper
[3]: https://github.com/akmadian/NZXTSharp
[4]: https://github.com/mikeobrien/HidLibrary
[5]: https://www.newtonsoft.com/json
[6]: http://zedgraph.sourceforge.net/samples.html
[7]: https://github.com/lich426/FanControl/blob/master/LICENSE
[8]: https://github.com/lich426/FanControl/raw/master/Portable/FanControl_v1.0.9.zip
