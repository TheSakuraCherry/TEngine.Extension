using GameLogic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;
namespace GameLogic{

    public enum EInputAction{
    Attack,
    Skill,
    EnhanceSkill,
    Sprint,
    Jump,
}

public enum EInputAxis{
    MoveForward,
    MoveRight,
    LookUp,
    LookRight,
    ZoomUp,
    ZoomDown,
}

public enum EInputState{
    Press,
    Hold,
    Release,
}

public enum EInputCommandType{
    None,
    Skill,
    Jump,
    Sprint,
    //等等具体行为
}


//输入指令最上游
public struct InputEvent{
    public InputEvent(EInputAction action,EInputState state,double time){
        Action = action;
        State = state;
        Time = time;
    }
    public EInputAction Action;
    public EInputState State;
    public double Time;
}

//用于具体执行指令，最下游
public struct SInputCommand{

    public SInputCommand(EInputCommandType commandType,int intValue,int tagValue){
        CommandType = commandType;
        IntValue = intValue;
        TagValue = tagValue;
    }
    public EInputCommandType CommandType;
    //储存技能ID等
    public int IntValue;
    //储存为玩家添加Tag等
    public int TagValue;
}
//中游，由layer转化
public struct InputCommand{

    public InputCommand(EInputAction action,EInputState state,SInputCommand command,double time,int index){
        Action = action;
        State = state;
        Command = command;
        Time = time;
        Index = index;
    }
    public EInputAction Action;
    public EInputState State;

    public double Time;
    public SInputCommand Command;
    public int Index;
}

public struct InputCache{
    public InputCache(EInputAction action,EInputState state,double eventTime,double time,double accumulateTime){
        Action = action;
        State = state;
        EventTime = eventTime;
        Time = time;
        AccumulateTime = accumulateTime;
    }
    public EInputAction Action;
    public EInputState State;
    //原始输入事件时间
    public double EventTime;
    //缓存创建时的世界时间
    public double Time;
    //缓存累计存在时间
    public double AccumulateTime;
}


    public static class InputDefine{
        private static GameInputActions _inputActions;

        public static void RegisterInputActions(){
            _inputActions = new GameInputActions();
            _inputActions.Enable();

            _inputActions.GamePlay.Attack.started += (ctx)=>{
                GameModule.Input.SetInputAction(EInputAction.Attack,EInputState.Press,ctx.time);
            };

            _inputActions.GamePlay.Attack.performed += (ctx)=>{
                if(ctx.interaction is HoldInteraction){
                    GameModule.Input.SetInputAction(EInputAction.Attack,EInputState.Hold,ctx.time);
                }
            };

            _inputActions.GamePlay.Attack.canceled += (ctx)=>{
                GameModule.Input.SetInputAction(EInputAction.Attack,EInputState.Release,ctx.time);
            };


            _inputActions.GamePlay.Skill.started += (ctx)=>{
                GameModule.Input.SetInputAction(EInputAction.Skill,EInputState.Press,ctx.time);
            };

            _inputActions.GamePlay.Skill.performed += (ctx)=>{
                if(ctx.interaction is HoldInteraction){
                    GameModule.Input.SetInputAction(EInputAction.Skill,EInputState.Hold,ctx.time);
                }
            };
            
            _inputActions.GamePlay.Skill.canceled += (ctx)=>{
                GameModule.Input.SetInputAction(EInputAction.Skill,EInputState.Release,ctx.time);
            };

            _inputActions.GamePlay.EnhanceSkill.started += (ctx)=>{
                GameModule.Input.SetInputAction(EInputAction.EnhanceSkill,EInputState.Press,ctx.time);
            };

            _inputActions.GamePlay.EnhanceSkill.canceled += (ctx)=>{
                GameModule.Input.SetInputAction(EInputAction.EnhanceSkill,EInputState.Release,ctx.time);
            };

            _inputActions.GamePlay.EnhanceSkill.performed += (ctx)=>{
                if(ctx.interaction is HoldInteraction){
                    GameModule.Input.SetInputAction(EInputAction.EnhanceSkill,EInputState.Hold,ctx.time);
                }
            };
            _inputActions.GamePlay.Move.performed += (ctx)=>{
                GameModule.Input.SetInputAxis(EInputAxis.MoveForward,ctx.ReadValue<Vector2>().y);
                GameModule.Input.SetInputAxis(EInputAxis.MoveRight,ctx.ReadValue<Vector2>().x);
            };
            _inputActions.GamePlay.Move.canceled += (ctx)=>{
                GameModule.Input.SetInputAxis(EInputAxis.MoveForward,0);
                GameModule.Input.SetInputAxis(EInputAxis.MoveRight,0);
            };

            _inputActions.GamePlay.Look.performed += (ctx)=>{
                GameModule.Input.SetInputAxis(EInputAxis.LookUp,ctx.ReadValue<Vector2>().y);
                GameModule.Input.SetInputAxis(EInputAxis.LookRight,ctx.ReadValue<Vector2>().x);
            };
            _inputActions.GamePlay.Look.canceled += (ctx)=>{
                GameModule.Input.SetInputAxis(EInputAxis.LookUp,0);
                GameModule.Input.SetInputAxis(EInputAxis.LookRight,0);
            };
        }

    

    }
}