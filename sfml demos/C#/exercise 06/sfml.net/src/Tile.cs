using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sfml.net.src
{
    class Tile : Transformable, Drawable
    {
        #region Fields

        private VertexArray vertices;
        Texture tileset;

        #endregion


        #region Public

        public void Draw(RenderTarget target, RenderStates states)
        {
            states.Transform = Transform;
            states.Texture = tileset;
            target.Draw(vertices, states);
        }

        public bool Load(string tilesetPath, int width, int height)
        {
            if ((tileset = new Texture(tilesetPath)) == null)
                return false;

            vertices = new VertexArray(PrimitiveType.Quads);
            vertices.Resize(4);

            vertices.Append(new Vertex(new Vector2f(0, 0), new Vector2f(0, 0)));
            vertices.Append(new Vertex(new Vector2f(width, 0), new Vector2f(width, 0)));
            vertices.Append(new Vertex(new Vector2f(width, height), new Vector2f(width, height)));
            vertices.Append(new Vertex(new Vector2f(0, height), new Vector2f(0, height)));

            return true;
        }

        public bool Load(string tilesetPath, Vector2u tileSize, int[] tiles, uint width, uint height)
        {
            if ((tileset = new Texture(tilesetPath)) == null)
                return false;

            vertices = new VertexArray(PrimitiveType.Quads);
            vertices.Resize(width * height * 4);

            for (uint i = 0; i < width; i++)
            {
                for (uint j = 0; j < height; j++)
                {
                    int tileNumber = tiles[i + j * width];

                    int tu = tileNumber % (int)(tileset.Size.X / tileSize.X);
                    int tv = tileNumber / (int)(tileset.Size.X / tileSize.X);

                    var quadIndex = (i + j * width) * 4;

                    vertices[quadIndex] = new Vertex(new Vector2f(i * tileSize.X, j * tileSize.Y), new Vector2f(tu * tileSize.X, tv * tileSize.Y));
                    vertices[quadIndex + 1] = new Vertex(new Vector2f((i + 1) * tileSize.X, j * tileSize.Y), new Vector2f((tu + 1) * tileSize.X, tv * tileSize.Y));
                    vertices[quadIndex + 2] = new Vertex(new Vector2f((i + 1) * tileSize.X, (j + 1) * tileSize.Y), new Vector2f((tu + 1) * tileSize.X, (tv + 1) * tileSize.Y));
                    vertices[quadIndex + 3] = new Vertex(new Vector2f(i * tileSize.X, (j + 1) * tileSize.Y), new Vector2f(tu * tileSize.X, (tv + 1) * tileSize.Y));
                }
            }

            return true;
        }

        #endregion
    }
}
