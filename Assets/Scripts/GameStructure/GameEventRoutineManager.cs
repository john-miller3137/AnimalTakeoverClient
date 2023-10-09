using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using SharedLibrary;
using UnityEngine;
namespace Scripts.GameStructure
{
    public class GameEventRoutineManager : MonoBehaviour
    {
        private static GameEventRoutineManager instance;
        public static GameEventRoutineManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<GameEventRoutineManager>();
                    if (instance == null)
                    {
                        GameObject go = new GameObject("GameEventRoutineManager");
                        instance = go.AddComponent<GameEventRoutineManager>();
                    }
                }
                return instance;
            }
        }

        private void Start()
        {
            _routinesScheduled = new List<GameEventRoutine>();
            routinesScheduled = new ConcurrentDictionary<int, bool>();
        }

        private object lockObject = new object(); // Lock object for synchronization

        private Queue<Tuple<int, GameEventRoutine>> attackQueue = new Queue<Tuple<int, GameEventRoutine>>();
        private Queue<Tuple<int, GameEventRoutine>> movementQueue = new Queue<Tuple<int, GameEventRoutine>>();
        private Queue<Tuple<int, GameEventRoutine>> effectsQueue = new Queue<Tuple<int, GameEventRoutine>>();
        private ConcurrentDictionary<int, bool> routinesScheduled = new ConcurrentDictionary<int, bool>();
        private ConcurrentDictionary<int, bool> routinesCompleted = new ConcurrentDictionary<int, bool>();
        private List<GameEventRoutine> _routinesScheduled;// New dictionary to track completed routines

        private object schedulerlock = new object();
        private static int threadSafeCounter = 0;

        private void Update()
        {
            ProcessRoutinesQueue(attackQueue);
            ProcessRoutinesQueue(effectsQueue);
            ProcessRoutinesQueue(movementQueue);
        }

        private void ProcessRoutinesQueue(Queue<Tuple<int, GameEventRoutine>> queue)
        {
            lock (lockObject)
            {
                while (queue.Count > 0)
                {
                    var tuple = queue.Dequeue(); // Peek to check, not dequeue
                    int key = tuple.Item1;
                    var routine = tuple.Item2;

                    

                    StartCoroutine(ExecuteRoutine(key, routine, queue));
                }
            }
        }

        public void AddRoutine(GameEventRoutine routine)
        {
            lock (lockObject)
            {
                Interlocked.Increment(ref threadSafeCounter);
                int key = threadSafeCounter;

                if (routine.gameEvent == GameEvent.Attack)
                {
                    attackQueue.Enqueue(new Tuple<int, GameEventRoutine>(key, routine));
                }
                else if (routine.gameEvent == GameEvent.Move)
                {
                    movementQueue.Enqueue(new Tuple<int, GameEventRoutine>(key, routine));
                }
                else if (routine.gameEvent == GameEvent.EndTurnEffects)
                {
                    effectsQueue.Enqueue(new Tuple<int, GameEventRoutine>(key, routine));
                }
            }
        }

        private IEnumerator ExecuteRoutine(int key, GameEventRoutine routine, Queue<Tuple<int, GameEventRoutine>> queue)
        {
            Debug.Log($"routine waiting for conflict (executing...) - animalId: {routine.animalId} targetId: {routine.targetId}" +
                      $"event: {routine.gameEvent}");
            // Wait until there are no more conflicting routines
            yield return WaitForConflicts(routine);

            lock (schedulerlock)
            {
                _routinesScheduled.Add(routine);
            }

            Debug.Log($"routine not conflict (executing...) - animalId: {routine.animalId} targetId: {routine.targetId}" +
                      $"event: {routine.gameEvent}");
            // Execute the routine
            yield return new WaitForEndOfFrame();
            
            yield return routine.Execute();
            
            yield return new WaitForEndOfFrame();
            
            lock (schedulerlock)
            {
                _routinesScheduled.Remove(routine);
            }
           
            yield return null;
        }
        
        
        
        private IEnumerator WaitForConflicts(GameEventRoutine routine)
        {
            lock (schedulerlock)
            {
                Debug.Log("scheduler lock locked");

                bool hasConflicts;

                if (routine.targetId == -1)
                {
                    // When targetId is -1, only check for conflicts based on animalId.
                    hasConflicts = _routinesScheduled.Any(r =>
                        (r.animalId == routine.animalId || r.targetId == routine.animalId) && r != routine);
                }
                else
                {
                    // Check for conflicts based on both animalId and targetId.
                    hasConflicts = _routinesScheduled.Any(r =>
                        (r.animalId == routine.animalId || r.targetId == routine.targetId || 
                         r.animalId == routine.targetId || r.targetId == routine.animalId) && r != routine);
                }

                while (hasConflicts)
                {
                    Debug.Log($"{hasConflicts} hasConflicts");
                    yield return new WaitForEndOfFrame(); // Wait until the end of the frame

                    if (routine.targetId == -1)
                    {
                        // When targetId is -1, only check for conflicts based on animalId.
                        hasConflicts = _routinesScheduled.Any(r =>
                            (r.animalId == routine.animalId || r.targetId == routine.animalId) && r != routine);
                    }
                    else
                    {
                        // Check for conflicts based on both animalId and targetId.
                        hasConflicts = _routinesScheduled.Any(r =>
                            (r.animalId == routine.animalId || r.targetId == routine.targetId || 
                             r.animalId == routine.targetId || r.targetId == routine.animalId) && r != routine);
                    }
                }
            }

            yield return null;
        }
        public bool IsConflict(int animalId)
        {
            
            Debug.Log($"{animalId} is THE routines animalId");
            lock (lockObject)
            {
                foreach (var existingRoutine in attackQueue)
                {
                    if ((existingRoutine.Item2.animalId == animalId || existingRoutine.Item2.targetId == animalId) && animalId != -1)
                    {
                        // Conflict found
                        return true;
                    }
                }
            }
            
            
            // No conflicts
            return false;
        }
        
        
        private bool IsConflict(GameEventRoutine routine)
        {
            lock (lockObject)
            {
                foreach (var existingRoutine in attackQueue)
                {
                    if (existingRoutine.Item2 == routine)
                        continue;

                    if (existingRoutine.Item2.targetId == -1 && routine.targetId == -1 && existingRoutine.Item2.animalId != routine.animalId)
                        continue;

                    if (existingRoutine.Item2.animalId == routine.animalId || existingRoutine.Item2.targetId == routine.targetId
                                                                           || existingRoutine.Item2.animalId == routine.targetId || existingRoutine.Item2.targetId == routine.animalId)
                    {
                        // Conflict found
                        return true;
                    }
                }
            }
            

            // No conflicts
            return false;
        }
        
    }
}