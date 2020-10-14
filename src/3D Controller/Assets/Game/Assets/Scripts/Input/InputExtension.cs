using UnityEngine;
using System;
using System.Threading.Tasks;

namespace UnityEngine
{
    public class BetterInput : Input
    {
        public BetterInput() : base() { }

        public static bool GetButtonTime(string button, out float time) 
        {
            if (!GetButtonDown(button)) 
            {
                time  = 0;
                return false;
            }

            

            time = 1;
            return true;
        }
    }
}