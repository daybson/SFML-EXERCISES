#pragma once

#include <SFML\Graphics.hpp>
#include <iostream>

using namespace sf;
using namespace std;

class Gamepad : public Joystick
{

private:
	bool active = false;
	bool hasZ = false;
	bool move = false;
	int buttons = 0;	
	int deadzone = 15;
public:
	Gamepad(const int index);
	~Gamepad();
	int index;

	string getID();
	int getDeadZone() { return deadzone; };
	int getButtonPressed();
};

