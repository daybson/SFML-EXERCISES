#include "Player.h"
#include "Game.h"
#include <iostream>

Player::Player()
{
	currentFrameTime = 0.f;
	if (texture.loadFromFile(pathTexture))
	{
		tile = *new IntRect(0, 0, tileWidth, tileHeight);
		sprite = *new Sprite(texture, tile);
		sprite.setOrigin(0, 0);
		sprite.move(0, -tileHeight);
	}
}


Player::~Player()
{
}

void Player::Move(Vector2f delta)
{
	sprite.move(delta);
}

void Player::UpdateSprite(float millisUpdateTime)
{
	currentFrameTime += millisUpdateTime;

	if (currentFrameTime >= frameTime)
	{
		if (tile.left + tileWidth >= 96)
			tile.left = 0;
		else
			tile.left += tileWidth;
		currentFrameTime = 0.0f;
		sprite.setTextureRect(tile);
	}
}
