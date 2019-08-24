using System.Collections.Generic;
using UnityEngine;

namespace Source.AI
{
    public abstract class AIBrainAsset : ScriptableObject
    {
        public abstract AIBrain GetBrain(AIContext context, AIControllersRepository controllersRepository);
    }
    
    public abstract class AIBrain
    {
        private readonly Queue<ICommand> _commandPool = new Queue<ICommand>();

        protected AIContext Context { get; }
        
        private readonly AIControllersRepository _controllersRepository;

        protected Transform Transform => _controllersRepository.Transform;

        public bool PollCommands(out ICommand command)
        {
            if (_commandPool.Count == 0)
            {
                command = null;
                return false;
            }

            command = _commandPool.Dequeue();
            return true;
        }

        public abstract void UpdateBrain(float dt);

        protected AIBrain(AIContext context, AIControllersRepository controllersRepository)
        {
            _controllersRepository = controllersRepository;
            Context = context;
        }

        protected T GetComponent<T>() where T : Component
        {
            return _controllersRepository.GetComponent<T>();
        }

        protected void SubmitCommand(ICommand command)
        {
            _commandPool.Enqueue(command);
        }
    }
}