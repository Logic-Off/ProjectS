using System.Threading.Tasks;

namespace LogicOff.DatabaseDownloader {
	public interface IDownloader {
		string Name { get; }
		Task Download();
	}
}