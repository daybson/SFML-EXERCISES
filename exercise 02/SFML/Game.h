#pragma once

#include <SFML\Graphics.hpp>
#include "World.h"
#include "Player.h"

using namespace sf;
using namespace std;

class Game
{
public:
	Game();
	Game(String title, Uint32 style);
	~Game();

	void run();
	static const int screenWidth = 800;
	static const int screenHeight = 600;
	
	World world;
	Player* player;

private:
	void handlePlayerInput(Keyboard::Key key, bool isPressed);
	void ProcessEvents();
	void Update();
	void Render();
	
	Clock clock;
	RenderWindow mWindow;

protected:
	bool mIsMovingUp, mIsMovingDown, mIsMovingLeft, mIsMovingRight;
};

