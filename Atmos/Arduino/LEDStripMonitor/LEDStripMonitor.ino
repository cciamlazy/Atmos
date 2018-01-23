#include <Adafruit_NeoPixel.h>

#define PIN 6
 
// Parameter 1 = number of pixels in strip
// Parameter 2 = pin number (most are valid)
// Parameter 3 = pixel type flags, add together as needed:
//   NEO_KHZ800  800 KHz bitstream (most NeoPixel products w/WS2812 LEDs)
//   NEO_KHZ400  400 KHz (classic 'v1' (not v2) FLORA pixels, WS2811 drivers)
//   NEO_GRB     Pixels are wired for GRB bitstream (most NeoPixel products)
//   NEO_RGB     Pixels are wired for RGB bitstream (v1 FLORA pixels, not v2)
Adafruit_NeoPixel strip = Adafruit_NeoPixel(60, PIN, NEO_GRB + NEO_KHZ800);

void setup() {
  Serial.begin(9600); // 115200 ??
  
  strip.begin();
  strip.show(); // Initialize all pixels to 'off'
}

byte inputStart;
byte inputCommand;
byte inputArg0;
byte inputArg1;
byte inputArg2;

void loop() {
  // To see what is being sent
  /*if (Serial.available() > 0) {
    // read the incoming byte:
    incomingByte = Serial.read();
    
    // say what you got:
    Serial.print("I received: ");
    Serial.println(incomingByte, DEC);
  }*/

  // Deal with data being sent
  if(Serial.available() > 0) {
    //Read buffer
    inputStart = Serial.read();
    delay(100);    
    inputCommand = Serial.read();
    delay(100);      
    inputArg0 = Serial.read();
    if(Serial.available() > 3) {
      delay(100);      
      inputArg1 = Serial.read();
      if(Serial.available() > 4) {
        delay(100);
        inputArg2 = Serial.read(); 
      }
    }
  }
  if(inputStart == 8) {
    switch (inputCommand) {
      case 0:

        break;
    }
  }
}

void SetBrightness(int i) {
  strip.setBrightness(i);
}

