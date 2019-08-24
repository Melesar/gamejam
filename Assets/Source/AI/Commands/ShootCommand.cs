using UnityEngine;

namespace Source.AI
{
    public class ShootCommand : ICommand
    {
        public Vector3 Direction { get; set; }
        
        public void Execute(AIControllersRepository controllersRepository)
        {
            var controller = controllersRepository.GetComponent<AIShootingBehaviour>();
            controller.Shoot(Direction);
        }
    }
}