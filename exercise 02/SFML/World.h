#pragma once

#include <SFML\Graphics.hpp>
using namespace sf;

class World
{
public:
	World();
	~World();

	RectangleShape sky;
	RectangleShape lineStreetLeft;
	RectangleShape lineStreetRight;
	RectangleShape street;	

	ConvexShape forestLeft;
	ConvexShape forestRight;
};

