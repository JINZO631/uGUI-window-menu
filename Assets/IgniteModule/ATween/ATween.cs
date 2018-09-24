using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ATweening
{
    public partial class ATween : MonoBehaviour
    {
        private static ATween instance;
        public static ATween Instance
        {
            get
            {
                Initialize();
                return instance;
            }
        }

        private static bool initialized = false;
        private static object lockObj = new object();
        private static List<IEnumerator> coroutines = new List<IEnumerator>();
        private static List<IATweener> tweeners = new List<IATweener>();
        private static List<Sequence> sequences = new List<Sequence>();

        public static void Initialize()
        {
            if (initialized) return;

            instance = new GameObject("[ATween]").AddComponent<ATween>();

            initialized = true;
        }

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            if (0 < tweeners.Count)
            {
                var tweenerList = new List<IATweener>(tweeners);
                tweeners.Clear();

                foreach (var tweener in tweenerList)
                {
                    if (DefaultAutoPlay)
                    {
                        StartCoroutine(tweener.TweenCoroutine());
                    }
                }
            }

            if (0 < sequences.Count)
            {
                var sequenceList = new List<Sequence>(sequences);
                sequences.Clear();
                foreach (var sequence in sequenceList)
                {
                    if (DefaultAutoPlay)
                    {
                        StartCoroutine(sequence.TweenCoroutine());
                    }
                }
            }

            if (0 < coroutines.Count)
            {
                lock (lockObj)
                {
                    var commitingList = new List<IEnumerator>(coroutines);
                    coroutines.Clear();

                    foreach (var coroutine in commitingList)
                    {
                        StartCoroutine(coroutine);
                    }
                }
            }
        }

        public void Commit(IEnumerator ienumerator)
        {
            lock (lockObj)
            {
                coroutines.Add(ienumerator);
            }
        }
    }
}