using System.Collections;
using GGG.Tool.Singleton;
using UnityEngine;
using System.Collections.Generic;
using System;
using System.Timers;
using UnityEngine.PlayerLoop;

namespace Action_ARPG
{
    public class TimerManager : Singleton<TimerManager>
    {
        [SerializeField] private int _initMaxTimerCount;
        
        
        private Queue<GameTimer> _notWorkerTimer = new Queue<GameTimer>();
        private List<GameTimer> _workeringTimer = new List<GameTimer>();

        private void Start()
        {
            InitTimerManager();
        }

        private void Update()
        {
            UpdateWorkeringTimer();
        }

        private void InitTimerManager()
        {
            for (int i = 0; i < _initMaxTimerCount; i++)
            {
                CreatTimer();
            }
        }

        private void CreatTimer()
        {
            var timer = new GameTimer();
            _notWorkerTimer.Enqueue(timer);
        }

        public void TryGetOneTimer(float time, Action action)
        {
            if (_notWorkerTimer.Count == 0)
            {
                CreatTimer();
                GameTimer timer = _notWorkerTimer.Dequeue();
                timer.StartTimer(time,action);
                _workeringTimer.Add(timer);
            }
            else
            {
                GameTimer timer = _notWorkerTimer.Dequeue();
                timer.StartTimer(time,action);
                _workeringTimer.Add(timer);
            }
        }

        private void UpdateWorkeringTimer()
        {
            if(_workeringTimer.Count == 0) return;
            for (int i = 0; i < _workeringTimer.Count; i++)
            {
                if (_workeringTimer[i].GetTimerState() == TimerState.WORKERING)
                {
                    _workeringTimer[i].UpdateTimer();
                }
                else
                {
                    _notWorkerTimer.Enqueue(_workeringTimer[i]);
                    _workeringTimer[i].ResetTimer();
                    _workeringTimer.Remove(_workeringTimer[i]);
                }
            }
        }
        
    }
}