using Farmer;

public class LevelService : ILevelService
{
    private readonly ILevelGenerator _levelGenerator;
    private readonly LevelConfig _levelConfig;

    private NodeBehaviour[,] _level;

    public LevelService(ILevelGenerator levelGenerator, LevelConfig levelConfig)
    {
        _levelGenerator = levelGenerator;
        _levelConfig = levelConfig;
    }

    public void GenerateLevel()
    {
        _level = _levelGenerator.GenerateLevel(_levelConfig);
    }

    public NodeBehaviour[,] GetLevel()
    {
        return _level;
    }
}