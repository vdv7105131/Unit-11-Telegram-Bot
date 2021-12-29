using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
    /// Token to access the HTTP API
    /// </summary>
    public static class BotCredentials
    {
        /// <summary>
        /// Token to access the HTTP API
        /// </summary>
        public static readonly string BotToken = "2136604647:AAH1ghd5fbxsOhdW";

    }

    /// <summary>
    /// Класс, отвечающий за работу клиента бота на верхнем уровне 
    /// </summary>
    public class BotWorker
    {
        private ITelegramBotClient botClient;
        private BotMessageLogic logic;

        /// <summary>
        /// создаем клиент бота
        /// </summary>
        public void Inizalize()
        {
            botClient = new TelegramBotClient(BotCredentials.BotToken);
            logic = new BotMessageLogic(botClient);
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
            if (e.Message != null)
            {
                await logic.Response(e);
            }
        }
    }

    /// <summary>
    ///  Объект этого класса должен управлять логикой обработки полученных сообщений через метод Response. Объект класса должен быть создан при инициализации BotWorker в объект logic. Запишите измененный метод Inizalize объекта BotWorker.
    /// </summary>
    class BotMessageLogic
    {
        private Messenger messanger;

        private Dictionary<long,
        Conversation> chatList;

        private ITelegramBotClient botClient;

        public BotMessageLogic(ITelegramBotClient botClient)
        {
            messanger = new Messenger();
            chatList = new Dictionary<long, Conversation>();
            this.botClient = botClient;
        }

        public async Task Response(MessageEventArgs e)
        {
            var Id = e.Message.Chat.Id;

            if (!chatList.ContainsKey(Id))
            {
                var newchat = new Conversation(e.Message.Chat);

                chatList.Add(Id, newchat);
            }

            var chat = chatList[Id];

            chat.AddMessage(e.Message);

            await SendTextMessage(chat);

        }

        private async Task SendTextMessage(Conversation chat)
        {
            var text = messanger.CreateTextMessage(chat);

            await botClient.SendTextMessageAsync(
            chatId: chat.GetId(), text: text);
        }
    }

    /// <summary>
    /// Сообщение
    /// </summary>
    public class Messenger
    {
        public string CreateTextMessage(Conversation chat)
        {
            var delimiter = ",";
            var text = "Your history: " + string.Join(delimiter, chat.GetTextMessages().ToArray());

            return text;
        }
    }

    /// <summary>
    /// Беседа. 
    /// Этот класс представляет собой объект чата, так как чатов у нас может быть немало, 
    /// нашему боту нужно отвечать каждому пользователю индивидуально. 
    /// В этом классе нам требуется создать список сообщений и объект Телеграм-чата. Объекты должны быть приватные
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

        public void AddMessage(Message message)
        {
            telegramMessages.Add(message);
        }

        public List<string> GetTextMessages()
        {
            var textMessages = new List<string>();

            foreach (var message in telegramMessages)
            {
                if (message.Text != null)
                {
                    textMessages.Add(message.Text);
                }
            }

            return textMessages;
        }

        public long GetId() => telegramChat.Id;
    }
}
