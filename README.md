# FanControl

FanControl is a software that allows you to automatically control the fan speed on your PC.<br>
Requires .NET framework 4.6 or higher.<br>
Gigabyte board : AppCenter installation is required (https://www.gigabyte.com/Support/Utility/Motherboard)<br>
NZXT Kraken : x2, x3 support (z3 series is not supported)<br>
EVGA CLC : support

![FanControl](https://github.com/lich426/FanControl/blob/master/img/1.png)<br>
![Auto Fan Control](https://github.com/lich426/FanControl/blob/master/img/2.png)

## Used Library
Sensor and Fan Control : [LibreHardwareMonitorLib][0], [OpenHardwareMonitorLib][1]<br>
Nvidia Graphic card Sensor and Fan Control : [NvAPIWrapper][2]<br>
NZXT Kraken USB Communication: [HidLibrary][3]
EVGA CLC USB Communication : [SiUSBXp][4] or [libusb-1.0][5]<br>
Json : [Newtonsoft Json][6]<br>
Graph : [ZedGraph][7]<br>

## License
[GNU General Public License v3.0][8]

## Precautions
 - FanControl is free software with a GNU GPLv3 license.<br>
 - You can malfunction while other fan control programs are on at the same time.<br>
 - I am not responsible for hardware that has failed or is not working using this software.<br>
 - Not all types of hardware are supported.<br>
 
## Portable
Download : [FanControl_v1.1.0.zip][9]

## Donate with PayPal
[![PayPal](https://www.paypalobjects.com/en_US/i/btn/btn_donateCC_LG.gif)](https://www.paypal.com/cgi-bin/webscr?cmd=_donations&business=AUCEJ8KGCNJTC&currency_code=USD&source=url)

[0]: https://github.com/LibreHardwareMonitor/LibreHardwareMonitor
[1]: https://github.com/openhardwaremonitor/openhardwaremonitor
[2]: https://github.com/falahati/NvAPIWrapper
[3]: https://github.com/mikeobrien/HidLibrary
[4]: https://www.silabs.com/products/development-tools/software/direct-access-drivers
[5]: https://libusb.info
[6]: https://www.newtonsoft.com/json
[7]: http://zedgraph.sourceforge.net/samples.html
[8]: https://github.com/lich426/FanControl/blob/master/LICENSE
[9]: https://github.com/lich426/FanControl/raw/master/Portable/FanControl_v1.1.0.zip
