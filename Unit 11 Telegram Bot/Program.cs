using System;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace Unit_11_Telegram_Bot
{
    class Program
    {
        static ITelegramBotClient botClient;
        static void Main(string[] args)
        {
            
            // Теперь, используя токен, создадим клиент для работы с ботом:
            /// <summary>
            ///Это объект класса, который позволяет взаимодействовать с конкретным ботом. Этот объект обрабатывает все обращения к боту. 
            /// </summary>
            var botClient = new TelegramBotClient(BotCredentials.BotToken);

            /// <summary>
            ///Это базовый метод, он возвращает описание бота
            /// </summary>
            botClient.GetMeAsync();

            // проверка работоспособности
            var me = botClient.GetMeAsync().Result;
            Console.WriteLine("Всем привет! Меня зовут {0}.", me.FirstName);


            // плохое решение
            botClient = new TelegramBotClient(BotCredentials.BotToken);

            botClient.OnMessage += Bot_OnMessage;
            botClient.StartReceiving(); // старт получения сообщений

            Console.WriteLine("Нажмите любую кнопку для остановки");
            Console.ReadKey();

            botClient.StopReceiving(); // окончанчание получения сообщений

        }
        /// <summary>
        /// метод, который выполняется, когда к нам приходит сообщение
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static async void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            if (e.Message.Text != null)
            {
                Console.WriteLine($"Получено сообщение в чате: {e.Message.Chat.Id}.");

                await botClient.SendTextMessageAsync(
                chatId: e.Message.Chat, text: "Вы написали:\n" + e.Message.Text);
            }
        }
    }

    public static class BotCredentials
    {
        /// <summary>
        /// Use this token to access the HTTP API:
        /// 2136604647:AAH1ghd5fbxOhdW_-6ZU-fFgAIEOgXHUv-4
        /// </summary>
        public static readonly string BotToken = "2136604647:AAH1ghd5fbxOhdW_-6ZU-fFgAIEOgXHUv-4";


    }
}
