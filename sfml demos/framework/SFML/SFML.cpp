#pragma once
#include <SFML\Graphics.hpp>
#include "Game.h"
using namespace sf;

int main()
{
	Game game("SFML Input", Style::Default);
	game.run();
};

