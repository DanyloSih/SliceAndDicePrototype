using System;

namespace SliceAndDicePrototype.DiceSystem
{
    public class NotInitializedException : Exception
    {
        public NotInitializedException(string typeName) 
            : base($"You must initialize \"{typeName}\" object before using it.")
        {
        }
    }
}
