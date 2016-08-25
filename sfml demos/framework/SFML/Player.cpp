#include "Player.h"

Player::Player()
{
	name = "Player 1";
	shape = *new RectangleShape(Vector2f(20, 20));
	shape.setFillColor(Color::Cyan);
	isMovng = false;
	inputs = new KeyboardInput();
	currentSpeed = walkSpeed;
}

Player::~Player()
{
}

Player::Player(string name)
{
	this->name = name;
	shape = *new RectangleShape(Vector2f(10, 10));
	shape.setFillColor(Color::Blue);
	isMovng = false;
	inputs = new KeyboardInput();
	currentSpeed = walkSpeed;
}

void Player::update(float deltaTime)
{
	if (isMovng)
		move(Vector2f(currentSpeed.x * currentDirection.x, currentSpeed.y * currentDirection.y) * deltaTime);
}

RectangleShape Player::getRender()
{
	return shape;
}


void Player::processEvents(const Event & event)
{	
	if (event.type == Event::KeyReleased)
	{
		if (event.key.code == RUN)
			currentSpeed = walkSpeed;
		if ((event.key.code == UP) || (event.key.code == DOWN) || (event.key.code == LEFT) || (event.key.code == RIGHT))
			//setMovementDirection(VECTOR2_ZERO);
			isMovng = false;
	}
	
	if (event.type == Event::KeyPressed)
	{
		if (event.key.code == RUN)
			currentSpeed = runSpeed;
		if (event.key.code == UP)
			setMovementDirection(VECTOR2_UP);
		if (event.key.code == DOWN)
			setMovementDirection(VECTOR2_DOWN);
		if (event.key.code == LEFT)
			setMovementDirection(VECTOR2_LEFT);
		if (event.key.code == RIGHT)
			setMovementDirection(VECTOR2_RIGHT);
	}
}

void Player::move(Vector2f deltaMove)
{
	this->shape.move(deltaMove);
}

void Player::setMovementDirection(Vector2f direction)
{
	currentDirection = direction;
	isMovng = direction != VECTOR2_ZERO;
}
