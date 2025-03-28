using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattleMovesSystem
{
    public class BattleMoves
    {
        // Fields
        public string name;
        public int range;
        public int accuracy;
        public int damage;

        // constructor
        public BattleMoves(string name, int range, int accuracy, int damage)
        {
            this.name = name;
            this.range = range;
            this.accuracy = accuracy;
            this.damage = damage;
        }
    }
}
