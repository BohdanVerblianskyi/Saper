using System.Collections.Generic;

public interface IInitCell: ICell
{
    void Init(List<IInitCell> neighbors);
    void AddBomb();
}