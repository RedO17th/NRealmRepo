using System;

public interface IScalable
{
    event Action OnIncreaseEvent;
    event Action OnDecreaseEvent;

    float CurrentScale { get; }

    void Increase();
    void Decrease();
}
