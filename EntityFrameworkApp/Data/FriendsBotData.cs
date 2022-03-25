using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

using StateMachineTest;
//using StateMachineLibrary;
using EntityFrameworkApp.FriendsBotLibrary;

namespace EntityFrameworkApp.Data
{
    internal class FriendsBotData
    {
        //public Dictionary<string, StateMachine.FunctionHandler> actionsDictionary;
        //public Dictionary<string, Func<string, bool>> criteriaDictionary;
        public FriendsBotData() {
            
        }
        //public Dictionary<string, FunctionHandler> GetActionsDictionary(ISet<string> statesNames)
        //{
        //    Dictionary<string, FunctionHandler> dict =
        //        new Dictionary<string, FunctionHandler>();
        //    StateTelegramActions actions = new StateTelegramActions(StateMachineData.States.getInstance());
        //    foreach (var stateName in statesNames)
        //    {
        //        FunctionHandler? act = actions.getAction(stateName);
        //        if (act is null)
        //            throw new ArgumentNullException();
        //        dict.Add(stateName, act);
        //    }
        //    return dict;
        //}

        //public Dictionary<string, Predicate<string>> GetCriteriaDictionary
        //    (ISet<string> transitionsNames)
        //{
        //    Dictionary<string, Predicate<string>> dict =
        //        new Dictionary<string, Predicate<string>>();
        //    Criteria criteria = new Criteria();
        //    foreach (var transitionName in transitionsNames)
        //    {
        //        string endState = transitionName.Split(':')[1];
        //        Predicate<string>? func = criteria.getCriteria(endState);
        //        if (func is null)
        //            throw new ArgumentNullException();
        //        dict.Add(transitionName, func);
        //    }
        //    return dict;
        //}

        //public class States
        //{
        //    public const string home = "home";
        //    public const string find = "find";
        //    public const string edit = "edit";
        //    public const string help = "help";
        //    public const string findPerson = "findPerson";
        //}
        public class Criteria
        {
            public Dictionary<string, Predicate<string>> criteriaDictionary { get; private set; }
            //public Criteria(IEnumerable<ITransition> transitions)
            //{
            //    criteriaDictionary = new Dictionary<string, Predicate<string>>();
            //    foreach (var transition in transitions)
            //    {
            //        var pred = this.getCriteria(transition.endState.name);
            //        if (pred is null)
            //            throw new NotImplementedException("Dont implemented criteria");
            //        criteriaDictionary.Add(transition.name, pred);
            //    }
            //}
            //public Criteria(StateMachineData.States states)
            //{
            //    this.states = states;
            //    stateToButtonText = new FrontendData.ButtonData(states).stateToButtonText;
            //}
            public Criteria(StateMachineTest.StateMachine stateMachine, StateMachineData.States states)
            {
                criteriaDictionary = new Dictionary<string, Predicate<string>>();
                foreach (var transition in stateMachine.transitionDictionary.Values)
                {
                    var criteria = this.getCriteria(transition.transitionModel.endState.name);
                    if (criteria is null)
                        throw new NotImplementedException("Dont implemented criteria");
                    criteriaDictionary.Add(transition.transitionModel.name, criteria);
                }

                this.states = states;
                stateToButtonText = new FrontendData.ButtonData(states).stateToButtonText;
            }

            //private FrontendData.ButtonData buttonData { get; init; } = new FrontendData.ButtonData();
            public StateMachineData.States states { get; set; }
            public Dictionary<string, string> stateToButtonText { get; set; }
            public Predicate<string>? getCriteria(string toStateName) {
                StateMachineData.States states =
                    StateMachineData.States.getInstance();

                if (toStateName == states.home)
                {
                    return toHome;
                }
                else if (toStateName == states.find)
                {
                    return toFind;
                }
                else if (toStateName == states.edit)
                {
                    return toEdit;
                }
                else if (toStateName == states.help)
                {
                    return toHelp;
                }
                else if (toStateName == states.findPerson)
                {
                    return toFindPerson;
                }
                else
                {
                    return null;
                }
            }
            public bool toHome(string input)
            {
                return input == stateToButtonText[states.home];
            }
            public bool toFind(string input)
            {
                return input == stateToButtonText[states.find];
            }
            public bool toEdit(string input)
            {
                return input == stateToButtonText[states.edit];
            }
            public bool toHelp(string input)
            {
                return input == stateToButtonText[states.help];
            }
            public bool toFindPerson(string input)
            {
                return input != stateToButtonText[states.home];
            }
            public bool toTest(string input) {
                return input == stateToButtonText[states.home]; 
            }
            
        }
        public class StateTelegramActions
        {
            public Dictionary<string,FunctionHandler> actionsDictionary { get; set; }
            private static FrontendData.CaseText caseText { get; set; }
            private static StateMachineData.States states { get; set; }
            public StateTelegramActions(StateMachineData.States states)
            {
                states = states;
                caseText = new FrontendData.CaseText(states);

                actionsDictionary = new Dictionary<string, FunctionHandler>();
                foreach (var state in states.stateSets)
                {
                    actionsDictionary.Add(state, this.getAction(state));
                }
            }
            public FunctionHandler? getAction(string name) {
                StateMachineData.States states =
                    StateMachineData.States.getInstance();

                if (name == states.home)
                {
                    return CaseHome;
                }
                else if (name == states.find)
                {
                    return CaseFind;
                }
                else if (name == states.edit)
                {
                    return CaseEdit;
                }
                else if (name == states.help)
                {
                    return CaseHelp;
                }
                else if (name == states.findPerson)
                {
                    return CaseFindPerson;
                }
                else
                {
                    return null;
                }
                //switch (name)
                //{
                //    case nameof(states.home):
                //        return CaseEdit;
                //    default:
                //        break;
                //}
            }
            //public async Task<Message> CaseHomeOld(FriendsBot botClient ,string cmd)
            //{
            //    string text = $"Choose mode: {States.home} {States.find} {States.edit} {States.help}";
            //    long id = (long) botClient?.update?.Message?.Chat?.Id;
            //    IReplyMarkup replyMarkup = null;

            //    var ret = await botClient.SendTextMessageAsync(id,text, HomeButtons());
            //    //var ret = await botClient.SendTextMessageAsync(update, text);
            //    return null;
            //}

            public static async Task<Message> CaseHome2(object sender, EventArgs e) // CaseHome to CaseHome2
            { 
                if (sender is null)
                    return null;
                if (sender is FriendsBot bot)
                {
                    //FriendsBot.BotCommand botCommand = e as FriendsBot.BotCommand;
                    //var command = botCommand.command;
                    string text = caseText.stateToCaseText[states.home];
                    long id =  (long) bot?.update?.Message?.Chat?.Id;
                    return await bot.SendTextMessageAsync(id, text, HomeButtons2());
                }
                return null;
            }
            public static async Task<Message> CaseHome(IStateModel model, CommandBase commandBase) // CaseHome to CaseHome2
            {
                try
                {
                    var data = commandBase as CaseHomeData;
                    var bot = data.bot;
                    return await bot.SendTextMessageAsync(
                        data.message.Chat.Id,
                        data.caseText,
                        data.buttons
                        );
                }
                catch (Exception)
                {
                    throw new NullReferenceException(); ;
                }
            }
            public static async Task<Message> CaseFind(object sender, EventArgs e)
            {
                if (sender is null)
                    return null;
                if (sender is FriendsBot bot)
                {
                    string text = caseText.stateToCaseText[states.find];
                    long id = (long)bot?.update?.Message?.Chat?.Id;
                    return await bot.SendTextMessageAsync(id, text, HomeButtons2());
                }
                return null;
            }
            public static async Task<Message> CaseFindPerson(object sender, EventArgs e)
            {
                if (sender is null)
                    return null;
                if (sender is FriendsBot bot)
                {
                    long id = (long)bot?.update?.Message?.Chat?.Id;
                    DataBase.Person person = new DataBase.Person().Find(bot.botCommand.command);
                    if (person is null)
                    {
                        string text = caseText.stateToCaseText[states.findPerson];
                        return await bot.SendTextMessageAsync(id, text, HomeButtons2());

                    }
                    else
                    {
                        Message message = await bot.SendPhotoAsync(id,person.photo);
                        message = await bot.SendTextMessageAsync(id, person.Print(), HomeButtons2());
                        return message;
                    }
                }
                return null;
            }
            public static async Task<Message> CaseEdit(object sender, EventArgs e)
            {
                if (sender is null)
                    return null;
                if (sender is FriendsBot bot)
                {
                    string text = caseText.stateToCaseText[states.edit];
                    long id = (long)bot?.update?.Message?.Chat?.Id;
                    return await bot.SendTextMessageAsync(id, text, HomeButtons2());
                }
                return null;
            }
            public static async Task<Message> CaseHelp(object sender, EventArgs e)
            {
                if (sender is null)
                    return null;
                if (sender is FriendsBot bot)
                {
                    string text = caseText.stateToCaseText[states.help];
                    //string text = "abc abc";
                    long id = (long)bot?.update?.Message?.Chat?.Id;
                    return await bot.SendTextMessageAsync(id, text, HomeButtons2());
                }
                return null;
            }

        }

        public static IReplyMarkup HomeButtons()
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
        public static IReplyMarkup HomeButtons2()
        {
            var states = StateMachineData.States.getInstance();
            FrontendData.ButtonData button = new FrontendData.ButtonData(states);
            return new ReplyKeyboardMarkup
            (
                new List<List<KeyboardButton>> {
                    new List<KeyboardButton>{
                        new KeyboardButton (button.stateToButtonText[states.home]),
                        new KeyboardButton (button.stateToButtonText[states.find]),
                        new KeyboardButton (button.stateToButtonText[states.edit]),
                        new KeyboardButton (button.stateToButtonText[states.help]),
                    }
                }
            );
        }
        public static IReplyMarkup DefaultButton()
        {
            var states = StateMachineData.States.getInstance();
            FrontendData.ButtonData button = new FrontendData.ButtonData(states);
            return new ReplyKeyboardMarkup
            (
                new List<List<KeyboardButton>> {
                    new List<KeyboardButton>{
                        new KeyboardButton (button.stateToButtonText[states.home])
                    }
                }
            );
        }

        // TEST
        public class EventData : EventDataBase
        {
            public virtual string caseText { get; set; }
            public virtual IReplyMarkup buttons { get; set; }
        }
        public class BotInputData : InputDataBase
        {
            public virtual FriendsBot bot { get; set; }
            public virtual Message message { get; set; }
            public BotInputData(FriendsBot bot, Message message) : base(message?.Text)
            {
                this.bot = bot;
                this.message = message;
            }
        }
        public class StateData : IStateData
        {
            public EventDataBase eventData { get; set; }  // new EventDataBase();
            public InputDataBase inputData { get; set; } = new BotInputData(null,null);
        }

        public class BotCommandBase : CommandBase
        {
            public virtual FriendsBot bot { get; set; }
            public virtual Message message { get; set; }
            public virtual string caseText { get; set; }
            public virtual IReplyMarkup buttons { get; set; }
            public BotCommandBase(FriendsBot bot, Message message) : base(message?.Text)
            {
                this.bot = bot;
                this.message = message;
                this.caseText = "Default text";
                this.buttons = DefaultButton();
            }
        }
        public class CaseHomeData : BotCommandBase
        {
            //public override FriendsBot bot { get; set; }
            //public override long chatId { get; set; }
            //public override string text { get; set; }
            //public override IReplyMarkup buttons { get; set; }
            public CaseHomeData(FriendsBot bot, Message message) : base(bot, message)
            {
                var states = StateMachineData.States.getInstance();
                var caseText = new FrontendData.CaseText(states);
                this.caseText = caseText.stateToCaseText[states.home];
                this.buttons = HomeButtons2();
            }
        }
    }
}
