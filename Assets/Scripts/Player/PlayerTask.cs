using System.Threading.Tasks;

public abstract class PlayerTask
{
    public abstract PlayerTaskType Type { get; }
    public abstract Task Execute();

    public virtual Task Cancel()
    {
        return Task.CompletedTask;
    }
}