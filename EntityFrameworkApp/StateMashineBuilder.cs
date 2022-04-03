using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntityFrameworkApp.Data;
using StateMachineLibrary;
using Telegram.Bot.Types.ReplyMarkups;
using static EntityFrameworkApp.Data.FriendsBotData;

namespace EntityFrameworkApp
{
    internal class StateMashineBuilder
    {
        public IEnumerable<StateDataSet> statesData { get; set; }
        public IEnumerable<TrasitionDataSet> transitionsData { get; set; }
        public string startState { get; set; }
        public StateMashineBuilder(IEnumerable<StateDataSet> statesData, IEnumerable<TrasitionDataSet> transitionsData, string startState)
        {
            this.statesData = statesData;
            this.transitionsData = transitionsData;
            this.startState = startState;
        }
        public StateMachine Build() {
            var states = from sd in statesData
                         select new StateBuilder(sd).Build();
            var transitions = from td in transitionsData
                              select new TransitionBuilder(td).Build();
            ValidateData(states, transitions);
            ConnectStatesAndTransitions(ref states, ref transitions);
            return new StateMachine(states,transitions,startState);
            throw new NotImplementedException();
        }
        private void ValidateData(IEnumerable<State> states, IEnumerable<Transition> transitions)
        {
            // check repiting names in states
            // check repiting names in transitions
            throw new NotImplementedException();
        }
        private void ConnectStatesAndTransitions(ref IEnumerable<State> states, ref IEnumerable<Transition> transitions)
        {

        }
        public void test()
        {
            // init data  
            string home = "home";
            ISet<ITransitionModel> transitionModels = null;
            FunctionHandler fh = (sd) => { return null; };
            string caseText = "";
            IReplyMarkup buttons = null;
            var inputdata = new BotInputData(null, null);
            //init 2
            var states = StateMachineData.States.getInstance();

            //
            //StateDataNew state1 = new StateDataNew() 
            //{
            //    name = "home",
            //    transitions = null,
            //    functionHandler = null,
            //    caseText = "",
            //    buttons = null,
            //    botInputData = null
            //};
            //StateDataNew state2 = (StateDataNew)state1.Clone();
            //state2.name = "find";
            //state2.caseText = "text";
            TrasitionDataNew trasition1 = new TrasitionDataNew() 
            {
                name = "",
                entryState = null,
                endState = null,
                Criteria = null
            };
            // test
            StateDataSetBase stateBase = new StateDataSetBase("base", null, null, null);
            StateDataSet state3 = new StateDataSet("find","write person name",stateBase); //(StateDataNew2)stateBase.Clone();

        }
        class StateBuilder
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
        //class StateModelBuilder
        //{
        //    public string stateName { get; }
        //    public StateModelBuilder(string stateName)
        //    {
        //        this.stateModel = Build(stateName);
        //    }
        //    private StateModel Build(string stateName) {
        //        return new StateModel(stateName);
        //    }
        //}
        //class StateEventBuilder
        //{
        //    public StateEvent stateEvent { get; }
        //    public StateEventBuilder(FunctionHandler functionHandler)
        //    {
        //        this.stateEvent = Build(functionHandler);
        //    }
        //    private StateEvent Build(FunctionHandler functionHandler) {
        //        return new StateEvent(functionHandler);
        //    }
        //}
        //class StateDataBuilder
        //{
        //    public StateData stateData { get; }
        //    public StateDataBuilder(string caseText, IReplyMarkup buttons, InputDataBase inputData)
        //    {
        //        this.stateData = Build(caseText, buttons, inputData);
        //    }
        //    private StateData Build(string caseText, IReplyMarkup buttons, InputDataBase inputData)
        //    {
        //        var eventData = new EventData(caseText, buttons); 
        //        return new StateData(eventData, inputData);
        //    }
        //}
        //class StateEventDataBuilder
        //{
        //    public EventData eventData { get; }
        //    public StateEventDataBuilder(string caseText, IReplyMarkup buttons)
        //    {
        //        this.eventData = Build(caseText, buttons);
        //    }
        //    private EventData Build(string caseText, IReplyMarkup buttons)
        //    {
        //        return new EventData(caseText, buttons);
        //    }
        //}
        //

        class TransitionBuilder
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
        //class TransitionModelBuilder
        //{
        //    public ITransitionModel transitionModel { get; }
        //    public TransitionModelBuilder(string name)
        //    {
        //        this.transitionModel = Build(name);
        //    }
        //    private ITransitionModel Build(string name)
        //    {
        //        return new TransitionModel(name);
        //    }
        //    public void AddStatesModels(StateModel entryState, StateModel endState) { 
        //        transitionModel.entryState = entryState;
        //        transitionModel.endState = endState;
        //    }
        //}
        //class TransitionCriteriaBuilder
        //{
        //    public ITransitionCriteria transitionCriteria { get; }
        //    public TransitionCriteriaBuilder(Predicate<string> criteria)
        //    {
        //        this.transitionCriteria = Build(criteria);
        //    }
        //    private TransitionCriteria Build(Predicate<string> criteria)
        //    {
        //        return new TransitionCriteria(criteria);
        //    }
        //}

    }
    class NameBase
    {
        public string name { get; set; }
        public NameBase(string name) => this.name = name;        
    }
    class StateDataSetBase : NameBase, ICloneable
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
    class StateDataSet : StateDataSetBase
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
    class StateDataNew : ICloneable
    {
        public string name { get; set; }
        public ISet<ITransitionModel> transitions { get; set; }
        public FunctionHandler functionHandler { get; set; }
        public string caseText { get; set; }
        public IReplyMarkup buttons { get; set; }
        public BotInputData botInputData { get; set; }
        public object Clone()
        {
            return MemberwiseClone();
        }
    }
    class TrasitionDataNew
    {
        public string name { get; set; }
        public IStateModel entryState { get; set; }
        public IStateModel endState { get; set; }
        public Predicate<string> Criteria { get; set; }
    }
    class TrasitionDataSet : NameBase
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
// test14StateMachine()