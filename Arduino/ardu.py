import serial
import time

ser = serial.Serial('COM3', 9600)
time.sleep(2)
ser.reset_input_buffer()

try:
    while True:
        # Отправляем команду для включения светодиода
        ser.write(b'H')
        time.sleep(1)  # Подождите 1 секунду

        # Отправляем команду для выключения светодиода
        ser.write(b'L')
        time.sleep(1)  # Подождите 1 секунду

except KeyboardInterrupt:
    print("Keyboard Interrupt")

finally:
    ser.close()
