using System;

namespace SuperPag.Framework.Data.Components.AutoDataLayer {
	public enum MethodTypes {
		Query,
		Update,
		Insert,
		Delete,
		Custom,
		QueryProc
	}

	public enum SortOrder {
		ASC,
		DESC
	}

	public enum Join {
		Inner,
		Left,
		Right
	}

	public enum Filter {
		Equal,
		NotEqual,
		LessThan,
		GreaterThan,
		LessOrEqual,
		GreaterOrEqual,
		Like,
		LikeLeft,
		LikeRight,
		LikeLeftRight,
		NotLike,
		IsNull,
		NotIsNull
	}

	public enum Link {	
		And,
		Or
	}

	public enum Block {
		None,
		Begin,
		End
	}

	public enum AggregationType {
		Count,
		Max,
		Min,
		Avg,
		Sum
	}
}
