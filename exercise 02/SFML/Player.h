#pragma once

#include <SFML\Graphics.hpp>
using namespace sf;

class Player
{
public:
	Player();
	~Player();

	void Move(Vector2f delta);
	void UpdateSprite(float millisUpdateTime);

	Texture texture;
	Sprite sprite;
	IntRect tile;
	int tileWidth = 32;
	int tileHeight = 32;
	float frameTime = 0.016f;
	float currentFrameTime;
	String pathTexture = "picture.png";
};

