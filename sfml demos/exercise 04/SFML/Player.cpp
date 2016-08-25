#include "Player.h"

Player::Player() : ActionTarget(_playerInputs), _shape(Vector2f(32, 32)), _isMoving(false)
{
	_shape.setFillColor(Color::Blue);
	_shape.setOrigin(16, 16);

	bind(PlayerInputs::Up, [this](const Event&)
	{
		_isMoving = true;
	});

	bind(PlayerInputs::Left, [this](const Event&)
	{
		_isMoving = true;
	});

	bind(PlayerInputs::Right, [this](const Event&)
	{
		_isMoving = true;
	});

	bind(PlayerInputs::Down, [this](const Event&)
	{
		_isMoving = true;
	});
}

void Player::processEvents()
{
	_isMoving = false;
	ActionTarget::processEvent();
}

void Player::setDefaultInputs()
{
	_playerInputs.map(PlayerInputs::Up, Action(Keyboard::Up));
	_playerInputs.map(PlayerInputs::Right, Action(Keyboard::Right));
	_playerInputs.map(PlayerInputs::Left, Action(Keyboard::Left));
	_playerInputs.map(PlayerInputs::Down, Action(Keyboard::Down));
}
