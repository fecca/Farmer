public class LevelService : ILevelService
{
    private readonly ILevelGenerator _levelGenerator;
    private readonly ILevelRepresentation _levelRepresentation;

    private float[,] _level;

    public LevelService(ILevelGenerator levelGenerator, ILevelRepresentation levelRepresentation)
    {
        _levelRepresentation = levelRepresentation;
        _levelGenerator = levelGenerator;
    }

    public void GenerateLevel()
    {
        _level = _levelGenerator.GenerateLevel();
    }

    public void DrawLevel(int x, int z)
    {
        _levelRepresentation.DrawLevel(_level, x, z);
    }

    public float[,] GetLevel()
    {
        return _level;
    }

    public void CenterNodes(int x, int z)
    {
        // _levelGenerator.CenterNodes(x, z);
    }

    public void SetStartNodes(int x, int z)
    {
        // _levelGenerator.SetStartNodes(x, z);
    }
}