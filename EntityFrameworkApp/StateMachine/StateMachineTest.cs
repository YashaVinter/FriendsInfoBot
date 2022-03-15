using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameworkAppTest
{

    public class State : IState
    {
        public string name { get; set; }
        public List<ITransition> transitions { get; set; } = null!;

        public event Action<string>? action = null;

        public State(string name)
        {
            this.name = name;
        }
        public void DoCommand(string cmd)
        {
            if (action is null)
            {
                throw new Exception("Action not added");
            }
            action?.Invoke(cmd);
        }
    }

    public class Transition : ITransition
    {
        public string name { get; set ; }
        public IState entryState { get; set; } = null;
        public IState endState { get; set; } = null;
        public Transition(string name) {
            this.name = name;
        }
        public Func<string, bool>? Criteria { get; set; } = null;

    }


    internal class StateMachineTest : StateMachineBase
    {
        public StateMachineTest(List<string> states, List<string> transitions, string startState) : base(states, transitions, startState)
        {
            
        }
        protected override void DictionaryBilder(List<string> stateNames, List<string> transitionNames)
        {
            stateDictionary = new Dictionary<string, IState>();
            transitionDictionary = new Dictionary<string, ITransition>();

            foreach (var stateName in stateNames)
            {
                State state = new State(stateName);
                stateDictionary.Add(stateName, state);
            }
            foreach (var transitionName in transitionNames)
            {
                var names = transitionName.Split(new char[] { ':'});
                string entryStateName = names[0];
                string endStateName = names[1];
                State entryState = stateDictionary[entryStateName] as State;
                State endState = stateDictionary[endStateName] as State;

                Transition transition = new Transition(transitionName) { entryState = entryState, endState = endState };
                transitionDictionary.Add(transitionName, transition);
            }
            foreach (var state in stateDictionary.Values)
            {
                var foundTransitions = transitionDictionary.Values.Select(x=>x).Where(x => x.entryState.name == state.name).ToList();
                state.transitions = foundTransitions;
            }
        }

        public override void AddAction(string state, Action<string> action)
        {
            stateDictionary[state].action += action;
        }
        public void AddActionRange(IEnumerable<string> states, IEnumerable<Action<string>> actions) {
            if (states.Count() != actions.Count())
            {
                throw new IndexOutOfRangeException();
            }
            var enumerator = actions.GetEnumerator();
            enumerator.MoveNext();
            foreach (var state in states)
            {
                stateDictionary[state].action += enumerator.Current;
                enumerator.MoveNext();
            }
        }

        public override void RemoveAction(string state, Action<string> action)
        {
            stateDictionary[state].action -= action;
        }

        public override void AddCritera(string transition, Func<string, bool> critera)
        {
            transitionDictionary[transition].Criteria += critera;
        }
        public void AddCriteraRange(IEnumerable<string> transitions, IEnumerable<Func<string, bool>> critera)
        {
            if (transitions.Count() != critera.Count())
            {
                throw new IndexOutOfRangeException();
            }
            var enumerator = critera.GetEnumerator();
            enumerator.MoveNext();
            foreach (var transition in transitions)
            {
                transitionDictionary[transition].Criteria += enumerator.Current;
                enumerator.MoveNext();
            }
        }
        public override void RemoveCritera(string transition)
        {
            transitionDictionary[transition].Criteria = null;
        }
        public override string? CheckTransitions(string state, string command)
        {
            foreach (var transition in stateDictionary[state].transitions)
            {
                if (transition.Criteria is null)
                    throw new Exception("Сriterion not added");

                if (transition.Criteria(command))
                {
                    return transition.endState.name;
                }
            }
            return null;
        }





        public override void test()
        {
            throw new NotImplementedException();
        }


    }
}
