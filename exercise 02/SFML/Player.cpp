#include "Player.h"
#include "Game.h"
#include <iostream>

Player::Player()
{
	spriteSheet = new SpriteSheet(spriteSheetName);
}

Player::~Player()
{
	delete spriteSheet;
}

void Player::Move(Vector2f delta)
{
	spriteSheet->GetSprite().move(delta);
}

void Player::Update(float millisUpdateTime)
{
	spriteSheet->UpdateAnimation(millisUpdateTime);
}

Sprite& Player::GetCurrentSprite()
{
	return spriteSheet->GetSprite();
}
