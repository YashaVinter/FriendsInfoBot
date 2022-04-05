//using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;

using EntityFrameworkApp.DataBase;
using StateMachineLibrary;

using EntityFrameworkApp.Data;
using Program;

namespace EntityFrameworkApp.FriendsBotLibrary
{

    public sealed class FriendsBot
    {
        private static TelegramBotClient telegramBotClient { get; set; }
        private static StateMachine stateMachine { get; set; }
        public FriendsBot(string token)
        {
            telegramBotClient = new TelegramBotClient(token);
            //StateMachineBuilder();
            stateMachine = BuildStateMachine();
        }
        //public Update update { get; set; }

        //private void StateMachineBuilder() {
        //    var smd = StateMachineData.Instance();
        //    stateMachine = new StateMachine(smd.states.stateSets, smd.transitions.transitionSets, smd.states.home);

        //    var friendsBotData = new FriendsBotData(smd);
        //    stateMachine.AddEventData(friendsBotData.eventDatabyState);
        //    stateMachine.AddFunctionHandler(friendsBotData.actionByState);
        //    stateMachine.AddCriteraRange(friendsBotData.criteriaByTransition);

        //}

        private StateMachine BuildStateMachine()
        {
            var smd = StateMachineData.Instance();
            var fd = new FrontendData(smd.states);
            var fbd = new FriendsBotData(smd.states, fd);

            var statesData = StateDataSetBuilder(fbd);
            var transitionsData = TrasitionDataSetBuilder(fbd);
            var factory = new TelegramStateMashineFactory(statesData, transitionsData, smd.states.home);
            return new StateMachine(factory);
        }
        //private StateMachine BuildStateMachine()
        //{
        //    var smd = StateMachineData.Instance();
        //    var states = smd.states;
        //    var eventTexts = FrontendData.EventText.Instance(states);
        //    var actions = new FriendsBotData.StateTelegramActions(smd);
        //    var botInputData = new FriendsBotData.BotInputData(this, null);

        //    var frontendData = new FrontendData(states);
        //    var keyboardBuilder = new FrontendData.KeyboardBuilder(frontendData.buttonsData);
        //    var mainKeyboard = keyboardBuilder.BuildKeyboard(new List<string>()
        //    {
        //        states.home,
        //        states.find,
        //        states.edit,
        //        states.help
        //    });
        //    var homeKeyboard = keyboardBuilder.BuildKeyboard(new List<string>()
        //    {
        //        states.home,
        //    });

        //    StateDataSetBase defaultState = new StateDataSetBase("defultnName", actions.DefaultCase, homeKeyboard, botInputData);

        //    StateDataSet home = new StateDataSet(states.home, eventTexts.home,actions.DefaultCase, mainKeyboard, botInputData);
        //    StateDataSet find = new StateDataSet(states.find, eventTexts.find, defaultState);
        //    StateDataSet edit = new StateDataSet(states.edit, eventTexts.edit, defaultState);
        //    StateDataSet help = new StateDataSet(states.help, eventTexts.help, defaultState);
        //    StateDataSet findPerson = new StateDataSet(states.findPerson, eventTexts.findPerson,actions.CaseFindPerson, homeKeyboard, botInputData);
        //    var statesData = new List<StateDataSet> 
        //    {
        //        home,
        //        find,
        //        edit,
        //        help,
        //        findPerson
        //    };
        //    //
        //    var tr = (string s1, string s2) => { return s1 + ':' + s2; };
        //    var criteria = new FriendsBotData.Criteria(smd, frontendData);
        //    var defaultTransitions = new List<string> 
        //    {
        //            tr(states.home,states.find),
        //            tr(states.home,states.edit),
        //            tr(states.home,states.help),
        //            tr(states.find,states.home),
        //            tr(states.findPerson,states.home),
        //            tr(states.edit,states.home),
        //            tr(states.help,states.home)
        //    };

        //    var trasitionsData = (from t in defaultTransitions
        //             select new TrasitionDataSet(t, criteria.EqualPredicate(t.Split(':')[1]) )).ToList(); // EqualPredicate(t.Split(':')[1])


        //    TrasitionDataSet tr1 = new TrasitionDataSet(tr(states.find, states.findPerson), criteria.NotEqualPredicate(states.home));
        //    trasitionsData.Add(tr1);

        //    var smb = new StateMashineBuilder(statesData, trasitionsData, states.home);
        //    return smb.Build();
        //}
        private async Task Answer(Update update)
        {
            string text = update?.Message?.Text;
            //this.update = update;
            //this.botCommand.command = this.update?.Message?.Text;
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
            // Only process text messages
            if (update.Message!.Type != MessageType.Text)
                return;

            string message = update.Message.Text;
            Console.WriteLine($"Received a '{update.Message.Text}' message from {update.Message.From}");
            // test
            //string str = @"[Link](example.com) (test)";
            //await botClient.SendTextMessageAsync(
            //    chatId: update.Message.Chat.Id,
            //    text: str,
            //    parseMode: Telegram.Bot.Types.Enums.ParseMode.Markdown
            //    );
            //
            await Answer(update);
            Console.WriteLine($"Message '{update.Message.Text}'from {update.Message.From} send");
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
        //private IEnumerable<StateDataSet> StateDataSetBuilderNew()
        //{
        //    var states = new { home = "home" };
        //    var eventTextByState = new Dictionary<string, string> 
        //    {
        //        { states.home,"Choose mode"}
        //    };

        //    throw new();
        //}
    }
}
