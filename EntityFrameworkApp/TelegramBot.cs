using System;
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


    public class TelegramBot : TelegramBotClient
    {
        public TelegramBot() : base(token) { }
        private const string token = "5156337859:AAFswaM91RTFckRSgA45jrhyKmYA77E0k14";
        private static TelegramBotClient client;
        private static List<Person> persons = null;

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

        public void test() {
           
        
        }
    }
}
