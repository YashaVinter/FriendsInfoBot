using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;
using StateMachineLibrary;

namespace EntityFrameworkApp.Data
{
    /// <summary>
    /// //////////////////////////////////////////////////////////////////////
    /// </summary>
    public class FrontendData
    {
        private ISet<ButtonData> buttonsData { get; init; }
        public Keyboards keyboards { get; set; }
        public Dictionary<string,string> buttonsTextByState{ get; set; }
        public Dictionary<string, string> eventTextByState { get; set; }

        //public Dictionary<string, EventDataBase> eventDatabyState { get; init; }
        //private Dictionary<string, IReplyMarkup> keyboardByState { get; init; }
        //private Dictionary<string, string> eventTextByState { get; init; }
        //private StateMachineData.States states { get; init; }
        public FrontendData(StateMachineData.States states)
        {
            //this.states = states;
            this.buttonsData = BuildButtons(states);
            //this.keyboardByState = BuildKeyboards(buttonsData);
            //this.eventTextByState = BuildEventTexts(states);
            //this.eventDatabyState = BuildEventData();
            this.keyboards = new Keyboards(buttonsData,states);
            this.buttonsTextByState = buttonsData.ToDictionary(bd => bd.stateName, bd => bd.buttonText);
            this.eventTextByState = new EventText(states).eventTextByState;
        }

        private ISet<ButtonData> BuildButtons(StateMachineData.States states)
        {
            return new HashSet<ButtonData> 
            {
                new ButtonData(states.home, J3QQ4.Emoji.House),
                new ButtonData(states.find, J3QQ4.Emoji.Mag_Right),
                new ButtonData(states.edit, J3QQ4.Emoji.Pencil),
                new ButtonData(states.help, J3QQ4.Emoji.Books),
                new ButtonData("All")
            };
        }

        //private Dictionary<string, EventDataBase> BuildEventData()
        //{

        //    //var v1 = eventTextByState.Join(keyboardByState,e => e,k => k,(e,k) = >  )
        //    IEnumerable<KeyValuePair<string, EventDataBase>> pairs = from e in eventTextByState
        //             join k in keyboardByState on e.Key equals k.Key
        //             select new KeyValuePair<string, EventDataBase>(e.Key, new EventData(e.Value, k.Value));
        //    var dict = new Dictionary<string, EventDataBase>(pairs);
        //    return dict;
        //}

        //private Dictionary<string, IReplyMarkup> BuildKeyboards(ISet<ButtonData> buttonsData)
        //{
        //    var keyboardBuilder = new KeyboardBuilder(buttonsData);
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

        //    var dict = new Dictionary<string, IReplyMarkup>();
        //    dict.Add(states.home,       mainKeyboard);
        //    dict.Add(states.find,       homeKeyboard);
        //    dict.Add(states.edit,       homeKeyboard);
        //    dict.Add(states.help,       homeKeyboard);
        //    dict.Add(states.findPerson, homeKeyboard);

        //    return dict;
        //}
        //private Dictionary<string, string> BuildEventTexts(StateMachineData.States states)
        //{
        //    var eventText = EventText.Instance(states);
        //    var dict = new Dictionary<string, string>();
            
        //    dict.Add(states.home, eventText.home);
        //    dict.Add(states.find, eventText.find);
        //    dict.Add(states.edit, eventText.edit);
        //    dict.Add(states.help, eventText.help);
        //    dict.Add(states.findPerson, eventText.findPerson);

        //    return dict;
        //}
        public class KeyboardBuilder
        {
            private ISet<ButtonData> buttons { get; init; }
            public KeyboardBuilder(ISet<ButtonData> buttons) => this.buttons = buttons;
            public ReplyKeyboardMarkup BuildKeyboard(List<string> keyboardButtonsNames)
            {
                try
                {
                    //var buttonsList = buttons
                    //    .Where(b1 => keyboardButtonsNames.Any(b2 => b2 == b1.stateName))
                    //    .OrderBy(b3 => keyboardButtonsNames.IndexOf(b3.stateName))
                    //    .Select(b4 => b4.button as KeyboardButton)
                    //    .ToList();
                    var buttonsList = (from b in buttons
                                        join kn in keyboardButtonsNames on b.stateName equals kn
                                        orderby keyboardButtonsNames.IndexOf(b.stateName)
                                        select b.button as KeyboardButton)
                                        .ToList();

                    if (buttonsList.Count != keyboardButtonsNames.Count)
                        throw new("Dont not Implemented some buttons");

                    var replyKeyboardMarkup = new ReplyKeyboardMarkup(
                        new List<List<KeyboardButton>> { buttonsList }) { ResizeKeyboard = true};
                    return replyKeyboardMarkup;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
        public class ButtonData
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
            //private static EventText instance;
            //private static StateMachineData.States states { get; set; }
            //public string home { get; init; }
            //public string find { get; init; }
            //public string edit { get; init; }
            //public string help { get; init; }
            //public string findPerson { get; init; }
            public Dictionary<string,string> eventTextByState { get; set; }
            //private EventText(StateMachineData.States st) {
            //    states = st;
            //    home = $"Choose mode: {states.home} {states.find} {states.edit} {states.help}";
            //    find = "Write person name, If you want see all persons write \"ALL\"";
            //    edit = "EDIT Write person name, If you want add or edit person";
            //    help = "HELP Its a friendBot, Here you can add informations about your friends";
            //    findPerson = "Person not found, try again or return home";
            //}
            //public static EventText Instance(StateMachineData.States st) 
            //{
            //    if (instance is null)
            //        instance = new EventText(st);
            //    return instance;
            //}
            public EventText(StateMachineData.States states)
            {
                eventTextByState = new Dictionary<string, string> 
                {
                    { states.home,$"Choose mode: {states.home} {states.find} {states.edit} {states.help}"},
                    { states.find,"Write person name, If you want see all persons write \"ALL\""},
                    { states.edit,"EDIT Write person name, If you want add or edit person" },
                    { states.help,"HELP Its a friendBot, Here you can add informations about your friends"},
                    { states.findPerson,"Person not found, try again or return home"}
                };
            }
        }

        public class Keyboards
        {
            public ReplyKeyboardMarkup mainKeyboard { get; set; }
            public ReplyKeyboardMarkup homeKeyboard { get; set; }
            public ReplyKeyboardMarkup homeAll { get; set; }
            public Keyboards(ISet<ButtonData> buttons, StateMachineData.States states)
            {
                var keyboardBuilder = new KeyboardBuilder(buttons);
                mainKeyboard = keyboardBuilder.BuildKeyboard(new List<string>()
                    {
                        states.home,
                        states.find,
                        states.edit,
                        states.help
                    });
                homeKeyboard = keyboardBuilder.BuildKeyboard(new List<string>()
                    {
                        states.home,
                    });
                homeAll = keyboardBuilder.BuildKeyboard(new List<string>()
                    {
                        states.home,
                        "All"
                    });
            }
        }
    }
    public class EventData : EventDataBase
    {
        public virtual string caseText { get; set; }
        public virtual IReplyMarkup buttons { get; set; }
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