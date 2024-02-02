from bluedot import BlueDot
from picamera2 import Picamera2, Preview
from libcamera import controls
from signal import pause
import time
bd = BlueDot()
picam2 = Picamera2()
def take_picture():
   camera_config = picam2.create_still_configuration(main={"size": (1920, 1080)}, lores={"size": (1280, 720)}, display="lores")
   picam2.configure(camera_config)
   picam2.start()
   picam2.capture_file("/home/david/skullOS/Captures/" + str(int(time.time())) + ".jpg")
   picam2.stop()
bd.when_pressed = take_picture
pause()