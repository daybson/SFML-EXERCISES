#include "Action.h"

Action::Action(const Keyboard::Key & key, int type) : _type(type)
{
	_event.type = Event::EventType::KeyPressed;
	_event.key.code = key;
}

Action::Action(const Mouse::Button & button, int type)
{
	_event.type = Event::EventType::MouseButtonPressed;
	_event.mouseButton.button = button;
}

bool Action::test() const
{
	bool res = false;

	if (_event.type == Event::EventType::KeyPressed)
	{
		if (_type & Type::Pressed)
			res = Keyboard::isKeyPressed(_event.key.code);
	}
	else if (_event.type == Event::EventType::MouseButtonPressed)
	{
		if (_type & Type::Pressed)
			res = Mouse::isButtonPressed(_event.mouseButton.button);
	}
	return res;
}

bool Action::operator==(const Event & event) const
{
	bool res = false;
	switch (event.type)
	{
	case Event::EventType::KeyPressed:
		if (_type & Type::Pressed && _event.type == Event::EventType::KeyPressed)
			res = event.key.code == _event.key.code;
		break;
	case Event::EventType::KeyReleased:
		if (_type & Type::Released && _event.type == Event::EventType::KeyPressed)
			res = event.key.code == _event.key.code;
		break;
	case Event::MouseButtonPressed:
		if (_type & Type::Pressed && _event.type == Event::EventType::MouseButtonPressed)
			res = event.mouseButton.button == _event.mouseButton.button;
		break;
	case Event::EventType::MouseButtonReleased:
		if (_type & Type::Released && _event.type == Event::EventType::MouseButtonPressed)
			res = event.mouseButton.button == _event.mouseButton.button;
		break;
	default:
		break;
	}
	return res;
}

bool Action::operator==(const Action & other) const
{
	return _type == other._type && other == _event;
}
