using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

//
// Goal: Create a very lightweight (minimize memory usage) thread-safe datastructure holding a reference to 0, one or more objects.
// Optimize for scenario with 0 or 1 referenced objects (algorithm's hot path). Once a single datastructure instance holds more than 1 object it's considered "strange" and
// does not have to be optimized anymore.
//

class ThreadSafeObjectHolder {

    volatile object item;
    public volatile ThreadSafeObjectHolder next; 
	public ThreadSafeObjectHolder() { }

    /// <summary>
    /// If the item equals null, program is going to replace item with obj parameter.
    /// It is the first situation, when we add the first item to the list.
    /// If the first condition is not met, we are going to add the element at the of the list.
    /// NewHolder is going to contain the new item and with CurHolder we are going to move from the beginning
    /// of the list to the end.
    /// </summary>
    /// <param name="obj"></param>
	public void AddObject(object obj) {

		if (Interlocked.CompareExchange(ref item,obj,null) == null) 
            return;    


        ThreadSafeObjectHolder newHolder = new ThreadSafeObjectHolder();
        Interlocked.Exchange(ref newHolder.item,obj);
        ThreadSafeObjectHolder curHolder = this;

        while (Interlocked.CompareExchange(ref curHolder,newHolder,null) != null) {
            curHolder = curHolder.next;
        }

        
	}

	public object GetFirstObject() {
        return item;
	}
}

class Program {
	static void Main(string[] args) {
        
	}
}
