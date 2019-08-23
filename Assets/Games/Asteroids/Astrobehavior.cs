using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KvaGames.Asteroids
{
    public abstract class AstroBehaviour : MonoBehaviour
    {
        protected Game controller;
        protected Rigidbody rb;
        public Rigidbody Rb
        { get { if (rb) return Rb; rb = GetComponent<Rigidbody>(); return rb; } }

        public float escapeTime = 60.0f;
        private float escapeTimer = 0.0f;
        public bool escaped = false;
        private Vector3 escapeVel = Vector3.zero;

        Rect PlayArea { get => Controller.Playarea; }
        public Game Controller {get{
                if (controller == null)
                {
                    Transform p = transform;
                    while (controller == null && p != null)
                    {
                        controller = p.GetComponent<Game>();
                        p = p.parent;
                    }
                }
                return controller;
            }}

        // Start is called before the first frame update
        void Start()
        {

        }

        //For  player, OutOfBounds gives a warning, then later kills the player >:D
        //For other objects, expect Escape to be called.
        protected abstract void OutofboundsY(bool upper);
        // Update is called once per frame
        protected abstract void HandleEscaped();
        protected abstract void HandleWarped(Vector3 warp);
        protected void Update()
        {
            if (escaped)
            {
                EscapeUpdate();
                return;
            }

            float lowerX = PlayArea.x;
            float upperX = PlayArea.width + PlayArea.x;
            Vector3 warp = Vector3.zero;
            if (transform.position.x < lowerX)
            {
                warp = Vector3.right * PlayArea.width;
            }
            else if(transform.position.x > upperX)
            {
                warp = Vector3.left * PlayArea.width;
            }
            transform.position += warp;
            if (warp.sqrMagnitude > 0)
                HandleWarped(warp);

            float lowerY = PlayArea.y;
            float upperY = PlayArea.height + PlayArea.y;
            if (transform.position.y < lowerY)
                OutofboundsY(false);
            else if (transform.position.y > upperY)
                OutofboundsY(true);
        }
        public void Escape()
        {
            escaped = true;
            escapeVel = rb.velocity + Vector3.forward * 5;
            rb.isKinematic = true;
        }
        public void EscapeUpdate()
        {
            transform.position += escapeVel * Time.deltaTime;
            escapeTimer += Time.deltaTime;
            if(escapeTimer > escapeTime)
            {
                HandleEscaped();
            }
        }
    }
}