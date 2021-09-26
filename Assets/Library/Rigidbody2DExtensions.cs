using UnityEngine;

namespace Assets.Library
{
    public static class Rigidbody2DExtensions
    {
        public static void ChangeDirection(this Rigidbody2D rigidbody2d, float speed, Vector2 direction)
        {
            rigidbody2d.velocity = Vector2.zero;
            Vector2 movement = direction * speed;

            rigidbody2d.AddForce(movement, ForceMode2D.Force);
        }

    }
}
