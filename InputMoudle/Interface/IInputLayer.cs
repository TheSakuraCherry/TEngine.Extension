using TEngine;
using Unity.IO.LowLevel.Unsafe;

namespace GameLogic{

public interface IInputLayer : IMemory{

    int Priority{get;}

    SInputCommand HandlePress();

    SInputCommand HandleRelease();

    SInputCommand HandleHold();

    void HandlePressEvent();

    void HandleReleaseEvent();

    void HandleHoldEvent();
    }
}
