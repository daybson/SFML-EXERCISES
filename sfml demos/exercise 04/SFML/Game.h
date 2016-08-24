#pragma once

#include <SFML\Graphics.hpp>
#include "UIButton.h"

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
private:
	void ProcessEvents();
	void Update();
	void Render();

	RenderWindow mWindow;
	UIButton* buttonExit;
	Texture texture;
	Sprite sprite;

protected:
};

