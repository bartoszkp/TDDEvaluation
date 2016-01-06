﻿using System;

namespace Domain
{
    public class Datum<T>
    {
        public DateTime Timestamp { get; set; }

        public T Value { get; set; }
    }
}
