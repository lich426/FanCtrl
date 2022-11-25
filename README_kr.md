# FanCtrl

FanCtrl은 Desktop PC에서 팬을 제어하는 프로그램입니다.<br>

## 요구사항
- .NET 프레임워크 4.7.2 에서 동작합니다.<br>
- [Visual C++ 재배포 패키지 2019(x64)][15]가 설치되어 있어야 합니다.<br>
- OSD 기능은 [Rivatuner Statistics Server][16]가 설치되고 실행되어 있어야 합니다.<br>

## 지원
- 마더보드를 지원합니다.(일부 미지원)<br>
- NZXT Kraken을 지원합니다.<br>
- EVGA CLC를 지원합니다.<br>
- NZXT RGB & Fan Controller를 지원합니다.<br>
- DIMM 온도 센서를 지원합니다.(예 : 지스킬 램의 온도센서)<br>
- liquidctl을 플러그인 형태로 지원합니다.

## 메인화면
![main_kr](https://user-images.githubusercontent.com/26077884/109587942-44aec480-7b4b-11eb-99d9-c8b9c8709101.png)<br>
- 온도, 팬 속도, 팬 제어를 볼 수 있습니다.<br>
- 팬 제어의 퍼센트를 변경해서 간단히 pwm 제어를 할 수 있습니다.(저장은 되지 않음)<br>
- 각 항목의 이름을 변경 할 수 있습니다.<br>

## 옵션
![1](https://user-images.githubusercontent.com/26077884/202845217-abef7d3d-982d-4b07-ba4e-626b4dda0261.png)<br>
- LibreHardwareMonitor : 라이브러리를 사용할 것인지 선택 가능하고, 제어에 필요한 디바이스를 선택 할 수 있습니다.<br>
- NvAPIWrapper : NVIDIA 그래픽카드 제어 라이브러리인 NvAPIWrapper를 추가할 수 있습니다.<br>
- DIMM sensor : DIMM 온도 센서를 지원합니다.(지스킬 램등)<br>
- NZXT Kraken : NZXT Kraken X2, X3 지원 (Z3 시리즈 미지원)<br>
- EVGA CLC : EVGA CLC 지원<br>
- NZXT RGB & Fan Controller : NZXT RGB & Fan Controller 지원<br>
- HWiNFO : [HWiNFO][17] 와 통신 해 센서 온도 및 rpm 가져옴 (참조 : [링크][18])<br>
- [liquidctl][19]을 플러그인 형태로 지원합니다.<br>
- 언어선택 : 영어, 한국어, 일본어<br>
- 트레이아이콘 애니메이션 : 자동 팬 제어 활성화에 체크 시 트레이 아이콘에 팬이 돌아갑니다.<br>
- 화씨온도표시 : 온도를 화씨온도로 합니다.<br>
- 최소화 모드로 시작 : 프로그램 실행 시에 최소화로 시작됩니다.<br>
- 윈도우 시작 시 자동실행 : 윈도우 시작 시 자동으로 실행됩니다.<br>
- 지연시간(초) : 윈도우 시작 시 자동 실행되기 전에 딜레이 시간입니다.<br>
- 초기화 : 모든 설정 및 라이브러리를 초기화 합니다.<br>

## 팬 제어 설정
![control_kr](https://user-images.githubusercontent.com/26077884/109588504-31e8bf80-7b4c-11eb-9cf9-4d6d43930383.png)<br>
- 자동 팬 제어 활성화에 체크 후 타겟으로 설정할 온도 센서를 선택, 제어할 팬을 추가하고 리스트에서 선택하면 그래프가 나오는데 적당히 그래프를 조정해서 설정하면 됩니다.<br>
- 모드 : 4개로 이름만 나눠 놓은 것일 뿐, 직접 따로따로 설정해야 합니다.<br>
- 프리셋 : 현재 그래프의 설정상태를 저장 또는 로드할 수 있습니다.<br>
- 유닛 : 온도와 pwm 퍼센트의 단위를 변경 할 수 있습니다.(1, 5, 10)<br>
- 이력 온도 : 온도가 떨어질 때 pwm이 변하게 되면 이력온도만큼 더 떨어져야 pwm이 변하게 됩니다.<br>
- 계단형 : 그래프 설정을 계단형으로 설정할 지 선형으로 설정할 지 결정합니다.<br>
- 오토 : 설정한 온도까지 팬 제어가 bios default 상태로 동작합니다. (LHM, OHM, Gigabyte, NvAPIWrapper만 가능, 참조 : [링크][18])<br>
- 적용 및 확인을 누르면 파일로 저장되고 설정한 세팅으로 동작합니다.<br>
- 다음 프로그램 실행 시 자동으로 파일에서 읽어 동작합니다.<br>

## On Screen Display (RTSS)
![osd_kr](https://user-images.githubusercontent.com/26077884/109588751-9c99fb00-7b4c-11eb-963c-249a76543e10.png)<br>
![osd2](https://user-images.githubusercontent.com/26077884/109588760-a15eaf00-7b4c-11eb-88cd-75881940499b.png)<br>
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
![lighting1](https://user-images.githubusercontent.com/26077884/109588878-c9e6a900-7b4c-11eb-9459-1d13b1d6ea3d.png)<br>
Logo : Spectrum wave<br>
Ring : Spectrum wave<br>
<br>
![lighting2](https://user-images.githubusercontent.com/26077884/109588888-ce12c680-7b4c-11eb-8ac1-d88d29b02435.png)<br>
Logo : Fading<br>
Ring : Pulse<br>

## liquidctl
사용법 : [링크참조][20]

## 플러그인
사용법 : [링크참조][21]

## 사용한 라이브러리들
Sensor and Fan Control : [LibreHardwareMonitorLib][0]<br>
Nvidia Graphic card Sensor and Fan Control : [NvAPIWrapper][3]<br>
NZXT Kraken USB Communication : [HIDSharp][4]<br>
EVGA CLC USB Communication : [SiUSBXp][5] or [libusb-1.0][6]<br>
Json : [Newtonsoft Json][7]<br>
Graph : [ZedGraph][8]<br>
liquidctl plugin : [liquidctl][19]

## 라이센스
[GNU General Public License v3.0][9]<br>

## 주의사항
 - FanCtrl은 GNU GPLv3 라이센스를 가진 무료 소프트웨어입니다.<br>
 - 다른 팬 제어 프로그램이 동시에 켜져 있는 상태에서는 오동작 할 수 있습니다.<br>
 - 이 소프트웨어를 사용하여 고장 나거나 작동하지 않는 하드웨어에 대해 책임을 지지 않습니다.<br>
 - 현재 모든 종류의 하드웨어를 지원하지 않습니다.<br>

## 기부하기
![donate2](https://user-images.githubusercontent.com/26077884/198750928-54814d12-5d1f-4f35-8a07-ab8c397b19d5.png)

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
