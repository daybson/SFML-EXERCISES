#pragma once

#include <SFML\Graphics.hpp>
#include <SFML\Window.hpp>
#include "GameEntity.h"
#include <string>

using namespace sf;

class Player : public GameEntity
{
public:
	Player();
	~Player();
	Player(string name);
	void update(float deltaTime)override;
	void processEvents(const Event& event)override;

	RectangleShape getRender();

	enum KeyboardInput 
	{
		UP = Keyboard::W ,
		DOWN = Keyboard::S ,
		LEFT = Keyboard::A ,
		RIGHT = Keyboard::D ,
		JUMP = Keyboard::Space,
		RUN = Keyboard::RShift ,
		ATTACK = Keyboard::Return,
		INTERACT = Keyboard::LControl
	};	

	void move(Vector2f direction);

protected:
	bool isMovng;
	RectangleShape shape;

	Vector2f walkSpeed = Vector2f(4000, 0);
	Vector2f runSpeed = Vector2f(7000, 0);
	Vector2f currentSpeed;
	Vector2f currentDirection;
	
	KeyboardInput* inputs;

	void setMovementDirection(Vector2f direction);

	const Vector2f VECTOR2_ZERO = Vector2f(0, 0);
	const Vector2f VECTOR2_UP = Vector2f(0, -1);
	const Vector2f VECTOR2_DOWN = Vector2f(0, 1);
	const Vector2f VECTOR2_LEFT = Vector2f(-1, 0);
	const Vector2f VECTOR2_RIGHT = Vector2f(1, 0);
};

