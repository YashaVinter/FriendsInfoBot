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
        private StateMachine BuildStateMachine()
        {
            var smd = StateMachineData.Instance();
            var states = smd.states;
            var eventTexts = FrontendData.EventText.Instance(states);
            var actions = new FriendsBotData.StateTelegramActions(smd);
            var botInputData = new FriendsBotData.BotInputData(this, null);

            var frontendData = new FrontendData(states);
            var keyboardBuilder = new FrontendData.KeyboardBuilder(frontendData.buttonsData);
            var mainKeyboard = keyboardBuilder.BuildKeyboard(new List<string>()
            {
                states.home,
                states.find,
                states.edit,
                states.help
            });
            var homeKeyboard = keyboardBuilder.BuildKeyboard(new List<string>()
            {
                states.home,
            });

            StateDataSetBase defaultState = new StateDataSetBase("defultnName", actions.DefaultCase, homeKeyboard, botInputData);

            StateDataSet home = new StateDataSet(states.home, eventTexts.home,actions.DefaultCase, mainKeyboard, botInputData);
            StateDataSet find = new StateDataSet(states.find, eventTexts.find, defaultState);
            StateDataSet edit = new StateDataSet(states.edit, eventTexts.edit, defaultState);
            StateDataSet help = new StateDataSet(states.help, eventTexts.help, defaultState);
            StateDataSet findPerson = new StateDataSet(states.findPerson, eventTexts.findPerson, defaultState);
            var statesData = new List<StateDataSet> 
            {
                home,
                find,
                edit,
                help,
                findPerson
            };
            //
            var tr = (string s1, string s2) => { return s1 + ':' + s2; };
            var criteria = new FriendsBotData.Criteria(smd, frontendData);
            var defaultTransitions = new List<string> 
            {
                    tr(states.home,states.find),
                    tr(states.home,states.edit),
                    tr(states.home,states.help),
                    tr(states.find,states.home),
                    tr(states.findPerson,states.home),
                    tr(states.edit,states.home),
                    tr(states.help,states.home)
            };

            var trasitionsData = from t in defaultTransitions
                     select new TrasitionDataSet(t, criteria.EqualPredicate(t.Split(':')[1]) );

            TrasitionDataSet tr1 = new TrasitionDataSet(tr(states.find, states.findPerson), criteria.NotEqualPredicate(states.home));
            trasitionsData.Append(tr1);

            var v1 = new StateMashineBuilder(statesData, trasitionsData, states.home);

            throw new NotImplementedException();
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
