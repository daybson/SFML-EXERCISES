#include "UIButton.h"


UIButton::UIButton(string normalText, string clickedText, Vector2f position)
{
	try
	{
		if (!normalTexture.loadFromFile(normalText))
			cout << "Error loading file" << endl;
		
		if (!clickedTexture.loadFromFile(clickedText))
			cout << "Error loading file" << endl;

		this->normal.setTexture(normalTexture);
		this->clicked.setTexture(clickedTexture);
		this->normal.setPosition(position);
		this->clicked.setPosition(position);
		setState(false);
	}
	catch (int e)
	{
		cout << "ERROR" << endl;
	}
}

UIButton::~UIButton()
{
}

void UIButton::checkClick(Vector2f mousePos)
{
	int width = currentSprite.getGlobalBounds().width;
	int height = currentSprite.getGlobalBounds().height;

	if (mousePos.x > currentSprite.getPosition().x && mousePos.x < (currentSprite.getPosition().x + width) &&
		mousePos.y > currentSprite.getPosition().y && mousePos.y < (currentSprite.getPosition().y + height))
		setState(!currentState);
}

void UIButton::setState(bool which)
{
	currentState = which;
	if (currentState)
	{
		currentSprite = clicked;
		return;
	}
	currentSprite = normal;
}
