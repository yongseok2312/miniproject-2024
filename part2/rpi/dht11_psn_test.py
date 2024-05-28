# dht11_test.py
import adafruit_dht
import time
import RPi.GPIO as GPIO
import board

log_num = 0
sensor_pin = 18 
# RGB LED PinNumber setting
red_pin = 4
green_pin = 6
blue_pin = 5

GPIO.setmode(GPIO.BCM)
GPIO.setup(sensor_pin, GPIO.IN)
GPIO.setup(red_pin, GPIO.OUT) # 4pin output
dhtDevice = adafruit_dht.DHT11(board.D18) # Problem!!!

GPIO.output(red_pin, False)

while (True):
    try:
        temp = dhtDevice.temperature
        humid = dhtDevice.humidity
        print(f'{log_num} - Temp : {temp}C / Humid : {humid}%')
        log_num += 1

        if temp > 27:
            GPIO.output(red_pin, True)
        else:
            GPIO.output(red_pin, False)

        time.sleep(2)
    except RuntimeError as ex:
        print(ex.args[0])
    except KeyboardInterrupt:
        break

dhtDevice.exit()
