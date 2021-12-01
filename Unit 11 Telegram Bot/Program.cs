using System;
using System.Collections.Generic;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;

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

    /// <summary>
    /// Токен
    /// </summary>
    public static class BotCredentials
    {
        /// <summary>
        /// Use this token to access the HTTP API:
        /// 2136604647:AAH1ghd5fbxOhdW_-6ZU-fFgAIEOgXHUv-4
        /// </summary>
        public static readonly string BotToken = "2136604647:AAH1ghd5fbxOhdW_-6ZU-fFgAIEOgXHUv-4";

    }

    /// <summary>
    /// Класс, отвечающий за работу клиента бота на верхнем уровне 
    /// </summary>
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

    /// <summary>
    ///  Объект этого класса должен управлять логикой обработки полученных сообщений через метод Response. Объект класса должен быть создан при инициализации BotWorker в объект logic. Запишите измененный метод Inizalize объекта BotWorker.
    /// </summary>
    class BotMessageLogic
    {

        private BotMessageLogic logic;
        private ITelegramBotClient botClient;


        private Messenger messanger;

        private Dictionary<long, Conversation> chatList;

        public void Inizalize()
        {
            botClient = new TelegramBotClient(BotCredentials.BotToken);
            logic = new BotMessageLogic();
        }
    }

    /// <summary>
    /// Этот класс представляет собой объект чата, так как чатов у нас может быть немало, нашему боту нужно отвечать каждому пользователю индивидуально. В этом классе нам требуется создать список сообщений и объект Телеграм-чата. Объекты должны быть приватные
    /// </summary>
    public class Conversation
    {
        private Chat telegramChat;

        private List<Message> telegramMessages;

        public Conversation(Chat chat)
        {
            telegramChat = chat;
            telegramMessages = new List<Message>();
        }
    }
}
