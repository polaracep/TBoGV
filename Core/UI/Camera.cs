using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TBoGV
{
    public class Camera
    {
        public Matrix Transform { get; private set; }
        private Vector2 _position;
        private int Offset = 200;

        public void SetCenter(Viewport viewport, Vector2 position)
        {
            // Calculate the camera's top-left position based on the desired center.
            _position = position - new Vector2(viewport.Width, viewport.Height) / 2f;

            // Update the transformation matrix.
            Transform = Matrix.CreateTranslation(new Vector3(-_position, 0));
        }

        public void Update(Viewport viewport, Vector2 targetPosition)
        {

            // Only adjust camera if the target is too close to the left or right edge.
            if (targetPosition.X < _position.X + Offset)
            {
                _position.X = targetPosition.X - Offset;
            }
            else if (targetPosition.X > _position.X + viewport.Width - Offset)
            {
                _position.X = targetPosition.X - (viewport.Width - Offset);
            }

            // Only adjust camera if the target is too close to the top or bottom edge.
            if (targetPosition.Y < _position.Y + Offset)
            {
                _position.Y = targetPosition.Y - Offset;
            }
            else if (targetPosition.Y > _position.Y + viewport.Height - Offset)
            {
                _position.Y = targetPosition.Y - (viewport.Height - Offset);
            }

            // Clamp the camera position so it stays within the room boundaries.
            //_position.X = MathHelper.Clamp(_position.X, -Offset, RoomWidth + Offset - _viewport.Width);
            //_position.Y = MathHelper.Clamp(_position.Y, -Offset, RoomHeight + Offset - _viewport.Height);

            // Update the transform matrix.
            Transform = Matrix.CreateTranslation(new Vector3(-_position, 0));
        }
    }
}
