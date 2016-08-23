#include "SpriteSheet.h"
#include <iostream>
#include <fstream>
#include <sstream>
#include "Game.h"
using namespace std;

SpriteSheet::SpriteSheet()
{
}

SpriteSheet::SpriteSheet(string pathTexture)
{
	this->pathTexture = pathTexture;

	//read a txt file 'metadata' with informations about the sprite sheet
	stringstream ss;
	ss << this->pathTexture.substr(0, this->pathTexture.length() - 3) << "txt";
	ifstream file(ss.str());
	while (file >> this->tileWidth >> this->tileHeight >> this->rows >> this->columns >> this->frameCount >> this->animationTime >> this->frameTime);

	//load texture or throw expcetion
	texture.loadFromFile(this->pathTexture);
	tile = IntRect(0, 0, this->tileWidth, this->tileHeight);
	sprite.move(0, 0);
	sprite.setTexture(texture);
	sprite.setTextureRect(tile);
	sprite.setPosition(Game::screenWidth / 2, Game::screenHeight / 2);

	currentFrame = 0;
	currentFrameTime = 0;
}

SpriteSheet::~SpriteSheet()
{
}

void SpriteSheet::UpdateAnimation(float millisUpdateTime)
{
	currentFrameTime += millisUpdateTime;

	if (currentFrameTime >= frameTime)
	{
		if (currentFrame + 1 >= frameCount)
		{
			currentFrame = 0;
			tile.left = 0;
		}
		else
		{
			currentFrame++;
			tile.left += tileWidth;
		}

		currentFrameTime = 0.0f;
		sprite.setTextureRect(tile);
	}
}

Sprite& SpriteSheet::GetSprite()
{
	return sprite;
}