#pragma once

#include <SFML\Graphics.hpp>
#include "UIButton.h"
#include "Gamepad.h"

using namespace sf;
using namespace std;

class Game
{
public:
	Game();
	Game(String title, Uint32 style);
	~Game();
	Vector2f speed;
	void run();
	static const int screenWidth = 800;
	static const int screenHeight = 600;
private:
	void processEvents();
	void Update();
	void Render();

	RenderWindow window;
	UIButton* buttonExit;
	Gamepad * gamepad;
	Texture texture;
	Sprite sprite;
	RectangleShape square;
protected:
};

