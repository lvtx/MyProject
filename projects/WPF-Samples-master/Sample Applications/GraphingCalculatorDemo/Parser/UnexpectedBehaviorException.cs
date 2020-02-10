// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace GraphingCalculatorDemo.Parser
{
    [Serializable]
    public class UnexpectedBehaviorException : FunctionParserException
    {
        public UnexpectedBehaviorException(string msg) : base(msg)
        {
        }
    }
}