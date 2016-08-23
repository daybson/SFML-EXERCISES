#pragma once
#include <SFML\Graphics.hpp>
#include "Game.h"
using namespace sf;

int main()
{
	Game game("Teste SFML", Style::Default);
	game.run();
};

