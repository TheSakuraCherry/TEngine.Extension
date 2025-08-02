using System;
using System.Collections.Generic;

namespace GameLogic{

public interface IInputModule{

        T AddInputHander<T>() where T : class, IInputHandler,new();

        void RemoveInputHander<T>() where T : class, IInputHandler,new();

        T AddInputLayer<T>(int EntityID) where T : class, IInputLayer,new();
        
        void RemoveInputLayer<T>(int EntityID) where T : class, IInputLayer,new();

        List<IInputLayer> GetInputLayers(int EntityID);

        /// <summary>
        /// 接收来自引擎或者UI的动作输入
        /// </summary>
        void SetInputAction(EInputAction action,EInputState state,double time);

        /// <summary>
        /// 接收来自引擎或者UI的轴输入
        /// </summary>
        void SetInputAxis(EInputAxis axis,float value);

        /// <summary>
        /// 获取轴输入
        /// </summary>
        float GetInputAxis(EInputAxis axis);
    
    }
}