using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;
using StateMachineLibrary;

namespace EntityFrameworkApp.Data
{
    //public class FrontendData
    //{
    //    public Dictionary<String, IButton> stateToButton { get; init; }
    //    public Dictionary<string, string> stateToCaseText { get; init; }
    //    public FrontendData(StateMachineData stateMachineData)
    //    {

    //    }
    //    public class ButtonData
    //    {
    //        private static ButtonData instance = null!;
    //        public Dictionary<string, string> stateToButtonText { get; init; }

    //        private ButtonData(StateMachineData.States states)
    //        {
    //            string homeEmj = char.ConvertFromUtf32(0x1F3E0);
    //            string findEmj = char.ConvertFromUtf32(0x1F50D);
    //            string editEmj = char.ConvertFromUtf32(0x2699);
    //            string helpEmj = char.ConvertFromUtf32(0x1F4DA);
    //            var createPair = delegate (string name, string emoji)
    //            {
    //                return new KeyValuePair<string, string>(name, name + emoji);
    //            };
    //            IEnumerable<KeyValuePair<string, string>> collection = new[]
    //            {
    //                createPair(states.home,homeEmj),
    //                createPair(states.find,findEmj),
    //                createPair(states.edit,editEmj),
    //                createPair(states.help,helpEmj),
    //            };
    //            stateToButtonText = new Dictionary<string, string>(collection);
    //        }
    //        public static ButtonData getInstance(StateMachineData.States states = default!)
    //        {
    //            if (states is null)
    //                throw new InvalidOperationException("Dont added initial states");
    //            if (instance is null)
    //                instance = new ButtonData(states);
    //            return instance;
    //        }

    //    }
    //    public class Buttons2
    //    {
    //        public Dictionary<String, IButton> stateToButton { get; init; }
    //        private StateMachineData.States states { get; init; }
    //        public Buttons2(StateMachineData.States states)
    //        {
    //            this.states = states;
    //            stateToButton = ButtionBuilder(states);

    //        }

    //        private Dictionary<string, IButton> ButtionBuilder(StateMachineData.States states)
    //        {
    //            var dict = new Dictionary<string, IButton>();
    //            // home button
    //            var b1 = new Button(null, null);


    //            return null;
    //        }


    //        public class Button : IButton
    //        {
    //            public string text { get; set; }
    //            public IReplyMarkup buttons { get; set; }
    //            public Button(string text, IReplyMarkup buttons)
    //            {
    //                this.text = text;
    //                this.buttons = buttons;
    //            }
    //        }
    //    }
    //    public interface IButton
    //    {
    //        string text { get; set; }
    //        public IReplyMarkup buttons { get; set; }
    //    }
    //    public class Buttons
    //    {
    //        public Dictionary<String, IReplyMarkup> buttonsDictionary { get; set; }
    //        public Buttons()
    //        {
    //            var states = StateMachineData.States.getInstance();
    //            //var caseText = new FrontendData.CaseText(states);
    //            var buttonData = FrontendData.ButtonData.getInstance(states);
    //            var dict = new Dictionary<String, IReplyMarkup>();

    //            var homeButtons = ButtonsBuilder(new List<string>
    //            {
    //                buttonData.stateToButtonText[states.home]
    //            });
    //            var defaultButtons = ButtonsBuilder(new List<string>
    //            {
    //                buttonData.stateToButtonText[states.home],
    //                buttonData.stateToButtonText[states.find],
    //                buttonData.stateToButtonText[states.edit],
    //                buttonData.stateToButtonText[states.help]
    //            });

    //            dict.Add(states.home, defaultButtons);
    //            dict.Add(states.find, homeButtons);
    //            dict.Add(states.findPerson, homeButtons);
    //            dict.Add(states.edit, homeButtons);
    //            dict.Add(states.help, homeButtons);
    //            buttonsDictionary = dict;
    //        }
    //        private IReplyMarkup ButtonsBuilder(IEnumerable<string> buttonsNames)
    //        {
    //            var b1 = new List<KeyboardButton>();
    //            foreach (var buttonName in buttonsNames)
    //            {
    //                b1.Add(new KeyboardButton(buttonName));
    //            }
    //            var b2 = new List<List<KeyboardButton>> { b1 };
    //            var kb = new ReplyKeyboardMarkup(b2);
    //            kb.ResizeKeyboard = true;
    //            return kb;

    //            //var v = new ReplyKeyboardMarkup(null);
    //        }
    //    }
    //    public class CaseText
    //    {
    //        private static CaseText instance = null!;
    //        public Dictionary<string, string> stateToCaseText { get; set; }
    //        private CaseText(StateMachineData.States states)
    //        {
    //            string home = $"Choose mode: {states.home} {states.find} {states.edit} {states.help}";
    //            string find = "Write person name, If you want see all persons write \"ALL\"";
    //            string findPerson = "Person not found, try again or return home";
    //            string edit = "EDIT Write person name, If you want add or edit person";
    //            string help = "HELP Its a friendBot, Here you can add informations about your friends";

    //            stateToCaseText = new Dictionary<string, string>()
    //            {
    //                { states.home,home},
    //                { states.find,find},
    //                { states.edit,edit},
    //                { states.help,help},
    //                { states.findPerson,findPerson}
    //            };

    //        }

    //        public static CaseText getInstance(StateMachineData.States states = default!)
    //        {
    //            if (states is null)
    //                throw new InvalidOperationException("Dont added initial states");
    //            if (instance is null)
    //                instance = new CaseText(states);
    //            return instance;
    //        }
    //    }

    //}


    /// <summary>
    /// //////////////////////////////////////////////////////////////////////
    /// </summary>
    public class FrontendDataNew
    {
        private Dictionary<string, IReplyMarkup> keyboardByState { get; init; }
        private Dictionary<string, string> eventTextByState { get; init; }
        public Dictionary<string, EventDataBase> eventDatabyState { get; set; }
        //public Dictionary<string,> MyProperty { get; set; }
        //TODO перенести класс FriendsBotData.EventData в фронтенд и в FriendsBotData убрать текущее использование
        //public EventData eventData { get; set; }
        private StateMachineData.States states { get; set; }
        public FrontendDataNew(StateMachineData.States states)
        {
            this.states = states;
            this.keyboardByState = BuildKeyboards(states);
            this.eventTextByState = BuildEventTexts(states);
            this.eventDatabyState = BuildEventData();
        }

        private Dictionary<string, EventDataBase> BuildEventData()
        {

            //var v1 = eventTextByState.Join(keyboardByState,e => e,k => k,(e,k) = >  )
            IEnumerable<KeyValuePair<string, EventDataBase>> pairs = from e in eventTextByState
                     join k in keyboardByState on e.Key equals k.Key
                     select new KeyValuePair<string, EventDataBase>(e.Key, new EventData(e.Value, k.Value));
            var dict = new Dictionary<string, EventDataBase>(pairs);
            return dict;
        }

        private Dictionary<string, IReplyMarkup> BuildKeyboards(StateMachineData.States states)
        {
            var homeButton = new ButtonData(states.home, J3QQ4.Emoji.House);
            var findButton = new ButtonData(states.find, J3QQ4.Emoji.Mag_Right);
            var editButton = new ButtonData(states.edit, J3QQ4.Emoji.Pencil);
            var helpButton = new ButtonData(states.help, J3QQ4.Emoji.Books);

            var keyboardBuilder2 = new KeyboardBuilder(new HashSet<ButtonData>() 
            {
                homeButton,
                findButton,
                editButton,
                helpButton
            });
            var mainKeyboard = keyboardBuilder2.BuildKeyboard(new List<string>()
            {
                states.home,
                states.find,
                states.edit,
                states.help
            });
            var homeKeyboard = keyboardBuilder2.BuildKeyboard(new List<string>()
            {
                states.home,
            });

            var dict = new Dictionary<string, IReplyMarkup>();
            dict.Add(states.home,       mainKeyboard);
            dict.Add(states.find,       homeKeyboard);
            dict.Add(states.edit,       homeKeyboard);
            dict.Add(states.help,       homeKeyboard);
            dict.Add(states.findPerson, homeKeyboard);

            return dict;
        }
        private Dictionary<string, string> BuildEventTexts(StateMachineData.States states)
        {
            var eventText = new EventText();
            var func = delegate(string st, string eventText) {  
                return new KeyValuePair<string, string>(st, eventText);
            };
            var dict = new Dictionary<string, string>();
            
            dict.Add(states.home, eventText.home);
            dict.Add(states.find, eventText.find);
            dict.Add(states.edit, eventText.edit);
            dict.Add(states.help, eventText.help);
            dict.Add(states.findPerson, eventText.findPerson);

            return dict;
        }
        public class KeyboardBuilder
        {
            public ISet<ButtonData> buttons { get; set; }

            //public Dictionary<string, ReplyKeyboardMarkup> keyboardByState { get; set; } // ReplyMarkupBase
            public KeyboardBuilder(ISet<ButtonData> buttons)
            {
                this.buttons = buttons;
                //keyboardByState = new Dictionary<string, ReplyKeyboardMarkup>();
            }
            public IReplyMarkup BuildKeyboard(List<string> keyboardButtonsNames)
            {
                try
                {
                    //if (this.keyboardByState.ContainsKey(buttonStateName))
                    //    throw new("Keyboard already exist");
                    var buttonsList = buttons
                        .Where(b1 => keyboardButtonsNames.Any(b2 => b2 == b1.stateName))
                        .OrderBy(b3 => keyboardButtonsNames.IndexOf(b3.stateName))
                        .Select(b4 => b4.button as KeyboardButton)
                        .ToList();

                    if (buttonsList.Count != keyboardButtonsNames.Count)
                        throw new("Dont not Implemented some buttons");

                    var replyKeyboardMarkup = new ReplyKeyboardMarkup(
                        new List<List<KeyboardButton>> { buttonsList });
                    replyKeyboardMarkup.ResizeKeyboard = true;
                    return replyKeyboardMarkup;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
        public class ButtonData // : IButtonData
        {
            public string stateName { get; init; }
            public string emoji { get; init; }
            public string buttonText { get; init; }
            public IKeyboardButton button { get; init; }
            public ButtonData(string state, string emoji = default!)
            {
                this.stateName = state;
                this.emoji = emoji;
                this.buttonText = state + emoji;
                button = new KeyboardButton(buttonText);
            }
        }
        public class EventText
        {
            public static StateMachineData.States states { get; set; } = StateMachineData.States.getInstance();
            public string home { get; init; } = $"Choose mode: {states.home} {states.find} {states.edit} {states.help}";
            public string find { get; init; } = "Write person name, If you want see all persons write \"ALL\"";
            public string edit { get; init; } = "EDIT Write person name, If you want add or edit person";
            public string help { get; init; } = "HELP Its a friendBot, Here you can add informations about your friends";
            public string findPerson { get; init; } = "Person not found, try again or return home";

        }

    }
    public class EventData : EventDataBase
    {
        public virtual string caseText { get; set; } = "Default text";
        public virtual IReplyMarkup buttons { get; set; } = new ReplyKeyboardMarkup(new KeyboardButton("Default text")); // DefaultButton()
        public EventData(string caseText, IReplyMarkup buttons)
        {
            this.caseText = caseText;
            this.buttons = buttons;
        }
    }
    //////////////////////////////////////////////////////////////////////
    //public class EventData : IEventData
    //{
    //    public string eventText { get; init; }
    //    public EventData(string eventText)
    //    {
    //        this.eventText = eventText;
    //    }
    //}

    //public class ButtonsBuilder
    //{
    //    public Dictionary<string, string> emojiByState { get; set; }
    //    public Dictionary<string, string> buttonTextByState { get; set; }
    //    public Dictionary<string, KeyboardButton> keyboardButtonByState { get; set; }
    //    public Dictionary<string, ReplyKeyboardMarkup> replyKeyboardMarkupByState { get; set; }
    //    public ButtonsBuilder(StateMachineData stateMachineData)
    //    {
    //        var states = stateMachineData.states;
    //        this.emojiByState = BuildEmoji(states);
    //        this.buttonTextByState = BuildButtonsTexts(states);
    //        this.keyboardButtonByState = BuildKeyboardButtons(states);
    //        this.replyKeyboardMarkupByState = BuildReplyKeyboardMarkupByState(states);
    //    }

    //    private Dictionary<string, ReplyKeyboardMarkup> BuildReplyKeyboardMarkupByState(StateMachineData.States states)
    //    {
    //        var dict = new Dictionary<string, ReplyKeyboardMarkup>();
    //        var func = delegate (string state) { return new KeyValuePair<string, KeyboardButton>(state, keyboardButtonByState[state]); };

    //        var list = new List<KeyValuePair<string, KeyboardButton>>()
    //        {
    //            func(states.home)////////////
    //        };
    //        var v1 = new KeyValuePair<string, List<KeyboardButton>>(states.home, new List<KeyboardButton>()
    //        {
    //            keyboardButtonByState[states.home]
    //        });

    //        return null;
    //    }
    //    private IReplyMarkup IReplyMarkupBuilder(IEnumerable<KeyboardButton> keyboardButtons)
    //    {
    //        var list1 = new List<KeyboardButton>(keyboardButtons);
    //        var list2 = new List<List<KeyboardButton>> { list1 };
    //        return new ReplyKeyboardMarkup(list2);
    //    }

    //    private Dictionary<string, KeyboardButton> BuildKeyboardButtons(StateMachineData.States states)
    //    {
    //        var func = delegate (string state)
    //        {
    //            return new KeyValuePair<string, KeyboardButton>(state, new KeyboardButton(buttonTextByState[state]));
    //        };
    //        var dict = new Dictionary<string, KeyboardButton>(new List<KeyValuePair<string, KeyboardButton>>()
    //        {
    //            func(states.home),
    //            func(states.find),
    //            func(states.edit),
    //            func(states.help)

    //        });
    //        return dict;
    //    }

    //    private Dictionary<string, string> BuildEmoji(StateMachineData.States states)
    //    {
    //        string homeEmj = char.ConvertFromUtf32(0x1F3E0);
    //        string findEmj = char.ConvertFromUtf32(0x1F50D);
    //        string editEmj = char.ConvertFromUtf32(0x2699);
    //        string helpEmj = char.ConvertFromUtf32(0x1F4DA);
    //        var dict = new Dictionary<string, string>()
    //        {
    //            {states.home, homeEmj},
    //            {states.find, findEmj},
    //            {states.edit, editEmj},
    //            {states.help, helpEmj},
    //        };
    //        return dict;
    //    }
    //    private Dictionary<string, string> BuildButtonsTexts(StateMachineData.States states)
    //    {
    //        var func = delegate (string state) { return new KeyValuePair<string, string>(state, state + emojiByState[state]); };
    //        var list = new List<KeyValuePair<string, string>>()
    //        {
    //            func(states.home),
    //            func(states.find),
    //            func(states.edit),
    //            func(states.help)
    //        };
    //        var dict = new Dictionary<string, string>(list);
    //        return dict;
    //    }
    //    public IButtonData Build()
    //    {


    //        return null;
    //    }
    //    public void test()
    //    {
    //        var states = StateMachineData.States.getInstance();

    //        var homeEmj = char.ConvertFromUtf32(0x1F3E0);
    //        var homeText = states.home;

    //        var homeButtonText = homeText + homeEmj;
    //        var b1 = new KeyboardButton(homeButtonText);

    //        //var mainNuttons = new ReplyKeyboardMarkup(null);
    //        var b = new ButtonData("mainButtons", null);
    //    }

    //}
    ////


}