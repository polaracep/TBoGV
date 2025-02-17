using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TBoGV
{
    public class Camera
    {
        public Matrix Transform { get; private set; }
        private Vector2 _position;
        private Viewport _viewport;
        private int RoomWidth, RoomHeight, Offset;

        public Camera(Viewport viewport, int roomWidth, int roomHeight)
        {
            _viewport = viewport;
            RoomWidth = roomWidth;
            RoomHeight = roomHeight;
            Offset = 100;
        }
		public void SetCenter(Vector2 position)
		{
			// Calculate the camera's top-left position based on the desired center.
			_position = position - new Vector2(_viewport.Width / 2f, _viewport.Height / 2f);

			// Update the transformation matrix.
			Transform = Matrix.CreateTranslation(new Vector3(-_position, 0));
		}

		public void Update(Vector2 targetPosition)
		{
			// The "deadzone" margin from the edge.
			float deadzoneMargin = Offset;

			// Only adjust camera if the target is too close to the left or right edge.
			if (targetPosition.X < _position.X + deadzoneMargin)
			{
				_position.X = targetPosition.X - deadzoneMargin;
			}
			else if (targetPosition.X > _position.X + _viewport.Width - deadzoneMargin)
			{
				_position.X = targetPosition.X - (_viewport.Width - deadzoneMargin);
			}

			// Only adjust camera if the target is too close to the top or bottom edge.
			if (targetPosition.Y < _position.Y + deadzoneMargin)
			{
				_position.Y = targetPosition.Y - deadzoneMargin;
			}
			else if (targetPosition.Y > _position.Y + _viewport.Height - deadzoneMargin)
			{
				_position.Y = targetPosition.Y - (_viewport.Height - deadzoneMargin);
			}

			// Clamp the camera position so it stays within the room boundaries.
			//_position.X = MathHelper.Clamp(_position.X, -Offset, RoomWidth + Offset - _viewport.Width);
			//_position.Y = MathHelper.Clamp(_position.Y, -Offset, RoomHeight + Offset - _viewport.Height);

			// Update the transform matrix.
			Transform = Matrix.CreateTranslation(new Vector3(-_position, 0));
		}
	}
}
