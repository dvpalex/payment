//jsc /target:library /out:eval.dll evaluator.js

package SuperPag.Framework.Internals {
	public class Eval {
		function Evaluate( formula : String ) {
			return eval( formula );
		}
	}
}

