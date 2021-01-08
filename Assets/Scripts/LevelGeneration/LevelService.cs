public class LevelService : ILevelService
{
    private readonly ILevelGenerator _levelGenerator;

    private NodeBehaviour[,] _level;

    public LevelService(ILevelGenerator levelGenerator)
    {
        _levelGenerator = levelGenerator;
    }

    public void GenerateLevel()
    {
        _level = _levelGenerator.GenerateLevel();
    }

    public NodeBehaviour[,] GetLevel()
    {
        return _level;
    }

    public void CenterNodes(int x, int z)
    {
        _levelGenerator.CenterNodes(x, z);
    }

    public void SetStartNodes(int x, int z)
    {
        _levelGenerator.SetStartNodes(x, z);
    }
}