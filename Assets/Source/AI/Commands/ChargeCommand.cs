using UnityEngine;

namespace Source.AI
{
    public class ChargeCommand : ICommand
    {
        public GameObject Player { get; set; }
        
        public void Execute(AIControllersRepository controllersRepository)
        {
            controllersRepository.GetComponent<AIGroundChargeAttack>().StartAttacking(Player);
        }
    }
}