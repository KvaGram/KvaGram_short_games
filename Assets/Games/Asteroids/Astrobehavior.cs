using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KvaGames.Asteroids
{
    public abstract class AstroBehaviour : MonoBehaviour
    {
        protected Game controller;
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

        protected abstract void OutofboundsY(bool upper);
        // Update is called once per frame
        protected abstract void HandleWarped(Vector3 warp);
        protected void Update()
        {
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
    }
}