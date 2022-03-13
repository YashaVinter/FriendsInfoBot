using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace EntityFrameworkApp
{
    public class StateMachine
    {
        public string currentState { get; set; } = null;
        public SMData smData { get; set; } = new SMData();
        public StateMachine() { 
            //treeNode.
        }
        public void AddStateAction(string stateName, Action<string> action, string command) {
            smData.AddAction(stateName, action, command);

        }
        public void ExecuteCurrentCommand() {
            smData.ExecuteCommand(currentState);
        }

        public void test() {
            //State one = new State();
            //State two = new State();
            //State three = new State();
            //State four = new State();

            //StateTransition onetwo = new StateTransition(ref one, ref two);
            //StateTransition onethree = new StateTransition(ref one, ref three);
            //StateTransition twofour = new StateTransition(ref two, ref four);
            //StateTransition threefour = new StateTransition(ref three, ref four);

            //one.transitionFrom = new List<StateTransition> { onetwo, onethree };
            //two.transitionFrom = new List<StateTransition> { twofour };
            //three.transitionFrom = new List<StateTransition> { threefour };
            //four.transitionFrom = new List<StateTransition> { };

            // start
            StateMachine stateMachine = new StateMachine();

            stateMachine.smData.SetTransition("one", new List<string>() {
                "onetwo",
                "onethree",
            });
            stateMachine.smData.SetTransition("two", new List<string>() {
                "twofour"
            });
            stateMachine.smData.SetTransition("three", new List<string>() {
                "threefour"
            });

            stateMachine.smData.SetStateForStateTransition("one", "two");
            stateMachine.smData.SetStateForStateTransition("one", "three");
            stateMachine.smData.SetStateForStateTransition("two", "four");
            stateMachine.smData.SetStateForStateTransition("three", "four");
            // end creating
            stateMachine.currentState = "one";

            State state = new State("state");
            state.Notify += new SMCommands().OneCommand;

            //state.DoCommands();

            stateMachine.AddStateAction("one", new SMCommands().OneCommand, "write one!");
            stateMachine.currentState = "one";
            stateMachine.ExecuteCurrentCommand();

            //stateMachine.SMData.SetTransition("one", new List<StateTransition>());
            //stateMachine.currentState = one;


        }
    }
    public class State
    {   
        public string name { get; set; }
        public string command { get; set; }
        //List<StateTransition> transitionTo { get; set; } = null!;
        public List<string> transitionNames { get; set; } = null!;//from this State
        public State(string name) {
            this.name = name;
        }

        public event Action<string>? Notify;
        public void DoCommands() {
            Notify?.Invoke(command);
        }
    }
    public class StateTransition
    {
        public string name { get; set; }
        public string initState { get; set; } = null!;
        public string finalState { get; set; } = null!;
        public string criteria { get; set; } = null!;
        public StateTransition(string initState, string finalState)
        {
            this.name = initState+ finalState;
            this.initState = initState;
            this.finalState = finalState;
        }

        public Func<string,bool>? func;
        public bool TransitionCriteria(string s) {
            Func<string, bool> f = str => { return str == ""; };
            return true;
        }
    }
    public enum CurrentStateInfo { 
        init,
        first
    }
    public class SMData
    {// TO DO make Dictionary<string, State> StateCreator (List<string> states, List<string> StateTransition) and as well for StateTransitionCreator
        //private List<State> states = new List<State> {
        //    new State("one"),
        //    new State("two"),
        //    new State("three"),
        //    new State("four"),
        //};
        //private List<StateTransition> stateTransitions = new List<StateTransition> {
        //    new StateTransition("one","two"),
        //    new StateTransition("one","three"),
        //    new StateTransition("two","four"),
        //    new StateTransition("three","four")
        //};

        private Dictionary<string, State> stateDictionary = new Dictionary<string, State>() 
        {
            {"one",new State("one") },
            {"two",new State("two") },
            {"three",new State("three") },
            {"four",new State("four") },
        };
        private Dictionary<string, StateTransition> stateTransitionDictionary = new Dictionary<string, StateTransition>()
        {
            {"onetwo", new StateTransition("one","two") },
            {"onethree", new StateTransition("one","three") },
            {"twofour", new StateTransition("two","four") },
            {"threefour", new StateTransition("three","four") }
        };

        public SMData() {
            //states.First(st => st.name == "");
        }
        public void AddAction(string stateName, Action<string> action, string command) {
            stateDictionary[stateName].Notify += action;
            stateDictionary[stateName].command = command;
        }


        public void RemoveAction(string stateName, Action<string> action) =>
            stateDictionary[stateName].Notify += action;
        public void ExecuteCommand(string stateName) {
            stateDictionary[stateName].DoCommands();
        }
        public void SetTransition(string stateName, List<string> transitions) {
            stateDictionary[stateName].transitionNames = transitions;
        }
        public void SetStateForStateTransition(string initState, string finalState)
        {
            string stateTransitionName = initState + finalState;
            stateTransitionDictionary[stateTransitionName].initState = initState;
            stateTransitionDictionary[stateTransitionName].finalState = finalState;
        }
    }
    public class SMCommands
    {
        public void OneCommand(string st) {
            Console.WriteLine(st);
        }
    }
    public enum EState
    {
        one,
        two,
        three,
        four
    }
    public enum EStateTransition
    {
        onetwo,
        onethree,
        twofour,
        threefour
    }
}
