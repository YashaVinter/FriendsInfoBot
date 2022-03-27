using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace EntityFrameworkApp.Data
{
    public class FrontendData
    {
        public Dictionary<String, IButton> stateToButton { get; init; }
        public Dictionary<string, string> stateToCaseText { get; init; }
        public FrontendData(StateMachineData stateMachineData)
        {

        }
        public class ButtonData
        {
            private static ButtonData instance = null!;
            public Dictionary<string, string> stateToButtonText { get; init; }

            private ButtonData(StateMachineData.States states)
            {
                string homeEmj = char.ConvertFromUtf32(0x1F3E0);
                string findEmj = char.ConvertFromUtf32(0x1F50D);
                string editEmj = char.ConvertFromUtf32(0x2699);
                string helpEmj = char.ConvertFromUtf32(0x1F4DA);
                var createPair = delegate (string name, string emoji)
                {
                    return new KeyValuePair<string, string>(name, name + emoji);
                };
                IEnumerable<KeyValuePair<string, string>> collection = new[]
                {
                    createPair(states.home,homeEmj),
                    createPair(states.find,findEmj),
                    createPair(states.edit,editEmj),
                    createPair(states.help,helpEmj),
                };
                stateToButtonText = new Dictionary<string, string>(collection);
            }
            public static ButtonData getInstance(StateMachineData.States states = default!)
            {
                if (states is null)
                    throw new InvalidOperationException("Dont added initial states");
                if (instance is null)
                    instance = new ButtonData(states);
                return instance;
            }

        }
        public class Buttons2
        {
            public Dictionary<String, IButton> stateToButton { get; init; }
            private StateMachineData.States states { get; init; }
            public Buttons2(StateMachineData.States states)
            {
                this.states = states;
                stateToButton = ButtionBuilder(states);

            }

            private Dictionary<string, IButton> ButtionBuilder(StateMachineData.States states)
            {
                var dict = new Dictionary<string, IButton>();
                // home button
                var b1 = new Button(null, null);


                return null;
            }


            public class Button : IButton
            {
                public string text { get; set; }
                public IReplyMarkup buttons { get; set; }
                public Button(string text, IReplyMarkup buttons)
                {
                    this.text = text;
                    this.buttons = buttons;
                }
            }
        }
        public interface IButton
        {
            string text { get; set; }
            public IReplyMarkup buttons { get; set; }
        }

        public class Buttons
        {
            public Dictionary<String, IReplyMarkup> buttonsDictionary { get; set; }
            public Buttons()
            {
                var states = StateMachineData.States.getInstance();
                //var caseText = new FrontendData.CaseText(states);
                var buttonData = FrontendData.ButtonData.getInstance(states);
                var dict = new Dictionary<String, IReplyMarkup>();

                var homeButtons = ButtonsBuilder(new List<string>
                {
                    buttonData.stateToButtonText[states.home]
                });
                var defaultButtons = ButtonsBuilder(new List<string>
                {
                    buttonData.stateToButtonText[states.home],
                    buttonData.stateToButtonText[states.find],
                    buttonData.stateToButtonText[states.edit],
                    buttonData.stateToButtonText[states.help]
                });

                dict.Add(states.home, defaultButtons);
                dict.Add(states.find, homeButtons);
                dict.Add(states.findPerson, homeButtons);
                dict.Add(states.edit, homeButtons);
                dict.Add(states.help, homeButtons);
                buttonsDictionary = dict;
            }
            private IReplyMarkup ButtonsBuilder(IEnumerable<string> buttonsNames)
            {
                var b1 = new List<KeyboardButton>();
                foreach (var buttonName in buttonsNames)
                {
                    b1.Add(new KeyboardButton(buttonName));
                }
                var b2 = new List<List<KeyboardButton>> { b1 };
                var kb = new ReplyKeyboardMarkup(b2);
                kb.ResizeKeyboard = true;
                return kb;

                //var v = new ReplyKeyboardMarkup(null);
            }
        }
        public class CaseText
        {
            private static CaseText instance = null!;
            public Dictionary<string, string> stateToCaseText { get; set; }
            private CaseText(StateMachineData.States states)
            {
                string home = $"Choose mode: {states.home} {states.find} {states.edit} {states.help}";
                string find = "Write person name, If you want see all persons write \"ALL\"";
                string findPerson = "Person not found, try again or return home";
                string edit = "EDIT Write person name, If you want add or edit person";
                string help = "HELP Its a friendBot, Here you can add informations about your friends";

                stateToCaseText = new Dictionary<string, string>()
                {
                    { states.home,home},
                    { states.find,find},
                    { states.edit,edit},
                    { states.help,help},
                    { states.findPerson,findPerson}
                };

            }

            public static CaseText getInstance(StateMachineData.States states = default!)
            {
                if (states is null)
                    throw new InvalidOperationException("Dont added initial states");
                if (instance is null)
                    instance = new CaseText(states);
                return instance;
            }
        }

    }
    /// <summary>
    /// //////////////////////////////////////////////////////////////////////
    /// </summary>
    public class FrontendDataNew : IFrontendData
    {
        public Dictionary<string, IButtonData> buttonsByState { get; init; }
        public Dictionary<string, IEventData> caseEventByState { get; init; }
        private StateMachineData.States states { get; set; }
        public FrontendDataNew(StateMachineData stateMachineData)
        {
            this.states = stateMachineData.states;

        }
        private Dictionary<string, IButtonData> dict1()
        {
            string homeEmj = char.ConvertFromUtf32(0x1F3E0);
            string findEmj = char.ConvertFromUtf32(0x1F50D);
            string editEmj = char.ConvertFromUtf32(0x2699);
            string helpEmj = char.ConvertFromUtf32(0x1F4DA);
            var buttonEmoji = delegate (string s1, string s2) { return s1 + s2; };

            string homeText = buttonEmoji(states.home, homeEmj);
            string findText = buttonEmoji(states.find, findEmj);
            string editText = buttonEmoji(states.edit, editEmj);
            string helpText = buttonEmoji(states.help, helpEmj);

            var mainButtonsList = new List<string>()
            {
                homeText,
                findText,
                editText,
                helpText
            };
            var homeButtons = ButtonsBuilder(mainButtonsList);
            var homeButtonsList = new List<string>()
            {
                homeText,
            };


            var dict = new Dictionary<string, IButtonData>()
            {
                {states.home,new ButtonData(buttonEmoji(states.home,homeEmj), null) }
            };

            return null;
        }
        private IReplyMarkup ButtonsBuilder(IEnumerable<string> buttonsNames)
        {
            var b1 = new List<KeyboardButton>();
            foreach (var buttonName in buttonsNames)
            {
                b1.Add(new KeyboardButton(buttonName));
            }
            var b2 = new List<List<KeyboardButton>> { b1 };
            var kb = new ReplyKeyboardMarkup(b2);
            kb.ResizeKeyboard = true;
            return kb;

            //var v = new ReplyKeyboardMarkup(null);
        }
    }
    public class ButtonData : IButtonData
    {
        public string text { get; init; }
        public IReplyMarkup buttons { get; init; }
        public ButtonData(string text, IReplyMarkup buttons)
        {
            this.text = text;
            this.buttons = buttons;
        }
    }
    public class EventData : IEventData
    {
        public string eventText { get; init; }
        public EventData(string eventText)
        {
            this.eventText = eventText;
        }
    }

    public class ButtonsBuilder
    {
        public Dictionary<string,string> emojiByState { get; set; }
        public Dictionary<string, string> buttonTextByState { get; set; }
        public Dictionary<string, KeyboardButton> keyboardButtonByState { get; set; }
        public Dictionary<string, ReplyKeyboardMarkup> replyKeyboardMarkupByState { get; set; }
        public ButtonsBuilder(StateMachineData stateMachineData)
        {
            var states = stateMachineData.states;
            this.emojiByState = BuildEmoji(states);
            this.buttonTextByState = BuildButtonsTexts(states);
            this.keyboardButtonByState = BuildKeyboardButtons(states);
            this.replyKeyboardMarkupByState = BuildReplyKeyboardMarkupByState(states);
        }

        private Dictionary<string, ReplyKeyboardMarkup> BuildReplyKeyboardMarkupByState(StateMachineData.States states)
        {
            var dict = new Dictionary<string, ReplyKeyboardMarkup>();
            var func = delegate (string state) { return new KeyValuePair<string, KeyboardButton>(state, keyboardButtonByState[state]); };

            var list = new List<KeyValuePair<string, KeyboardButton>>() 
            {
                func(states.home)////////////
            };
            var v1 = new KeyValuePair<string, List<KeyboardButton>>(states.home,new List<KeyboardButton>() 
            {
                keyboardButtonByState[states.home]
            });

            return null;
        }
        private IReplyMarkup IReplyMarkupBuilder(IEnumerable<KeyboardButton> keyboardButtons)
        {
            var list1 = new List<KeyboardButton>(keyboardButtons);
            var list2 = new List<List<KeyboardButton>> { list1 };
            return new ReplyKeyboardMarkup(list2);
        }

        private Dictionary<string, KeyboardButton> BuildKeyboardButtons(StateMachineData.States states)
        {
            var func = delegate (string state) {
                return new KeyValuePair<string, KeyboardButton>(state, new KeyboardButton(buttonTextByState[state]) );
            };
            var dict = new Dictionary<string, KeyboardButton>(new List<KeyValuePair<string, KeyboardButton>>() 
            {  
                func(states.home),
                func(states.find),
                func(states.edit),
                func(states.help)

            });
            return dict;
        }

        private Dictionary<string, string> BuildEmoji(StateMachineData.States states) {
            string homeEmj = char.ConvertFromUtf32(0x1F3E0);
            string findEmj = char.ConvertFromUtf32(0x1F50D);
            string editEmj = char.ConvertFromUtf32(0x2699);
            string helpEmj = char.ConvertFromUtf32(0x1F4DA);
            var dict = new Dictionary<string, string>()
            {
                {states.home, homeEmj},
                {states.find, findEmj},
                {states.edit, editEmj},
                {states.help, helpEmj},
            };
            return dict;
        }
        private Dictionary<string, string> BuildButtonsTexts(StateMachineData.States states)
        {
            var func = delegate (string state) { return new KeyValuePair<string, string>(state, state + emojiByState[state]); };
            var list = new List<KeyValuePair<string, string>>() 
            {
                func(states.home),
                func(states.find),
                func(states.edit),
                func(states.help)
            };
            var dict = new Dictionary<string, string>(list);           
            return dict;
        }
        public IButtonData Build()
        {


            return null;
        }
        public void test() {
            var states = StateMachineData.States.getInstance();

            var homeEmj = char.ConvertFromUtf32(0x1F3E0);
            var homeText = states.home;

            var homeButtonText = homeText + homeEmj;  
            var b1 = new KeyboardButton(homeButtonText);

            //var mainNuttons = new ReplyKeyboardMarkup(null);
            var b = new ButtonData("mainButtons", null);
        }
    }
}