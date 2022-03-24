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
        public StateMachineLibrary.StateMachineCommand botCommand { get; set; } = new StateMachineLibrary.StateMachineCommand();
        public void StateMachineBuilder() {
            StateMachineData.States states1 = StateMachineData.States.getInstance();
            StateMachineData.Transitions transitions1 = StateMachineData.Transitions.getInstance();
            stateMachine = new StateMachine(states1.stateSets, transitions1.transitionSets, states1.home); 


            //StateMachineData SMdata = new StateMachineData();
            //var states = new StateMachineData.States();
            //FriendsBotData friendsBotData = new FriendsBotData();
            //stateMachine = new StateMachine(
            //    SMdata.states,
            //    SMdata.transitions,
            //    states.home
            //    );

            //stateMachine.AddFunctionHandler(states.home, FriendsBotData.StateTelegramActions.CaseHome);
            //stateMachine.AddFunctionHandler(states.find, FriendsBotData.StateTelegramActions.CaseFind);
            //stateMachine.AddFunctionHandler(states.edit, FriendsBotData.StateTelegramActions.CaseEdit);
            //stateMachine.AddFunctionHandler(states.help, FriendsBotData.StateTelegramActions.CaseHelp);
            //stateMachine.AddFunctionHandler(states.findPerson, FriendsBotData.StateTelegramActions.CaseFindPerson);
            //stateMachine.AddCriteraRange(SMdata.transitions, SMdata.criteria);

            // work
            //stateMachine.AddFunctionHandler(friendsBotData.GetActionsDictionary(SMdata.states));
            //stateMachine.AddCriteraRange(friendsBotData.GetCriteriaDictionary(SMdata.transitions));

            // new
            var criteriaDictionary = new FriendsBotData.Criteria(stateMachine.transitionDictionary.Values).criteriaDictionary;
            stateMachine.AddCriteraRange(criteriaDictionary);

        }
        public void Answer(Update update)
        {
            string text = update?.Message?.Text;
            this.update = update;
            this.botCommand.command = this.update?.Message?.Text;
            stateMachine.Execute(this, this.botCommand);

            //stateMachine.Execute(command);
        }
        public async Task<Message> SendTextMessageAsync(Update update, string text) {
            var Id = update?.Message?.Chat.Id;

            var message = await this.SendTextMessageAsync(
                chatId: Id,
                text: text,
                parseMode: Telegram.Bot.Types.Enums.ParseMode.MarkdownV2,
                replyMarkup: FriendsBotData.HomeButtons()
                );
            return message; 
            // 
            //message = await friendsBot.SendTextMessageAsync(update, "message from friendsBot");
            //
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

        public async Task<Message> SendPhotoAsync(Update update, string photoURL)
        {
            var Id = update?.Message?.Chat.Id;
            return await this.SendPhotoAsync(
                chatId: Id,
                photo: photoURL,
                replyMarkup: FriendsBotData.HomeButtons()
                );
        }
        public async Task<Message> SendPhotoAsync(long chatId, string photoURL)
        {
            return await this.SendPhotoAsync(
                chatId: chatId,
                photo: photoURL,
                replyMarkup: FriendsBotData.HomeButtons()
                );
        }

    }
}
