using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityFrameworkApp.Data;
using StateMachineLibrary;
using Telegram.Bot.Types.ReplyMarkups;
//using  EntityFrameworkApp.Data.FriendsBotData;

namespace EntityFrameworkApp
{
    public class TelegramStateMashineFactory : IStateMashineFactory
    {
        private IState[] states;
        private ITransition[] transitions;
        private string startState;
        public TelegramStateMashineFactory(IEnumerable<StateDataSet> statesData, IEnumerable<TrasitionDataSet> transitionsData, string startState)
        {
            this.startState = startState;
            ValidateData(statesData, transitionsData);
            this.states = BuildStates(statesData);
            this.transitions = BuildTransitions(transitionsData);
            ConnectStatesAndTransitions(ref states, ref transitions);
        }
        private void ValidateData(IEnumerable<StateDataSet> statesData, IEnumerable<TrasitionDataSet> transitionsData)
        {
            if (statesData.Count() != statesData.Distinct().Count())
                throw new();
            if (transitionsData.Count() != transitionsData.Distinct().Count())
                throw new();
        }
        private IState[] BuildStates(IEnumerable<StateDataSet> statesData)
        {
            return (from sd in statesData
                    select new StateBuilder(sd).Build())
                   .ToArray();
        }
        private ITransition[] BuildTransitions(IEnumerable<TrasitionDataSet> transitionsData)
        {
            return (from td in transitionsData
                    select new TransitionBuilder(td).Build())
                   .ToArray();
        }
        private void ConnectStatesAndTransitions(ref IState[] states, ref ITransition[] transitions)
        {
            foreach (var s in states)
            {
                var tr = from t in transitions
                         where t.transitionModel.name.Split(':')[0] == s.stateModel.name
                         select t;
                s.stateModel.transitions = tr.ToHashSet();
            }
            foreach (var t in transitions)
            {
                t.transitionModel.entryState = states.First(s => s.stateModel.name == t.transitionModel.name.Split(':')[0]);
                t.transitionModel.endState = states.First(s => s.stateModel.name == t.transitionModel.name.Split(':')[1]);
            }
        }
        public string BuildStartState()
        {
            return startState;
        }
        public Dictionary<string, IState> BuildStateDictionary()
        {
            return states.ToDictionary(s=> s.stateModel.name);
        }
        public Dictionary<string, ITransition> BuildTransitionDictionary()
        {
            return transitions.ToDictionary(t => t.transitionModel.name);
        }
    }
    public class StateBuilder
    {
        public StateDataSet data { get; set; }
        public StateBuilder(StateDataSet data)
        {
            this.data = data;
        }
        public State Build()
        {
            var stateModel = new StateModel(data.name);
            var stateEvent = new StateEvent(data.functionHandler);
            var stateData = new StateData(new EventData(data.caseText, data.buttons), data.botInputData);
            return new State(stateModel, stateEvent, stateData);
        }
    }
    public class TransitionBuilder
    {
        public TrasitionDataSet data { get; }
        public TransitionBuilder(TrasitionDataSet data)
        {
            this.data = data;
        }
        public Transition Build()
        {
            var transitionModel = new TransitionModel(data.name);
            var transitionCriteria = new TransitionCriteria(data.criteria);
            return new Transition(transitionModel, transitionCriteria);
        }
    }
    public class NameBase
    {
        public string name { get; set; }
        public NameBase(string name) => this.name = name;        
    }
    public class StateDataSetBase : NameBase, ICloneable
    {
        public FunctionHandler functionHandler { get; set; }
        public IReplyMarkup buttons { get; set; }
        public BotInputData botInputData { get; set; } = null!;
        public StateDataSetBase(string name,
                             FunctionHandler functionHandler, 
                             IReplyMarkup buttons, 
                             BotInputData botInputData) : base(name)
        {
            this.functionHandler = functionHandler;
            this.buttons = buttons;
            this.botInputData = botInputData;
        }
        public StateDataSetBase(string name, StateDataSetBase stateDataSetBase) : base(name)
        {
            this.functionHandler = stateDataSetBase.functionHandler;
            this.buttons = stateDataSetBase.buttons;
            this.botInputData = stateDataSetBase.botInputData;
        }
        public object Clone()
        {
            return MemberwiseClone();
        }
    }
    public class StateDataSet : StateDataSetBase
    {
        public ISet<ITransitionModel> transitions { get; set; } = null!;
        public string caseText { get; set; }
        public StateDataSet(string name,string caseText, FunctionHandler functionHandler, IReplyMarkup buttons, BotInputData botInputData)
            : base(name, functionHandler, buttons, botInputData)
        {
            this.caseText = caseText;
        }
        public StateDataSet(string name, string caseText, StateDataSetBase stateDataSetBase) 
            : base(name, stateDataSetBase)
        {
            this.caseText = caseText;
        }

    }
    public class TrasitionDataSet : NameBase
    {
        public IStateModel entryState { get; set; } = null!;
        public IStateModel endState { get; set; } = null!;
        public Predicate<string> criteria { get; set; }
        public TrasitionDataSet(string name, Predicate<string> criteria) : base(name)
        {
            this.criteria = criteria;
        }
    }
}