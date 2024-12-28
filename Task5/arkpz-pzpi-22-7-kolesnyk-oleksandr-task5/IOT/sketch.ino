#include <Adafruit_Sensor.h>
#include <DHT_U.h>
#include <WiFi.h>
#include <PubSubClient.h>
#include <ESP32Servo.h>
#include <FastLED.h>
#include <OneWire.h>
#include <DallasTemperature.h>

// Налаштування для DS18B20
#define ONE_WIRE_BUS 0 // Пін GPIO0, до якого підключено DQ

OneWire oneWire(ONE_WIRE_BUS);
DallasTemperature sensors(&oneWire);

// Налаштування для DHT22
#define DHTPIN 12
#define DHTTYPE DHT22
DHT_Unified dht(DHTPIN, DHTTYPE);
uint32_t delayMS;

// Налаштування Wi-Fi
const char* ssid = "Wokwi-GUEST";
const char* password = "";

// Налаштування MQTT
const char* mqtt_server = "test.mosquitto.org";
const char* mqtt_topic = "MedMon";
const char* clientID = "ESP32-wokwi";

// Налаштування для LED та сервоприводу
#define LED 26
#define SERVO_PIN 4
#define LED_PIN 16
#define NUM_LEDS 16
Servo servo;
CRGB leds[NUM_LEDS];

// Змінні для даних
float temperature = 0.0;
float oxygen = 0.0;
int pulse = 0;

WiFiClient espClient;
PubSubClient mqttClient(espClient);

unsigned long previousMills = 0;
const long interval = 3000;
String msgStr = "";
float temp, hum;

void setup_wifi() {
  delay(10);
  WiFi.begin(ssid, password);
  while (WiFi.status() != WL_CONNECTED) {
    delay(500);
    Serial.print(".");
  }
  Serial.println("");
  Serial.println("WiFi підключено");
  Serial.println("IP-адреса: ");
  Serial.println(WiFi.localIP());
}

void reconnect() {
  while (!mqttClient.connected()) {
    if (mqttClient.connect(clientID)) {
      Serial.println("MQTT підключено");
      mqttClient.subscribe("lights");
      mqttClient.subscribe("servo");
      mqttClient.subscribe("lights/neopixel");
      Serial.println("Підписано на топіки");
    } else {
      Serial.print("Невдача, код помилки=");
      Serial.print(mqttClient.state());
      Serial.println(" спробуйте знову через 5 секунд");
      delay(5000);
    }
  }
}

// Обробка отриманих повідомлень
void callback(char* topic, byte* payload, unsigned int length) {
  Serial.print("Повідомлення отримано у топіку: ");
  Serial.println(topic);
  String data = "";
  for (int i = 0; i < length; i++) {
    data += (char)payload[i];
  }
  Serial.println(data);

  if (String(topic) == "lights") {
    if (data == "ON") {
      digitalWrite(LED, HIGH);
    } else {
      digitalWrite(LED, LOW);
    }
  } else if (String(topic) == "servo") {
    int degree = data.toInt();
    servo.write(degree);
  } else if (String(topic) == "lights/neopixel") {
    int red, green, blue;
    sscanf(data.c_str(), "%d,%d,%d", &red, &green, &blue);
    fill_solid(leds, NUM_LEDS, CRGB(red, green, blue));
    FastLED.show();
  }
}

// Відправка даних до MQTT
void sendDataToMQTT(float temperature, float oxygen, int pulse) {
  String payload = String(temperature) + "," + String(oxygen) + "," + String(pulse);
  char msg[payload.length() + 1];
  payload.toCharArray(msg, payload.length() + 1);

  if (mqttClient.connected()) {
    if (mqttClient.publish(mqtt_topic, msg)) {
      Serial.println("Дані успішно відправлені!");
    } else {
      Serial.println("Помилка відправки даних!");
    }
  } else {
    Serial.println("Помилка: MQTT клієнт не підключено!");
  }
}

void setup() {
  Serial.begin(115200);
  dht.begin();
  sensor_t sensor;
  dht.temperature().getSensor(&sensor);
  dht.humidity().getSensor(&sensor);

  pinMode(LED, OUTPUT);
  digitalWrite(LED, LOW);

  servo.attach(SERVO_PIN, 500, 2400);
  servo.write(0);

  FastLED.addLeds<WS2812, LED_PIN, GRB>(leds, NUM_LEDS);

  setup_wifi();
  mqttClient.setServer(mqtt_server, 1883);
  mqttClient.setCallback(callback);
}

void loop() {
  if (!mqttClient.connected()) {
    reconnect();
  }
  mqttClient.loop();

  unsigned long currentMillis = millis();
  if (currentMillis - previousMills >= interval) {
    previousMills = currentMillis;

    sensors.requestTemperatures();
    temperature = sensors.getTempCByIndex(0);

    if (temperature == DEVICE_DISCONNECTED_C) {
      Serial.println("Помилка: датчик температури не підключено!");
      delay(2000);
      return;
    }

    oxygen = 98.0 - (temperature - 36.6) * 0.5;
    pulse = 75 + (temperature - 36.6) * 10;

    if (oxygen < 90.0) oxygen = 90.0;
    if (oxygen > 100.0) oxygen = 100.0;
    if (pulse < 50) pulse = 50;
    if (pulse > 150) pulse = 150;

    Serial.print("Температура: ");
    Serial.print(temperature);
    Serial.println(" °C");

    Serial.print("Насичення киснем: ");
    Serial.print(oxygen);
    Serial.println(" %");

    Serial.print("Пульс: ");
    Serial.print(pulse);
    Serial.println(" уд/хв");

    sendDataToMQTT(temperature, oxygen, pulse);

    delay(1);
  }
}
