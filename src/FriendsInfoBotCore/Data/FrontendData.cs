using System;
using Telegram.Bot.Types.ReplyMarkups;
using StateMachineLibrary;

namespace EntityFrameworkApp.Data
{
    public class FrontendData
    {
        private ISet<ButtonData> buttonsData { get; init; }
        public Keyboards keyboards { get; set; }
        public Dictionary<string,string> buttonsTextByState{ get; set; }
        public Dictionary<string, string> eventTextByState { get; set; }
        public FrontendData(StateMachineData.States states)
        {
            this.buttonsData = BuildButtons(states);
            this.keyboards = new Keyboards(buttonsData,states);
            this.buttonsTextByState = buttonsData.ToDictionary(bd => bd.stateName, bd => bd.button.Text);
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
            public string emoji { get; set; }
            //public string buttonText { get; init; }
            public IKeyboardButton button { get; set; }
            public ButtonData(string state, string emoji = default!,string buttonText = default!)
            {
                this.stateName = state;
                this.emoji = emoji;
                //this.buttonText = state + emoji;
                if(buttonText is null)
                    button = new KeyboardButton(state+emoji);
                else 
                    button = new KeyboardButton(buttonText);
            }
        }
        public class EventText
        {
            public Dictionary<string,string> eventTextByState { get; set; }
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
}