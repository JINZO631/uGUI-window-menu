using UniRx;
using System;

namespace IgniteModule
{
    // https://qiita.com/toRisouP/items/1bd2f3b07bf868953178
    public class Single : IObservable<Unit>, IDisposable
    {
        private readonly AsyncSubject<Unit> asyncSubject = new AsyncSubject<Unit>();
        private readonly object lockObject = new object();

        public void Done()
        {
            lock (lockObject)
            {
                if(asyncSubject.IsCompleted)
                {
                    return;
                }

                asyncSubject.OnNext(Unit.Default);
                asyncSubject.OnCompleted();
            }
        }

        public IDisposable Subscribe(IObserver<Unit> observer)
        {
            lock (lockObject)
            {
                return asyncSubject.Subscribe(observer);
            }
        }

        public void Dispose()
        {
            lock (lockObject)
            {
                asyncSubject.Dispose();
            }
        }
    }
}
