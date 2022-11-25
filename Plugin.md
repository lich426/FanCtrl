# Plugins

FanCtrl supports external programs through the socket communication plugin.<br>

## How to use it
![11](https://user-images.githubusercontent.com/26077884/203911641-785929b7-703b-40a9-b7dc-dc53cfb3ed71.png)
- If you check Plugin in the option, Plugin button is created on the main screen<br><br>
![3](https://user-images.githubusercontent.com/26077884/203911900-d8bea006-1439-4dde-b47a-9525d9ba0e69.png)
- ① Set up a server to support Plugins<br>
- ② The connected client is displayed<br>
- ③ Create a Temperature sensor (The same key cannot be used)<br>
- ④ Create a Fan speed (The same key cannot be used)<br>
- ⑤ Create a Fan control (The same key cannot be used)<br><br>
![4](https://user-images.githubusercontent.com/26077884/203912779-ae642268-cb6a-45a2-be6b-1c3888d83435.png)
- Once set, the Plugin sensor appears on the main screen.<br>

## Structure
![3333](https://user-images.githubusercontent.com/26077884/203915325-47bd8573-5c46-4843-980f-13a6486e0746.png)
- Send the temperature sensor and rpm data read by your program to FanCtrl.
- FanCtrl sends pwm data to your program.

## Packet
- The packet is simple. start with STX(0xFA) at the beginning, followed by data size, and data.<br>
- The data is a json string.<br><br>
![4444](https://user-images.githubusercontent.com/26077884/203922229-931b804f-0a28-40ff-b3e9-c4e3f352f990.png)

### Json
- The json string to send to FanCtrl can be sent as follows.<br>
- type : 0 (temperature), 1 (fan speed)<br>
```json
{
   "list" : [
     {
       "key" : "1",
       "type" : 0,
       "value" : 50
     },
     {
       "key" : "2",
       "type" : 1,
       "value" : 2000
     }
   ]
}
```

- The json string FanCtrl sends to your program is
- type : 2 (fan control)
```json
{
   "key" : "3",
   "type" : 2,
   "value" : 50
}
```
## Example
- https://github.com/lich426/FanCtrl_Plugin
