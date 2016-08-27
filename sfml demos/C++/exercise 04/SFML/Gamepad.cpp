#include "Gamepad.h"



Gamepad::Gamepad(const int index)
{
	this->index = index;
	if (this->isConnected(this->index))
	{
		cout << "Gamepad connected\n" << endl;
		cout << getID() << endl;
		active = true;
		buttons = getButtonCount(this->index);
		hasZ = hasAxis(this->index, Z);
	}
	else
	{
		cout << "Gamepad disconnected\n" << endl;
		active = false;
	}
}


Gamepad::~Gamepad()
{
}

string Gamepad::getID()
{
	Identification id = getIdentification(this->index);	
	return 
		"\n\tVendor ID: " + to_string(id.vendorId) +
		"\n\tJoystick use: " + id.name.toAnsiString() +
		"\n\tProduct ID: " + to_string(id.productId) +
		"\n\tButton count: " + to_string(buttons) +
		"\n\tHas z-axis: " + to_string(hasZ);
}

int Gamepad::getButtonPressed()
{
	for (int i = 0; i < buttons; i++)
		if (isButtonPressed(index, i))
			return i;
}
