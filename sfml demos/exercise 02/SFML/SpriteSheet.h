#pragma once

#include <SFML\Graphics.hpp>
#include <vector>
#include <string>
using namespace sf;
using namespace std;


class SpriteSheet
{
public:
	SpriteSheet();
	SpriteSheet(string pathTexture);
	~SpriteSheet();
	void UpdateAnimation(float millisUpdateTime);
	void SetDirection(Vector2f direction);
	Sprite& GetSprite();
	Sprite sprite;
	
private:
	Texture texture;
	IntRect tile;
	string pathTexture;
	
	int tileHeight;
	int tileWidth;
	int rows;
	int columns;
	int frameCount;
	float animationTime;
	float frameTime;
	float currentFrameTime;
	float currentFrame;
};

