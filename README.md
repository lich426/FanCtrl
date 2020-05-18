# FanControl

FanControl is a software that allows you to automatically control the fan speed on your PC.<br>
Requires .NET framework 4.6 or higher.<br>
<br>
Gigabyte board : AppCenter installation is required (https://www.gigabyte.com/Support/Utility/Motherboard)<br>
NZXT Kraken : x2, x3 support (z3 series is not supported)<br>
EVGA CLC : support

![FanControl](https://github.com/lich426/FanControl/blob/master/img/1.png)<br>
![Auto Fan Control](https://github.com/lich426/FanControl/blob/master/img/2.png)<br>
<br>
## Lighting
You can see the lighting packet on the link.<Br>
NZXT Kraeken X2 : [X2.txt][11]<br>
NZXT Kraeken X3 : [X3.txt][12]<br>
EVGA CLC : [clc.txt][13]<br>
<br>
 Example X2<br>
![Lighting](https://github.com/lich426/FanControl/blob/master/img/3.png)<br>
Logo : Spectrum wave, Ring : Spectrum wave<br>
<br>
![Lighting2](https://github.com/lich426/FanControl/blob/master/img/4.png)<br>
Logo : Fading, Ring : Pulse<br>
<br>

## Using external Libraries
Sensor and Fan Control : [LibreHardwareMonitorLib][0] or [OpenHardwareMonitorLib][1]<br>
Gigabyte board Sensor and Fan Control : [AppCenter][2]<br>
Nvidia Graphic card Sensor and Fan Control : [NvAPIWrapper][3]<br>
NZXT Kraken USB Communication : [HidLibrary][4]<br>
EVGA CLC USB Communication : [SiUSBXp][5] or [libusb-1.0][6]<br>
Json : [Newtonsoft Json][7]<br>
Graph : [ZedGraph][8]<br>

## License
[GNU General Public License v3.0][9]

## Precautions
 - FanControl is free software with a GNU GPLv3 license.<br>
 - You can malfunction while other fan control programs are on at the same time.<br>
 - I am not responsible for hardware that has failed or is not working using this software.<br>
 - Not all types of hardware are supported.<br>
 
## Portable
Download : [FanControl_v1.1.2.zip][10]

## Donate with PayPal
[![PayPal](https://www.paypalobjects.com/en_US/i/btn/btn_donateCC_LG.gif)](https://www.paypal.com/cgi-bin/webscr?cmd=_donations&business=AUCEJ8KGCNJTC&currency_code=USD&source=url)

[0]: https://github.com/LibreHardwareMonitor/LibreHardwareMonitor
[1]: https://github.com/openhardwaremonitor/openhardwaremonitor
[2]: https://www.gigabyte.com/Support/Utility/Motherboard
[3]: https://github.com/falahati/NvAPIWrapper
[4]: https://github.com/mikeobrien/HidLibrary
[5]: https://www.silabs.com/products/development-tools/software/direct-access-drivers
[6]: https://libusb.info
[7]: https://www.newtonsoft.com/json
[8]: http://zedgraph.sourceforge.net/samples.html
[9]: https://github.com/lich426/FanControl/blob/master/LICENSE
[10]: https://github.com/lich426/FanControl/raw/master/Portable/FanControl_v1.1.2.zip
[11]: https://github.com/lich426/FanControl/blob/master/Packet/X2.txt
[12]: https://github.com/lich426/FanControl/blob/master/Packet/X3.txt
[13]: https://github.com/lich426/FanControl/blob/master/Packet/clc.txt
