#include "Tile.h"


Tile::Tile()
{
}


Tile::~Tile()
{
}

bool Tile::Load(const string & tilesetPath, int width, int height)
{
	if (!tileset.loadFromFile(tilesetPath))
		return false;

	vertices.setPrimitiveType(Quads);
	vertices.resize(4);

	vertices[0].position = Vector2f(0, 0);
	vertices[1].position = Vector2f(width, 0);
	vertices[2].position = Vector2f(width, height);
	vertices[3].position = Vector2f(0, height);

	vertices[0].texCoords = Vector2f(0, 0);
	vertices[1].texCoords = Vector2f(width, 0);
	vertices[2].texCoords = Vector2f(width, height);
	vertices[3].texCoords = Vector2f(0, height);

	return true;
}

bool Tile::Load(const string & tilesetPath, Vector2u tileSize, const int * tiles, unsigned int width, unsigned int height)
{
	if (!tileset.loadFromFile(tilesetPath))
		return false;

	vertices.setPrimitiveType(Quads);
	vertices.resize(width * height * 4);

	for (unsigned int i = 0; i < width; ++i)
	{
		for (unsigned int j = 0; j < height; ++j)
		{
			int tileNumber = tiles[i + j * width];

			int tu = (int)tileNumber % (tileset.getSize().x / tileSize.x);
			int tv = (int)tileNumber / (tileset.getSize().x / tileSize.x);

			Vertex* quad = &vertices[(i + j * width) * 4];

			quad[0].position = Vector2f(i * tileSize.x, j * tileSize.y);
			quad[1].position = Vector2f((i + 1) * tileSize.x, j * tileSize.y);
			quad[2].position = Vector2f((i + 1) * tileSize.x, (j + 1) * tileSize.y);
			quad[3].position = Vector2f(i * tileSize.x, (j + 1) * tileSize.y);

			quad[0].texCoords = Vector2f(tu * tileSize.x, tv * tileSize.y);
			quad[1].texCoords = Vector2f((tu + 1) * tileSize.x, tv * tileSize.y);
			quad[2].texCoords = Vector2f((tu + 1) * tileSize.x, (tv + 1) * tileSize.y);
			quad[3].texCoords = Vector2f(tu * tileSize.x, (tv + 1) * tileSize.y);
		}
	}

	return true;
}
