using UnityEngine;

namespace Source.AI
{
    public class MoveCommand : ICommand
    {
        public Vector3 Direction { get; set; }
        
        public void Execute(AIControllersRepository controllersRepository)
        {
            var movementController = controllersRepository.GetComponent<AIMovement>();
            movementController.Move(Direction);
        }
    }
}