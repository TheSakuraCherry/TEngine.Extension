using TEngine;

namespace GameLogic{

public interface IInputHandler : IMemory{
    void HandleInputEvent(EInputAction action , EInputState state , double time);

    void HandleInputAxis(EInputAxis axis,float value);

    float GetInputAxis(EInputAxis axis);

    void PostProcessInput(float elapseSeconds, float realElapseSeconds);
    }
}   