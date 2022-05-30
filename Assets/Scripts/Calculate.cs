using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;
using System.Collections.Generic;

namespace Calculator
{
    public static class Calculate
    {
        public static double DoMath(string key,double x=0, double y=0)
        {
            switch(key)
            {
                case "+":
                    return x + y;
                    break;

                case "-":
                    return x - y;
                    break;
                        
                case "×":
                    return x * y;
                    break;
                        
                case "÷":
                    return x / y;
                    break;

                case "%":
                    return x*0.01;
                    break;

                case "√":
                    return Math.Sqrt(x);
                    break;

                default:
                    return 0;
            }            
        }

    }
}
