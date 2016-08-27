#pragma once

#include <SFML\Graphics.hpp>
#include "ActionTarget.h"
#include <string>
using namespace sf;

class Player : public ActionTarget<int>
{
public:
	Player(const Player&) = delete;
	Player& operator=(const Player&) = delete;
	Player();
	~Player();

	void processEvents();
	void update(Time deltaTime);

	enum PlayerInputs { Up, Down, Left, Right };
	static void setDefaultInputs();

private:
	RectangleShape _shape;
	Vector2f _velocity;
	bool _isMoving;
	static ActionMap<int> _playerInputs;
};

