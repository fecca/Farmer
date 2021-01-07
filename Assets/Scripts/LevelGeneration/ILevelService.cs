using Farmer;

public interface ILevelService
{
    void GenerateLevel();
    NodeBehaviour[,] GetLevel();
}