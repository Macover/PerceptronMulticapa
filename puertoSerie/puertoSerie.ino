#include <ArduinoJson.h>
//Variables
int trigger = 5;
int echo = 18;
float tiempo, distancia;

void setup() {
  pinMode(trigger, OUTPUT);
  pinMode(echo, INPUT);
  Serial.begin(9600);
}

void loop() {
  // put your main code here, to run repeatedly:    
    digitalWrite(trigger, LOW);
    delayMicroseconds(2);
    digitalWrite(trigger, HIGH);
    delayMicroseconds(10);
    digitalWrite(trigger, LOW);
    tiempo = pulseIn (echo, HIGH);
    distancia = (tiempo / 2) / 29.15;
    if(distancia > 150){
      distancia = 150;
    }
    Serial.println(distancia);
}
