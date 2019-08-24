namespace Source.AI
{
    public class StopChargeAttackCommand : ICommand
    {
        public void Execute(AIControllersRepository controllersRepository)
        {
            controllersRepository.GetComponent<AIGroundChargeAttack>().StopAttacking();
        }
    }
}