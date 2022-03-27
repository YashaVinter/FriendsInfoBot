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
        //public Dictionary<string, StateMachine.FunctionHandler> actionsDictionary;
        //public Dictionary<string, Func<string, bool>> criteriaDictionary;
        public FriendsBotData() {
            
        }
        public class Criteria
        {
            public Dictionary<string, Predicate<string>> criteriaDictionary { get; init; }
            public Criteria(StateMachine stateMachine, StateMachineData.States states)
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
                stateToButtonText = FrontendData.ButtonData.getInstance(states).stateToButtonText;
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
                caseText = FrontendData.CaseText.getInstance(); ;

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
            public static async Task<Message> CaseHome(IStateData stateData) // CaseHome to CaseHome2
            {
                try
                {
                    var data = stateData as StateData;
                    var inputData = data.inputData as BotInputData;
                    var eventData = data.eventData as EventData;

                    var bot = inputData.bot;
                    return await bot.SendTextMessageAsync(
                        inputData.message.Chat.Id,
                        eventData.caseText,
                        eventData.buttons
                        );
                }
                catch (Exception)
                {
                    throw new NullReferenceException(); ;
                }
            }
            public static async Task<Message> CaseFind(IStateData stateData)
            {
                try
                {
                    var data = stateData as StateData;
                    var inputData = data.inputData as BotInputData;
                    var eventData = data.eventData as EventData;

                    var bot = inputData.bot;
                    return await bot.SendTextMessageAsync(
                        inputData.message.Chat.Id,
                        eventData.caseText,
                        eventData.buttons
                        );
                }
                catch (Exception)
                {
                    throw new NullReferenceException(); ;
                }
            }
            public static async Task<Message> CaseFindPerson(IStateData stateData)
            {
                try
                {
                    var data = stateData as StateData;
                    var inputData = data.inputData as BotInputData;
                    var eventData = data.eventData as EventData;

                    var bot = inputData.bot;
                    DataBase.Person person = new DataBase.Person().Find(inputData.command);
                    if (person is null)
                    {
                        return await bot.SendTextMessageAsync(
                            inputData.message.Chat.Id,
                            eventData.caseText,
                            eventData.buttons
                            );
                    }
                    else
                    {
                        Message message = await bot.SendPhotoAsync(
                            inputData.message.Chat.Id, 
                            person.photo,
                            eventData.buttons
                            );
                        return await bot.SendTextMessageAsync(
                            inputData.message.Chat.Id,
                            person.Print(),
                            eventData.buttons
                            );
                        return message;
                    }

                }
                catch (Exception)
                {
                    throw new NullReferenceException();
                }
            }
            public static async Task<Message> CaseEdit(IStateData stateData)
            {
                try
                {
                    var data = stateData as StateData;
                    var inputData = data.inputData as BotInputData;
                    var eventData = data.eventData as EventData;

                    var bot = inputData.bot;
                    return await bot.SendTextMessageAsync(
                        inputData.message.Chat.Id,
                        eventData.caseText,
                        eventData.buttons
                        );
                }
                catch (Exception)
                {
                    throw new NullReferenceException(); ;
                }
            }
            public static async Task<Message> CaseHelp(IStateData stateData)
            {
                try
                {
                    var data = stateData as StateData;
                    var inputData = data.inputData as BotInputData;
                    var eventData = data.eventData as EventData;

                    var bot = inputData.bot;
                    return await bot.SendTextMessageAsync(
                        inputData.message.Chat.Id,
                        eventData.caseText,
                        eventData.buttons
                        );
                }
                catch (Exception)
                {
                    throw new NullReferenceException(); ;
                }
            }

        }
        public class Events
        {
            public Dictionary<String, EventDataBase> eventsDictionary { get; set; }
            public Events()
            {
                var states = StateMachineData.States.getInstance();
                var caseText = FrontendData.CaseText.getInstance(); ;
                var buttons = new FrontendData.Buttons();
                var dict = new Dictionary<String, EventDataBase>();
                foreach (var state in states.stateSets)
                {
                    var eventData = new EventData()
                    {
                        caseText = caseText.stateToCaseText[state],
                        buttons = buttons.buttonsDictionary[state]
                    };
                    //var stateData = new StateData(null)
                    //{
                    //    eventData = eventData
                    //};
                    dict.Add(state, eventData);
                }
                eventsDictionary = dict;
            }
        }

        public static IReplyMarkup HomeButtons2()
        {
            var states = StateMachineData.States.getInstance();
            FrontendData.ButtonData button = FrontendData.ButtonData.getInstance(states);
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
        // TEST
        public class EventData : EventDataBase
        {
            public virtual string caseText { get; set; } = "Default text";
            public virtual IReplyMarkup buttons { get; set; } = new ReplyKeyboardMarkup(new KeyboardButton("Default text")); // DefaultButton()
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
    }
}
