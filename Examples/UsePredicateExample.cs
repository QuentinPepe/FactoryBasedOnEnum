using System;

namespace FactoryBasedOnEnum.Examples
{
    public class UsePredicateExample
    {
        private readonly GenericFactory<AgentType, IAgent> _agentFactory;

        public UsePredicateExample(IServiceProvider serviceProvider)
        {
            _agentFactory = new GenericFactory<AgentType, IAgent>(serviceProvider: serviceProvider);
        }

        public IAgent CreateAgent(AgentType type, bool missionCritical)
        {
            return _agentFactory.GetInstance(type, agent => IsMissionSuitableForAgent(agent, missionCritical));
        }

        private bool IsMissionSuitableForAgent(IAgent agent, bool missionCritical)
        {
            return agent is Spy && missionCritical || agent is Diplomat && !missionCritical;
        }
    }

    public enum AgentType
    {
        Spy,
        Diplomat,
        Soldier
    }

    public interface IAgent
    {
        void PerformMission();
    }

    [EnumAssociated(typeof(AgentType), AgentType.Spy)]
    public class Spy : IAgent
    {
        public void PerformMission()
        {
            Console.WriteLine("Spy performing a covert operation.");
        }
    }

    [EnumAssociated(typeof(AgentType), AgentType.Diplomat)]
    public class Diplomat : IAgent
    {
        public void PerformMission()
        {
            Console.WriteLine("Diplomat engaging in negotiations.");
        }
    }

    [EnumAssociated(typeof(AgentType), AgentType.Soldier)]
    public class Soldier : IAgent
    {
        public void PerformMission()
        {
            Console.WriteLine("Soldier executing a combat mission.");
        }
    }
}