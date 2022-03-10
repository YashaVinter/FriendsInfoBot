// See https://aka.ms/new-console-template for more information
using Microsoft.EntityFrameworkCore;

using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using EntityFrameworkApp;



namespace Program // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        private const string token = "5156337859:AAFswaM91RTFckRSgA45jrhyKmYA77E0k14";
        private static TelegramBot telegramBot = new TelegramBot();
        private static BotState botState;
        private static Person? person = new Person();
        static async Task Main(string[] args)
        {
            ////

            //person.test();
            //var p = person.Find("Артем");
            ////

            var botClient = new TelegramBotClient(token);

            using var cts = new CancellationTokenSource();

            // StartReceiving does not block the caller thread. Receiving is done on the ThreadPool.
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = { } // receive all update types
            };
            botClient.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                receiverOptions,
                cancellationToken: cts.Token);

            var me = await botClient.GetMeAsync();
            

            Console.WriteLine($"Start listening for @{me.Username}");
            Console.ReadLine();

            // Send cancellation request to stop bot
            cts.Cancel();

        }
        private static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            // Only process Message updates: https://core.telegram.org/bots/api#message
            if (update.Type != UpdateType.Message)
                return;
            // Only process text messages
            if (update.Message!.Type != MessageType.Text)
                return;

            var chatId = update.Message.Chat.Id;
            var messageText = update.Message.Text;

            Console.WriteLine($"Received a '{messageText}' message in chat {chatId}.");

            // Echo received message text
            Message message = new Message();
            switch (messageText)
            {
                case "Home":
                    message = await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: "buttons",
                        replyMarkup: HomeButtons()
                        );
                    botState = BotState.home;
                    break;
                case "Find":
                    message = await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: "Write person name. If you want see all persons write \"ALL\""
                        );
                    botState = BotState.find;
                    break;
                case "Edit":
                    /// TODO add adding persons
                    message = await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: "Write person name"
                        );
                    botState = BotState.edit;
                    break;
                case "Help":
                    message = await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: messageText,
                        replyMarkup: GetButtons()
                        );
                    break;
                default:
                    if (botState == BotState.find)
                    {
                        person =  person.Find(messageText);
                        if (person is null)
                        {
                            message = await botClient.SendTextMessageAsync(
                                chatId: chatId,
                                text: "Person not found, try again",
                                replyMarkup: GetButtons()
                                );
                            break;
                        }
                        message = await botClient.SendPhotoAsync(
                            chatId: chatId,
                            photo: person.photo
                            );
                        message = await botClient.SendTextMessageAsync(
                            chatId: chatId,
                            parseMode: ParseMode.MarkdownV2,
                            text: person.Print(),
                            replyMarkup: GetButtons()
                            );
                        botState = BotState.common;
                    }
                    break;
            }
            //Message sentMessage = await botClient.SendTextMessageAsync(
            //    chatId: chatId,
            //    text: "You said:\n" + messageText,
            //    cancellationToken: cancellationToken);
            //sentMessage = await botClient.SendTextMessageAsync(
            //    chatId: chatId,
            //    text: "buttons",
            //    replyMarkup: HomeButtons()
            //    );
            
        }

        private static Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }

        private static IReplyMarkup HomeButtons()
        {
            return new ReplyKeyboardMarkup
            (
                new List<List<KeyboardButton>> {
                    new List<KeyboardButton>{
                        new KeyboardButton ("Home"+ char.ConvertFromUtf32(0x1F4A5)),
                        new KeyboardButton ("Find"),
                        new KeyboardButton ("Edit"),
                        new KeyboardButton ("Help"),
                    }
                }
            );
        }

        private static IReplyMarkup GetButtons() {
            return null;
        }
        private enum BotState
        {
            home,
            find,
            edit,
            help,
            common
        }
    }

}

