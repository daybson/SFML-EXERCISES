using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SFMLFramework.src
{
    public abstract class Observable<T> : IObservable<T>
    {
        private List<IObserver<T>> observers;

        public Observable()
        {
            this.observers = new List<IObserver<T>>();
        }

        protected void Notify(T obj)
        {
            foreach (var o in observers)
                o.OnNext(obj);
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            if (!this.observers.Contains(observer))
                this.observers.Add(observer);
            return new Unsubscriber(observers, observer);
        }

        private class Unsubscriber : IDisposable
        {
            private List<IObserver<T>> observers;
            private IObserver<T> observer;

            public Unsubscriber(List<IObserver<T>> observers, IObserver<T> observer)
            {
                this.observers = observers;
                this.observer = observer;
            }

            public void Dispose()
            {
                if (this.observer != null && this.observers.Contains(this.observer))
                    this.observers.Remove(this.observer);
            }
        }
    }
}
