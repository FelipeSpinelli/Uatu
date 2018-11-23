using System;
using System.Collections.Generic;
using System.Linq;

namespace Uatu.Core.Seedwork
{
    public class Result
    {
        public bool IsSuccess => !_errors.Any();

        private List<string> _errors;
        public IReadOnlyList<string> Errors => _errors;

        public Result()
        {
            _errors = new List<string>();
        }
        
        public void AddError(string error)
        {
            _errors.Add(error);
        }
    }

    public class Result<T> : Result
    {
        private T _content;

        public T Content
        {
            get
            {
                if (!IsSuccess)
                    throw new InvalidOperationException();

                return _content;
            }
        }

        public void SetContent(T value)
        {
            if (!IsSuccess)
                throw new InvalidOperationException();

            _content = value;
        }
    }
}
