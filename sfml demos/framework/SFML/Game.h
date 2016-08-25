#pragma once

#include <SFML\Graphics.hpp>
#include "Player.h"
using namespace sf;
using namespace std;


class Game
{
public:
	Game(String title, Uint32 style) ;
	~Game();
	void run();
private:
	static const int screenWidth = 800;
	static const int screenHeight = 600;
	RenderWindow window;

	void processEvents();
	void update();
	void render();

	Player* player;
	Clock* clock;
protected:
};

