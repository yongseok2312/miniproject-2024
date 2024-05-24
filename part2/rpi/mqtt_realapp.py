# 온습도 센서데이터 통신, Rgb LED setting
# mqtt_simple.py
import paho.mqtt.client as mqtt
import RPi.GPIO as GPIO
import time
import adafruit_dht
import board
import datetime as dt
import json

red_pin = 4
green_pin = 6
dev_id = 'pknu74'
loop_num = 0
dht_pin = 18

## 초기화 시작
def onConnect(client, userdata, flags, reason_code, properties):
    print(f'Connected : {reason_code}')
    client.subscribe('pknu/rcv/')

def onMessage(client, userdata, msg):
    print(f'{msg.topic} + {msg.payload}')

GPIO.cleanup()
GPIO.setmode(GPIO.BCM)
GPIO.setup(red_pin, GPIO.OUT)
GPIO.setup(green_pin, GPIO.OUT) # LED 켜는것
GPIO.setup(dht_pin, GPIO.IN) # 온습도값을 Rpi에서 받는 것
dhtDevice = adafruit_dht.DHT11(board.D18)
## 초기화 끝

## 실행 시작

mqttc = mqtt.Client(mqtt.CallbackAPIVersion.VERSION2) # 2023.9
mqttc.on_connect = onConnect
mqttc.on_message = onMessage

# ip 주의 
mqttc.connect('192.168.5.2',1883,60)

mqttc.loop_start()
while True:

    time.sleep(2) # DHT11 2초 마다 갱신이 잘댐
    try:
    # 온습도 값 받아서 MQTT로 전송
        temp = dhtDevice.temperature
        humid = dhtDevice.humidity
        print(f'{loop_num} - Temp:{temp}/humid:{humid}')

        origin_data = { 'DEV_ID' : dev_id,
                        'CURR_DT' : dt.datetime.now().strftime('%Y-%m-%d %H:%M:%S'),
                        'TYPE' : 'TEMPHUMID',
                        'VALUE': f'{temp}|{humid}'} #dictionary data
        pub_data = json.dumps(origin_data,ensure_ascii=False)

        mqttc.publish('pknu/data/',pub_data)
        loop_num+=1
    except RuntimeError as ex:
        print(ex.args[0])
    except KeyboardInterrupt:
        break

mqttc.loop_stop()
dhtDevice.exit()