namespace Ecs.Common {
	public static class IdGenerator {
		private static int _index = 0;

		public static Id GetNext() => new(_index++);
	}
}