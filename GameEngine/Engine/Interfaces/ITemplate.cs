#region
using System.Collections.Generic;
#endregion

namespace ECS {
	public interface ITemplate {
		List<Component> Components { get; set; }
	}
}