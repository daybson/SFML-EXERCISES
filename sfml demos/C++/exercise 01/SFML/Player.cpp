#include "Player.h"



Player::Player()
{
	Vector2f position = Vector2f(0.f, 0.f);

	body = *new RectangleShape(Vector2f(15, 50));
	body.setOrigin(position);
	body.setPosition(-7.5, -50);
	body.setFillColor(Color::Magenta);

	head = *new CircleShape(10);
	head.setOrigin(position);
	head.setPosition(body.getPosition().x - 2.5, -body.getSize().y - 10);
	head.setFillColor(Color::Yellow);

	legLeft = *new RectangleShape(Vector2f(5, 20));
	legLeft.setOrigin(position);
	legLeft.setPosition(body.getPosition().x - 2.5, 0);
	legLeft.setFillColor(Color::Yellow);

	legRight = *new RectangleShape(Vector2f(5, 20));
	legRight.setOrigin(position);
	legRight.setPosition(-body.getPosition().x - 2.5, 0);
	legRight.setFillColor(Color::Yellow);

	armLeft = *new RectangleShape(Vector2f(-30, 3));
	armLeft.setOrigin(position);
	armLeft.setPosition(body.getPosition().x - 5, -45);
	armLeft.rotate(-75);
	armLeft.setFillColor(Color::Yellow);

	armRight = *new RectangleShape(Vector2f(30, 3));
	armRight.setOrigin(position);
	armRight.setPosition(-body.getPosition().x + 5, -45);
	armRight.rotate(75);
	armRight.setFillColor(Color::Yellow);

	hat = *new CircleShape(10, 3);
	hat.setOrigin(position);
	hat.setPosition(body.getPosition().x - 2.5, -body.getSize().y - 10 - 15);
	hat.setFillColor(Color::Red);

	nose = *new CircleShape(3);
	nose.setOrigin(position);
	nose.setPosition(body.getPosition().x + head.getRadius()/2, head.getPosition().y + head.getRadius());
	nose.setFillColor(Color::Red);
}


Player::~Player()
{
}

void Player::Move(Vector2f delta)
{
	body.move(delta);
	head.move(delta);
	legLeft.move(delta);
	legRight.move(delta);
	armLeft.move(delta);
	armRight.move(delta);
	hat.move(delta);
	nose.move(delta);
}
