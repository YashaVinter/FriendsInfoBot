// See https://aka.ms/new-console-template for more information
using EntityFrameworkApp.FriendsBotLibrary;

namespace Program // Note: actual namespace depends on the project name.
{
    internal class Program
    {
        private const string token = "5156337859:AAFswaM91RTFckRSgA45jrhyKmYA77E0k14";
        static async Task Main(string[] args)
        {
            // start test
            var test = new EntityFrameworkApp.Test();
            //test.test10();
            //new Person().test();
            //end test
            var botClient = new FriendsBot(token);
            await botClient.StartBot();
        }
        /*
            var botClient = new FriendsBot(token);
            await botClient.StartBot();

            // start working code
            //var botClient = new TelegramBotClient(token);
            using var cts = new CancellationTokenSource();
            // StartReceiving does not block the caller thread. Receiving is done on the ThreadPool.
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = { } // receive all update types
            };
            botClient.telegramBotClient.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                receiverOptions,
                cancellationToken: cts.Token);
            var me = await botClient.telegramBotClient.GetMeAsync();
            Console.WriteLine($"Start listening for @{me.Username}");
            Console.ReadLine();
            // Send cancellation request to stop bot
            cts.Cancel();
         */
        //private static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        //{
        //    // Only process Message updates: https://core.telegram.org/bots/api#message
        //    if (update.Type != UpdateType.Message)
        //        return;
        //    // Only process text messages
        //    if (update.Message!.Type != MessageType.Text)
        //        return;

        //    string message = update.Message.Text;
        //    Console.WriteLine($"Received a '{update.Message.Text}' message from {update.Message.From}");

        //    // Echo received message text
        //    //Message message = new Message();
        //    var friendsBot = botClient as FriendsBot;
        //    //friendsBot.update = update;
        //    //FriendsBotData.StateTelegramActions.CaseHome(friendsBot, new());
        //    //Task.WaitAll();
        //    //FriendsBotData.StateTelegramActions.CaseHelp(friendsBot, new());
        //    //Task.WaitAll();

        //    friendsBot.Answer(update);
        //    Console.WriteLine($"Message '{update.Message.Text}'from {update.Message.From} send");
        //}
        //private static Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        //{
        //    var ErrorMessage = exception switch
        //    {
        //        ApiRequestException apiRequestException
        //            => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
        //        _ => exception.ToString()
        //    };

        //    Console.WriteLine(ErrorMessage);
        //    return Task.CompletedTask;
        //}

    }

}
/*TODO1
TODO разобраться с гитом, ветки и слияния
TODO SQL знать запросы нативные, миграции, джоины
TODO поиграться с БД, выборки, слияния, выбор средних знач и уникальных знач
TODO переделать актионы как класс с переопределением чтобы уйти от цепочки иф эсле иф элсе

 
 
 */

//before deliting unuseful elenemts
