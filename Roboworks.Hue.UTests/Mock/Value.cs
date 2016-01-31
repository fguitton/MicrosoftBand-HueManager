using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roboworks.Hue.UTests.Tests.Mock
{
    public static class Value
    {
        public static IMockWhenPart When(params object[] args)
        {
            return new MockWhenPart(args);
        }

        public static T Any<T>()
        {
            object value;

            if (typeof(T) == typeof(string))
            {
                value = Guid.NewGuid().ToString();
            }
            else
            {
                throw new NotSupportedException($"\"{nameof(T)}\" type is not supported.");
            }

            return (T)value;
        }
    }

    public interface IMockWhenPart
    {
        IMockReturnPart Equals(params object[] args);
    }

    public interface IMockReturnPart
    {
        T Return<T>(T result);

        Task<T> ReturnAsync<T>(T result);
    }

    public class MockWhenPart : IMockWhenPart
    {
        private object[] _args;

        public MockWhenPart(object[] args)
        {
            this._args = args;
        }

        public IMockReturnPart Equals(params object[] args)
        {
            if (this._args.Length != args.Length)
            {
                // TODO: write a message
                throw new InvalidOperationException();
            }

            var areEqual = true;

            for(int i = 0; i < this._args.Length; ++i)
            {
                if (this._args[i] != null && args[i] != null)
                {
                    this.TypeEqualityCheck(this._args[i], args[i]);

                    if (!this._args[i].Equals(args[i]))
                    {
                        areEqual = false;
                    }
                }
                else if (this._args[i] == null && args[i] == null)
                {
                    // NULL equals NULL
                }
                else
                {
                    areEqual = false;
                }

                if (!areEqual)
                {
                    break;
                }
            }

            return areEqual ? MockReturnPart.PassedArgument : MockReturnPart.Default;
        }

        private void TypeEqualityCheck(object x, object y)
        {
            var xType = x.GetType();
            var yType = y.GetType();

            // TODO: improve - case with inherited classes
            if (xType != yType)
            {
                throw 
                    new InvalidOperationException(
                        $"Cannot compare values of two different types - \"{xType}\" and \"{yType}\""
                    );
            }
        }
    }

    public class MockReturnPart : IMockReturnPart
    {
        public static IMockReturnPart Default = new MockReturnPart(true);

        public static IMockReturnPart PassedArgument = new MockReturnPart(false);

        private readonly bool _isDefault;

        private MockReturnPart(bool isDefault)
        {
            this._isDefault = isDefault;
        }

        public T Return<T>(T result)
        {
            return this._isDefault ? default(T) : result;
        }

        public Task<T> ReturnAsync<T>(T result)
        {
            return Task.FromResult(this.Return(result));
        }
    }
}
