# FanControl

FanControl은 PC에서 자동으로 팬 속도를 제어할 수 있는 소프트웨어 입니다.<br>
닷넷 프레임워크 4.6 이상에서 정상 동작합니다.

## 사용한 라이브러리
센서 및 팬 제어 : [LibreHardwareMonitorLib][0]<br>
NZXT Kraken 제어 : [NZXTSharp][1]<br>
USB 통신 : [HidLibrary][2]<br>
Json : [Newtonsoft Json][3]<br>
Graph : [ZedGraph][4]<br>

## License
[GNU General Public License v3.0][5]

## 주의사항
 - FanControl은 GNU GPLv3 라이센스를 가진 무료 소프트웨어입니다.<br>
 - 다른 팬 제어 프로그램이 동시에 켜져 있는 상태에서는 오동작 할 수 있습니다.<br>
 - 이 소프트웨어를 사용하여 고장 나거나 작동하지 않는 하드웨어에 대해 책임을 지지 않습니다.<br>
 - 현재 모든 종류의 하드웨어를 지원하지 않습니다.<br>
 
## Portable
다운로드 : [FanControl_Portable_v1.0.0.zip][6]

[0]: https://github.com/LibreHardwareMonitor/LibreHardwareMonitor
[1]: https://github.com/akmadian/NZXTSharp
[2]: https://github.com/mikeobrien/HidLibrary
[3]: https://www.newtonsoft.com/json
[4]: http://zedgraph.sourceforge.net/samples.html
[5]: https://github.com/lich426/FanControl/blob/master/LICENSE
[6]: https://github.com/lich426/FanControl/raw/master/Portable/FanControl_Portable_v1.0.0.zip
