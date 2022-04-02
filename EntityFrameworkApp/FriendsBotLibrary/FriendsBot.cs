//using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

using EntityFrameworkApp.DataBase;
using StateMachineLibrary;

using EntityFrameworkApp.Data;

namespace EntityFrameworkApp.FriendsBotLibrary
{

    public class FriendsBot : TelegramBotClient
    {
        public FriendsBot(string token) : base(token) 
        {
            StateMachineBuilder();
        }
        public Update update { get; set; }
        protected StateMachine stateMachine { get; set; }
        private void StateMachineBuilder() {
            var smd = StateMachineData.Instance();
            stateMachine = new StateMachine(smd.states.stateSets, smd.transitions.transitionSets, smd.states.home);

            var friendsBotData = new FriendsBotData(smd);
            stateMachine.AddEventData(friendsBotData.eventDatabyState);
            stateMachine.AddFunctionHandler(friendsBotData.actionByState);
            stateMachine.AddCriteraRange(friendsBotData.criteriaByTransition);

        }
        public void Answer(Update update)
        {
            string text = update?.Message?.Text;
            //this.update = update;
            //this.botCommand.command = this.update?.Message?.Text;
            var inputData = new FriendsBotData.BotInputData(this, update.Message);

            stateMachine.Execute(inputData);
        }
        public async Task<Message> SendTextMessageAsync(long chatId, string text,IReplyMarkup replyMarkup)
        {
            return await this.SendTextMessageAsync(
                chatId: chatId,
                text: text,
                parseMode: Telegram.Bot.Types.Enums.ParseMode.MarkdownV2,
                replyMarkup: replyMarkup
                );
        }

        public async Task<Message> SendPhotoAsync(long chatId, string photoURL, IReplyMarkup replyMarkup)
        {
            return await this.SendPhotoAsync(
                chatId: chatId,
                photo: photoURL,
                replyMarkup: replyMarkup
                );
        }

    }
}
