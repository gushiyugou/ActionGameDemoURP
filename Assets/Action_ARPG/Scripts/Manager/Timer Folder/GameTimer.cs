using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Action_ARPG
{
    public enum TimerState
    {
        NOTWORKERED,//没有工作
        WORKERING,//工作中
        DONE//工作完成
    }
    
    public class GameTimer
    {
        private float _startTime;
        private Action _task;
        private bool _isStopTimer;
        private TimerState _timerState;
    
        public GameTimer()
        {
            ResetTimer();
        }
    
        public void StartTimer(float time, Action task)
        {
            _startTime = time;
            _task = task;
            _isStopTimer = false;
            _timerState = TimerState.WORKERING;
        }
        
        //更新计时器
        public void UpdateTimer()
        {
            if (_isStopTimer) return;
    
            _startTime -= Time.deltaTime;
            if (_startTime < 0f)
            {
                _task?.Invoke();
                _timerState = TimerState.DONE;
                _isStopTimer = true;
            }
        }
    
        public TimerState GetTimerState() => _timerState;
    
        public void ResetTimer()
        {
            _startTime = 0f;
            _task = null;
            _isStopTimer = true;
            _timerState = TimerState.NOTWORKERED;
        }
    }
}

