#pragma once

#include <SFML\Graphics.hpp>
#include "SpriteSheet.h"
#include <string>
using namespace sf;

class Player
{
public:
	Player();
	~Player();

	void Move(Vector2f delta);
	void Update(float millisUpdateTime);
	Sprite& GetCurrentSprite();

private:

	SpriteSheet* spriteSheet;
	string spriteSheetName = "dragon.png";
};

