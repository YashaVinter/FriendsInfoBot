using System;
using System.Text;
using System.Text.RegularExpressions;

using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

using StateMachineLibrary;

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

        public class Criteria
        {
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
        }
        public class StateTelegramActions
        {
            private DataBase.Person? person;
            private DataBase.Address? address;
            private static readonly RegexPattern pattern = new RegexPattern();
            public StateTelegramActions()
            {
                person = new DataBase.Person();
                address = new DataBase.Address();
            }
            public async Task<ExitCode> DefaultCase(IStateData stateData) // ExitCode
            {
                StateData data = (StateData)stateData;
                BotInputData inputData = (BotInputData)data.inputData;
                EventData eventData = (EventData)data.eventData;

                var bot = inputData.telegramBotClient;
                await bot.SendTextMessageAsync(
                    chatId: inputData.message.Chat.Id,
                    text: eventData.caseText,
                    parseMode: Telegram.Bot.Types.Enums.ParseMode.MarkdownV2,
                    replyMarkup: eventData.buttons
                    );
                return ExitCode.OK;

            }
            public async Task<ExitCode> CaseAdd(IStateData stateData) {
                var ex = new NotImplementedException();
                Console.WriteLine('\t'+ex.Message);
                throw ex;
            }
            public async Task<ExitCode> CaseAddPerson(IStateData stateData)
            {
                StateData data = (StateData)stateData;
                BotInputData inputData = (BotInputData)data.inputData;
                EventData eventData = (EventData)data.eventData;

                var bot = inputData.telegramBotClient;

                person = DataBase.Person.Find(inputData.command);
                if (person is not null)
                {
                    string text = "Person already exist";
                    await bot.SendTextMessageAsync(
                        chatId: inputData.message.Chat.Id,
                        text: text,
                        parseMode: Telegram.Bot.Types.Enums.ParseMode.MarkdownV2,
                        replyMarkup: eventData.buttons
                        );
                    return ExitCode.EventError;
                }
                else
                {
                    person = new DataBase.Person() { name = inputData.command };
                    await bot.SendTextMessageAsync(
                        chatId: inputData.message.Chat.Id,
                        text: eventData.caseText, // "OK, write person Age"
                        parseMode: Telegram.Bot.Types.Enums.ParseMode.MarkdownV2,
                        replyMarkup: eventData.buttons
                        );
                    return ExitCode.OK;
                }
            }
            public async Task<ExitCode> CaseAddPersonAge(IStateData stateData)
            {
                StateData data = (StateData)stateData;
                BotInputData inputData = (BotInputData)data.inputData;
                EventData eventData = (EventData)data.eventData;

                var bot = inputData.telegramBotClient;
                int age;
                try
                {
                    age = Convert.ToInt32(inputData.command);
                    if (age < 0 || age > 150)
                        throw new FormatException();
                    person!.age = age;
                    
                    await bot.SendTextMessageAsync(
                        chatId: inputData.message.Chat.Id,
                        text: eventData.caseText,
                        parseMode: Telegram.Bot.Types.Enums.ParseMode.MarkdownV2,
                        replyMarkup: eventData.buttons
                        );
                    return ExitCode.OK;
                }
                catch (FormatException)
                {
                    string text = "Invalid Age";
                    await bot.SendTextMessageAsync(
                        chatId: inputData.message.Chat.Id,
                        text: text,
                        parseMode: Telegram.Bot.Types.Enums.ParseMode.MarkdownV2,
                        replyMarkup: eventData.buttons
                        );
                    return ExitCode.EventError;
                    
                }
            }
            public async Task<ExitCode> CaseAddPersonCity(IStateData stateData)
            {
                StateData data = (StateData)stateData;
                BotInputData inputData = (BotInputData)data.inputData;
                EventData eventData = (EventData)data.eventData;

                var bot = inputData.telegramBotClient;
                string city;
                try
                {
                    city = inputData.command;
                    //Validating City:
                    if(!new Regex(pattern.cityPattern).IsMatch(city) )
                        throw new FormatException();
                    address!.city = city;

                    await bot.SendTextMessageAsync(
                        chatId: inputData.message.Chat.Id,
                        text: eventData.caseText,
                        parseMode: Telegram.Bot.Types.Enums.ParseMode.MarkdownV2,
                        replyMarkup: eventData.buttons
                        );
                    return ExitCode.OK;
                }
                catch (FormatException)
                {
                    string text = "Incorrect data about the city";
                    await bot.SendTextMessageAsync(
                        chatId: inputData.message.Chat.Id,
                        text: text,
                        parseMode: Telegram.Bot.Types.Enums.ParseMode.MarkdownV2,
                        replyMarkup: eventData.buttons
                        );
                    return ExitCode.EventError;

                }
            }
            public async Task<ExitCode> CaseAddPersonStreet(IStateData stateData)
            {
                StateData data = (StateData)stateData;
                BotInputData inputData = (BotInputData)data.inputData;
                EventData eventData = (EventData)data.eventData;

                var bot = inputData.telegramBotClient;
                string street;
                try
                {
                    street = inputData.command;
                    //Validating Street:
                    if (!new Regex(pattern.streetPattern).IsMatch(street)!)
                        throw new FormatException();
                    address!.street = street;

                    await bot.SendTextMessageAsync(
                        chatId: inputData.message.Chat.Id,
                        text: eventData.caseText,
                        parseMode: Telegram.Bot.Types.Enums.ParseMode.MarkdownV2,
                        replyMarkup: eventData.buttons
                        );
                    return ExitCode.OK;
                }
                catch (FormatException)
                {
                    string text = "Incorrect street information";
                    await bot.SendTextMessageAsync(
                        chatId: inputData.message.Chat.Id,
                        text: text,
                        parseMode: Telegram.Bot.Types.Enums.ParseMode.MarkdownV2,
                        replyMarkup: eventData.buttons
                        );
                    return ExitCode.EventError;

                }
            }
            public async Task<ExitCode> CaseAddPersonHome(IStateData stateData)
            {
                StateData data = (StateData)stateData;
                BotInputData inputData = (BotInputData)data.inputData;
                EventData eventData = (EventData)data.eventData;

                var bot = inputData.telegramBotClient;
                string home;
                try
                {
                    home = inputData.command;
                    //Validating home:
                    if (!new Regex(pattern.homePattern).IsMatch(home)!)
                        throw new FormatException();
                    address!.home = home;

                    await bot.SendTextMessageAsync(
                        chatId: inputData.message.Chat.Id,
                        text: eventData.caseText,
                        parseMode: Telegram.Bot.Types.Enums.ParseMode.MarkdownV2,
                        replyMarkup: eventData.buttons
                        );
                    return ExitCode.OK;
                }
                catch (FormatException)
                {
                    string text = "Incorrect data about the house";
                    await bot.SendTextMessageAsync(
                        chatId: inputData.message.Chat.Id,
                        text: text,
                        parseMode: Telegram.Bot.Types.Enums.ParseMode.MarkdownV2,
                        replyMarkup: eventData.buttons
                        );
                    return ExitCode.EventError;

                }
            }
            public async Task<ExitCode> CaseAddPersonFlat(IStateData stateData)
            {
                StateData data = (StateData)stateData;
                BotInputData inputData = (BotInputData)data.inputData;
                EventData eventData = (EventData)data.eventData;

                var bot = inputData.telegramBotClient;
                int flat;
                try
                {
                    flat = Convert.ToInt32(inputData.command);
                    if (flat < 0 || flat > 10000)
                        throw new FormatException();
                    address!.flat = flat;

                    await bot.SendTextMessageAsync(
                        chatId: inputData.message.Chat.Id,
                        text: eventData.caseText,
                        parseMode: Telegram.Bot.Types.Enums.ParseMode.MarkdownV2,
                        replyMarkup: eventData.buttons
                        );
                    return ExitCode.OK;
                }
                catch (FormatException)
                {
                    string text = "Incorrect information about the apartment";
                    await bot.SendTextMessageAsync(
                        chatId: inputData.message.Chat.Id,
                        text: text,
                        parseMode: Telegram.Bot.Types.Enums.ParseMode.MarkdownV2,
                        replyMarkup: eventData.buttons
                        );
                    return ExitCode.EventError;

                }
            }
            public async Task<ExitCode> CaseAddPersonPhoto(IStateData stateData)
            {
                StateData data = (StateData)stateData;
                BotInputData inputData = (BotInputData)data.inputData;
                EventData eventData = (EventData)data.eventData;

                var bot = inputData.telegramBotClient;
                try
                {
                    if (inputData.command is not null)
                        throw new Exception();
                    // case reduce photo
                    string? fileId = inputData!.message?.Photo?.First().FileId;
                    // case normal size photo
                    fileId ??= inputData!.message!.Document!.FileId;
                    person!.photo = fileId;
                    //add person to database
                    person.address = address;
                    if (!person.AddPerson(person))
                        throw new("Person not added");

                    await bot.SendTextMessageAsync(
                        chatId: inputData!.message.Chat.Id,
                        text: eventData.caseText,
                        parseMode: Telegram.Bot.Types.Enums.ParseMode.MarkdownV2,
                        replyMarkup: eventData.buttons
                        );
                    return ExitCode.OK;

                }
                catch (Exception)
                {
                    string text = "Send person photo";
                    await bot.SendTextMessageAsync(
                        chatId: inputData.message.Chat.Id,
                        text: text,
                        parseMode: Telegram.Bot.Types.Enums.ParseMode.MarkdownV2,
                        replyMarkup: eventData.buttons
                        );
                    return ExitCode.EventError;
                }
                throw new();
            }
            public async Task<ExitCode> CaseFindPerson(IStateData stateData)
            {
                try
                {
                    var data = stateData as StateData;
                    var inputData = data.inputData as BotInputData;
                    var eventData = data.eventData as EventData;

                    var bot = inputData.telegramBotClient;
                    DataBase.Person? person = DataBase.Person.Find(inputData.command);
                    if (person is null)
                    {
                        await bot.SendTextMessageAsync(
                            chatId: inputData.message.Chat.Id,
                            text: eventData.caseText,
                            parseMode: Telegram.Bot.Types.Enums.ParseMode.MarkdownV2,
                            replyMarkup: eventData.buttons
                            );
                        return ExitCode.EventError;
                    }
                    else
                    {
                        await bot.SendPhotoAsync(
                            chatId:      inputData.message.Chat.Id,
                            photo:       person.photo,
                            replyMarkup: eventData.buttons
                            );
                        await bot.SendTextMessageAsync(
                            chatId: inputData.message.Chat.Id,
                            text: person.Print(),
                            parseMode: Telegram.Bot.Types.Enums.ParseMode.MarkdownV2,
                            replyMarkup: eventData.buttons
                            );
                        return ExitCode.OK;
                    }

                }
                catch (Exception)
                {
                    throw new NullReferenceException();
                }
            }
            public async Task<ExitCode> CaseFindAll(IStateData stateData)
            {
                try
                {
                    var data = stateData as StateData;
                    var inputData = data.inputData as BotInputData;
                    var eventData = data.eventData as EventData;

                    var bot = inputData.telegramBotClient;
                    var persons = new DataBase.Person().AllPesons();
                    if (persons is null)
                    {
                        await bot.SendTextMessageAsync(
                            chatId: inputData.message.Chat.Id,
                            text: eventData.caseText,
                            parseMode: Telegram.Bot.Types.Enums.ParseMode.MarkdownV2,
                            replyMarkup: eventData.buttons
                            );
                        return ExitCode.EventError;
                    }
                    else
                    {
                        StringBuilder personsText = new("All friends: \n");
                        foreach (var person in persons)
                        {
                            personsText.Append($"{persons.IndexOf(person)+1}. {person.name}\n");
                        }
                        await bot.SendTextMessageAsync(
                            chatId: inputData.message.Chat.Id,
                            text: personsText.ToString(),
                            //parseMode: Telegram.Bot.Types.Enums.ParseMode.MarkdownV2,
                            replyMarkup: eventData.buttons
                            );
                        return ExitCode.OK;
                    }

                }
                catch (Exception)
                {
                    throw new NullReferenceException();
                }
            }

            public record RegexPattern(
                //strings that only contain letter words separated with a single space between them
                string cityPattern = @"^\p{L}+(?: \p{L}+)*$",
                string streetPattern = @"^\p{L}+(?: \p{L}+)*$",
                // ex: 10, 10a, 10/1
                string homePattern = @"^[1-9]\d*(?: ?(?:[a-z]|[/-] ?\d+[a-z]?))?$"
                );
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
    public class FriendsBotDataNew
    {
        private States states { get; }
        private Dictionary<string, string> eventTextByState { get; }
        private Dictionary<string, FunctionHandler> actionsByState { get; }
        private Dictionary<string, FrontendData.ButtonData> buttonsByState { get; }
        private Dictionary<string, ReplyKeyboardMarkup> keyboardsByState { get; }
        private Dictionary<string, Predicate<string>> predicateByTransition { get; }
        public FriendsBotDataNew()
        {
            //var states = new { home = "home",find = "find",edit = "edit",help="help",findPerson = "findPerson",findAll = "findAll" };

            states = new States();
            // States
            eventTextByState = new Dictionary<string, string>
            {
                { states.home,$"Choose mode: {states.home} {states.find} {states.edit} {states.help}"},
                { states.find,"Write person name, If you want see all persons write \"ALL\""},
                { states.myFriends,"Here you can add or edit your friends"},
                { states.add,"If you want add person write name, or go back"},
                { states.addPerson,"OK, write person Age"},
                { states.addPersonAge,"OK, write person City"},
                { states.addPersonCity,"OK, write person Street"},
                { states.addPersonStreet,"OK, write person Home"},
                { states.addPersonHome,"OK, write person Flat"},
                { states.addPersonFlat,"OK, send person photo"}, // or write url
                { states.addPersonPhoto,"OK, Person was Added"},
                { states.edit,"EDIT Write person name, If you want edit person" },
                { states.help,"HELP Its a friendBot, Here you can add informations about your friends"},
                { states.findPerson,"Person not found, try again or return home"},
                { states.findAll,"Persons not found"}
            };
            var act = new FriendsBotData.StateTelegramActions();
            actionsByState = new Dictionary<string, FunctionHandler> 
            {
                { states.home,act.DefaultCase},
                { states.find,act.DefaultCase},
                { states.myFriends,act.DefaultCase},
                { states.add,act.DefaultCase}, //
                { states.addPerson,act.CaseAddPerson},
                { states.addPersonAge,act.CaseAddPersonAge},
                { states.addPersonCity,act.CaseAddPersonCity},
                { states.addPersonStreet,act.CaseAddPersonStreet},
                { states.addPersonHome,act.CaseAddPersonHome},
                { states.addPersonFlat,act.CaseAddPersonFlat},
                { states.addPersonPhoto,act.CaseAddPersonPhoto},
                { states.edit,act.DefaultCase },
                { states.help,act.DefaultCase},
                { states.findPerson,act.CaseFindPerson},
                { states.findAll,act.CaseFindAll}
            };
            buttonsByState = new Dictionary<string, FrontendData.ButtonData> 
            {
                { states.home,         new(states.home,J3QQ4.Emoji.House) },
                { states.find,         new(states.find,J3QQ4.Emoji.Mag_Right) },
                { states.myFriends,    new(states.myFriends,J3QQ4.Emoji.Family) },
                { states.add,          new(states.add,J3QQ4.Emoji.Person_With_Blond_Hair,"Add") },
                { states.addPerson,    new(states.addPerson) },
                { states.addPersonAge, null },
                { states.addPersonCity, null },
                { states.addPersonStreet, null },
                { states.addPersonHome, null },
                { states.addPersonFlat, null },
                { states.addPersonPhoto, null },
                { states.edit,         new(states.edit,J3QQ4.Emoji.Pencil) },
                { states.help,         new(states.help,J3QQ4.Emoji.Books) },
                { states.findPerson,   null },
                { states.findAll,      new(states.findAll,buttonText: "All") }
            };
            var keyboardBuilder = new FrontendData.KeyboardBuilder(buttonsByState.Values.Where(b=>b is not null).ToHashSet());
            //
            var mainKeyboard = keyboardBuilder.BuildKeyboard(new List<string>()
            {
                states.home,
                states.find,
                states.myFriends,
                states.help
            });
            var homeKeyboard = keyboardBuilder.BuildKeyboard(new List<string>()
            {
                states.home,
            });
            var homeFindAllKeyboard = keyboardBuilder.BuildKeyboard(new List<string>()
            {
                states.home,
                states.findAll
            });
            var homeAddEditKeyboard = keyboardBuilder.BuildKeyboard(new List<string>()
            {
                states.home,
                states.add,
                states.edit
            });

            //
            keyboardsByState = new Dictionary<string, ReplyKeyboardMarkup>
            {
                { states.home,mainKeyboard },
                { states.find,homeFindAllKeyboard},
                { states.myFriends,homeAddEditKeyboard},
                { states.add,homeKeyboard},
                { states.addPerson,homeKeyboard},
                { states.addPersonAge,homeKeyboard},
                { states.addPersonCity,homeKeyboard},
                { states.addPersonStreet,homeKeyboard},
                { states.addPersonHome,homeKeyboard},
                { states.addPersonFlat,homeKeyboard},
                { states.addPersonPhoto,homeKeyboard},
                { states.edit,homeKeyboard},
                { states.help,homeKeyboard},
                { states.findPerson,homeFindAllKeyboard},
                { states.findAll,homeKeyboard}
            };

            // Transitions
            var buttonsTextByState = buttonsByState.Where(b => b.Value is not null).ToDictionary(k=> k.Key, v=> v.Value.button.Text);
            var tr = (string s1, string s2) => { return s1 + ':' + s2; };
            predicateByTransition = new Dictionary<string, Predicate<string>> {
                //home
                { tr(states.home,states.find),(string s) => {return s == buttonsTextByState[states.find]; } },
                { tr(states.home,states.myFriends),(string s) => {return s == buttonsTextByState[states.myFriends]; } },
                { tr(states.home,states.help),(string s) => {return s == buttonsTextByState[states.help]; } },
                //find
                { tr(states.find,states.home),(string s) => {return s == buttonsTextByState[states.home]; } },
                { tr(states.find,states.findPerson),
                    (string s) => {return (s != buttonsTextByState[states.home]) && (s!=buttonsTextByState[states.findAll]); } },
                { tr(states.find,states.findAll),(string s) => {return s == buttonsTextByState[states.findAll]; } },
                //findPerson
                { tr(states.findPerson,states.home),(string s) => {return s == buttonsTextByState[states.home]; } },
                { tr(states.findPerson,states.findAll),(string s) => {return s == buttonsTextByState[states.findAll]; } },//
                //findAll
                { tr(states.findAll,states.home),(string s) => {return s == buttonsTextByState[states.home]; } },
                { tr(states.findAll,states.findPerson),(string s) => {return s != buttonsTextByState[states.home]; } },
                //myFriends
                { tr(states.myFriends,states.home),(string s)=> {return s == buttonsTextByState[states.home]; } },
                { tr(states.myFriends,states.add),(string s)=> {return s == buttonsTextByState[states.add]; } },
                { tr(states.myFriends,states.edit),(string s) => {return s == buttonsTextByState[states.edit]; } },
                //Add
                { tr(states.add,states.home),(string s)=> {return s == buttonsTextByState[states.home]; } },
                { tr(states.add,states.addPerson),(string s)=> {return s != buttonsTextByState[states.home]; } },
                //addPerson
                { tr(states.addPerson,states.home),(string s)=> {return s == buttonsTextByState[states.home]; } },
                { tr(states.addPerson,states.addPersonAge),(string s)=> {return s != buttonsTextByState[states.home]; } },
                //addPersonAge
                { tr(states.addPersonAge,states.home),(string s)=> {return s == buttonsTextByState[states.home]; } },
                { tr(states.addPersonAge,states.addPersonCity),(string s)=> {return s != buttonsTextByState[states.home]; } },
                //addPersonCity
                { tr(states.addPersonCity,states.home),(string s)=> {return s == buttonsTextByState[states.home]; } },
                { tr(states.addPersonCity,states.addPersonStreet),(string s)=> {return s != buttonsTextByState[states.home]; } },
                //addPersonStreet
                { tr(states.addPersonStreet,states.home),(string s)=> {return s == buttonsTextByState[states.home]; } },
                { tr(states.addPersonStreet,states.addPersonHome),(string s)=> {return s != buttonsTextByState[states.home]; } },
                //addPersonHome
                { tr(states.addPersonHome,states.home),(string s)=> {return s == buttonsTextByState[states.home]; } },
                { tr(states.addPersonHome,states.addPersonFlat),(string s)=> {return s != buttonsTextByState[states.home]; } },
                //addPersonFlat
                { tr(states.addPersonFlat,states.home),(string s)=> {return s == buttonsTextByState[states.home]; } },
                { tr(states.addPersonFlat,states.addPersonPhoto),(string s)=> {return s != buttonsTextByState[states.home]; } },
                //addPersonPhoto
                { tr(states.addPersonPhoto,states.home),(string s)=> {return true; } },

                //edit
                { tr(states.edit,states.home),(string s)=> {return s == buttonsTextByState[states.home]; } },
                //help
                { tr(states.help,states.home),(string s) => {return s == buttonsTextByState[states.home]; } },

            };
        }
        public StateDataSet[] BuildStatesDataSet() 
        {

            return (from s in actionsByState
                    let name = s.Key
                    select new StateDataSet(
                        name,
                        eventTextByState[name],
                        actionsByState[name],
                        keyboardsByState[name],
                        new(null, null) ) )
                    .ToArray();

            //return new StateDataSet(
            //    stateName,
            //    eventTextByState[stateName],
            //    actionsByState[stateName],
            //    keyboardsByState[stateName],
            //    new(null, null));
        }
        public TrasitionDataSet[] BuildTrasitionsDataSet() 
        {
            return (from t in predicateByTransition
                    select new TrasitionDataSet(t.Key, t.Value))
                    .ToArray();
        }
        public record class States
        {
            public readonly string home = "home";
            public readonly string find = "find";
            public readonly string edit  = "edit";
            public readonly string help  = "help";
            public readonly string findPerson = "findPerson";
            public readonly string findAll = "findAll";
            public readonly string myFriends = "myFriends";
            public readonly string add = "Add";
            public readonly string addPerson = "addPerson";
            public readonly string addPersonAge = "addPersonAge";
            public readonly string addPersonCity = "addPersonCity";
            public readonly string addPersonStreet = "addPersonStreet";
            public readonly string addPersonHome = "addPersonHome";
            public readonly string addPersonFlat = "addPersonFlat";
            public readonly string addPersonPhoto = "addPersonPhoto";
        }
    }
}
