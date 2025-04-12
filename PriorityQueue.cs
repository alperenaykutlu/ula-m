using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace prolab2
{
	class PriorityQueue<TElement, TPriority> where TPriority : IComparable<TPriority>
	{
		private List<(TElement Element, TPriority Priority)> elements = new List<(TElement, TPriority)>();

		public int Count => elements.Count;

		public void Enqueue(TElement element, TPriority priority)
		{
			elements.Add((element, priority));
			elements.Sort((a, b) => a.Priority.CompareTo(b.Priority));
		}

		public TElement Dequeue()
		{
			if (elements.Count == 0)
			{
				throw new InvalidOperationException("Queue is empty");
			}

			TElement result = elements[0].Element;
			elements.RemoveAt(0);
			return result;
		}
	}
}