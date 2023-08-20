using UnityEngine;
using System.Collections;

namespace FKGame.UIWidgets{
	public interface IValidation<T> {
		bool Validate(T item);
	}
}