using HidApi;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace NESEps
{
    public static class TCPWorker
    {
        public static bool s_Connected = false;
        private static TcpClient s_TcpClient = null;
        private static NetworkStream s_NetworkStream = null;

        private static string ESP_IP_ADDRESS = "192.168.1.101";
        private const int ESP_PORT = 8888;
        private const int RECONNECT_DELAY_MS = 5000; // Задержка перед переподключением (5 секунд)
        private const int CONNECTION_TIMEOUT_MS = 3000; // Таймаут подключения (3 секунды)

        private static DateTime s_LastReconnectAttempt = DateTime.MinValue;
        private static string s_LastError = "";

        public static void Update()
        {
            if (s_Connected)
            {
                // Проверяем, что соединение все еще активно
                if (!IsConnectionAlive())
                {
                    s_Connected = false;
                    CloseConnection();
                    Console.WriteLine("Connection lost, will reconnect...");
                    return;
                }

                if (GamePadsManager.HasChange())
                {
                    try
                    {
                        ReadOnlySpan<byte> SendData = GamePadsManager.GetInputs();

                        // Отправляем данные на ESP32-C3
                        SendDataToESP(SendData);

                        // Опционально: получаем ответ от ESP32-C3
                        string response = ReceiveResponse();
                        if (!string.IsNullOrEmpty(response))
                        {
                            Console.WriteLine($"ESP32 response: {response}");
                            ProcessESPResponse(response);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error sending data: {ex.Message}");
                        s_Connected = false;
                        CloseConnection();
                    }
                }
            }
            else
            {
                Connect();
            }
        }

        public static void Connect()
        {
            // Не пытаемся переподключаться слишком часто
            if ((DateTime.Now - s_LastReconnectAttempt).TotalMilliseconds < RECONNECT_DELAY_MS)
                return;

            s_LastReconnectAttempt = DateTime.Now;

            try
            {
                Console.WriteLine($"Attempting to connect to {ESP_IP_ADDRESS}:{ESP_PORT}...");

                s_TcpClient = new TcpClient();

                // Асинхронное подключение с таймаутом
                var connectTask = s_TcpClient.ConnectAsync(ESP_IP_ADDRESS, ESP_PORT);
                if (connectTask.Wait(CONNECTION_TIMEOUT_MS))
                {
                    s_NetworkStream = s_TcpClient.GetStream();
                    s_Connected = true;
                    Console.WriteLine("Successfully connected to ESP32-C3!");

                    // Получаем приветственное сообщение от ESP32
                    string welcomeMsg = ReceiveResponse();
                    if (!string.IsNullOrEmpty(welcomeMsg))
                    {
                        Console.WriteLine($"Server welcome: {welcomeMsg}");
                    }
                }
                else
                {
                    throw new TimeoutException("Connection timeout");
                }
            }
            catch (Exception ex)
            {
                s_Connected = false;
                s_LastError = ex.Message;
                Console.WriteLine($"Failed to connect: {ex.Message}");
                CloseConnection();
            }
        }

        public static void CloseConnection()
        {
            try
            {
                s_NetworkStream?.Close();
                s_TcpClient?.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error closing connection: {ex.Message}");
            }
            finally
            {
                s_NetworkStream = null;
                s_TcpClient = null;
            }
        }

        private static bool IsConnectionAlive()
        {
            try
            {
                if (s_TcpClient == null || s_NetworkStream == null)
                    return false;

                // Проверяем, что клиент все еще подключен
                if (!s_TcpClient.Connected)
                    return false;

                // Проверяем, что сокет доступен для чтения/записи
                if (s_TcpClient.Client.Poll(0, SelectMode.SelectRead) &&
                    s_TcpClient.Client.Available == 0)
                    return false;

                return true;
            }
            catch
            {
                return false;
            }
        }

        private static void SendDataToESP(ReadOnlySpan<byte> data)
        {
            if (s_NetworkStream == null || !s_NetworkStream.CanWrite)
                throw new InvalidOperationException("Network stream is not available");

            // Преобразуем данные в байтовый массив
            byte[] sendBuffer = data.ToArray();

            // Отправляем данные
            s_NetworkStream.Write(sendBuffer, 0, sendBuffer.Length);

            Console.WriteLine($"Sent {sendBuffer.Length} bytes to ESP32");
        }

        private static string ReceiveResponse()
        {
            try
            {
                if (s_NetworkStream == null || !s_NetworkStream.CanRead)
                    return null;

                // Устанавливаем таймаут на чтение
                s_NetworkStream.ReadTimeout = 1000; // 1 секунда

                byte[] buffer = new byte[1024];
                int bytesRead = s_NetworkStream.Read(buffer, 0, buffer.Length);

                if (bytesRead > 0)
                {
                    string response = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                    return response.Trim();
                }
            }
            catch (TimeoutException)
            {
                // Таймаут - это нормально, просто нет ответа
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error receiving response: {ex.Message}");
            }

            return null;
        }

        private static string ConvertToCommand(byte[] data)
        {
            // Здесь нужно преобразовать данные от геймпада в команду для ESP32
            // Пример: data[0] - кнопки, data[1] - ось X, data[2] - ось Y и т.д.

            StringBuilder command = new StringBuilder();

            // Формируем команду в формате, понятном ESP32
            // Например: "joystick:1,2,3,4,5\r\n"
            command.Append("joystick:");

            for (int i = 0; i < data.Length; i++)
            {
                if (i > 0) command.Append(",");
                command.Append(data[i]);
            }

            command.Append("\r\n");
            return command.ToString();
        }

        private static void ProcessESPResponse(string response)
        {
            // Обрабатываем ответ от ESP32
            // Здесь можно логировать или реагировать на ответы сервера

            if (response.Contains("Unknown command"))
            {
                Console.WriteLine("ESP32 didn't recognize the command format");
            }
            else if (response.Contains("Free heap"))
            {
                Console.WriteLine($"ESP32 status: {response}");
            }
        }

        // Вспомогательный метод для отправки текстовых команд (для тестирования)
        public static void SendTextCommand(string command)
        {
            if (!s_Connected)
            {
                Console.WriteLine("Not connected to ESP32");
                return;
            }

            try
            {
                byte[] commandBytes = Encoding.ASCII.GetBytes(command + "\r\n");
                s_NetworkStream.Write(commandBytes, 0, commandBytes.Length);

                string response = ReceiveResponse();
                if (!string.IsNullOrEmpty(response))
                {
                    Console.WriteLine($"Response: {response}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending command: {ex.Message}");
                s_Connected = false;
                CloseConnection();
            }
        }

        // Метод для ручного переподключения
        public static void Reconnect()
        {
            CloseConnection();
            s_Connected = false;
            Connect();
        }

        // Получение статуса подключения с дополнительной информацией
        public static string GetConnectionStatus()
        {
            if (s_Connected && IsConnectionAlive())
            {
                return $"Connected to {ESP_IP_ADDRESS}:{ESP_PORT}";
            }
            else if (s_Connected)
            {
                return "Connection lost, reconnecting...";
            }
            else
            {
                return $"Disconnected. Last error: {s_LastError}";
            }
        }
    }
}