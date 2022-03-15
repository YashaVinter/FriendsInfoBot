using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameworkAppTest
{
    internal interface StateMachineInterfaces
    {
    }
    public interface Iname
    {
        string name { get; set; }
    }
    public interface IState : Iname
    {
        List<ITransition> transitions { get; set; }

        event Action<string>? action;
        void DoCommand(string cmd);


    }
    public interface ITransition : Iname
    {
        IState entryState { get; set; }
        IState endState { get; set; }
        Func<string, bool>? Criteria { get; set; } //
    }


    public abstract class StateMachineBase
    {
        public Dictionary<string, IState> stateDictionary { get; protected set; } = null;
        public Dictionary<string, ITransition> transitionDictionary { get; protected set; } = null; // TODO разобратьс япочему есть доступ несмотря на protected
        public string currentState { get; protected set; } = null;

        public StateMachineBase(List<string> states, List<string> transitions, string startState)
        {
            DictionaryBilder(states, transitions);
            this.currentState = startState;
        }
        protected abstract void DictionaryBilder(List<string> states, List<string> transitions);

        public abstract void AddAction(string state, Action<string> action);
        public abstract void RemoveAction(string state, Action<string> action);
        public abstract void AddCritera(string transition, Func<string, bool> critera);
        public abstract void RemoveCritera(string transition);
        public abstract string? CheckTransitions(string state, string command);// ret name transition?

        public void Execute(string command)
        {
            string? transitionTo = CheckTransitions(currentState, command);
            if (transitionTo is null)
            {
                stateDictionary[currentState].DoCommand(command);
            }
            else
            {
                this.currentState = transitionTo;
                stateDictionary[currentState].DoCommand(command);
            }
        }
        public abstract void test();


    }
}
