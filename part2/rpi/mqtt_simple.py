# mqtt_simple.py
import paho.mqtt.client as mqtt
import time

loop_num = 0

def onConnect(client, userdata, flags, reason_code, properties):
    print(f'Connected result code : {reason_code}')
    client.subscribe('pknu52/rcv')

def onMessage(client, userdata, msg):
    print(f'{msg.topic} +{msg.payload}')

mqttc = mqtt.Client(mqtt.CallbackAPIVersion.VERSION2) # 2023.9 이후 버전업
mqttc.on_connect = onConnect # 접속시 이벤트
mqttc.on_message = onMessage # messaging

# 192.168.5.2 window ip
mqttc.connect('210.119.12.52', 1883, 60)

mqttc.loop_start()
while True:
    mqttc.publish('pknu52/data', loop_num)
    loop_num += 1
    time.sleep(1)

mqttc.loop_stop()