#pragma once

#include <SFML\Graphics.hpp>
#include <iostream>

using namespace std;
using namespace sf;

class UIButton
{
private:
	Sprite normal;
	Sprite clicked;
	Sprite currentSprite;
	bool currentState;
	Texture normalTexture;
	Texture clickedTexture;

public:
	UIButton(string normalText, string clickedText, Vector2f position);
	~UIButton();
	void checkClick(Vector2f mousePos);
	void setState(bool);
	bool getState() { return currentState; };
	Sprite getDrawButton() { return currentSprite; };

};

