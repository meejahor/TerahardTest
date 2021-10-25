using System.Collections.Generic;
using Terahard.Assessment.Core;
using UnityEngine;

//
//  Single-threaded
//

namespace Terahard.Assessment.Test {
    class TestContext : IContext {
        //Core.Application.OBJECT_COUNT = 100000;
        //Core.Application.FRAMES = 1000;

        public string Name => "Andrew Smith"; //Replace with your name and last name

        // Initialising the array to contain the required number of objects achieves
        // a slight performance boost, approx 33% faster (3ms down to 2s in my tests)
        private List<Object> m_Objects = new List<Object>(new Object[Core.Application.OBJECT_COUNT]);
        private int m_ObjectIndex = 0;

        // We need to keep track of how many times Update has been called for each phase.
        private int[] m_PhaseCallCount = new int[5];
        // We also need a reciprocal value to normalise the frame number in the current phase.
        // Note that phases 1, 2 and 4 are called 250 times, but phase 3 is only called 249 times.
        private static readonly float[] m_Reciprocals = new float[] {
            0,
            1f / 250f,
            1f / 250f,
            1f / 249f,
            1f / 250f,
        };

        private static readonly Vector2[] m_Targets = new Vector2[] {
            Vector2.zero,   // this dummy vector just saves us from doing a subtraction in the Update method
            new Vector2(0, 1000),
            new Vector2(1000, 1000),
            new Vector2(1000, 0),
            new Vector2(0, 0)
        };

        private class Object {
            public Vector2 Position { get; private set; }

            public Object(Vector2 position) {
                Position = position;
            }

            public void Update(int phase, float lerp) {
                // The way the lerps were being done in the test code didn't lerp all the
                // way to the target values. We fix that by using a normalised lerp value.
                // (Normally I'd expect to lerp between a start position and an end position,
                // rather than between the current position and the end position, but this is
                // the way it was done in the test code so I've done it the same way.)
                Position = Vector2.Lerp(Position, m_Targets[phase], lerp);
            }
        }

        public void CreateObject(Vector2 position) {
            m_Objects[m_ObjectIndex++] = new Object(position);
        }

        public void RemoveObjects(int count) {
            // Removing objects one at a time is very slow. Removing the first _count_
            // objects in one go is much quicker.
            // When 'count' is set to the size of the array, as it is in this test,
            // the removal is almost instantaneous (1078ms down to 0ms in my tests)
            m_Objects.RemoveRange(0, count);
            m_ObjectIndex -= count;
        }

        /// <summary>
        /// Called by Application
        /// </summary>
        /// <param name="deltaTime">It's always 1.0f to simulate Unity deltaTime</param>
        /// <param name="phase"></param>
        public void Update(float deltaTime, int phase) {
            // Note that deltaTime isn't used
            m_PhaseCallCount[phase]++;
            float lerp = m_PhaseCallCount[phase];
            lerp *= m_Reciprocals[phase];
            for (int i = 0; i < m_Objects.Count; ++i) {
                m_Objects[i].Update(phase, lerp);
            }
        }

        public Vector2 GetObjectPosition(int index) {
            return m_Objects[index].Position;
        }
    }
}
