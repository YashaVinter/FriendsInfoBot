using EntityFrameworkApp.FriendsBotLibrary;

namespace Program
{
    internal class Program
    {
        private const string token = "";
        static async Task Main(string[] args)
        {
            // start test
            //var test = new EntityFrameworkApp.Test();
            //test.test10();
            //new Person().test();
            //end test
            var botClient = new FriendsBot(token);
            await botClient.StartBot();
        }
    }
}
// before deliting comments