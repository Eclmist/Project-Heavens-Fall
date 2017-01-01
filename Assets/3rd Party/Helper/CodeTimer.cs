#region Using Directives

using System;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

#endregion

namespace GenLib.TestingAndProfiling
{
	public class CodeTimer : IDisposable
	{
		private readonly string m_text;
		private readonly Stopwatch m_stopwatch;

		public CodeTimer(string text) {
			m_text = text;
			m_stopwatch = Stopwatch.StartNew();
		}

		public void Dispose() {
			m_stopwatch.Stop();
			Debug.Log(string.Format("#Profiling# {0}: {1}ms ({2} ticks)", m_text, m_stopwatch.ElapsedMilliseconds, m_stopwatch.ElapsedTicks));
		}
	}
}