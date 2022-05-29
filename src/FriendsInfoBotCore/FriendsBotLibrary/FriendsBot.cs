using System;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;

using EntityFrameworkApp.DataBase;
using StateMachineLibrary;

using EntityFrameworkApp.Data;

namespace EntityFrameworkApp.FriendsBotLibrary
{

    public sealed class FriendsBot
    {
        private static TelegramBotClient telegramBotClient { get; set; } = null!;
        private static StateMachine stateMachine { get; set; } = null!;
        public FriendsBot(string token)
        {
            telegramBotClient = new TelegramBotClient(token);
            //StateMachineBuilder();
            stateMachine = BuildStateMachine();
        }
        private StateMachine BuildStateMachine()
        {
            var smd = StateMachineData.Instance();
            var fd = new FrontendData(smd.states);
            var fbd = new FriendsBotData(smd.states, fd);

            //var statesData = StateDataSetBuilder(fbd);
            //var transitionsData = TrasitionDataSetBuilder(fbd);
            //test
            var fbdn = new FriendsBotDataNew();
            var statesData = fbdn.BuildStatesDataSet();
            var transitionsData = fbdn.BuildTrasitionsDataSet();

            var factory = new TelegramStateMashineFactory(statesData, transitionsData, smd.states.home);
            return new StateMachine(factory);
        }
        private async Task Answer(Update update)
        {
            string text = update?.Message?.Text;
            var inputData = new BotInputData(telegramBotClient, update.Message);
            stateMachine.Execute(inputData);

            await Task.Delay(0);
        }
        public async Task StartBot() {
            using var cts = new CancellationTokenSource();

            // StartReceiving does not block the caller thread. Receiving is done on the ThreadPool.
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = { } // receive all update types
            };
            telegramBotClient.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                receiverOptions,
                cancellationToken: cts.Token);

            var me = await telegramBotClient.GetMeAsync();

            Console.WriteLine($"Start listening for @{me.Username}");
            Console.ReadLine();

            // Send cancellation request to stop bot
            cts.Cancel();
        }
        private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            // Only process Message updates: https://core.telegram.org/bots/api#message
            if (update.Type != UpdateType.Message)
                return;

            string? message = update?.Message?.Text;
            Console.WriteLine($"Received a '{update?.Message?.Text}' message from {update?.Message?.From}");
            await Answer(update);
            Console.WriteLine($"Message '{update?.Message?.Text}'from {update?.Message?.From} send");
        }
        private Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
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
        private IEnumerable<StateDataSet> StateDataSetBuilder(FriendsBotData fbd)
        {
            var states = fbd.states;
            var eventTextByState = fbd.frontendData.eventTextByState;
            var actions = fbd.actions;
            var botInputData = new BotInputData(null, null);
            var keyboards = fbd.frontendData.keyboards;

            StateDataSetBase defaultState = new StateDataSetBase("name", actions.DefaultCase, keyboards.homeKeyboard, botInputData);

            StateDataSet home = new StateDataSet(
                states.home, eventTextByState[states.home], actions.DefaultCase, keyboards.mainKeyboard, botInputData);
            StateDataSet find = new StateDataSet(states.find, eventTextByState[states.find],actions.DefaultCase,keyboards.homeAll,botInputData);
            StateDataSet edit = new StateDataSet(states.edit, eventTextByState[states.edit], defaultState);
            StateDataSet help = new StateDataSet(states.help, eventTextByState[states.help], defaultState);
            StateDataSet findPerson = new StateDataSet(
                states.findPerson, eventTextByState[states.findPerson], actions.CaseFindPerson, keyboards.homeKeyboard, botInputData);
            StateDataSet findAll = new("findAll", "not found", actions.CaseFindAll, keyboards.homeAll, botInputData);
            
            return new List<StateDataSet>
            {
                home,
                find,
                edit,
                help,
                findPerson,
                findAll
            };
        }
        private IEnumerable<TrasitionDataSet> TrasitionDataSetBuilder(FriendsBotData fbd) {
            var states = fbd.states;
            var buttonsTextByState = fbd.frontendData.buttonsTextByState;
            var tr = (string s1, string s2) => { return s1 + ':' + s2; };
            var end = (string s) => { return s.Split(':')[1]; };
            var criteria = fbd.criteria;

            var defaultTransitions = new List<string>
            {
                    tr(states.home,states.find),
                    tr(states.home,states.edit),
                    tr(states.home,states.help),
                    tr(states.find,states.home),
                    tr(states.findPerson,states.home),
                    tr(states.edit,states.home),
                    tr(states.help,states.home)
            };

            var trasitionsData = (from tn in defaultTransitions
                                  select new TrasitionDataSet(
                                      tn, criteria.EqualPredicate(buttonsTextByState[end(tn)] ) )).ToList();

            TrasitionDataSet specialTransition = new TrasitionDataSet(
                tr(states.find, states.findPerson),(string s) => { return (s != buttonsTextByState[states.home]) && (s != "All");  } ); // criteria.NotEqualPredicate(buttonsTextByState[states.home])
            trasitionsData.Add(specialTransition);
            // find all
            TrasitionDataSet findFindAll = new(tr(states.find, "findAll"), (string s) => { return s == "All"; });
            TrasitionDataSet findAllhome = new(tr("findAll", states.home), criteria.EqualPredicate(buttonsTextByState[states.home]));
            trasitionsData.Add(findFindAll);
            trasitionsData.Add(findAllhome);
            return trasitionsData;
        }
    }
}
