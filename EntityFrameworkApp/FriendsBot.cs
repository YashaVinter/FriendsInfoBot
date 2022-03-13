//using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace EntityFrameworkApp
{


    public class FriendsBot : TelegramBotClient
    {
        public FriendsBot(string token) : base(token) { }
        public BotState botState { get; set; } = BotState.common;
        public FindState findState { get; set; }
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
        public async Task<Message> SendTextMessageAsync(Update update, string text) {
            var Id = update?.Message?.Chat.Id;

            var message = await this.SendTextMessageAsync(
                chatId: Id,
                text: text,
                parseMode: Telegram.Bot.Types.Enums.ParseMode.MarkdownV2,
                replyMarkup: HomeButtons()
                );
            return message; 
            // 
            //message = await friendsBot.SendTextMessageAsync(update, "message from friendsBot");
            //
        }
        public async Task<Message> SendPhotoAsync(Update update, string photoURL)
        {
            var Id = update?.Message?.Chat.Id;
            return await this.SendPhotoAsync(
                chatId: Id,
                photo: photoURL,
                replyMarkup: HomeButtons()
                );
        }

        public void Answer(Update update) {
            string command = update.Message.Text;
            switch (command)
            {
                case Commands.home:
                    CaseHome(update);
                    break;
                case Commands.find:
                    CaseFind(update);
                    break;
                case Commands.edit:
                    CaseEdit();
                    break;
                case Commands.help:
                    CaseHelp();
                    break;
                default:
                    OtherCases(update);
                    break;
            }
            async Task<Message> CaseHome(Update update) {
                string text = "Home";
                var ret = await this.SendTextMessageAsync(update, text);

                botState = BotState.home;
                return ret;
            }
            async Task<Message> CaseFind(Update update)
            {
                string text = "Write person name. If you want see all persons write ALL";
                var ret = await this.SendTextMessageAsync(update, text);
                Task.WaitAll();
                botState = BotState.findPerson;
                return ret;
            }
            void CaseEdit()
            {

            }
            void CaseHelp()
            {

            }

            void OtherCases(Update update) {
                switch (botState)
                {
                    case BotState.findPerson:
                        CaseFindPerson(update);
                        break;
                    case BotState.common:
                        CaseCommon(update);
                        break;
                    default:
                        break;
                }

                async Task<Message> CaseFindPerson(Update update)
                {
                    string name = update.Message.Text;
                    Message? message;
                    Person person = new Person().Find(name);
                    if (person is null)
                    {
                        message = await this.SendTextMessageAsync(update, "Person not found, try again");
                        Task.WaitAll();
                        botState = BotState.personNotFound;
                        return message;
                    }
                    else
                    {
                        message = await this.SendPhotoAsync(update, person.photo);
                        message = await this.SendTextMessageAsync(update, person.Print());
                        botState = BotState.personIsFound;
                        return message;
                    }
                }
                async Task<Message> CaseCommon(Update update)
                {
                    string text = "Choose mode";
                    var ret = await this.SendTextMessageAsync(update, text);

                    botState = BotState.common;
                    return ret;
                }

            }
        }

        public void test() {

        }
        public enum BotState
        {
            home,
            find,
            findPerson,
            personIsFound,
            personNotFound,
            edit,
            help,
            common
        }

        public enum FindState
        {
            none,
            findPerson,
            personIsFound,
            personNotFound,
        }

    }
}
