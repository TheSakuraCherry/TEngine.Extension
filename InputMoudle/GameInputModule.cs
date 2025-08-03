using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using TEngine;

namespace GameLogic{
public class GameInputModule : Module,IInputModule,IUpdateModule{
        private List<IInputHandler> _inputHandlers = new List<IInputHandler>();

        private Dictionary<int,List<IInputLayer>> _inputLayersDict = new Dictionary<int,List<IInputLayer>>();

        private Dictionary<EInputAxis,float> _inputAxisDict = new Dictionary<EInputAxis,float>();

        public override void OnInit()
        {
            InputDefine.RegisterInputActions();
            _inputHandlers = new List<IInputHandler>();
            _inputLayersDict = new Dictionary<int,List<IInputLayer>>();
        }

        public T AddInputHander<T>() where T : class, IInputHandler,new(){
            var hander = MemoryPool.Acquire<T>();
            _inputHandlers.Add(hander);
            return hander;
        }

        public void RemoveInputHander<T>() where T : class, IInputHandler,new(){
            foreach(IInputHandler hander in _inputHandlers){
                if(hander is T){
                    _inputHandlers.Remove(hander);
                    MemoryPool.Release(hander);
                    break;
                }
            }
        }

        public T AddInputLayer<T>(int EntityID) where T : class, IInputLayer,new(){
            var layer = MemoryPool.Acquire<T>();
            if(_inputLayersDict.ContainsKey(EntityID)){
                var layers = _inputLayersDict[EntityID];
                layers.Add(layer);
            }else{
                _inputLayersDict.Add(EntityID,new List<IInputLayer>(){layer});
            }
            return layer;
        }

        public void RemoveInputLayer<T>(int EntityID) where T : class, IInputLayer,new(){
            if(_inputLayersDict.ContainsKey(EntityID)){
                var layers = _inputLayersDict[EntityID];
                for(int i = layers.Count - 1; i >= 0; i--){
                    if(layers[i] is T){
                        layers.RemoveAt(i);
                        MemoryPool.Release(layers[i]);
                        break;
                    }
                }
                if(layers.Count == 0){
                    _inputLayersDict.Remove(EntityID);
                }
            }
        }

        public List<IInputLayer> GetInputLayers(int EntityID)
        {
            if(_inputLayersDict.ContainsKey(EntityID)){
                return _inputLayersDict[EntityID].OrderBy(layer => layer.Priority).ToList();
            }
            return null;
        }

        public void Update(float elapseSeconds, float realElapseSeconds)
        {
            foreach(IInputHandler hander in _inputHandlers){
                hander.PostProcessInput(elapseSeconds,realElapseSeconds);
            }
        }

        /// <summary>
        /// 接收来自引擎或者UI的动作输入
        /// </summary>
        public void SetInputAction(EInputAction action,EInputState state,double time)
        {
            //遍历InputHandler
            foreach(IInputHandler hander in _inputHandlers){
                hander.HandleInputEvent(action,state,time);
            }
        }

        /// <summary>
        /// 接收来自引擎或者UI的轴输入
        /// </summary>
        public void SetInputAxis(EInputAxis axis,float value)
        {
            if(_inputAxisDict.ContainsKey(axis)){
                _inputAxisDict[axis] = value;
            }else{
                _inputAxisDict.Add(axis,value);
            }
            foreach(IInputHandler hander in _inputHandlers){
                hander.HandleInputAxis(axis,value);
            }
        }


        public float GetInputAxis(EInputAxis axis)
        {
            if(_inputAxisDict.ContainsKey(axis)){
                return _inputAxisDict[axis];
            }
            return 0;
        }



        public override void Shutdown()
        {
            
        }

        
    }
}

