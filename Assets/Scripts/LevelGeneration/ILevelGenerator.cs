using Farmer;

public interface ILevelGenerator
{
    NodeBehaviour[,] GenerateLevel(LevelConfig levelConfig);
}