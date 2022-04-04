using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

using StateMachineLibrary;
using EntityFrameworkApp.FriendsBotLibrary;
using EntityFrameworkApp.Data;

namespace EntityFrameworkApp.Data
{
    internal class FriendsBotData
    {
        public StateMachineData.States states { get; }
        public FrontendData frontendData { get; }
        public Criteria criteria { get; }
        public StateTelegramActions actions { get; }
        public FriendsBotData(StateMachineData.States states, FrontendData frontendData)
        {
            this.states = states;
            this.frontendData = frontendData;
            this.actions = new StateTelegramActions();
            this.criteria = new Criteria();
        }
        //public Dictionary<string, StateMachine.FunctionHandler> actionsDictionary;
        //public Dictionary<string, Func<string, bool>> criteriaDictionary;
        //private FrontendData frontendData { get; init; }
        //private Criteria criteria { get; init; }
        //private StateTelegramActions stateTelegramActions { get; init; }
        //public Dictionary<string,EventDataBase> eventDatabyState { get => frontendData.eventDatabyState; }
        //public Dictionary<string,FunctionHandler> actionByState { get => stateTelegramActions.actionByState; }
        //public Dictionary<string, Predicate<string>> criteriaByTransition { get => criteria.criteriaByTransition; }
        //public FriendsBotData(StateMachineData stateMachineData) {
        //    var smd = stateMachineData;
        //    //data for states
        //    //this.frontendData = new FrontendData(smd.states);
        //    //data for transitions
        //    //this.stateTelegramActions = new StateTelegramActions();
        //    //this.criteria = new Criteria();
        //}
        public class Criteria
        {
            //public Dictionary<string, Predicate<string>> criteriaByTransition { get; init; }
            ////public Criteria(StateMachine stateMachine, StateMachineData.States states)
            ////{ // TODO              
            ////    criteriaDictionary = new Dictionary<string, Predicate<string>>();
            ////    foreach (var transition in stateMachine.transitionDictionary.Values)
            ////    {
            ////        var criteria = this.getCriteria(transition.transitionModel.endState.name);
            ////        if (criteria is null)
            ////            throw new NotImplementedException("Dont implemented criteria");
            ////        criteriaDictionary.Add(transition.transitionModel.name, criteria);
            ////    }

            ////    this.states = states;
            ////    //stateToButtonText = FrontendData.ButtonData.getInstance(states).stateToButtonText;
            ////    //stateToButtonText = new FrontendDataNew()
            ////}
            //public Criteria(StateMachineData stateMachineData, FrontendData frontendData)
            //{
            //    var transitions = stateMachineData.transitions;
            //    var states = stateMachineData.states;
            //    //var eventDatabyState = frontendDataNew.eventDatabyState;
            //    // upcasting
            //    var buttonsData = frontendData.buttonsData;
                
            //    //creating dictionary criteria for all buttons
            //    var predicateByState = BuildPredicateByState(buttonsData);
            //    // creating dictionary simple transitions criteria
            //    var predicateByTransitionEqual = BuildEqualPredicates(transitions.transitionSets, predicateByState);
            //    //creating dictionary criteria for findPerson button
            //    var predicateByTransitionSpecial =
            //        BuildNotEqualPredicates(transitions.transitionSets.Where(t=>t.Split(':')[1] == states.findPerson), states.home, buttonsData);

            //    var predicateByTransition = new Dictionary<string, Predicate<string>>(
            //            predicateByTransitionEqual.Union(predicateByTransitionSpecial));
            //    if (predicateByTransition.Count() != transitions.transitionSets.Count)
            //    {
            //        throw new Exception("Not all transitions are assigned criteria");
            //    }
            //    criteriaByTransition = predicateByTransition;
            //}
            //private Dictionary<string, Predicate<string>> BuildPredicateByState
            //    (IEnumerable<FrontendData.ButtonData> buttonsData)
            //{
            //    var predicateByState = from bd in buttonsData
            //                           select new KeyValuePair<string, Predicate<string>>(
            //                               bd.stateName, EqualPredicate(bd.buttonText));
            //    return new Dictionary<string, Predicate<string>>(predicateByState);
            //}
            //private Dictionary<string, Predicate<string>> BuildEqualPredicates
            //    (IEnumerable<string> transitions, Dictionary<string, Predicate<string>> predicateByState)
            //{
            //    //var dict = new Dictionary<string, Predicate<string>>();
            //    //foreach (var transition in transitions)
            //    //{
            //    //    string endState = transition.Split(':')[1];
            //    //    if(predicateByState.ContainsKey(endState))
            //    //        dict.Add(transition, predicateByState[endState]);
            //    //}
            //    //return dict;
            //    var predicateByTransition = from tr in transitions
            //                                where predicateByState.ContainsKey(tr.Split(':')[1])
            //                                select new KeyValuePair<string,Predicate<string>>(
            //                                    tr,predicateByState[tr.Split(':')[1] ] );
            //    return new Dictionary<string, Predicate<string>>(predicateByTransition);
            //}
            //private Dictionary<string, Predicate<string>> BuildNotEqualPredicates
            //    (IEnumerable<string> transitions, string endState, IEnumerable<FrontendData.ButtonData> buttonsData)
            //{
            //    //var buttonText = buttonsData
            //    //    .Where(b => b.stateName == endState)
            //    //    .Select(b => b.buttonText)
            //    //    .FirstOrDefault();

            //    //var dict = new Dictionary<string, Predicate<string>>();

            //    //foreach (var transition in transitions)
            //    //{
            //    //    dict.Add(transition, NotEqualPredicate(buttonText));
            //    //}
            //    //return dict;

            //    var buttonText = (from bd in buttonsData
            //              where bd.stateName == endState
            //              select bd.buttonText)
            //             .FirstOrDefault();
            //    var predicateByTransition = from tr in transitions
            //                                select new KeyValuePair<string, Predicate<string>>(
            //                                    tr, NotEqualPredicate(buttonText));
            //    return new Dictionary<string, Predicate<string>>(predicateByTransition);

            //}
            public Predicate<string> EqualPredicate(string sample) 
            {
                return (string input) => { return sample == input; };
            }
            public Predicate<string> NotEqualPredicate(string sample)
            {
                return (string input) => { return sample != input; };
            }
            public Predicate<string> AlwaysTruePredicate()
            {
                return (string input) => { return true; };
            }
            //private FrontendData.ButtonData buttonData { get; init; } = new FrontendData.ButtonData();
            //public StateMachineData.States states { get; set; }
            //public Dictionary<string, string> stateToButtonText { get; set; }
            //public Predicate<string>? getCriteria(string toStateName) {
            //    StateMachineData.States states =
            //        StateMachineData.States.getInstance();

            //    if (toStateName == states.home)
            //    {
            //        return toHome;
            //    }
            //    else if (toStateName == states.find)
            //    {
            //        return toFind;
            //    }
            //    else if (toStateName == states.edit)
            //    {
            //        return toEdit;
            //    }
            //    else if (toStateName == states.help)
            //    {
            //        return toHelp;
            //    }
            //    else if (toStateName == states.findPerson)
            //    {
            //        return toFindPerson;
            //    }
            //    else
            //    {
            //        return null;
            //    }
            //}
            //public bool toHome(string input)
            //{
            //    return input == stateToButtonText[states.home];
            //}
            //public bool toFind(string input)
            //{
            //    return input == stateToButtonText[states.find];
            //}
            //public bool toEdit(string input)
            //{
            //    return input == stateToButtonText[states.edit];
            //}
            //public bool toHelp(string input)
            //{
            //    return input == stateToButtonText[states.help];
            //}
            //public bool toFindPerson(string input)
            //{
            //    return input != stateToButtonText[states.home];
            //}
            //public bool toTest(string input) {
            //    return input == stateToButtonText[states.home]; 
            //}            
        }
        public class StateTelegramActions
        {
            //public Dictionary<string,FunctionHandler> actionByState { get; set; }
            //public StateTelegramActions(StateMachineData.States states)
            //{
            //    states = states;
            //    //caseText = FrontendData.CaseText.getInstance(); ;

            //    actionsDictionary = new Dictionary<string, FunctionHandler>();
            //    foreach (var state in states.stateSets)
            //    {
            //        actionsDictionary.Add(state, this.getAction(state));
            //    }
            //}
            //public StateTelegramActions(StateMachineData stateMachineData) // new 
            //{
            //    var states = stateMachineData.states;
            //    var dict = new Dictionary<string, FunctionHandler> 
            //    {
            //        { states.home,DefaultCase },
            //        { states.find,DefaultCase },
            //        { states.edit,DefaultCase },
            //        { states.help,DefaultCase },
            //        { states.findPerson,CaseFindPerson},
            //    };
            //    actionByState = dict;
            //}
            //public FunctionHandler? getAction(string name) {
            //    StateMachineData.States states =
            //        StateMachineData.States.getInstance();

            //    if (name == states.home)
            //    {
            //        return CaseHome;
            //    }
            //    else if (name == states.find)
            //    {
            //        return CaseFind;
            //    }
            //    else if (name == states.edit)
            //    {
            //        return CaseEdit;
            //    }
            //    else if (name == states.help)
            //    {
            //        return CaseHelp;
            //    }
            //    else if (name == states.findPerson)
            //    {
            //        return CaseFindPerson;
            //    }
            //    else
            //    {
            //        return null;
            //    }
            //    //switch (name)
            //    //{
            //    //    case nameof(states.home):
            //    //        return CaseEdit;
            //    //    default:
            //    //        break;
            //    //}
            //}

            //public static async Task<Message> CaseHome2(object sender, EventArgs e) // CaseHome to CaseHome2
            //{ 
            //    if (sender is null)
            //        return null;
            //    if (sender is FriendsBot bot)
            //    {
            //        //FriendsBot.BotCommand botCommand = e as FriendsBot.BotCommand;
            //        //var command = botCommand.command;
            //        string text = caseText.stateToCaseText[states.home];
            //        long id =  (long) bot?.update?.Message?.Chat?.Id;
            //        return await bot.SendTextMessageAsync(id, text, HomeButtons2());
            //    }
            //    return null;
            //}

            public async Task<Message> DefaultCase(IStateData stateData)
            {
                try
                {
                    var data = stateData as StateData;
                    var inputData = data.inputData as BotInputData;
                    var eventData = data.eventData as EventData;

                    var bot = inputData.telegramBotClient;
                    return await bot.SendTextMessageAsync(
                        chatId:      inputData.message.Chat.Id,
                        text:        eventData.caseText,
                        parseMode:   Telegram.Bot.Types.Enums.ParseMode.MarkdownV2,
                        replyMarkup: eventData.buttons
                        );
                }
                catch (Exception)
                {
                    throw new NullReferenceException(); ;
                }
            }
            public async Task<Message> CaseFindPerson(IStateData stateData)
            {
                try
                {
                    var data = stateData as StateData;
                    var inputData = data.inputData as BotInputData;
                    var eventData = data.eventData as EventData;

                    var bot = inputData.telegramBotClient;
                    DataBase.Person person = new DataBase.Person().Find(inputData.command);
                    if (person is null)
                    {
                        return await bot.SendTextMessageAsync(
                            chatId: inputData.message.Chat.Id,
                            text: eventData.caseText,
                            parseMode: Telegram.Bot.Types.Enums.ParseMode.MarkdownV2,
                            replyMarkup: eventData.buttons
                            );
                    }
                    else
                    {
                        Message message = await bot.SendPhotoAsync(
                            chatId:      inputData.message.Chat.Id,
                            photo:       person.photo,
                            replyMarkup: eventData.buttons
                            );
                        message = await bot.SendTextMessageAsync(
                            chatId: inputData.message.Chat.Id,
                            text: person.Print(),
                            parseMode: Telegram.Bot.Types.Enums.ParseMode.MarkdownV2,
                            replyMarkup: eventData.buttons
                            );
                        return message;
                    }

                }
                catch (Exception)
                {
                    throw new NullReferenceException();
                }
            }

        }
        public class BotInputData : InputDataBase
        {
            public virtual TelegramBotClient telegramBotClient { get; set; }
            public virtual Message message { get; set; }
            public BotInputData(TelegramBotClient telegramBotClient, Message message) : base(message?.Text)
            {
                this.telegramBotClient = telegramBotClient;
                this.message = message;
            }
        }
    }
}
