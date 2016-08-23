#pragma once

#include <SFML\Graphics.hpp>
#include <string>

using namespace std;
using namespace sf;

class Tile : public Drawable, public Transformable
{
private:
	VertexArray vertices;
	Texture tileset;

	virtual void draw(RenderTarget& target, RenderStates states) const
	{
		states.transform *= getTransform();
		states.texture = &tileset;
		target.draw(vertices, states);
	}

public:
	Tile();
	~Tile();

	bool Load(const string& tilesetPath, int width, int height);
	bool Load(const string& tilesetPath, Vector2u tileSize, const int* tiles, unsigned int width, unsigned int height);
};

