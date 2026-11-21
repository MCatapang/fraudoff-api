using Business.Enums;

namespace Models.Wrappers
{
    public class MethodResult<T>
    {
        /// <summary>A message regarding the result of the relevant method.</summary>
        public string Message { get; set; }

        /// <summary>The object that a method returned - if any.</summary>
        /// <remarks>
        ///     Not applicable for methods with a return type of <see cref="void"/>.
        ///     Will be assigned a default value of <see cref="T"/> if the method is unsuccesful.
        /// </remarks>
        public T? Result { get; set; }

        /// <summary>
        ///     The primary indicator of how a method's execution went 
        ///     relative to its specific purpose.
        /// </summary>
        public ResultType ResultType { get; set; }

        public MethodResult()
        {
            // TODO: Create constant
            Message = "Something went wrong.";
            Result = default;
            ResultType = ResultType.Error;
        }
    }
}
