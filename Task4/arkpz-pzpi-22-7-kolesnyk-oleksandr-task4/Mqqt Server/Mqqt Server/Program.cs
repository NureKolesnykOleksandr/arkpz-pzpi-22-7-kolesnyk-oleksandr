using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using MQTTnet;
using MQTTnet.Client;
using Newtonsoft.Json;

class Program
{
    private const string MqttBrokerAddress = "test.mosquitto.org";
    private const string MqttTopic = "MedMon";
    private const string ApiEndpoint = "https://localhost:5202/api/Device/SensorData/AB123";

    static async Task Main(string[] args)
    {
        Console.WriteLine("MQTT to HTTP Redirector started...");

        var mqttFactory = new MqttFactory();
        var mqttClient = mqttFactory.CreateMqttClient();

        var mqttOptions = new MqttClientOptionsBuilder()
            .WithTcpServer(MqttBrokerAddress, 1883)
            .Build();

        mqttClient.ConnectedAsync += async e =>
        {
            Console.WriteLine("Connected to MQTT Broker!");

            var subscribeOptions = mqttFactory.CreateSubscribeOptionsBuilder()
                .WithTopicFilter(f => f.WithTopic(MqttTopic))
                .Build();

            await mqttClient.SubscribeAsync(subscribeOptions);
            Console.WriteLine($"Subscribed to topic: {MqttTopic}");
        };

        mqttClient.DisconnectedAsync += e =>
        {
            Console.WriteLine("Disconnected from MQTT Broker!");
            return Task.CompletedTask;
        };

        mqttClient.ApplicationMessageReceivedAsync += async e =>
        {
            var message = e.ApplicationMessage?.Payload == null
                ? string.Empty
                : Encoding.UTF8.GetString(e.ApplicationMessage.Payload);

            Console.WriteLine($"Received MQTT Message: {message}");

            if (!string.IsNullOrWhiteSpace(message))
            {
                var parts = message.Split(',');
                if (parts.Length == 3)
                {
                    Console.WriteLine($"Temperature: {parts[0]} °C");
                    Console.WriteLine($"Oxygen Level: {parts[1]} %");
                    Console.WriteLine($"Pulse: {parts[2]} bpm");

                    await SendDataToApi(parts[0], parts[1], parts[2]);
                }
                else
                {
                    Console.WriteLine("Invalid data format received.");
                }
            }
        };

        try
        {
            await mqttClient.ConnectAsync(mqttOptions);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error connecting to MQTT broker: {ex.Message}");
            return;
        }

        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();

        await mqttClient.DisconnectAsync();
    }

    private static async Task SendDataToApi(string temperature, string oxygen, string pulse)
    {
        using (var httpClient = new HttpClient())
        {
            try
            {
                var payload = new
                {
                    heartRate = pulse,
                    bloodOxygenLevel = oxygen,
                    bodyTemperature = temperature,
                    activityLevel = "normal",
                    sleepPhase = "awake"
                };

                var jsonPayload = JsonConvert.SerializeObject(payload);
                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                Console.WriteLine($"Sending HTTP POST: {jsonPayload}");

                var response = await httpClient.PostAsync(ApiEndpoint, content);
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Data sent successfully!");
                }
                else
                {
                    Console.WriteLine($"Failed to send data. Status Code: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending data to API: {ex.Message}");
            }
        }
    }
}
