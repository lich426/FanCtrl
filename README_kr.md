# FanCtrl

FanCtrl은 Desktop PC에서 팬을 제어하는 프로그램입니다.<br>

## 요구사항
- .NET 프레임워크 4.6 이상에서 동작합니다.<br>
- [Visual C++ 재배포 패키지 2019(x64)][15]가 설치되어 있어야 합니다.<br>
- OSD 기능은 [Rivatuner Statistics Server][16]가 설치되고 실행되어 있어야 합니다.<br>

## 지원
- 기가바이트 마더보드 : 만약 [AppCenter][2](기가바이트 유틸리티)가 설치되어 있으면 EasytuneEngineService와 통신해 센서, 팬, 제어를 할 수 있습니다.<br>
- 다른 제조사 마더보드 : [LibreHardwareMonitorLib][0] 또는 [OpenHardwareMonitorLib][1] 로 센서, 팬, 제어를 할 수 있습니다.<br>
- NZXT Kraken x2, x3 시리즈를 지원합니다.(z3 시리즈는 지원되지 않습니다)<br>
- EVGA CLC를 지원합니다.<br>
- NZXT RGB & Fan Controller를 지원합니다.<br>
- DIMM 온도 센서를 지원합니다.(예 : 지스킬 램의 온도센서)<br>

## 메인화면
![FanCtrl](https://github.com/lich426/FanCtrl/blob/master/img/1.png)<br>
온도, 팬 속도, 팬 제어를 볼 수 있습니다.<br>
팬 제어의 퍼센트를 변경해서 간단히 pwm 제어를 할 수 있습니다.(저장은 되지 않음)<br>

## 옵션
![Option](https://github.com/lich426/FanCtrl/blob/master/img/option.png)<br>
- Gigabyte : [AppCenter][2](기가바이트 유틸리티)가 설치되어 있으면, AppCenter를 통해 제어됩니다.<br>
  (없을 경우 LibreHardwareMonitor 또는 OpenHardwareMonitor로 제어됨)<br>
- LibreHardwareMonitor 또는 OpenHardwareMonitor : 센서, 팬 속도, 팬 제어 라이브러리를 어떤 것을 사용할 지 선택합니다.<br>
- NvAPIWrapper : NVIDIA 그래픽카드 제어 라이브러리를 NvAPIWrapper로 변경합니다.<br>
- DIMM sensor : DIMM 온도 센서를 지원합니다.(지스킬 램등)<br>
- NZXT Kraken : NZXT Kraken X2, X3 지원<br>
- EVGA CLC : EVGA CLC 지원<br>
- NZXT RGB & Fan Controller : NZXT RGB & Fan Controller 지원<br>
- 트레이아이콘 애니메이션 : 자동 팬 제어 활성화에 체크 시 트레이 아이콘에 팬이 돌아갑니다.<br>
- 화씨온도표시 : 온도를 화씨온도로 합니다.<br>
- 최소화 모드로 시작 : 프로그램 실행 시에 최소화로 시작됩니다.<br>
- 윈도우 시작 시 자동실행 : 윈도우 시작 시 자동으로 실행됩니다.<br>
- 지연시간(초) : 윈도우 시작 시 자동 실행되기 전에 딜레이 시간입니다.<br> 

## 팬 제어 설정
![Auto Fan Control](https://github.com/lich426/FanCtrl/blob/master/img/7.png)<br>
- 자동 팬 제어 활성화에 체크 후 타겟으로 설정할 온도 센서를 선택, 제어할 팬을 추가하고 리스트에서 선택하면 그래프가 나오는데 적당히 그래프를 조정해서 설정하면 됩니다.<br>
- 모드 : 4개로 이름만 나눠 놓은 것일 뿐, 직접 따로따로 설정해야 합니다.<br>
- 프리셋 : 현재 그래프의 설정상태를 저장 또는 로드할 수 있습니다.<br>
- 유닛 : 온도와 pwm 퍼센트의 단위를 변경 할 수 있습니다.(1, 5, 10)<br>
- 이력 온도 : 온도가 떨어질 때 pwm이 변하게 되면 이력온도만큼 더 떨어져야 pwm이 변하게 됩니다.<br>
- 계단형 : 그래프 설정을 계단형으로 설정할 지 선형으로 설정할 지 결정합니다.<br>
- 적용 및 확인을 누르면 파일로 저장되고 설정한 세팅으로 동작합니다.<br>
- 다음 프로그램 실행 시 자동으로 파일에서 읽어 동작합니다.<br>

## On Screen Display (RTSS)
![OSD](https://github.com/lich426/FanCtrl/blob/master/img/5.png)<br>
![OSD2](https://github.com/lich426/FanCtrl/blob/master/img/6.png)<br>
- OSD 기능은 [Rivatuner Statistics Server][16]가 설치되고 실행되어 있어야 합니다.<br>
- 그룹은 한 줄을 의미하고, 아이템은 하나의 데이터를 의미합니다.<br>

## 조명설정
조명설정은 각각 링크에서 패킷을 확인할 수 있고, 아래 예제와 같이 설정하면 됩니다.<br>
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

## 사용한 라이브러리들
Sensor and Fan Control : [LibreHardwareMonitorLib][0] or [OpenHardwareMonitorLib][1]<br>
Gigabyte board Sensor and Fan Control : [AppCenter][2]<br>
Nvidia Graphic card Sensor and Fan Control : [NvAPIWrapper][3]<br>
NZXT Kraken USB Communication : [HIDSharp][4]<br>
EVGA CLC USB Communication : [SiUSBXp][5] or [libusb-1.0][6]<br>
Json : [Newtonsoft Json][7]<br>
Graph : [ZedGraph][8]<br>

## 라이센스
[GNU General Public License v3.0][9]<br>

## 주의사항
 - FanCtrl은 GNU GPLv3 라이센스를 가진 무료 소프트웨어입니다.<br>
 - 다른 팬 제어 프로그램이 동시에 켜져 있는 상태에서는 오동작 할 수 있습니다.<br>
 - 이 소프트웨어를 사용하여 고장 나거나 작동하지 않는 하드웨어에 대해 책임을 지지 않습니다.<br>
 - 현재 모든 종류의 하드웨어를 지원하지 않습니다.<br>
 - AMD CPU의 경우 윈도우 시작 시에 센서가 제대로 로드되지 않을 수도 있으니, 옵션 - 지연시간을 10초 이상 주기 바랍니다.<br>

## 기부하기
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
[15]: https://support.microsoft.com/ko-kr/help/2977003/the-latest-supported-visual-c-downloads
[16]: https://www.guru3d.com/files-details/rtss-rivatuner-statistics-server-download.html
