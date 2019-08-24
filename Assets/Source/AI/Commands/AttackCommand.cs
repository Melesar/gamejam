using UnityEngine;

namespace Source.AI
{
    public class AttackCommand : ICommand
    {
        public GameObject Player { get; set; }
        
        public void Execute(AIControllersRepository controllersRepository)
        {
            controllersRepository.GetComponent<AIGroundChargeAttack>().StartAttacking(Player);
        }
    }
}