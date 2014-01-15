using System;
using System.Reflection;
using System.Collections;

namespace SuperPag.Framework.Helper
{
	
	[Serializable()]
	public class TreeExplorer
	{
		private System.Collections.ArrayList _lst;
		private ArrayList _stack;

		public TreeExplorer()
		{
			_stack = new ArrayList(10);
			_lst = new System.Collections.ArrayList();
		}

		private BaseTree _current;
		
		//Current Tree
		public BaseTree Current
		{ 
			get
			{ 
				return _current; 
			} 
		}

		#region List
		public BaseTree this[int index]
		{
			get
			{
				return (BaseTree)_lst[index];
			}
			set
			{
				_lst[index] = value;
			}
		}

		public void RemoveAt(int index)
		{
			_lst.RemoveAt(index);
		}

		public void Insert(int index, BaseTree value)
		{
			value.OnSelect +=new SuperPag.Framework.Helper.BaseTree.OnSelectArgs(value_OnSelect);
			_lst.Insert(index, value);
		}

		public void Remove(BaseTree value)
		{
			_lst.Remove(value);
		}

		public bool Contains(BaseTree value)
		{
			return _lst.Contains(value);;
		}

		public void Clear()
		{
			_lst.Clear();
		}

		public int IndexOf(BaseTree value)
		{
			return _lst.IndexOf(value);;
		}

		public int Add(BaseTree value)
		{
			value.OnSelect += new SuperPag.Framework.Helper.BaseTree.OnSelectArgs(value_OnSelect);			
			return _lst.Add(value);
		}

		public int AddAndSetCurrent(BaseTree value)
		{
			if(value != null)
			{
				value.OnSelect += new SuperPag.Framework.Helper.BaseTree.OnSelectArgs(value_OnSelect);
				value.SetCurrent();
				return _lst.Add(value);
			} 
			else
			{
				return -1;
			}
		}
		
		public int Count
		{
			get
			{
				return _lst.Count;
			}
		}

		#endregion List

		public void Undo()
		{
			if(_stack.Count < 1) return;
			_current = (BaseTree)_stack[0];
			_stack.RemoveAt(0);
		}

		private void value_OnSelect(BaseTree selectedTree)
		{
			_stack.Insert(0, _current);
			while(_stack.Count > 10)
			{
				_stack.RemoveAt(_stack.Count - 1);
			}
			
			_current = selectedTree;
		}
	}


	/// <summary>
	/// Summary description for TreeHelper.
	/// </summary>
	[Serializable]
	public class BaseTree : Object 
	{
		internal bool _selected = false;
		private BaseTree _parent = null;

		internal event OnSelectArgs OnSelect;
		internal delegate void OnSelectArgs(BaseTree selectedTree);

		public BaseTree Parent
		{
			get
			{
				return _parent;
			}
		}

		public BaseTree(bool filled, BaseTree parent) 
		{
			this._parent = parent; 
		}

		public bool Has(Type TreeToFind)
		{
			return this.IsInTree(TreeToFind);
		}

		public BaseTree Get(Type TreeToFind)
		{
			return this.GetTree(TreeToFind);
		}

		private BaseTree _seekTree;
		
		private BaseTree GetTree(Type TreeToFind)
		{
			_seekTree = null;
			findTree(this, TreeToFind);
			return _seekTree;
		}
		private bool IsInTree(Type TreeToFind) 
		{
			_seekTree = null;
			findTree(this, TreeToFind);
			if (null != _seekTree) return true; else return false;
//			return true;
		}

		private void findTree(BaseTree TreeInstance, Type TreeToFind) 
		{
			if (null != TreeInstance) 
			{
				Type treeType = TreeInstance.GetType();

				if ( treeType == TreeToFind) 
				{
					_seekTree = TreeInstance;
					return;
				} 
				else 
				{
					FieldInfo[] fields = treeType.GetFields();
					foreach ( FieldInfo field in fields) 
					{
						if (field.FieldType.IsSubclassOf(typeof(BaseTree))) 
						{
							if (field.FieldType == TreeToFind) 
							{
								_seekTree = (BaseTree)field.GetValue(TreeInstance);
								return;
							} 
							else 
							{
								findTree((BaseTree)field.GetValue(TreeInstance), TreeToFind);
							}
						}
					}
				}
			}
		}

		public override string ToString()
		{
			return base.ToString ();
		}

		public void SetCurrent()
		{			
			BaseTree seekTree = this;
			while(seekTree.OnSelect == null)
			{
				if(seekTree.Parent == null)
				{
					throw new Exception("A tree deve estar dentro de um TreeExplorer para ser marcada como corrente!");
				}
				
				seekTree = seekTree.Parent;
			}

			this._selected = true;
			seekTree.OnSelect(this);				
		}
	}

}

//		#region IEnumerable Members
//
//		public System.Collections.IEnumerator GetEnumerator()
//		{
//			return _lst.GetEnumerator();
//		}
//
//		#endregion
//
//public void CopyTo(BaseTree[] array, int index)
//{
//_lst.CopyTo(array, index);
//}
//
//
//public object SyncRoot
//{
//get
//{
//return _lst.SyncRoot;
//}
//}
//
//
//public bool IsReadOnly
//{
//get
//{
//return false;
//}
//}
//public bool IsFixedSize
//{
//get
//{
//return false;
//}
//}
//public bool IsSynchronized
//{
//get
//{				
//return _lst.IsSynchronized;
//}
//}
//
//public BaseTree[] GetChilds()
//{
//FieldInfo[] fields = this.GetType().GetFields();
//ArrayList lst = new System.Collections.ArrayList(fields.Length);
//
//Type typeBaseTree = typeof(BaseTree);
//foreach(FieldInfo f in fields)
//{
//if(f.FieldType.IsSubclassOf(typeBaseTree))
//{
//lst.Add(f.GetValue(this));
//}
//}
//return (BaseTree[])lst.ToArray(typeBaseTree);
//}
//BaseTree[] childs = value.GetChilds();
//foreach(BaseTree c in childs)
//{
//c.OnSelect += new SuperPag.Framework.BaseTree.OnSelectArgs(value_OnSelect);
//}
//
//public bool IsCurrent()
//{
//return _selected;
//}
//foreach(BaseTree t in _lst)
//{
//if(t != rootTree)
//{
//t._selected = false;
//}
//}