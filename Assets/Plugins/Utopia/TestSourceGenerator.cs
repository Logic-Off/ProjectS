using Microsoft.CodeAnalysis;
using UnityEngine;

namespace Utopia {
	public class TestSourceGenerator : ISourceGenerator{
		public void Initialize(GeneratorInitializationContext context) {
			Debug.Log("START");
		}

		public void Execute(GeneratorExecutionContext context) {
			
		}
	}
}