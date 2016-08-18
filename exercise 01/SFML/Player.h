#pragma once

#include <SFML\Graphics.hpp>
using namespace sf;

class Player
{
public:
	Player();
	~Player();

	void Move(Vector2f delta);

	RectangleShape body;
	RectangleShape legLeft;
	RectangleShape legRight;
	RectangleShape armLeft;
	RectangleShape armRight;
	CircleShape head;
	CircleShape hat;
	CircleShape nose;

};

