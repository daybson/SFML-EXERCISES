#pragma once

#include "Action.h"
#include <unordered_map> //unordered_map

using namespace std;

template<typename T = int>
class ActionMap
{
private:
	unordered_map<T, Action> _map;

public:
	ActionMap(const ActionMap<T>&) = delete;
	ActionMap<T>& operator=(const ActionMap<T>&)=delete;
	ActionMap() = default;

	void map(const T& key, const Action& action) 
	{
		_map.emplace(key, action);
	}

	const Action& get(const T& key)const 
	{
		return _map.at(key);
	}

};

