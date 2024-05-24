# mqtt_simple.py
import paho.mqtt.client as mqtt
import time

loop_num = 0

def onConnect(client, userdata, flags, reason_code, properties):
    print(f'Connected result code : {reason_code}')
    client.subscribe('pknu_74/rcv')

def onMessage(client, userdata, msg):
    print(f'{msg.topic} + {msg.payload}')

mqttc = mqtt.Client(mqtt.CallbackAPIVersion.VERSION2) # 2023.9
mqttc.on_connect = onConnect
mqttc.on_message = onMessage

# ip
mqttc.connect('192.168.5.2',1883,60)

mqttc.loop_start()
while True:
    mqttc.publish('pknu74/data',loop_num)
    loop_num += 1
    time.sleep(1)

mqttc.loop_stop()