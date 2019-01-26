using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public static class MonoBehaviourExtension
{
    private static YieldInstruction waitForFixedUpdate = new WaitForFixedUpdate();
    private static YieldInstruction waitForEndOfFrame = new WaitForEndOfFrame();

    /// <summary>
    /// Run action on next update
    /// </summary>
    /// <param name="action">The Action to be executed</param>
    /// <returns>Coroutine</returns>
    public static Coroutine RunOnNextUpdate(this MonoBehaviour monoBehaviour, Action action)
    {
        return monoBehaviour.StartCoroutine(NextUpdateCoroutine(action));
    }
    private static IEnumerator NextUpdateCoroutine(Action action)
    {
        yield return null;
        action();
    }

    /// <summary>
    /// Run action on next fixed update
    /// </summary>
    /// <param name="action">The Action to be executed</param>
    /// <returns>Coroutine</returns>
    public static Coroutine RunOnNextFixedUpdate(this MonoBehaviour monoBehaviour, Action action)
    {
        return monoBehaviour.StartCoroutine(NextFixedUpdateCoroutine(action));
    }
    private static IEnumerator NextFixedUpdateCoroutine(Action action)
    {
        yield return waitForFixedUpdate;
        action();
    }

    /// <summary>
    /// Run action on next end of frame
    /// </summary>
    /// <param name="action">The Action to be executed</param>
    /// <returns>Coroutine</returns>
    public static Coroutine RunOnNextEndOfFrame(this MonoBehaviour monoBehaviour, Action action)
    {
        return monoBehaviour.StartCoroutine(NextEndOfFrameCoroutine(action));
    }
    private static IEnumerator NextEndOfFrameCoroutine(Action action)
    {
        yield return waitForEndOfFrame;
        action();
    }

    /// <summary>
    /// Run action after a specific time 
    /// </summary>
    /// <param name="second">The number of seconds to wait before executing the action</param>
    /// <param name="action">The Action to be executed</param>
    /// <returns>Coroutine</returns>
    public static Coroutine RunAfterXSecond(this MonoBehaviour monoBehaviour, float second, Action action)
    {
        return monoBehaviour.StartCoroutine(AfterXSecondCoroutine(second, action));
    }
    private static IEnumerator AfterXSecondCoroutine(float second, Action action)
    {
        float startTime = Time.time;
        while (Time.time < startTime + second)
            yield return null;
        action();
    }

    /// <summary>
    /// Run action after a specific time 
    /// note: call StopCoroutine() to stop
    /// </summary>
    /// <param name="second">The number of seconds between each executing the action</param>
    /// <param name="action">The Action to be executed</param>
    /// <returns>Coroutine</returns>
    public static Coroutine RunEveryXSecond(this MonoBehaviour monoBehaviour, float second, Action action)
    {
        return monoBehaviour.StartCoroutine(EveryXSecondCoroutine(second, action));
    }
    private static IEnumerator EveryXSecondCoroutine(float second, Action action)
    {
        while (true)
        {
            float startTime = Time.time;
            while (Time.time < startTime + second)
                yield return null;
            action();
        }
    }

    /// <summary>
    /// Run action on specific condition function return true
    /// note: condition function will be called each frame
    /// </summary>
    /// <param name="condition">A function return bool to determine when to run the action</param>
    /// <param name="action">The Action to be executed</param>
    /// <returns>Coroutine</returns>
    public static Coroutine RunWhen(this MonoBehaviour monoBehaviour, Func<bool> condition, Action action)
    {
        return monoBehaviour.StartCoroutine(WhenCoroutine(condition, action));
    }
    private static IEnumerator WhenCoroutine(Func<bool> condition, Action action)
    {
        while (!condition())
            yield return null;
        action();
    }

    /// <summary>
    /// Run action every update until specific condition function return true
    /// note: condition function will be called each frame
    /// </summary>
    /// <param name="condition">A function return bool</param>
    /// <param name="action">The Action to be executed</param>
    /// <returns>Coroutine</returns>
    public static Coroutine RunUntil(this MonoBehaviour monoBehaviour, Func<bool> condition, Action action)
    {
        return monoBehaviour.StartCoroutine(UntilCoroutine(condition, action));
    }
    private static IEnumerator UntilCoroutine(Func<bool> condition, Action action)
    {
        while (!condition())
        {
            action();
            yield return null;
        }
    }

    /// <summary>
    /// Run action every update for X second
    /// note: condition function will be called each frame
    /// </summary>
    /// <param name="condition">A function return bool</param>
    /// <param name="action">The Action to be executed</param>
    /// <returns>Coroutine</returns>
    public static Coroutine RunFor(this MonoBehaviour monoBehaviour, float second, Action action)
    {
        return monoBehaviour.StartCoroutine(ForCoroutine(second, action));
    }
    private static IEnumerator ForCoroutine(float second, Action action)
    {
        float startTime = Time.time;
        while (Time.time < startTime + second)
        {
            action();
            yield return null;
        }
    }

    /// <summary>
    /// Start a CoroutineChain
    /// </summary>
    /// <returns>CoroutineChain</returns>
    public static CoroutineChain Chain(this MonoBehaviour monoBehaviour)
    {
        return new CoroutineChain(monoBehaviour);
    }

    public class CoroutineChain
    {
        private MonoBehaviour monoBehaviour;
        private Queue<IEnumerator> chainQueue = new Queue<IEnumerator>();
        public bool isPlaying = true;
        public CoroutineChain(MonoBehaviour monoBehaviour)
        {
            this.monoBehaviour = monoBehaviour;
        }
        private IEnumerator Coroutine()
        {
            yield return null;
            while (chainQueue.Count > 0)
            {
                if(isPlaying)
                {
                    yield return chainQueue.Dequeue();
                }
                else
                {
                    yield return null;
                }
            }
        }


        /// <summary>
        /// wait a specific time 
        /// </summary>
        /// <param name="second">The number of seconds to wait before executing the action</param>
        /// <returns>CoroutineChain</returns>
        public CoroutineChain Wait(float second)
        {
            chainQueue.Enqueue(WaitCoroutine(second));
            return this;
        }

        /// <summary>
        /// wait for next update
        /// </summary>
        /// <returns>CoroutineChain</returns>
        public CoroutineChain WaitForNextUpdate()
        {
            chainQueue.Enqueue(NextUpdateCoroutine());
            return this;
        }

        /// <summary>
        /// wait for next fixed update
        /// </summary>
        /// <returns>CoroutineChain</returns>
        public CoroutineChain WaitForNextFixedUpdate()
        {
            chainQueue.Enqueue(NextFixedUpdateCoroutine());
            return this;
        }

        /// <summary>
        /// wait for next End Of Frame
        /// </summary>
        /// <returns>CoroutineChain</returns>
        public CoroutineChain WaitForNextEndOfFrame()
        {
            chainQueue.Enqueue(NextEndOfFrameCoroutine());
            return this;
        }

        /// <summary>
        /// Run action on specific condition function return true
        /// </summary>
        /// <param name="condition">A function return bool to determine when to run the action</param>
        /// <returns>CoroutineChain</returns>
        public CoroutineChain WaitUntil(Func<bool> condition)
        {
            chainQueue.Enqueue(WaitUntilCoroutine(condition));
            return this;
        }

        /// <summary>
        /// Run a set of coroutines same time and wait for all finished
        /// </summary>
        /// <param name="actions">a set of coroutines to be run</param>
        /// <returns>CoroutineChain</returns>
        public CoroutineChain Parallel(params IEnumerator[] actions)
        {
            chainQueue.Enqueue(ParallelCoroutine(this.monoBehaviour, actions));
            return this;
        }

        /// <summary>
        /// Run a action
        /// </summary>
        /// <param name="action">a action to be run</param>
        /// <returns>CoroutineChain</returns>
        public CoroutineChain Run(Action action)
        {
            chainQueue.Enqueue(RunCoroutine(action));
            return this;
        }

        /// <summary>
        /// Run a action for X second
        /// </summary>
        /// <param name="action">a action to be run</param>
        /// <returns>CoroutineChain</returns>
        public CoroutineChain RunFor(float second, Action action)
        {
            chainQueue.Enqueue(RunForCoroutine(second, action));
            return this;
        }

        /// <summary>
        /// Run a coroutine
        /// </summary>
        /// <param name="coroutine">a coroutine to be run</param>
        /// <returns>CoroutineChain</returns>
        public CoroutineChain Run(IEnumerator coroutine)
        {
            chainQueue.Enqueue(coroutine);
            return this;
        }

        /// <summary>
        /// Tween a value in a specific time by Linear function
        /// </summary>
        /// <param name="action">a action to be run each update, a float will be passed tween between from and to</param>
        /// <param name="timeLength">the tween duration in term of seconds</param>
        /// <param name="from">the tween value start from (default: 0f)</param>
        /// <param name="to">the value tween to (default: 1f)</param>
        /// <returns>CoroutineChain</returns>
        public CoroutineChain TweenLinear(Action<float> action, float timeLength, float from = 0f, float to = 1f)
        {
            chainQueue.Enqueue(TweenLinearCoroutine(action, timeLength, from, to));
            return this;
        }

        /// <summary>
        /// Tween a value in a specific time
        /// </summary>
        /// <param name="action">a action to be run each update, a float will be passed tween between from and to</param>
        /// <param name="easing">a easing function</param>
        /// <param name="timeLength">the tween duration in term of seconds</param>
        /// <param name="from">the tween value start from (default: 0f)</param>
        /// <param name="to">the value tween to (default: 1f)</param>
        /// <returns>CoroutineChain</returns>
        public CoroutineChain Tween(Action<float> action, Func<float, float> easing, float timeLength, float from = 0f, float to = 1f)
        {
            chainQueue.Enqueue(TweenCoroutine(action, timeLength, from, to, easing));
            return this;
        }

        /// <summary>
        /// Start the CoroutineChain and return the Coroutine Object
        /// </summary>
        /// <returns>Coroutine</returns>
        public Coroutine Start()
        {
            return monoBehaviour.StartCoroutine(this.Coroutine());
        }

        private static IEnumerator RunCoroutine(Action action)
        {
            action();
            yield return null;
        }
        private static IEnumerator RunForCoroutine(float second, Action action)
        {
            float startTime = Time.time;
            while (Time.time < startTime + second)
            {
                action();
                yield return null;
            }
        }
        private static IEnumerator WaitCoroutine(float second)
        {
            float startTime = Time.time;
            while (Time.time < startTime + second)
                yield return null;
        }
        private static IEnumerator WaitUntilCoroutine(Func<bool> condition)
        {
            while (!condition())
            {
                yield return null;
            }
        }
        private static IEnumerator NextUpdateCoroutine()
        {
            yield return null;
        }
        private static IEnumerator NextFixedUpdateCoroutine()
        {
            yield return waitForFixedUpdate;
        }
        private static IEnumerator NextEndOfFrameCoroutine()
        {
            yield return waitForEndOfFrame;
        }
        private static IEnumerator ParallelCoroutine(MonoBehaviour monoBehaviour, params IEnumerator[] actions)
        {
            var count = actions.Count();
            var done = 0;
            foreach (var action in actions)
            {
                monoBehaviour.Chain().Run(action).Run(() => { done++; }).Start();
            }
            while (done < count)
                yield return null;
        }
        private static IEnumerator TweenLinearCoroutine(Action<float> action, float timeLength, float from, float to)
        {
            float startTime = Time.time;
            var displacement = to - from;

            while (Time.time < startTime + timeLength)
            {
                var t = (Time.time - startTime) / timeLength * displacement + from;
                action(t);
                yield return null;
            }
            action(to);
        }
        private static IEnumerator TweenCoroutine(Action<float> action, float timeLength, float from, float to, Func<float, float> easing)
        {
            float startTime = Time.time;
            var displacement = to - from;

            while (Time.time < startTime + timeLength)
            {
                var t = easing((Time.time - startTime) / timeLength) * displacement + from;
                action(t);
                yield return null;
            }
            action(to);
        }
    }
}
