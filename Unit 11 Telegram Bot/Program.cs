using System;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace Unit_11_Telegram_Bot
{
    class Program
    {
        static void Main(string[] args)
        {
            var bot = new BotWorker();
            bot.Inizalize();
            bot.Start();

            Console.WriteLine("Напишите stop для прекращения работы");

            string command;
            do
            {
                command = Console.ReadLine();
            } while (command != "stop");
            {
                bot.Stop();
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

    public class BotWorker
    {
        private ITelegramBotClient botClient;

        /// <summary>
        /// создаем клиент бота
        /// </summary>
        public void Inizalize()
        {
            botClient = new TelegramBotClient(BotCredentials.BotToken);
        }

        /// <summary>
        /// старт получения сообщений
        /// </summary>
        public void Start()
        {
            botClient.OnMessage += Bot_OnMessage;
            botClient.StartReceiving();
        }

        /// <summary>
        /// окончанчание получения сообщений
        /// </summary>
        public void Stop()
        {
            botClient.StopReceiving();
        }

        /// <summary>
        /// метод, который выполняется, когда к нам приходит сообщение
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            if (e.Message.Text != null)
            {
                Console.WriteLine($"Получено сообщение в чате: {e.Message.Chat.Id}.");

                await botClient.SendTextMessageAsync(
                chatId: e.Message.Chat, text: "Вы написали сообщение:\n" + e.Message.Text);
            }
        }
    }

    class BotMessageLogic
    {
        private BotMessageLogic logic;
        private ITelegramBotClient botClient;

        public void Inizalize()
        {
            botClient = new TelegramBotClient(BotCredentials.BotToken);
            logic = new BotMessageLogic();
        }
    }
}
