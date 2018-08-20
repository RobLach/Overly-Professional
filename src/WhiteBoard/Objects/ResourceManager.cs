using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;


namespace WhiteBoard.Objects
{

    public class Pool<T> where T : new()
    {
        private Stack<T> _stack;

        public Pool()
        {
            _stack = new Stack<T>();
        }

        // Creates a Pool of objects for later use.
        public Pool(int size)
        {
            _stack = new Stack<T>(size);
            for (int i = 0; i < size; i++)
            {
                _stack.Push(new T());
            }
        }

        // Fetchs an object for use.
        public T Fetch()
        {
            if (_stack.Count > 0)
            {
                return _stack.Pop();
            }
            return new T();
        }

        // Returns an object to the Pool.
        public void Return(T item)
        {
            _stack.Push(item);
        }
    }


}