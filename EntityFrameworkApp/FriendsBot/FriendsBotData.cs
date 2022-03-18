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
        public class States
        {
            public const string home = "home";
            public const string find = "find";
            public const string edit = "edit";
            public const string help = "help";
            public const string findPerson = "findPerson";

        }
        public struct Criteria
        {
            public bool toHome(string st)
            {
                return st == States.home;
            }
            public bool toFind(string st)
            {
                return st == States.find;
            }
            public bool toEdit(string st)
            {
                return st == States.edit;
            }
            public bool toHelp(string st)
            {
                return st == States.help;
            }
            public bool toFindPerson(string st)
            {
                return st != States.home;
            }

        }
        public class StateTelegramActions
        {
            private static FrontendData.CaseText caseText = new FrontendData.CaseText();
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
                    string text = $"Choose mode: {States.home} {States.find} {States.edit} {States.help}";
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
                    string text = "Write person name. If you want see all persons write \"ALL\"";
                    long id = (long)bot?.update?.Message?.Chat?.Id;
                    return await bot.SendTextMessageAsync(id, text, HomeButtons());
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
                        string text = "Person not found, try again or return home";
                        return await bot.SendTextMessageAsync(id, text, HomeButtons());

                    }
                    else
                    {
                        return await bot.SendTextMessageAsync(id, person.Print(), HomeButtons());
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
                    string text = "Write person name. If you want add or edit person";
                    long id = (long)bot?.update?.Message?.Chat?.Id;
                    return await bot.SendTextMessageAsync(id, text, HomeButtons());
                }
                return null;
            }
            public static async Task<Message> CaseHelp(object sender, EventArgs e)
            {
                if (sender is null)
                    return null;
                if (sender is FriendsBot bot)
                {
                    string text = "Its a friendBot. Here you can add informations about your friends";
                    long id = (long)bot?.update?.Message?.Chat?.Id;
                    return await bot.SendTextMessageAsync(id, text, HomeButtons());
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
