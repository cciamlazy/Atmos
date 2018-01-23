#include <Adafruit_NeoPixel.h>

// IMPORTANT: To reduce NeoPixel burnout risk, add 1000 uF capacitor across
// pixel power leads, add 300 - 500 Ohm resistor on first pixel's data input
// and minimize distance between Arduino and first pixel.  Avoid connecting
// on a live circuit...if you must, connect GND first.

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
  colorWipe(strip.Color(0, 0, 0), 20);
  //strip.show(); // Initialize all pixels to 'off'
}

uint8_t inputStart;
uint8_t inputCommand;
uint8_t inputArg0;
uint8_t inputArg1;
uint8_t inputArg2;
uint8_t inputArg3;

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
	if(Serial.available() > 2) {
      delay(100);      
      inputArg0 = Serial.read();
      if(Serial.available() > 3) {
        delay(100);      
        inputArg1 = Serial.read();
        if(Serial.available() > 4) {
          delay(100);
          inputArg2 = Serial.read();
		  if(Serial.available() > 5) {
		    delay(100);
		    inputArg3 = Serial.read();
		  }
	    }
      }
    }
  }
  if(inputStart == 8) {
    switch (inputCommand) {
      case 0:
        colorWipe(strip.Color(0, 0, 0), 20);
        break;
	  case 1:
	    changeColor(strip.Color(0, 0, 0));
	    break;
	  case 2:
		setBrightness(inputArg0);
	    break;
	  case 3:
	    strip.setPixelColor(inputArg0, strip.Color(inputArg1, inputArg2, inputArg3));
	    break;
	  case 4:
	    changeColor(strip.Color(inputArg0, inputArg1, inputArg2));
	    break;
    }
	strip.show();
  }
}

void setBrightness(uint8_t i) {
  strip.setBrightness(i);
}

// From https://github.com/adafruit/Adafruit_NeoPixel/blob/master/examples/StrandtestBLE/StrandtestBLE.ino
void changeColor(uint32_t c) {
  for(uint16_t i=0; i<strip.numPixels(); i++) {
    strip.setPixelColor(i, c);
  }
  strip.show();
}

// From https://github.com/adafruit/Adafruit_NeoPixel/blob/master/examples/StrandtestBLE/StrandtestBLE.ino
// Fill the dots one after the other with a color
void colorWipe(uint32_t c, uint8_t wait) {
  for(uint16_t i=0; i<strip.numPixels(); i++) {
    strip.setPixelColor(i, c);
    delay(wait);
    strip.show();
    delay(wait);
  }
}

