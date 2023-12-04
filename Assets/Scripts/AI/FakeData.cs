using System;
using System.Collections.Generic;
using Gameplay.player;
using Random = UnityEngine.Random;

namespace AI
{
    public static class FakeData
    {
        private static readonly List<Player> bots = new()
        {
            new Player()
            {
            },
            new Player()
            {
            },
            new Player()
            {
            },
            new Player()
            {
            },
            new Player()
            {
            },
            new Player()
            {
            },
        };

        public static Player RandomPlayer()
        {
            var template = bots[Random.Range(0, bots.Count)];
            
            return new Player()
            {
            };
        }
    }
}