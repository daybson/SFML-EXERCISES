#pragma once

#include <SFML\Window\Event.hpp>
#include <iostream>

using namespace sf;
using namespace std;

class Action
{
public:
	enum Type
	{
		RealTime = 1,
		Pressed = 1 << 1,
		Released = 1 << 2
	};

	Action(const Keyboard::Key& key, int type = Type::RealTime | Type::Pressed);
	Action(const Mouse::Button& button, int type = Type::RealTime | Type::Pressed);

	bool test()const;
	bool operator==(const Event&event)const;
	bool operator==(const Action& other)const;

private:
	friend class ActionTarget;
	Event _event;
	int _type;
};

