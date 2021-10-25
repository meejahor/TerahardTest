using System.Collections.Generic;
using Terahard.Assessment.Core;
using UnityEngine;

//
// Original
//

namespace Terahard.Assessment.Test {
    class TestContext : IContext {
        //Core.Application.OBJECT_COUNT = 100000;
        //Core.Application.FRAMES = 1000;

        public string Name => "Andrew Smith"; //Replace with your name and last name

        private List<Object> m_Objects = new List<Object>();

        private static readonly Vector2 A = new Vector2(0, 1000);
        private static readonly Vector2 B = new Vector2(1000, 1000);
        private static readonly Vector2 C = new Vector2(1000, 0);
        private static readonly Vector2 D = new Vector2(0, 0);

        private class Object {
            public Vector2 Position { get; private set; }

            public Object(Vector2 position) {
                Position = position;
            }

            public void Update(float deltaTime, int phase) {
                if (phase == 1) {
                    Position = new Vector2
                    (
                        Mathf.Lerp(Position.x, A.x, deltaTime / Core.Application.FRAMES),
                        Mathf.Lerp(Position.y, A.y, deltaTime / Core.Application.FRAMES)
                    );
                }
                if (phase == 2) {
                    Position = new Vector2
                    (
                        Mathf.Lerp(Position.x, B.x, deltaTime / Core.Application.FRAMES),
                        Mathf.Lerp(Position.y, B.y, deltaTime / Core.Application.FRAMES)
                    );
                }
                if (phase == 3) {
                    Position = new Vector2
                    (
                        Mathf.Lerp(Position.x, C.x, deltaTime / Core.Application.FRAMES),
                        Mathf.Lerp(Position.y, C.y, deltaTime / Core.Application.FRAMES)
                    );
                }
                if (phase == 4) {
                    Position = new Vector2
                    (
                        Mathf.Lerp(Position.x, D.x, deltaTime / Core.Application.FRAMES),
                        Mathf.Lerp(Position.y, D.y, deltaTime / Core.Application.FRAMES)
                    );
                }
            }
        }

        public void CreateObject(Vector2 position) {
            m_Objects.Add(new Object(position));
        }

        public void RemoveObjects(int count) {
            for (int i = 0; i < count; ++i) {
                m_Objects.RemoveAt(0);
            }
        }


        /// <summary>
        /// Called by Application
        /// </summary>
        /// <param name="deltaTime">It's always 1.0f to simulate Unity deltaTime</param>
        /// <param name="phase"></param>
        public void Update(float deltaTime, int phase) {
            for (int i = 0; i < m_Objects.Count; ++i) {
                m_Objects[i].Update(deltaTime, phase);
            }
        }

        public Vector2 GetObjectPosition(int index) {
            return m_Objects[index].Position;
        }
    }
}