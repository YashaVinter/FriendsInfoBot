using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;


namespace EntityFrameworkApp.FriendsBot
{
    internal class FriendsBotData
    {
        //public Dictionary<string, StateMachine.FunctionHandler> actionsDictionary;
        //public Dictionary<string, Func<string, bool>> criteriaDictionary;
        public FriendsBotData() {
            
        }
        public Dictionary<string, StateMachine.FunctionHandler> GetActionsDictionary(List<string> statesNames)
        {
            Dictionary<string, StateMachine.FunctionHandler> dict =
                new Dictionary<string, StateMachine.FunctionHandler>();
            StateTelegramActions actions = new StateTelegramActions();
            foreach (var stateName in statesNames)
            {
                StateMachine.FunctionHandler? act = actions.getAction(stateName);
                if (act is null)
                    throw new ArgumentNullException();
                dict.Add(stateName, act);
            }
            return dict;
        }
        public Dictionary<string, Func<string,bool>> GetCriteriaDictionary
            (List<string> transitionsNames)
        {
            Dictionary<string, Func<string, bool>> dict =
                new Dictionary<string, Func<string, bool>>();
            Criteria criteria = new Criteria();
            foreach (var transitionName in transitionsNames)
            {
                string endState = transitionName.Split(':')[1];
                Func<string, bool>? func = criteria.getCriteria(endState);
                if (func is null)
                    throw new ArgumentNullException();
                dict.Add(transitionName, func);
            }
            return dict;
        }
        public class States
        {
            public const string home = "home";
            public const string find = "find";
            public const string edit = "edit";
            public const string help = "help";
            public const string findPerson = "findPerson";

        }
        public class Criteria
        {
            private FrontendData.ButtonData buttonData = new FrontendData.ButtonData();
            public Func<string, bool>? getCriteria(string toStateName) {
                StateMachine.StateMachineData.States states =
                    new StateMachine.StateMachineData.States();

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
                return input == buttonData.home;
            }
            public bool toFind(string input)
            {
                return input == buttonData.find;
            }
            public bool toEdit(string input)
            {
                return input == buttonData.edit;
            }
            public bool toHelp(string input)
            {
                return input == buttonData.help;
            }
            public bool toFindPerson(string input)
            {
                return input != buttonData.home;
            }
            public Predicate<string> test { get; set; } = (s) => { return s == ""; };
        }
        public class StateTelegramActions
        {
            private static FrontendData.CaseText caseText = new FrontendData.CaseText();

            public StateMachine.FunctionHandler? getAction(string name) {
                StateMachine.StateMachineData.States states =
                    new StateMachine.StateMachineData.States();

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
            public async Task<Message> CaseHomeOld(FriendsBot botClient ,string cmd)
            {
                string text = $"Choose mode: {States.home} {States.find} {States.edit} {States.help}";
                long id = (long) botClient?.update?.Message?.Chat?.Id;
                IReplyMarkup replyMarkup = null;

                var ret = await botClient.SendTextMessageAsync(id,text, HomeButtons());
                //var ret = await botClient.SendTextMessageAsync(update, text);
                return null;
            }

            public static async Task<Message> CaseHome(object sender, EventArgs e) {
                if (sender is null)
                    return null;
                if (sender is FriendsBot bot)
                {
                    //FriendsBot.BotCommand botCommand = e as FriendsBot.BotCommand;
                    //var command = botCommand.command;
                    string text = caseText.home;
                    long id =  (long) bot?.update?.Message?.Chat?.Id;
                    return await bot.SendTextMessageAsync(id, text, HomeButtons2());
                }
                return null;
            }
            public static async Task<Message> CaseFind(object sender, EventArgs e)
            {
                if (sender is null)
                    return null;
                if (sender is FriendsBot bot)
                {
                    string text = caseText.find;
                    long id = (long)bot?.update?.Message?.Chat?.Id;
                    return await bot.SendTextMessageAsync(id, text, HomeButtons2());
                }
                return null;
            }
            public static async Task<Message> CaseFindPerson(object sender, EventArgs e)//TODO makeit
            {
                if (sender is null)
                    return null;
                if (sender is FriendsBot bot)
                {
                    long id = (long)bot?.update?.Message?.Chat?.Id;
                    DataBase.Person person = new DataBase.Person().Find(bot.botCommand.command);
                    if (person is null)
                    {
                        string text = caseText.findPerson;
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
                    string text = caseText.edit;
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
                    string text = caseText.help;
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
            FrontendData.ButtonData button = new FrontendData.ButtonData();
            return new ReplyKeyboardMarkup
            (
                new List<List<KeyboardButton>> {
                    new List<KeyboardButton>{
                        new KeyboardButton (button.home),
                        new KeyboardButton (button.find),
                        new KeyboardButton (button.edit),
                        new KeyboardButton (button.help),
                    }
                }
            );
        }
    }
}
