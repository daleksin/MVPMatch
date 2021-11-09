﻿using System;

namespace MVPMatch.Common.Exceptions
{
    public class BadRequestException : Exception
    {
        public BadRequestException(string message, Exception ex = default)
            : base(message, ex)
        {
        }
    }
}
