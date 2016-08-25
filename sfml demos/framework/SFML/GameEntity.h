#pragma once

#include <iostream>
#include <string>
#include <SFML\Window.hpp>
#include <SFML\Graphics.hpp>

using namespace sf;
using namespace std;

class GameEntity
{
public:
	GameEntity();
	virtual void update(float deltaTime);
	virtual void processEvents(const Event& event);
protected:

	string name;
	bool isEnable;
};

