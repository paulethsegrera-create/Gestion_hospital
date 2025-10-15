using Gestion_Hospital.Interfaces;

namespace Gestion_Hospital.Models
{
	/// <summary>
	/// Represents a patient in the system.
	/// Inherits from <see cref="Person"/> for common data (name, document, phone, email)
	/// and implements <see cref="Gestion_Hospital.Interfaces.IRegistrable"/> to mark it as a registrable/persistable entity.
	/// </summary>
	public class Patient : Person, IRegistrable
	{
	/// <summary>Patient's age (years).</summary>
		public int Age { get; set; }
	}
}
