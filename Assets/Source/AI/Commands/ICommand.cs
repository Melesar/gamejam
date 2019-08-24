namespace Source.AI
{
    public interface ICommand
    {
        void Execute(AIControllersRepository controllersRepository);
    }
}