//using System;
//using System.Collections;

//namespace SuperPag.Framework.Helper
//{
//    public sealed class EvalBool
//    {
//        private bool _Value;
		
//        private EvalBool(){}

//        internal EvalBool( bool value )
//        {
//            _Value = value;
//        }

//        public static implicit operator bool(EvalBool evalBool) 
//        {
//            return evalBool._Value;
//        }

//        public override string ToString()
//        {
//            return _Value.ToString();
//        }
//    }

//    public sealed class EvalDouble
//    {
//        private double _Value;
		
//        private EvalDouble(){}

//        internal EvalDouble( double value )
//        {
//            _Value = value;
//        }

//        public static implicit operator double(EvalDouble evalDouble) 
//        {
//            return evalDouble._Value;
//        }

//        public override string ToString()
//        {
//            return _Value.ToString();
//        }
//    }
	
//    //public sealed class Eval
//    //{
//    //    private static EFinancial.Framework.Internals.Eval eval = null;

//    //    //decimal
//    //    internal static string DecimalToken = "."; /* , */
//    //    internal static string GroupToken = ",";
//    //    internal static int MaxDecimalDigits = 15;

//    //    private static string[] Tokens = new string[]
//    //        {
//    //            //outros
//    //            "(", ")",
//    //            //operadores binarios e unários
//    //            "+", "-", "*", "/", "%"
//    //        };

//    //    private static string[] NTokens = new string[]
//    //        {
//    //            //valores
//    //            "0", "1", "2", "3", "4", "5", "6", "7", "8", "9"				
//    //        };
		
//    //    private static string[] OpTokens = new string[]
//    //        {
//    //            //operadores
//    //            ">", "<", "!"
//    //        };		

//    //    private static string[] OpDTokens = new string[]
//    //        {
//    //            //operadores
//    //            ">=", "<=", "==", "!=",
//    //            //logicos
//    //            "&&" /* E */, "||" /* OU */
//    //        };

//    //    private static string[] VarTokens = new string[]
//    //        {
//    //            "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m",
//    //            "n", "o", "p", "q", "r", "s", "t", "u", "v", "x", "y", "z",
//    //            "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M",
//    //            "N", "O", "P", "Q", "R", "S", "T", "U", "V", "X", "Y", "Z",
//    //            "@", "_", "$"
//    //        };

//    //    private static string VarSeparatorToken = "-";
		
//    //    static Eval()
//    //    {
//    //        eval = new EFinancial.Framework.Internals.Eval();
			
//    //        Array.Sort( Tokens ); 
//    //        Array.Sort( OpTokens ); 
//    //        Array.Sort( OpDTokens );
//    //        Array.Sort( VarTokens );			
//    //    }

//    //    private Eval(){}

//    //    public static object Evaluate( bool validate, string formula, out bool isBoolean )
//    //    {
//    //        return Evaluate( validate, formula, null, out isBoolean );
//    //    }

//    //    public static object Evaluate( bool validate, string formula, Hashtable variables, out bool isBoolean )
//    //    {
//    //        try
//    //        {
//    //            object result = InternalEval( validate, formula, variables );
//    //            if( null != result )
//    //            {
//    //                if( result.GetType() == typeof(bool) )
//    //                {
//    //                    isBoolean = true;
//    //                    return new EvalBool( (bool)result );
//    //                }
//    //                if( result.GetType() == typeof(int) )
//    //                {
//    //                    isBoolean = false;
//    //                    return new EvalDouble( (double)(int)result );
//    //                }
//    //                if( result.GetType() == typeof(double) )
//    //                {
//    //                    isBoolean = false;
//    //                    return new EvalDouble( (double)result );
//    //                }
					
//    //                throw new ApplicationException( "Conversion Error at return" ); 
//    //            }
//    //            else
//    //            {
//    //                isBoolean = false;
//    //                return null;
//    //            }
//    //        }
//    //        catch( Exception e )
//    //        {
//    //            throw e;
//    //        }			
//    //    }
		
//    //    public static EvalBool EvaluateToBoolean( bool validate, string formula )
//    //    {
//    //        return EvaluateToBoolean( validate, formula, null );			
//    //    }

//    //    public static bool IsValidBool(string formula)			
//    //    {
//    //        return IsValidBool(formula, null);
//    //    }

//    //    public static bool IsValidBool(string formula, Hashtable variables)			
//    //    {
//    //        try
//    //        {
//    //            EvaluateToBoolean( true, formula, variables );

//    //            return ( 
//    //                (formula.IndexOf(">") != -1) || 
//    //                (formula.IndexOf("<") != -1) || 
//    //                (formula.IndexOf("==") != -1) || 
//    //                (formula.IndexOf("!=") != -1) || 
//    //                (formula.IndexOf(">=") != -1) ||
//    //                (formula.IndexOf("<=") != -1) );

//    //        } 
//    //        catch 
//    //        {
//    //            return false;
//    //        }
//    //    }
		
//    //    public static bool IsValid(string formula)
//    //    {
//    //        return IsValid(formula, null);
//    //    }

//    //    public static bool IsValid(string formula, Hashtable variables)
//    //    {
//    //        try
//    //        {
//    //            bool formulaResult;
//    //            Evaluate(true, formula, variables, out formulaResult);


//    //            if(	(formula.IndexOf(">") > -1) || 
//    //                (formula.IndexOf("<") > -1) || 
//    //                (formula.IndexOf("!=") > -1) || 
//    //                (formula.IndexOf(">=") > -1) ||
//    //                (formula.IndexOf("==") != -1) ||
//    //                (formula.IndexOf("<=") > -1) )
//    //            {
//    //                return false;
//    //            }

//    //            return true;
//    //        } 
//    //        catch 
//    //        {
//    //            return false;
//    //        }
//    //    }

//    //    public static EvalBool EvaluateToBoolean( bool validate, string formula, Hashtable variables )
//    //    {
//    //        try
//    //        {
//    //            object result = InternalEval( validate, formula, variables );
//    //            if( null != result )
//    //            {
//    //                return new EvalBool( Convert.ToBoolean( result ) );
//    //            }
//    //            else
//    //            {
//    //                return null;
//    //            }
//    //        }
//    //        catch( Exception e )
//    //        {
//    //            throw e;
//    //        }
//    //    }

//    //    public static Hashtable GetVariablesFromFormula( string formula )
//    //    {
//    //        int len = formula.Length;

//    //        Hashtable vars = new Hashtable();
			
//    //        for( int idx = 0; idx < len; idx++ )
//    //        {			
//    //            string one = formula.Substring( idx, 1 );

//    //            if( one == " " ) 
//    //                continue;

//    //            bool fValid = false;
								
//    //            System.Text.StringBuilder sbVariable = new System.Text.StringBuilder();
			
//    //            while( Array.BinarySearch( VarTokens, one ) > -1 )
//    //            {
//    //                fValid = true;

//    //                sbVariable.Append( one );
					
//    //                if( idx + 1 < len )
//    //                {
//    //                    one = formula.Substring( ++idx, 1 );
//    //                    if( VarSeparatorToken == one )
//    //                    {
//    //                        idx++;

//    //                        sbVariable.Append( one );

//    //                        if( idx + 1 < len )
//    //                            one = formula.Substring( idx, 1 );
//    //                    }
//    //                }
//    //                else
//    //                {
//    //                    break;
//    //                }					
//    //            }
				
//    //            if( fValid )
//    //            {
//    //                string variable = sbVariable.ToString();
//    //                if( ! vars.ContainsKey( variable ) ) 
//    //                    vars.Add( variable, null );
//    //            }				
//    //        }

//    //        return vars.Count > 0 ? vars : null;
//    //    }

//    //    public static string[] GetVariablesFromFormulaArrayOfString( string formula )
//    //    {
//    //        Hashtable hsVariables = GetVariablesFromFormula( formula );
//    //        if ( hsVariables == null )
//    //            return null;

//    //        // Move as chaves da hashtable para um array de string
//    //        int i=0;
//    //        string[] arrVariables = new string[hsVariables.Count];
//    //        foreach( string variable in hsVariables.Keys )
//    //        {
//    //            arrVariables[i++] = variable;
//    //        }

//    //        return arrVariables;
//    //    }

//    //    public static EvalDouble EvaluateToValue( bool validate, string formula )
//    //    {
//    //        return EvaluateToValue( validate, formula, null );
//    //    }

//    //    public static EvalDouble EvaluateToValue( bool validate, string formula, Hashtable variables )
//    //    {
//    //        try
//    //        {
//    //            object result = InternalEval( validate, formula, variables );
//    //            if( null != result )
//    //            {
//    //                if( result.GetType() == typeof(int) )
//    //                    return new EvalDouble( (double)(int)result );
//    //                return new EvalDouble( (double)result );
//    //            }
//    //            else
//    //            {
//    //                return null;
//    //            }
//    //        }
//    //        catch( Exception e )
//    //        {
//    //            throw e;
//    //        }
//    //    }

//    //    private static bool CheckVariables( Hashtable variables, string newFormula, ref string one, ref int idx, int len )
//    //    {
//    //        bool fValid = false;
//    //        bool fIgnoreVarSeparatorToken = false;
//    //        string last_invalid_varname = null;

//    //        step_back_point:
			
//    //            System.Text.StringBuilder sbVariable = new System.Text.StringBuilder();
			
//    //        while( Array.BinarySearch( VarTokens, one ) > -1 )
//    //        {
//    //            fValid = true;

//    //            sbVariable.Append( one );
					
//    //            if( idx + 1 < len )
//    //            {
//    //                one = newFormula.Substring( ++idx, 1 );
//    //                if( !fIgnoreVarSeparatorToken && VarSeparatorToken == one )
//    //                {
//    //                    idx++;

//    //                    sbVariable.Append( one );

//    //                    if( idx + 1 < len )
//    //                        one = newFormula.Substring( idx, 1 );
//    //                }
//    //            }
//    //            else
//    //            {
//    //                break;
//    //            }					
//    //        }
				
//    //        if( fValid )
//    //        {
//    //            string variable = sbVariable.ToString();
					
//    //            int variable_len = variable.Length - 1;
//    //            if( variable[ variable_len ].ToString() == VarSeparatorToken )
//    //            {
//    //                variable = variable.Substring( 0, variable_len );
//    //                idx--;
//    //            }
					
//    //            if( variables.ContainsKey( variable ) )
//    //            {

//    //                if( idx + 1 < len ) 
//    //                    idx--;

//    //                return true;
//    //            }
//    //            else
//    //            {	
//    //                if( !fIgnoreVarSeparatorToken && variable.IndexOf( VarSeparatorToken ) > -1 )
//    //                {
//    //                    last_invalid_varname = variable;

//    //                    fIgnoreVarSeparatorToken = true;
//    //                    idx -= ( variable.Length - ( idx + 1 < len ? 0 : 1 ) );
//    //                    one = newFormula.Substring( idx, 1 );
						
//    //                    goto step_back_point;
//    //                }
						
//    //                throw new ArgumentException( "Invalid Variable Name", last_invalid_varname != null ? String.Format( "{0} or {1}", variable, last_invalid_varname ) : variable );
//    //            }
//    //        }

//    //        return false;
//    //    }

//    //    private static bool CheckSimpleTokens( string one )
//    //    {
//    //        return Array.BinarySearch( Tokens, one ) > -1;			
//    //    }

//    //    private static bool CheckNumbers( string one )
//    //    {
//    //        return Array.BinarySearch( NTokens, one ) > -1;			
//    //    }
		
//    //    private static bool CheckOperators( string newFormula, string one, ref int idx, int len )
//    //    {
//    //        if( Array.BinarySearch( OpTokens, one ) > -1 )
//    //        {
//    //            if( idx + 1 < len )
//    //            {
//    //                if( Array.BinarySearch( OpDTokens, newFormula.Substring( idx, 2 ) ) > -1 )
//    //                {
//    //                    idx++;
//    //                }
//    //            }

//    //            return true;
//    //        }

//    //        return false;
//    //    }

//    //    private static bool CheckDoubleTokens( string newFormula, ref int idx, int len )
//    //    {
//    //        if( idx + 1 < len )
//    //        {
//    //            if( Array.BinarySearch( OpDTokens, newFormula.Substring( idx, 2 ) ) > -1 )
//    //            {	
//    //                idx++;

//    //                return true;
//    //            }
//    //        }

//    //        return false;
//    //    }

//    //    private static bool CheckDecimal( string newFormula, string one, int idx, int len )
//    //    {
//    //        if( one == DecimalToken )
//    //        {
//    //            bool fValid = false;
				
//    //            if( idx - 1 >= 0 )
//    //            {
//    //                fValid = Array.BinarySearch( NTokens, newFormula.Substring( idx - 1, 1 ) ) > -1;
//    //            }
					
//    //            if( fValid && idx + 1 <= len )
//    //            {
//    //                fValid = Array.BinarySearch( NTokens, newFormula.Substring( idx + 1, 1 ) ) > -1;
//    //            }

//    //            if( fValid ) 
//    //                return true;
//    //        }

//    //        return false;
//    //    }

//    //    private static void Tokenizer( string newFormula, Hashtable variables )
//    //    {
//    //        int len = newFormula.Length;
			
//    //        for( int idx = 0; idx < len; idx++ )
//    //        {			
//    //            string one = newFormula.Substring( idx, 1 );

//    //            if( one == " " ) 
//    //                continue;

//    //            if( CheckVariables( variables, newFormula, ref one, ref idx, len ) )
//    //                continue;
				
//    //            if( CheckSimpleTokens( one ) )
//    //                continue;

//    //            if( CheckNumbers( one ) )
//    //                continue;

//    //            if( CheckOperators( newFormula, one, ref idx, len ) ) 
//    //                continue;

//    //            if( CheckDoubleTokens( newFormula, ref idx, len ) )
//    //                continue;

//    //            if( CheckDecimal( newFormula, one, idx, len ) )
//    //                continue;								

//    //            //sintax error : invalid formula
//    //            throw new ApplicationException( String.Format( "Invalid Formula at {0} - Token Error {1}", idx + 1, one ) );
//    //        }
//    //    }

//    //    private static string ReplaceSeparator( string formula )
//    //    {
//    //        string newFormula = formula;

//    //        newFormula = newFormula.Replace( Eval.GroupToken, "~" );			
//    //        newFormula = newFormula.Replace( Eval.DecimalToken, Eval.GroupToken );
//    //        newFormula = newFormula.Replace( "~", Eval.DecimalToken );

//    //        return newFormula;
//    //    }

//    //    private static string ReplaceBooleanComparer( string formula )
//    //    {
//    //        string newFormula = formula;

//    //        int idx = 0;
//    //        while( ( idx = newFormula.IndexOf( "=", idx ) ) != -1 )
//    //        {
//    //            if( idx > 1 && newFormula[ idx - 1 ] != '!' && idx < newFormula.Length && newFormula[ idx + 1 ] != '=' )
//    //            {
//    //                newFormula = newFormula.Insert( idx, "=" );
//    //                idx++;
//    //            }
				
//    //            if( (idx + 1) < newFormula.Length  )
//    //                idx++;
//    //            else
//    //                break;
//    //        }

//    //        return newFormula;
//    //    }
		
//    //    private static string CleanGroupSeparator( string formula )
//    //    {
//    //        string newFormula = formula;
			
//    //        int idx = 0;
//    //        while( ( idx = newFormula.IndexOf( ",", idx ) ) != -1 )
//    //        {
//    //            newFormula = newFormula.Remove( idx, 1 ); 
//    //        }

//    //        return newFormula;
//    //    }
		
//    //    private static string PreParse( bool validate, string formula, Hashtable variables )
//    //    {
//    //        //Formula
//    //        string newFormula = formula.Replace( " E ", " && " );
//    //        newFormula = newFormula.Replace( " OU ", " || " );

//    //        newFormula = ReplaceBooleanComparer( newFormula );
			
//    //        newFormula = ReplaceSeparator( newFormula );
	
//    //        newFormula = CleanGroupSeparator( newFormula );

//    //        if( validate ) 
//    //            Tokenizer( newFormula, variables );

//    //        //Variables
//    //        if( null != variables )
//    //        {
//    //            string[] keys = new string[ variables.Keys.Count ];
//    //            variables.Keys.CopyTo( keys, 0 ); 				
//    //            Array.Sort( keys, new VariableComparer() );
				
//    //            System.Globalization.NumberFormatInfo ni = new System.Globalization.NumberFormatInfo();
//    //            ni.NumberDecimalDigits = Eval.MaxDecimalDigits;
//    //            ni.NumberDecimalSeparator = Eval.DecimalToken;
//    //            ni.NumberGroupSeparator = Eval.GroupToken;
				
//    //            foreach( string variable in keys )
//    //            {
//    //                if( newFormula.IndexOf( variable ) > -1 )
//    //                {				
//    //                    if( variables[ variable ] != null ) 
//    //                    {
//    //                        string variable_value = null;
							
//    //                        if( variables[ variable ] is string )
//    //                        {
//    //                            variable_value = ReplaceSeparator( variables[ variable ].ToString() );
//    //                        }
//    //                        else
//    //                        {
//    //                            variable_value = String.Format( ni, "{0}", variables[ variable ] );
//    //                        }

//    //                        newFormula = newFormula.Replace( variable, variable_value );
//    //                    }
//    //                    else
//    //                    {
//    //                        return null;
//    //                    }
//    //                }
//    //            }
//    //        }			

//    //        return CleanGroupSeparator( newFormula );
//    //    }

//        private static object InternalEval( bool validated, string formula, Hashtable variables )
//        {
//            string newFormula = PreParse( validated, formula, variables ); 
//            if( null != newFormula )
//                return eval.Evaluate( newFormula );
//            return null;			
//        }

//        class VariableComparer : IComparer
//        {
//            #region IComparer Members

//            public int Compare(object x, object y)
//            {
//                string sx = x.ToString();
//                string sy = y.ToString();

//                if( sx.Length > sy.Length )
//                    return -1;
//                else if( sx.Length < sy.Length )
//                    return 1;
//                return 0;				
//            }

//            #endregion
//        }

//    }
//}