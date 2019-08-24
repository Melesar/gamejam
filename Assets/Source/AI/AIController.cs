using UnityEngine;

namespace Source.AI
{
    // ReSharper disable once InconsistentNaming
    public class AIController : MonoBehaviour
    {
        [SerializeField] private AIBrainAsset _brainAsset;
        [SerializeField] private Transform _raycastOrigin;

        private AIBrain _brain;
        private AIContext _context;
        private AIControllersRepository _controllersRepository;

        private void Update()
        {
            _brain.UpdateBrain(Time.deltaTime);
        }

        private void LateUpdate()
        {
            while (_brain.PollCommands(out var command))
            {
                command.Execute(_controllersRepository);
            }
        }

        private void Awake()
        {
            var agent = gameObject;
            _context = new AIContext
            {
                raycastOrigin = _raycastOrigin,
            };

            _controllersRepository = new AIControllersRepository(agent);
            _brain = _brainAsset.GetBrain(_context, _controllersRepository);
        }
    }
}