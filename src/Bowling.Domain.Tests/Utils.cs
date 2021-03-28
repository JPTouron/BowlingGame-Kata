using System;
using System.Collections.Generic;
using System.Text;

namespace Bowling.Domain.Tests
{
public    static class Utils
    {
        public static int GetRandomNumberBetween(int min, int max)
        {
            var r = new Random();
            var expectedNumber = r.Next(min, max + 1);
            return expectedNumber;
        }
    }
}
