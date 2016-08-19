#pragma once

#include <SFML\Graphics.hpp>
using namespace sf;

class GameTime
{
public:
	GameTime();
	~GameTime();

	static float getDeltaTime();

private:
	static Clock clock;
	
};

