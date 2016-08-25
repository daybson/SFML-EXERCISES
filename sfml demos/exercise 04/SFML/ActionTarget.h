#pragma once

#include "Action.h"
#include "ActionMap.h"
#include <functional> //function
#include <utility> //pair
#include <list> //list

using namespace std;
using namespace sf;

template<typename T = int>
class ActionTarget
{
public:
	ActionTarget(const ActionTarget<T>&) = delete;
	ActionTarget<T>& operator = (const ActionTarget<T>&) = delete;

	using FuncType = function<void(const Event&)>;

	ActionTarget(const ActionMap<T>& map);


	template<typename T>
	bool processEvent(const Event& event)const
	{
		bool res = false;
		for (auto& pair : _eventsPoll)
		{
			if (_actionMap.get(pair.first) == event)
			{
				pair.second(event);
				res = true;
				break;
			}
		}
		return res;
	}

	template<typename T>
	void processEvent()const
	{
		for (auto& pair : _eventsRealTime)
		{
			const Action& action = _actionMap.get(pair.first);
			if (action.test())
				pair.second(action._event);
		}
	}

	template<typename T>
	void bind(const T& key, const FuncType& callback)
	{
		const Action& action = _actionMap.get(key);
		if (action._type & Action::Type::RealTime)
			_eventsRealTime.emplace_back(key, callback);
		else
			_eventsRealTime.emplace_back(key, callback);
	}

	template<typename T>
	void unbind(const T& key)
	{
		auto remove_func = [&key](const pair<T, FuncType>& pair)->bool
		{
			return pair.first == key;
		}

		const Actio& action = _actionMap.get(key);
		if (action._type & Action::Type::RealTime)
			_eventsRealTime.remove_if(remove_func);
		else
			_eventsPoll.remove_if(remove_func);
	}

private:
	list<pair<Action, FuncType>> _eventsRealTime;
	list<pair<Action, FuncType>> _eventsPoll;
	const ActionMap<T>& _actionMap;
};

